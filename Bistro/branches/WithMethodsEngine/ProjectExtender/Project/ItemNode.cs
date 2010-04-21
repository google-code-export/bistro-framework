using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio;
using System.IO;

namespace FSharp.ProjectExtender
{
    public class ItemNode
    {
        ItemList items;
        ItemNode parent;
        string node_key;

        SortedList<string, ItemNode> children = new SortedList<string, ItemNode>();
        Dictionary<uint, int> childrenMap;

        public ItemNode(ItemList items, uint itemId)
            : this(items, itemId, items.GetNodeKey(itemId))
        {
            uint child = items.GetNodeFirstChild(itemId);
            while (child != VSConstants.VSITEMID_NIL)
            {
                ItemNode node = new ItemNode(items, child);
                node.parent = this;
                children.Add(node.node_key, node);
                child = items.GetNodeSibling(child);
            }
            mapChildren();
        }

        public ItemNode(ItemList items, string node_key)
            : this(items, items.GetNextItemID(), node_key)
        { }

        public ItemNode(ItemList items, uint itemId, string node_key)
        {
            this.items = items;
            ItemId = itemId;
            this.node_key = node_key;
            items.Register(this);
        }

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
            parent.mapChildren();
            items.Unregister(ItemId);
        }

        internal void AddChild(uint itemidAdded)
        {
            ItemNode node = new ItemNode(items, itemidAdded);
            node.parent = this;
            children.Add(node.node_key, node);
            mapChildren();
        }

        private void mapChildren()
        {
            if (childrenMap == null)
                childrenMap = new Dictionary<uint, int>(children.Count);
            else
                childrenMap.Clear();
            int i = 0;
            foreach (var item in children)
                childrenMap.Add(item.Value.ItemId, i++);
        }

        internal void Remap()
        {
            parent.children.RemoveAt(parent.childrenMap[ItemId]);
            parent.childrenMap.Remove(ItemId);
            parent.children.Add(node_key, this);
            parent.mapChildren();
        }

        internal void SetShowAll(bool show_all)
        {
            string path = null;
            if (node_key.StartsWith(";"))
                path = Path.GetDirectoryName(node_key.Substring(1));
            else if (node_key.StartsWith("d;"))
                path = node_key.Substring(2);
            if (path != null)
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    if (children.ContainsKey("e;" + file))
                        continue;
                    ItemNode node = new ItemNode(items, "e;" + file);
                    node.parent = this;
                    children.Add(node.node_key, node);
                    mapChildren();
                }
                foreach (var child in children.Values)
                    child.SetShowAll(show_all);
            }
        }
    }
}
