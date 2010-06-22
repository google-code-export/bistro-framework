using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using System.IO;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;
using System.Threading;

namespace Bistro.Designer.Explorer
{
    
    internal partial class FileManager : IDisposable, IVsRunningDocTableEvents, 
        IVsRunningDocTableEvents4, IVsTextLinesEvents,IVsTrackProjectDocumentsEvents2
    {
        

        #region Constructions and Initialization
        /// <summary>
        /// Constructor of the object that will observe all inside and outside IDE file changes
        /// and report them to ChangesTracker object
        /// </summary>
        /// <param name="project">Project to observe</param>
        /// <param name="projExt">Project extension is necessary to create an instance of the parser</param>
        /// <param name="UIControl">ToolWindow that will listen what parser reports</param>
        public FileManager(IVsProject project, string projExt,object UIControl)
        {
            if (projExt == "csproj" || projExt == "fsproj")
            {
                this.project = project;
                this.projExt = projExt;
                tracker = new ChangesTracker(projExt);
                notifier = new Notifier(this);
                watchers = new Dictionary<string, FileSystemWatcher>();
                codeFiles = BuildCodeFileList();
                AdviseEvents();
                tracker.OnProjectOpened(codeFiles);
                tracker.Explorer = (ExplorerWindow)UIControl;
            }
        }

        internal List<string> CodeFiles { get { return codeFiles; } }
        protected IVsProject Project { get { return project; } }
        protected IVsTextView _textView = null;
        private IVsProject project;
        private Notifier notifier;
        internal ChangesTracker tracker;
        private IVsRunningDocumentTable vsRDT;
        private uint rdtCookie;
        private IVsTrackProjectDocuments2 vsTPD;
        private uint tpdCookie;
        private Dictionary<string, FileSystemWatcher> watchers;
        private List<string> codeFiles = null;
        private string activeCodeFile;
        private uint activeCodeFileCookie;
        /// <summary>
        /// indicates the delay (in milliseconds) of update when user is typing. 
        /// </summary>
        private const int DELAY = 3000;
        /// <summary>
        /// The timer for optimization of update tracking. If there would be some changes with time 
        /// between sequential changes less then DELAY, FileModofied will be fired only once.
        /// </summary>
        private Timer updateTimer;
        private string projExt;
        internal void AdviseEvents()
        {

            vsRDT = (IVsRunningDocumentTable)Package.GetGlobalService(typeof(SVsRunningDocumentTable));
            // 1. subscribe to internal file events - events initiated from within VS
            ErrorHandler.ThrowOnFailure(vsRDT.AdviseRunningDocTableEvents(this, out rdtCookie));

            // 2. subscribe to external file events
            foreach (string fileName in codeFiles)
                    AddWatcher(fileName);
            vsTPD = (IVsTrackProjectDocuments2)Package.GetGlobalService(typeof(SVsTrackProjectDocuments));
            ErrorHandler.ThrowOnFailure(vsTPD.AdviseTrackProjectDocumentsEvents(this, out tpdCookie));

        }

        #endregion



        private bool NeedsNotify(string path) 
        { 
            return path.EndsWith(projExt) || ToBeParsed(path);
        }
        
        protected void HandleTextViewChange(IVsTextView textView)
        {
            IVsTextLines buffer;
            Guid bufEventGuid = typeof(IVsTextLinesEvents).GUID;
            IConnectionPointContainer container;
            IConnectionPoint cpoint;
            if (_textView != null)
            {
                _textView.GetBuffer(out buffer);
                container = buffer as IConnectionPointContainer;
                container.FindConnectionPoint(ref bufEventGuid, out cpoint);
                cpoint.Unadvise(activeCodeFileCookie);
            }
            _textView = textView;
            if (_textView != null)
            {
                _textView.GetBuffer(out buffer);
                container = buffer as IConnectionPointContainer;
                container.FindConnectionPoint(ref bufEventGuid, out cpoint);
                cpoint.Advise(this, out activeCodeFileCookie);

            }
        }
        protected void FileAdded(string path)
        {
            if (NeedsNotify(path))
                notifier.FileAdded(path);
        }

        protected void FileRemoved(string path)
        {
            notifier.FileRemoved(path);
        }

        private void FileModified(object path)
        {
            if (NeedsNotify((string)path))
                 notifier.FileModified((string)path);
        }
        private List<string> BuildCodeFileList()
        {
            return BuildCodeFileList(VSConstants.VSITEMID_ROOT);
        }

        private List<string> BuildCodeFileList(uint item)
        {
            List<string> result = new List<string>();

            object obj;
            if (ErrorHandler.Failed(((IVsHierarchy)Project).GetProperty(item, (int)__VSHPROPID.VSHPROPID_FirstChild, out obj)))
                return result;

            item = (uint)(int)obj;
            while (item != VSConstants.VSITEMID_NIL)
            {
                Guid typeGuid;
                string fileName;
                if (
                    // we succesfully retrieved the property
                   ErrorHandler.Succeeded(((IVsHierarchy)Project).GetGuidProperty(item, (int)__VSHPROPID.VSHPROPID_TypeGuid, out typeGuid))
                    // this is a real file
                   && VSConstants.GUID_ItemType_PhysicalFile == typeGuid
                    // will be compiled when the project is built
                   && isCompilable(item)
                    // we succesfully retrieved the filename
                   && ErrorHandler.Succeeded(Project.GetMkDocument(item, out fileName)))
                {
                    result.Add(fileName);
                }

                result.AddRange(BuildCodeFileList(item));

                ((IVsHierarchy)Project).GetProperty(item, (int)__VSHPROPID.VSHPROPID_NextSibling, out obj);
                item = (uint)(int)obj;
            }

            return result;
        }

        private bool ToBeParsed(string path)
        {
            uint itemid;

            if (ErrorHandler.Failed(((IVsHierarchy)Project).ParseCanonicalName(path, out itemid)))
                return false;

            return isCompilable(itemid);
        }

        private bool isCompilable(uint itemid)
        {
            object browseObject;
            if (ErrorHandler.Succeeded(((IVsHierarchy)Project).GetProperty(itemid, (int)__VSHPROPID.VSHPROPID_BrowseObject, out browseObject)))
            {
                object action = browseObject.GetType().InvokeMember("BuildAction", System.Reflection.BindingFlags.GetProperty, null, browseObject, new object[] { });
                return (int)action == 1;
            }
            return false;
        }


        #region Handling file events external to VS

        private void AddWatcher(string path)
        {
            IVsHierarchy hier;
            uint itemid;
            IntPtr ppunkDocData;
            uint cookie;

            // if this file is registered with the RDT we don't need watch after it
            // modifications to this file will be processed through RDT events
            ErrorHandler.ThrowOnFailure(vsRDT.FindAndLockDocument((uint)_VSRDTFLAGS.RDT_NoLock, path, out hier, out itemid, out ppunkDocData, out cookie));
            if (ppunkDocData != IntPtr.Zero)
                return;

            FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(path), Path.GetFileName(path));            
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += new FileSystemEventHandler(fileWatcher_Changed);
            watcher.EnableRaisingEvents = true;

            lock (watchers)
            {
                if (!watchers.ContainsKey(path.ToLower()))
                    watchers.Add(path.ToLower(), watcher);
            }
        }

