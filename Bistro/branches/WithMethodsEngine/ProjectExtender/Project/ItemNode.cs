using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio;
using System.IO;
using Microsoft.VisualStudio.Shell.Interop;

namespace FSharp.ProjectExtender.Project
{
    abstract class ItemNode: IEnumerable<ItemNode>
    {
        protected ItemNode(ItemList items, ItemNode parent, uint itemId, ItemNodeType type, string path)
        {
            Items = items;
            this.parent = parent;
            ItemId = itemId;
            this.type = type;
            Path = path;
            items.Register(this);
        }

        protected ItemList Items { get; private set; }
        ItemNode parent;
        ItemNodeType type;
        protected string Path { get; private set; }
        string sort_key { get { return SortOrder + ';' + Path; } }

        protected void CreateChildNode(uint child)
        {
            AddChildNode(Items.CreateNode(this, child));
        }

        protected void AddChildNode(ItemNode child)
        {
            children.Add(child.sort_key, child);
        }

        protected bool ChildExists(string key)
        {
            return children.ContainsKey(key);
        }

        internal void CreatenMapChildNode(uint itemidAdded)
        {
            CreateChildNode(itemidAdded);
            MapChildren();
        }

        protected abstract string SortOrder { get; }

        protected void MapChildren()
        {
            if (childrenMap == null)
                childrenMap = new Dictionary<uint, int>(children.Count);
            else
                childrenMap.Clear();
            int i = 0;
            foreach (var item in children)
                childrenMap.Add(item.Value.ItemId, i++);
        }

        SortedList<string, ItemNode> children = new SortedList<string, ItemNode>();
        Dictionary<uint, int> childrenMap;

        public uint ItemId { get; private set; }

        public uint NextSibling
        {
            get
            {
                if (parent == null)
                    return VSConstants.VSITEMID_NIL;
                int index = parent.childrenMap[ItemId];
                if (index + 1 < parent.children.Count)
                    return parent.children.Values[index + 1].ItemId;
                return VSConstants.VSITEMID_NIL;
            }
        }

        public uint FirstChild
        {
            get
            {
                if (children.Count == 0)
                    return VSConstants.VSITEMID_NIL;
                return children.Values[0].ItemId;
            }
        }

        internal void Delete()
        {
            parent.children.RemoveAt(parent.childrenMap[ItemId]);
            parent.childrenMap.Remove(ItemId);
            parent.MapChildren();
            Items.Unregister(ItemId);
        }

        internal void Remap()
        {
            parent.children.RemoveAt(parent.childrenMap[ItemId]);
            parent.childrenMap.Remove(ItemId);
            parent.children.Add(sort_key, this);
            parent.MapChildren();
        }

        internal virtual void SetShowAll(bool show_all)
        {
        }

        internal virtual int GetProperty(int propId, out object property)
        {
            // this method should never be called for shadow nodes
            property = null;
            return VSConstants.E_INVALIDARG;
        }


        #region IEnumerable<ItemNode> Members

        public IEnumerator<ItemNode> GetEnumerator()
        {
            return children.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return children.Values.GetEnumerator();
        }

        #endregion
    }
}
