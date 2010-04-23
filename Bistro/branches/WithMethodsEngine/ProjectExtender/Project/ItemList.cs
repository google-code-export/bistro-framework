using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.Build.BuildEngine;
using Microsoft.VisualStudio.Shell.Interop;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.VisualStudio.Shell;
using OleConstants = Microsoft.VisualStudio.OLE.Interop.Constants;
using Microsoft.VisualStudio.OLE.Interop;
using FSharp.ProjectExtender.Project.Excluded;

namespace FSharp.ProjectExtender.Project
{
    /// <summary>
    /// Maintains a list of project items 
    /// </summary>
    /// <remarks>
    /// The purpose of this class is to maintain a shadow list of all project items for the project
    /// When the Solution Explorer displays the project tree it traverses the project using
    /// the IVsHierarchy.GetProperty method. The ProjectExtender redirects the GetProperty method calls to 
    /// provide them in the order defined by ItemList rather than the order of F# Project Manager
    /// </remarks>
    class ItemList: IVsHierarchyEvents
    {
        ProjectManager project;
        IVsHierarchy root_hierarchy;
        Dictionary<uint, ItemNode> itemMap = new Dictionary<uint, ItemNode>();
        ItemNode root;

        public ItemList(ProjectManager project)
        {
            this.project = project;
            root_hierarchy = (IVsHierarchy)project;
            root = CreateNode(null, VSConstants.VSITEMID_ROOT);
            //root = new ItemNode(this, VSConstants.VSITEMID_ROOT);
        }

        internal ItemNode CreateNode(uint itemId)
        {
            object parent;
            ErrorHandler.ThrowOnFailure(root_hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_Parent, out parent));
            
            string path;
            ErrorHandler.ThrowOnFailure(root_hierarchy.GetCanonicalName(itemId, out path));
            
            ItemNode parent_node;
            if (itemMap.TryGetValue((uint)parent, out parent_node))
                return new FakeFileNode(this, parent_node, path.ToString());
            return null;
        }

        internal void AddChild(ItemNode itemNode)
        {
            itemNode.Parent.AddChildNode(itemNode);
        }