        internal void removeWatcher(string path)
        {
            FileSystemWatcher watcher;
            lock (watchers)
            {
                if (watchers.TryGetValue(path.ToLower(), out watcher))
                {
                    watcher.EnableRaisingEvents = false;
                    watchers.Remove(path.ToLower());
                }
            }
        }

        void fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            FileModified(e.FullPath);
        }

        #endregion

        #region Handling internal file events - events initiated from with VS

        #region IVsRunningDocTableEvents Members

        public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            if ((grfAttribs & (uint)__VSRDTATTRIB.RDTA_DocDataReloaded) == 0)
                return VSConstants.S_OK;
            string path = PathOfCookie(docCookie);
            if (path == null)
                return VSConstants.S_OK;

            // this event fires in 2 situations:
            if (watchers.ContainsKey(path.ToLower()))
                // when the file is opened in a new window in the Visual Studio
                // it means that the document is not actually changed, but we do not need 
                // to watch for external changes anymore - Visual Studio will do it for us
                removeWatcher(path);
            else
                // when the file is reloaded in an existing Visual Studio window after it was
                // modified externally - i.e. by running SVN update
                FileModified(path);

            return VSConstants.S_OK;
        }

        public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            activeCodeFile = null;
            HandleTextViewChange(null);
            return VSConstants.S_OK;
        }

        public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterSave(uint docCookie)
        {
            string path = PathOfCookie(docCookie);
            if (path == null)
                return VSConstants.S_OK;
            
            notifier.FileModified(path);

            return VSConstants.S_OK;
        }

        private string PathOfCookie(uint docCookie)
        {
            uint pgrfRDTFlags;
            uint pdwReadLocks;
            uint pdwEditLocks;
            string result;
            IVsHierarchy ppHier;
            uint pitemid;
            IntPtr ppunkDocData;

            ErrorHandler.ThrowOnFailure(vsRDT.GetDocumentInfo(
                docCookie,
                out pgrfRDTFlags,
                out pdwReadLocks,
                out pdwEditLocks,
                out result,
                out ppHier,
                out pitemid,
                out ppunkDocData
            ));
            //if (project == (IVsProject)ppHier)
            int found;
            uint itemid;
            VSDOCUMENTPRIORITY[] priority = new VSDOCUMENTPRIORITY[1];
            ErrorHandler.ThrowOnFailure(project.IsDocumentInProject(result, out found, priority, out itemid));
            if (found == 1)
                return result;
            return null;
        }

        public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            IntPtr pCodeWindow;
            Guid riid = typeof(IVsCodeWindow).GUID;

            pFrame.QueryViewInterface(ref riid, out pCodeWindow);
            if (pCodeWindow.ToInt32() == 0)
            {
                //the window focus is set on is not codeWindow,so do not handle textView changes 
                HandleTextViewChange(null);
            }
            else
            {
                activeCodeFile = PathOfCookie(docCookie);
                if (NeedsNotify(activeCodeFile))
                {
                    try
                    {
                        IVsCodeWindow codeWindow = (IVsCodeWindow)Marshal.GetObjectForIUnknown(pCodeWindow);
                        IVsTextView textView = null;
                        codeWindow.GetPrimaryView(out textView);
                        HandleTextViewChange(textView);
                    }
                    finally
                    {
                        Marshal.Release(pCodeWindow);
                    }

                }
                else
                {
                    activeCodeFile = null;
                }
            }
            

            return VSConstants.S_OK;
        }

        public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        #endregion        

        #region IVsRunningDocTableEvents4 Members

        public int OnAfterLastDocumentUnlock(IVsHierarchy pHier, uint itemid, string pszMkDocument, int fClosedWithoutSaving)
        {
            int found;
            VSDOCUMENTPRIORITY[] priority = new VSDOCUMENTPRIORITY[1];
            ErrorHandler.ThrowOnFailure(project.IsDocumentInProject(pszMkDocument, out found, priority, out itemid));

            if (found == 1  && NeedsNotify(pszMkDocument))
                AddWatcher(pszMkDocument);

            return VSConstants.S_OK;
        }

        public int OnAfterSaveAll()
        {
            return VSConstants.S_OK;
        }

        public int OnBeforeFirstDocumentLock(IVsHierarchy pHier, uint itemid, string pszMkDocument)
        {
            return VSConstants.S_OK;
        }

        #endregion
        #region IVsTextLinesEvents Members

        void IVsTextLinesEvents.OnChangeLineAttributes(int iFirstLine, int iLastLine)
        {
        }

        void IVsTextLinesEvents.OnChangeLineText(TextLineChange[] pTextLineChange, int fLast)
        {
            if (updateTimer != null)
                updateTimer.Dispose();
            updateTimer = new Timer(FileModified, activeCodeFile, DELAY, Timeout.Infinite);
        }

        #endregion

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Releases the resources used by the <see cref="Wrapper"/>
        /// </summary>
        public void Dispose()
        // Implements IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private bool disposed = false;
        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Activity"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources. 
        /// </param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            lock (this)
            {
                if (this.disposed)
                    return;
                disposed = true;
            }

            // If disposing equals true, dispose all managed 
            // and unmanaged resources.
            if (disposing)
                DisposeManaged();

            // Call the appropriate methods to clean up 
            // unmanaged resources here.
            // If disposing is false, 
            // only the following code is executed.
            //DisposeUnmanaged();
        }

        /// <summary>
        /// Disposes of the managed resources.
        /// </summary>
        /// <remarks> When overriding this method make sure to call the base DisposeManaged <b>after</b> 
        /// disposing of your resources</remarks>
        protected virtual void DisposeManaged()
        {
            // we need has the flag of 'file manager' is stopped 
            // despite we unsubsrcibe VS events they may be processing 
            if (vsRDT != null)
                vsRDT.UnadviseRunningDocTableEvents(rdtCookie);
            vsRDT = null;

            notifier.Dispose();

            lock (watchers)
            {
                foreach (FileSystemWatcher watcher in watchers.Values)
                    watcher.EnableRaisingEvents = false;

                // if somewhere will be attemp to work with wother then we catch blow up
                // but our algoritm is working the way which don't allow similar case.
                watchers = null;
            }

        }


        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method 
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        /// <summary>
        /// <exclude/>
        /// </summary>
        ~FileManager()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion


        #region IVsTrackProjectDocumentsEvents2 Members (handler of adding/removing documents inside VS)

        public int OnAfterAddDirectoriesEx(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDDIRECTORYFLAGS[] rgFlags)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterAddFilesEx(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDFILEFLAGS[] rgFlags)
        {
            for (int i = 0; i < rgpProjects.Length; i++)
                if (rgpProjects[i] == Project)
                {
                    int upBound = (rgFirstIndices.Length > i + 1) ? rgFirstIndices[i + 1] : rgpszMkDocuments.Length;
                    for (int j = rgFirstIndices[i]; j < upBound; j++)
                        FileAdded(rgpszMkDocuments[j]);
                }

            return VSConstants.S_OK;
        }

        public int OnAfterRemoveDirectories(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEDIRECTORYFLAGS[] rgFlags)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterRemoveFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEFILEFLAGS[] rgFlags)
        {
            for (int i = 0; i < rgpProjects.Length; i++)
                if (rgpProjects[i] == Project)
                {
                    int upBound = (rgFirstIndices.Length > i + 1) ? rgFirstIndices[i + 1] : rgpszMkDocuments.Length;
                    for (int j = rgFirstIndices[i]; j < upBound; j++)
                        FileRemoved(rgpszMkDocuments[j]);
                }

            return VSConstants.S_OK;
        }

        public int OnAfterRenameDirectories(int cProjects, int cDirs, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEDIRECTORYFLAGS[] rgFlags)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterRenameFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEFILEFLAGS[] rgFlags)
        {
            OnAfterRemoveFiles(cProjects, cFiles, rgpProjects, rgFirstIndices, rgszMkOldNames, null);
            OnAfterAddFilesEx(cProjects, cFiles, rgpProjects, rgFirstIndices, rgszMkNewNames, null);
            return VSConstants.S_OK;
        }

        public int OnAfterSccStatusChanged(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, uint[] rgdwSccStatus)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryAddDirectories(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments, VSQUERYADDDIRECTORYFLAGS[] rgFlags, VSQUERYADDDIRECTORYRESULTS[] pSummaryResult, VSQUERYADDDIRECTORYRESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryAddFiles(IVsProject pProject, int cFiles, string[] rgpszMkDocuments, VSQUERYADDFILEFLAGS[] rgFlags, VSQUERYADDFILERESULTS[] pSummaryResult, VSQUERYADDFILERESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryRemoveDirectories(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments, VSQUERYREMOVEDIRECTORYFLAGS[] rgFlags, VSQUERYREMOVEDIRECTORYRESULTS[] pSummaryResult, VSQUERYREMOVEDIRECTORYRESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryRemoveFiles(IVsProject pProject, int cFiles, string[] rgpszMkDocuments, VSQUERYREMOVEFILEFLAGS[] rgFlags, VSQUERYREMOVEFILERESULTS[] pSummaryResult, VSQUERYREMOVEFILERESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryRenameDirectories(IVsProject pProject, int cDirs, string[] rgszMkOldNames, string[] rgszMkNewNames, VSQUERYRENAMEDIRECTORYFLAGS[] rgFlags, VSQUERYRENAMEDIRECTORYRESULTS[] pSummaryResult, VSQUERYRENAMEDIRECTORYRESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        public int OnQueryRenameFiles(IVsProject pProject, int cFiles, string[] rgszMkOldNames, string[] rgszMkNewNames, VSQUERYRENAMEFILEFLAGS[] rgFlags, VSQUERYRENAMEFILERESULTS[] pSummaryResult, VSQUERYRENAMEFILERESULTS[] rgResults)
        {
            return VSConstants.S_OK;
        }

        #endregion


    }
}
