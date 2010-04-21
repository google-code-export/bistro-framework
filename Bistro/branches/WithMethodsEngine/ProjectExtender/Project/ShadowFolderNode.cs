using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio;
using System.IO;
using FSharp.ProjectExtender.Project.Excluded;

namespace FSharp.ProjectExtender.Project
{
    abstract class ShadowFolderNode : ItemNode
    {
        protected ShadowFolderNode(ItemList items, ItemNode parent, uint itemId, ItemNodeType type, string path)
            : base(items, parent, itemId, type, path)
        {
            uint child = items.GetNodeFirstChild(itemId);
            while (child != VSConstants.VSITEMID_NIL)
            {
                CreateChildNode(child);
                child = items.GetNodeSibling(child);
            }
            MapChildren();
        }

        internal override void SetShowAll(bool show_all)
        {
            if (show_all)
            {
                foreach (var file in Directory.GetFiles(Path))
                {
                    if (ChildExists("e;" + file))
                        continue;
                    if (ToBeHidden(file))
                        continue;
                    AddChildNode(new FakeFileNode(Items, this, file));
                }
                MapChildren();
                foreach (var child in this)
                    child.SetShowAll(show_all);
            }
            else
            {
                foreach (var child in new List<ItemNode>(this))
                    if (child is FakeNode)
                        child.Delete();
            }
        }

        protected virtual bool ToBeHidden(string file)
        {
            return false;
        }

    }

    class PhysicalFolderNode : ShadowFolderNode
    {
        public PhysicalFolderNode(ItemList items, ItemNode parent, uint itemId, string path)
            : base(items, parent, itemId, ItemNodeType.PhysicalFolder, path)
        { }

        protected override string SortOrder
        {
            get { return "d"; }
        }
    }

    class VirtualFolderNode : ShadowFolderNode
    {
        public VirtualFolderNode(ItemList items, ItemNode parent, uint itemId, string path)
            : base(items, parent, itemId, ItemNodeType.VirtualFolder, path)
        { }

        protected override string SortOrder
        {
            get { return "c"; }
        }

        internal override void SetShowAll(bool show_all)
        {
        }
    }

    class SubprojectNode : ShadowFolderNode
    {
        public SubprojectNode(ItemList items, ItemNode parent, uint itemId, string path)
            : base(items, parent, itemId, ItemNodeType.SubProject, path)
        { }

        protected override string SortOrder
        {
            get { return "b"; }
        }
    }
}
