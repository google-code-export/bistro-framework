﻿using System;
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

namespace FSharp.ProjectExtender
{
    [ComVisible(true)]
    public class ProjectManager : FlavoredProjectBase, IProjectManager, IOleCommandTarget, IVsTrackProjectDocumentsEvents2
                                    
    {

        public ProjectManager()
            : base()
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
        private string fileName;
        private ItemList itemList;
        protected override void InitializeForOuter(string fileName, string location, string name, uint flags, ref Guid guidProject, out bool cancel)
        {
            //this.fileName = fileName;
            base.InitializeForOuter(fileName, location, name, flags, ref guidProject, out cancel);
        }

        protected override void OnAggregationComplete()
        {
            base.OnAggregationComplete();
            this.GetCanonicalName(VSConstants.VSITEMID_ROOT, out fileName);
            BuildManager = new MSBuildManager(fileName);
            itemList = new ItemList(this);
            hierarchy_event_cookie = AdviseHierarchyEvents(itemList);
            IVsTrackProjectDocuments2 documentTracker = (IVsTrackProjectDocuments2)Package.GetGlobalService(typeof(SVsTrackProjectDocuments));
            ErrorHandler.ThrowOnFailure(documentTracker.AdviseTrackProjectDocumentsEvents(this, out document_tracker_cookie));
        }
        bool renaimng_in_progress = false;
        protected override int GetProperty(uint itemId, int propId, out object property)
        {
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

        private void InvalidateParentItems(List<uint> itemIds)
        {
            var updates = new Dictionary<Microsoft.VisualStudio.FSharp.ProjectSystem.HierarchyNode, Microsoft.VisualStudio.FSharp.ProjectSystem.HierarchyNode>(); 
            foreach (var itemId in itemIds)
            {
                IOLEServiceProvider sp;
                ErrorHandler.ThrowOnFailure(innerProject.GetItemContext(itemId, out sp));

                IntPtr objPtr;
                Guid hierGuid = typeof(EnvDTE.ProjectItem).GUID;
                Guid UNKguid = Hill30Inc.ProjectExtender.ProjectBase.NativeMethods.IID_IUnknown;
                ErrorHandler.ThrowOnFailure(sp.QueryService(ref hierGuid, ref UNKguid, out objPtr));

                EnvDTE.ProjectItem projectItem = (EnvDTE.ProjectItem)Marshal.GetObjectForIUnknown(objPtr);
                var hierNode = (Microsoft.VisualStudio.FSharp.ProjectSystem.HierarchyNode)projectItem.Object;
                updates[hierNode.Parent] = hierNode;
            }

            uint lastItemId = VSConstants.VSITEMID_NIL;
            foreach (var item in updates)
            {
                item.Value.OnInvalidateItems(item.Key);
                lastItemId = item.Value.ID;
            }

            if (lastItemId != VSConstants.VSITEMID_NIL)
            {
                IVsUIHierarchyWindow uiWindow = Hill30Inc.ProjectExtender.ProjectBase.UIHierarchyUtilities.GetUIHierarchyWindow(serviceProvider, new Guid(EnvDTE.Constants.vsWindowKindSolutionExplorer));
                ErrorHandler.ThrowOnFailure(uiWindow.ExpandItem(this, lastItemId, EXPANDFLAGS.EXPF_SelectItem));
            }
        }

        void InvalidateParentItems(IEnumerable<string> fileNames)
        {
            int pfFound;
            VSDOCUMENTPRIORITY[] pdwPriority = new VSDOCUMENTPRIORITY[1];
            uint pItemid;
            List<uint> itemIds = new List<uint>();
            foreach (var fileName in fileNames)
            {
                ErrorHandler.ThrowOnFailure(innerProject.IsDocumentInProject(fileName, out pfFound, pdwPriority, out pItemid));
                if (pfFound == 0)
                    continue;
                itemIds.Add(pItemid);
            }
            InvalidateParentItems(itemIds);
        }

        #region IProjectManager Members

        public MSBuildManager BuildManager { get; private set; }
        #endregion

        #region IOleCommandTarget Members

        int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            int result = innerTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
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

            int result = innerTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
            if (result != VSConstants.S_OK)
                return result;

            // hide the FSharp project commands on the file nodes (moveup movedown, add above, add below)
            if (pguidCmdGroup.Equals(Constants.guidFSharpProjectCmdSet))
                prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_SUPPORTED | (uint)OLECMDF.OLECMDF_INVISIBLE;

            // show the Add new folder command on the project node
            if (pguidCmdGroup.Equals(Constants.guidStandardCommandSet97) && prgCmds[0].cmdID == 245)
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
            InvalidateParentItems(rgszMkNewNames);
            return VSConstants.S_OK;
        }

        public int OnAfterRenameFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices, string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEFILEFLAGS[] rgFlags)
        {
            renaimng_in_progress = false;
            InvalidateParentItems(rgszMkNewNames);
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
    }
}