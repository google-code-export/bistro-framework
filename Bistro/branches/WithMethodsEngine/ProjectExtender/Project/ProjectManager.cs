using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Flavor;
using Microsoft.VisualStudio;

using IOLEServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using ShellConstants = Microsoft.VisualStudio.Shell.Interop.Constants;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using System.ComponentModel.Design;
using Microsoft.Build.BuildEngine;
using System.IO;
using FSharp.ProjectExtender.Project;

namespace FSharp.ProjectExtender
{
    [ComVisible(true)]
    public class ProjectManager : FlavoredProjectBase, IProjectManager, IOleCommandTarget, IVsTrackProjectDocumentsEvents2
    {

        public ProjectManager() : base()
        { }

        /// <summary>
        /// Sets the service provider from which to access the services. 
        /// </summary>
        /// <param name="site">An instance to an Microsoft.VisualStudio.OLE.Interop object</param>
        /// <returns>A success or failure value.</returns>
        public int SetSite(Microsoft.VisualStudio.OLE.Interop.IServiceProvider site)
        {
            serviceProvider = new ServiceProvider(site);
            return VSConstants.S_OK;
        }

        uint hierarchy_event_cookie = (uint)ShellConstants.VSCOOKIE_NIL;
        uint document_tracker_cookie = (uint)ShellConstants.VSCOOKIE_NIL;
        private ItemList itemList;
        Microsoft.VisualStudio.FSharp.ProjectSystem.ProjectNode FSProjectManager;

        protected override void OnAggregationComplete()
        {
            base.OnAggregationComplete();

            FSProjectManager = getFSharpProjectNode(innerProject);
            BuildManager = new MSBuildManager(FSProjectManager.BuildProject);

            itemList = new ItemList(this);
            hierarchy_event_cookie = AdviseHierarchyEvents(itemList);
            IVsTrackProjectDocuments2 documentTracker = (IVsTrackProjectDocuments2)Package.GetGlobalService(typeof(SVsTrackProjectDocuments));
            ErrorHandler.ThrowOnFailure(documentTracker.AdviseTrackProjectDocumentsEvents(this, out document_tracker_cookie));
        }

        internal static Microsoft.VisualStudio.FSharp.ProjectSystem.ProjectNode getFSharpProjectNode(IVsProject root)
        {
            IOLEServiceProvider sp;
            ErrorHandler.ThrowOnFailure(root.GetItemContext(VSConstants.VSITEMID_ROOT, out sp));

            IntPtr objPtr;
            Guid hierGuid = typeof(VSLangProj.VSProject).GUID;
            Guid UNKguid = NativeMethods.IID_IUnknown;
            ErrorHandler.ThrowOnFailure(sp.QueryService(ref hierGuid, ref UNKguid, out objPtr));

            var OAVSProject = (VSLangProj.VSProject)Marshal.GetObjectForIUnknown(objPtr);
            var OAProject = (Microsoft.VisualStudio.FSharp.ProjectSystem.Automation.OAProject)OAVSProject.Project;
            return OAProject.Project;
        }

        protected override int ExecCommand(uint itemId, ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            int result;
            if (itemList.ExecCommand(itemId, ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut, out result))
                return result;
            
            return base.ExecCommand(itemId, ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        bool renaimng_in_progress = false;
        protected override int GetProperty(uint itemId, int propId, out object property)
        {
            if (itemId != VSConstants.VSITEMID_ROOT && itemId >= ItemList.FakeNodeStart)
                return itemList.GetProperty(itemId, propId, out property);

            if (!renaimng_in_progress)
                switch ((__VSHPROPID)propId)
                {
                    case __VSHPROPID.VSHPROPID_FirstChild:
                    case __VSHPROPID.VSHPROPID_FirstVisibleChild:
                        return itemList.GetFirstChild(itemId, out property);
                    case __VSHPROPID.VSHPROPID_NextSibling:
                    case __VSHPROPID.VSHPROPID_NextVisibleSibling:
                        return itemList.GetNextSibling(itemId, out property);
                    default:
                        break;
                }

            int result = base.GetProperty(itemId, propId, out property);
            if (result != VSConstants.S_OK)
                return result;

            if (itemId == VSConstants.VSITEMID_ROOT)
            {
                switch ((__VSHPROPID2)propId)
                {
                    case __VSHPROPID2.VSHPROPID_PropertyPagesCLSIDList:
                        {
                            //Add the CompileOrder property page.
                            var properties = new List<string>(property.ToString().Split(';'));
                            properties.Add(typeof(Page).GUID.ToString("B"));
                            property = properties.Aggregate("", (a, next) => a + ';' + next).Substring(1);
                            return VSConstants.S_OK;
                        }
                    case __VSHPROPID2.VSHPROPID_PriorityPropertyPagesCLSIDList:
                        {
                            // set the order for the project property pages
                            var properties = new List<string>(property.ToString().Split(';'));
                            properties.Insert(1, typeof(Page).GUID.ToString("B"));
                            property = properties.Aggregate("", (a, next) => a + ';' + next).Substring(1);
                            return VSConstants.S_OK;
                        }
                    default:
                        break;
                }
            }
            return result;
        }

        internal uint GetNodeChild(uint itemId)
        {
            object result;
            ErrorHandler.ThrowOnFailure(base.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_FirstChild, out result));
            return (uint)(int)result;
        }

        internal uint GetNodeSibling(uint itemId)
        {
            object result;
            ErrorHandler.ThrowOnFailure(base.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_NextSibling, out result));
            return (uint)(int)result;
        }

