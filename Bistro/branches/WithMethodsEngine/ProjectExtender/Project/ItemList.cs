using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio;
using Microsoft.Build.BuildEngine;
using Microsoft.VisualStudio.Shell.Interop;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FSharp.ProjectExtender
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
    public class ItemList: IVsHierarchyEvents
    {
        ProjectManager project;
        Dictionary<uint, ItemNode> itemMap = new Dictionary<uint, ItemNode>();
        ItemNode root;

        public ItemList(ProjectManager project)
        {
            this.project = project;
            root = new ItemNode(this, VSConstants.VSITEMID_ROOT);
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

        internal string GetNodeKey(uint itemId)
        {
            IVsHierarchy hier = (IVsHierarchy)project;
            object name;
            string fullName;
            ErrorHandler.ThrowOnFailure(hier.GetProperty(itemId, (int)__VSHPROPID.VSHPROPID_Caption, out name));
            ErrorHandler.ThrowOnFailure(hier.GetCanonicalName(itemId,out fullName));

            Guid type;
            try
            {
                ErrorHandler.ThrowOnFailure(hier.GetGuidProperty(itemId, (int)__VSHPROPID.VSHPROPID_TypeGuid, out type));
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
            string sort_order = "";
            if (type == Guid.Empty)
                sort_order = "a";
            else if (type == VSConstants.GUID_ItemType_PhysicalFile)
                sort_order = "e";
            else if (type == VSConstants.GUID_ItemType_PhysicalFolder)
                sort_order = "d";
            else if (type == VSConstants.GUID_ItemType_SubProject)
                sort_order = "b";
            else if (type == VSConstants.GUID_ItemType_VirtualFolder)
                sort_order = "c";
            if (!String.IsNullOrEmpty(fullName))
                return sort_order + ';' + fullName;

            else
                return sort_order + ';' + (string)name;
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
            n.AddChild(itemidAdded);
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
    }
}