        public ItemNode CreateNode(ItemNode parent, uint itemId)
        {
            object caption;
            string path;

            switch (get_node_type(itemId))
            {
                case ItemNodeType.Root:
                    ErrorHandler.ThrowOnFailure(root_hierarchy.GetCanonicalName(itemId, out path));
                    return new RootItemNode(this, Path.GetDirectoryName(path));
                case ItemNodeType.PhysicalFolder:
                    ErrorHandler.ThrowOnFailure(root_hierarchy.GetCanonicalName(itemId, out path));
                    return new PhysicalFolderNode(this, parent, itemId, path);
                case ItemNodeType.VirtualFolder:
                    ErrorHandler.ThrowOnFailure(root_hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_Caption, out caption));
                    return new VirtualFolderNode(this, parent, itemId, caption.ToString());
                case ItemNodeType.SubProject:
                    ErrorHandler.ThrowOnFailure(root_hierarchy.GetCanonicalName(itemId, out path));
                    return new SubprojectNode(this, parent, itemId, path);
                case ItemNodeType.Reference:
                    ErrorHandler.ThrowOnFailure(root_hierarchy.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_Caption, out caption));
                    return new ShadowFileNode(this, parent, itemId, caption.ToString());
                case ItemNodeType.PhysicalFile:
                    ErrorHandler.ThrowOnFailure(root_hierarchy.GetCanonicalName(itemId, out path));
                    return new ShadowFileNode(this, parent, itemId, path);
                default:
                    ErrorHandler.ThrowOnFailure(root_hierarchy.GetCanonicalName(itemId, out path));
                    throw new Exception("Unexpected node type for node " + itemId + "(" + path + ")");
            }
        }

        private ItemNodeType get_node_type(uint itemId)
        {
            Guid type;
            try
            {
                ErrorHandler.ThrowOnFailure(root_hierarchy.GetGuidProperty(itemId, (int)__VSHPROPID.VSHPROPID_TypeGuid, out type));
            }
            catch (COMException e)
            {
                // FSharp project returns Guid.Empty as the type guid for reference nodes, which causes the WAP to throw an exception
                var pinfo = e.GetType().GetProperty("HResult", BindingFlags.Instance | BindingFlags.NonPublic);
                if ((int)pinfo.GetValue(e, new object[] { }) == VSConstants.DISP_E_MEMBERNOTFOUND)
                    type = Guid.Empty;
                else
                    throw;
            }

            // set the sort order based on the item type
            if (type == Guid.Empty)
                return ItemNodeType.Reference;
            else if (type == VSConstants.GUID_ItemType_PhysicalFile)
                return ItemNodeType.PhysicalFile;
            else if (type == VSConstants.GUID_ItemType_PhysicalFolder)
                return ItemNodeType.PhysicalFolder;
            else if (type == VSConstants.GUID_ItemType_SubProject)
                return ItemNodeType.SubProject;
            else if (type == VSConstants.GUID_ItemType_VirtualFolder)
                return ItemNodeType.VirtualFolder;
            else if (type == Constants.guidFSharpProject)
                return ItemNodeType.Root;
            return ItemNodeType.Unknown;
        }

        public int GetNextSibling(uint itemId, out object value)
        {
            ItemNode n;
            value = null;
            if (itemMap.TryGetValue(itemId, out n))
            {
                value = n.NextSibling;
                return VSConstants.S_OK;
            }
            else
                return VSConstants.E_INVALIDARG;
        }

        public int GetFirstChild(uint itemId, out object value)
        {
            ItemNode n;
            value = null;
            if (itemMap.TryGetValue(itemId, out n))
            {
                value = n.FirstChild;
                return VSConstants.S_OK;
            }
            else
                return VSConstants.E_INVALIDARG;
        }

        internal uint GetNodeFirstChild(uint itemId)
        {
            return project.GetNodeChild(itemId);
        }

        internal int GetProperty(uint itemId, int propId, out object property)
        {
            ItemNode node;
            if (itemMap.TryGetValue(itemId, out node))
                return node.GetProperty(propId, out property);
            property = null;
            return VSConstants.E_INVALIDARG;
        }

        public const int FakeNodeStart = 0x010000;
        uint nextItemId = FakeNodeStart;

        internal uint GetNextItemID()
        {
            return nextItemId++;
        }

        internal uint GetNodeSibling(uint itemId)
        {
            return project.GetNodeSibling(itemId);
        }

        internal void Register(ItemNode itemNode)
        {
            itemMap.Add(itemNode.ItemId, itemNode);
        }

        internal void Unregister(uint itemId)
        {
            itemMap.Remove(itemId);
        }

        internal void SetShowAll(bool show_all)
        {
            root.SetShowAll(show_all);
        }

        #region IVsHierarchyEvents Members

        int IVsHierarchyEvents.OnInvalidateIcon(IntPtr hicon)
        {
            return VSConstants.S_OK;
        }

        int IVsHierarchyEvents.OnInvalidateItems(uint itemidParent)
        {
            return VSConstants.S_OK;
        }

        int IVsHierarchyEvents.OnItemAdded(uint itemidParent, uint itemidSiblingPrev, uint itemidAdded)
        {
            // for some reason during rename OnItemAdded is called twice - let us ignore the second one
            if (itemMap.ContainsKey(itemidAdded))
                return VSConstants.S_OK;

            ItemNode n;
            if (!itemMap.TryGetValue(itemidParent, out n))
                return VSConstants.E_INVALIDARG;
            n.CreatenMapChildNode(itemidAdded);
            return VSConstants.S_OK;
        }

        int IVsHierarchyEvents.OnItemDeleted(uint itemid)
        {
            ItemNode n;
            if (itemMap.TryGetValue(itemid, out n))
                n.Delete();
            return VSConstants.S_OK;
        }

        int IVsHierarchyEvents.OnItemsAppended(uint itemidParent)
        {
            return VSConstants.S_OK;
        }

        int IVsHierarchyEvents.OnPropertyChanged(uint itemid, int propid, uint flags)
        {

            if (propid == (int)__VSHPROPID.VSHPROPID_Caption)
            {
                ItemNode n;
                if (!itemMap.TryGetValue(itemid, out n))
                    return VSConstants.E_INVALIDARG;

                n.Remap();
                project.InvalidateParentItems(new uint[] {itemid});
            }
            return VSConstants.S_OK;
        }

        #endregion

        static readonly IVsMonitorSelection selectionMonitor = (IVsMonitorSelection)Package.GetGlobalService(typeof(SVsShellMonitorSelection));
        static readonly IVsUIShell shell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));

        internal bool ExecCommand(uint itemId, ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut, out int result)
        {
            result = (int)OleConstants.OLECMDERR_E_NOTSUPPORTED;
            if (itemId < ItemList.FakeNodeStart || itemId == VSConstants.VSITEMID_ROOT)
                return false;

			IntPtr hierarchyPtr = IntPtr.Zero;
			IntPtr selectionContainer = IntPtr.Zero;
            try
            {
                // Get the current project hierarchy, project item, and selection container for the current selection
                // If the selection spans multiple hierachies, hierarchyPtr is Zero
                uint itemid;
                IVsMultiItemSelect multiItemSelect = null;
                ErrorHandler.ThrowOnFailure(selectionMonitor.GetCurrentSelection(out hierarchyPtr, out itemid, out multiItemSelect, out selectionContainer));
                // We only care if there are one ore more nodes selected in the tree
                if (itemid != VSConstants.VSITEMID_NIL && hierarchyPtr != IntPtr.Zero)
                {
                    IVsHierarchy hierarchy = Marshal.GetObjectForIUnknown(hierarchyPtr) as IVsHierarchy;

                    if (itemid != VSConstants.VSITEMID_SELECTION)
                    {
                        // This is a single selection. Compare hirarchy with our hierarchy and get node from itemid
                        ItemNode node;
                        if (itemMap.TryGetValue(itemid, out node))
                        {
                            if (pguidCmdGroup == VsMenus.guidVsUIHierarchyWindowCmds)
                            {
                                switch (nCmdID)
                                {
                                    case (uint)VSConstants.VsUIHierarchyWindowCmdIds.UIHWCMDID_RightClick:
                                        // The UIHWCMDID_RightClick is what tells an IVsUIHierarchy in a UIHierarchyWindow 
                                        // to put up the context menu.  Since the mouse may have moved between the 
                                        // mouse down and the mouse up, GetCursorPos won't tell you the right place 
                                        // to put the context menu (especially if it came through the keyboard).  
                                        // So we pack the proper menu position into pvaIn by
                                        // memcpy'ing a POINTS struct into the VT_UI4 part of the pvaIn variant.  The
                                        // code to unpack it looks like this:
                                        //			ULONG ulPts = V_UI4(pvaIn);
                                        //			POINTS pts;
                                        //			memcpy((void*)&pts, &ulPts, sizeof(POINTS));
                                        // You then pass that POINTS into DisplayContextMenu.

                                        object variant = Marshal.GetObjectForNativeVariant(pvaIn);
                                        UInt32 pointsAsUint = (UInt32)variant;
                                        short x = (short)(pointsAsUint & 0x0000ffff);
                                        short y = (short)((pointsAsUint & 0xffff0000) / 0x10000);

                                        POINTS[] pnts = new POINTS[1];
                                        pnts[0].x = x;
                                        pnts[0].y = y;
                                        Guid menu = VsMenus.guidSHLMainMenu;// Constants.guidProjectExtenderCmdSet;
                                        result = shell.ShowContextMenu(0, ref menu, VsMenus.IDM_VS_CTXT_ITEMNODE, pnts, (Microsoft.VisualStudio.OLE.Interop.IOleCommandTarget)node);
                                        //var rc = shell.ShowContextMenu(0, ref pguidCmdGroup, (int)Constants.cmdidExcludedCTXMenu, pnts, (Microsoft.VisualStudio.OLE.Interop.IOleCommandTarget)node);
                                        return true;
                                    default:
                                        return false;
                                }
                            }
                        }
                    }
                    else if (multiItemSelect != null)
                    {
                        //// This is a multiple item selection.

                        ////Get number of items selected and also determine if the items are located in more than one hierarchy
                        //uint numberOfSelectedItems;
                        //int isSingleHierarchyInt;
                        //ErrorHandler.ThrowOnFailure(multiItemSelect.GetSelectionInfo(out numberOfSelectedItems, out isSingleHierarchyInt));
                        //bool isSingleHierarchy = (isSingleHierarchyInt != 0);

                        //// Now loop all selected items and add to the list only those that are selected within this hierarchy
                        //if (!isSingleHierarchy || (isSingleHierarchy && Utilities.IsSameComObject(this, hierarchy)))
                        //{
                        //    Debug.Assert(numberOfSelectedItems > 0, "Bad number of selected itemd");
                        //    VSITEMSELECTION[] vsItemSelections = new VSITEMSELECTION[numberOfSelectedItems];
                        //    uint flags = (isSingleHierarchy) ? (uint)__VSGSIFLAGS.GSI_fOmitHierPtrs : 0;
                        //    ErrorHandler.ThrowOnFailure(multiItemSelect.GetSelectedItems(flags, numberOfSelectedItems, vsItemSelections));
                        //    foreach (VSITEMSELECTION vsItemSelection in vsItemSelections)
                        //    {
                        //        if (isSingleHierarchy || Utilities.IsSameComObject(this, vsItemSelection.pHier))
                        //        {
                        //            HierarchyNode node = this.NodeFromItemId(vsItemSelection.itemid);
                        //            if (node != null)
                        //            {
                        //                selectedNodes.Add(node);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
            }
            finally
            {
                if (hierarchyPtr != IntPtr.Zero)
                {
                    Marshal.Release(hierarchyPtr);
                }
                if (selectionContainer != IntPtr.Zero)
                {
                    Marshal.Release(selectionContainer);
                }
            }
            return false;
        }

        internal int QueryStatusCommand(uint itemId, ref Guid pguidCmdGroup, uint cCmds, Microsoft.VisualStudio.OLE.Interop.OLECMD[] prgCmds, IntPtr pCmdText)
        {
            prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_SUPPORTED | (uint)OLECMDF.OLECMDF_ENABLED;
            return VSConstants.S_OK;
        }

        internal int IncludeItem(ItemNode node, string Path)
        {
            node.Delete();
            return project.AddItem(node.Parent.ItemId, Path);
        }
    }

    public enum ItemNodeType
    {
        Root, // 0
        Reference, // 1
        SubProject, // 2
        VirtualFolder, // 3
        PhysicalFolder, // 4
        PhysicalFile, // 5
        ExcludedFolder, //4
        ExcludedFile,  // 5
        Unknown
    }
}