        protected override void SetInnerProject(IntPtr innerIUnknown)
        {
            base.SetInnerProject(innerIUnknown);
            innerTarget = (IOleCommandTarget)Marshal.GetObjectForIUnknown(innerIUnknown);
            innerProject = (IVsProject)innerTarget;
        }
        IOleCommandTarget innerTarget;
        IVsProject innerProject;

        protected override void Close()
        {
            if (hierarchy_event_cookie != (uint)ShellConstants.VSCOOKIE_NIL)
                UnadviseHierarchyEvents(hierarchy_event_cookie);
            base.Close();
        }

        internal string GetMetadata(uint itemId, string property)
        {
            object browseObject;
            ErrorHandler.ThrowOnFailure(base.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_BrowseObject, out browseObject));
            return (string)browseObject.GetType().GetMethod("GetMetadata").Invoke(browseObject, new object[] { property });
        }

        internal string SetMetadata(uint itemId, string property, string value)
        {
            object browseObject;
            ErrorHandler.ThrowOnFailure(base.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_BrowseObject, out browseObject));
            return (string)browseObject.GetType().GetMethod("SetMetadata").Invoke(browseObject, new object[] { property, value });
        }

        internal void InvalidateParentItems(IEnumerable<uint> itemIds)
        {
            var updates = new Dictionary<Microsoft.VisualStudio.FSharp.ProjectSystem.HierarchyNode, Microsoft.VisualStudio.FSharp.ProjectSystem.HierarchyNode>(); 
            foreach (var itemId in itemIds)
            {
                var hierarchyNode = FSProjectManager.NodeFromItemId(itemId);
                updates[hierarchyNode.Parent] = hierarchyNode;
            }

            uint lastItemId = VSConstants.VSITEMID_NIL;
            foreach (var item in updates)
            {
                item.Value.OnInvalidateItems(item.Key);
                lastItemId = item.Value.ID;
            }

            if (lastItemId != VSConstants.VSITEMID_NIL)
            {
                IVsUIShell shell = serviceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;

                object pvar = null;
                IVsWindowFrame frame = null;
                Guid persistenceSlot = new Guid(EnvDTE.Constants.vsWindowKindSolutionExplorer);

                ErrorHandler.ThrowOnFailure(shell.FindToolWindow(0, ref persistenceSlot, out frame));
                ErrorHandler.ThrowOnFailure(frame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out pvar));

                if (pvar != null)
                    ErrorHandler.ThrowOnFailure(((IVsUIHierarchyWindow)pvar).ExpandItem(this, lastItemId, EXPANDFLAGS.EXPF_SelectItem));

            }
        }

        private void InvalidateParentItems(string[] oldFileNames, string[] newFileNames)
        {
            int pfFound;
            VSDOCUMENTPRIORITY[] pdwPriority = new VSDOCUMENTPRIORITY[1];
            uint pItemid;
            List<uint> itemIds = new List<uint>();
            for (int i = 0; i < newFileNames.Length; i++)
            {
                if (Path.GetFileName(newFileNames[i]) == Path.GetFileName(oldFileNames[i]))
                    continue;
                ErrorHandler.ThrowOnFailure(innerProject.IsDocumentInProject(newFileNames[i], out pfFound, pdwPriority, out pItemid));
                if (pfFound == 0)
                    continue;
                itemIds.Add(pItemid);
            }
            InvalidateParentItems(itemIds);
        }

        private bool show_all = false;
        #region IProjectManager Members

        public MSBuildManager BuildManager { get; private set; }
        
        public void FlipShowAll()
        {
            show_all = !show_all;
            itemList.SetShowAll(show_all);
            // as kludgy as it looks this is the only way I found to force the
            // refresh of the solution explorer window
            FSProjectManager.FirstChild.OnInvalidateItems(FSProjectManager);
        }

        #endregion

        #region IOleCommandTarget Members

        int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            ItemNode itemNode = null;
            if (pguidCmdGroup.Equals(Constants.guidStandardCommandSet2K) && nCmdID == (uint)VSConstants.VSStd2KCmdID.EXCLUDEFROMPROJECT && show_all)
                itemNode = itemList.CreateNode();

            int result = innerTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            if (itemNode != null)
            {
                itemList.AddChild(itemNode);
            }

            // In certain situations the F# project manager throws an exception while adding files
            // to subdirectories. We are lucky that this is happening after all the job of adding the file
            // to the project is completed. Whereas we are handling the file ordering ourselves anyway
            // all we need to do is to supress the error message
            if ((uint)result == 0x80131509) // Invalid Operation Exception
            {
                System.Diagnostics.Debug.Write("\n***** Supressing COM exception *****\n");
                System.Diagnostics.Debug.Write(Marshal.GetExceptionForHR(result));
                System.Diagnostics.Debug.Write("\n***** Supressed *****\n");
                return VSConstants.S_OK;
            }
            if ((uint)result == 0x80004003) // Null Pointer Exception
            {
                System.Diagnostics.Debug.Write("\n***** Supressing COM exception *****\n");
                System.Diagnostics.Debug.Write(Marshal.GetExceptionForHR(result));
                System.Diagnostics.Debug.Write("\n***** Supressed *****\n");
                return VSConstants.S_OK;
            }
            return result;
        }

        int IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {

            if (pguidCmdGroup.Equals(Constants.guidProjectExtenderCmdSet) && prgCmds[0].cmdID == (uint)Constants.cmdidProjectShowAll)
            {
                prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_SUPPORTED | (uint)OLECMDF.OLECMDF_ENABLED;
                if (show_all)
                    prgCmds[0].cmdf |= (uint)OLECMDF.OLECMDF_LATCHED;
                return VSConstants.S_OK;
            }

            int result = innerTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
            if (result != VSConstants.S_OK)
                return result;

            // hide the FSharp project commands on the file nodes (moveup movedown, add above, add below)
            if (pguidCmdGroup.Equals(Constants.guidFSharpProjectCmdSet))
                prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_SUPPORTED | (uint)OLECMDF.OLECMDF_INVISIBLE;

            // show the Add new folder command on the project node
            if (pguidCmdGroup.Equals(Constants.guidStandardCommandSet97) && prgCmds[0].cmdID == (uint)VSConstants.VSStd97CmdID.NewFolder)
                prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_SUPPORTED | (uint)OLECMDF.OLECMDF_ENABLED;

            return VSConstants.S_OK;
        }

        #endregion

        #region IVsTrackProjectDocumentsEvents2 Members

        public int OnAfterAddDirectoriesEx(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDDIRECTORYFLAGS[] rgFlags)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterAddFilesEx(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSADDFILEFLAGS[] rgFlags)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterRemoveDirectories(int cProjects, int cDirectories, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEDIRECTORYFLAGS[] rgFlags)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterRemoveFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgpszMkDocuments, VSREMOVEFILEFLAGS[] rgFlags)
        {
            return VSConstants.S_OK;
        }

        public int OnAfterRenameDirectories(int cProjects, int cDirs, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEDIRECTORYFLAGS[] rgFlags)
        {
            renaimng_in_progress = false;
            return VSConstants.S_OK;
        }

        public int OnAfterRenameFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEFILEFLAGS[] rgFlags)
        {
            renaimng_in_progress = false;
            InvalidateParentItems(rgszMkOldNames, rgszMkNewNames);
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
            renaimng_in_progress = true;
            return VSConstants.S_OK;
        }

        public int OnQueryRenameFiles(IVsProject pProject, int cFiles, string[] rgszMkOldNames, string[] rgszMkNewNames, VSQUERYRENAMEFILEFLAGS[] rgFlags, VSQUERYRENAMEFILERESULTS[] pSummaryResult, VSQUERYRENAMEFILERESULTS[] rgResults)
        {
            renaimng_in_progress = true;
            return VSConstants.S_OK;
        }

        #endregion

        internal int AddItem(uint parentID, string Path)
        {
            Microsoft.VisualStudio.FSharp.ProjectSystem.HierarchyNode parent;
            if (parentID == VSConstants.VSITEMID_ROOT)
                parent = FSProjectManager;
            else
                parent = FSProjectManager.NodeFromItemId(parentID);

            var node = FSProjectManager.AddNewFileNodeToHierarchy(parent, Path);

            InvalidateParentItems(new uint[] { node.ID });

            return VSConstants.S_OK;
        }
    }
}
