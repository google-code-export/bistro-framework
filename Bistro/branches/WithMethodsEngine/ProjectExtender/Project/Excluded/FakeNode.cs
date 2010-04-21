using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;

namespace FSharp.ProjectExtender.Project.Excluded
{
    abstract class FakeNode : ItemNode
    {
        protected FakeNode(ItemList items, ItemNode parent, ItemNodeType type, string path)
            : base(items, parent, items.GetNextItemID(), type, path)
        { }

        internal override int GetProperty(int propId, out object property)
        {
            switch ((__VSHPROPID)propId)
            {
                case __VSHPROPID.VSHPROPID_FirstChild:
                case __VSHPROPID.VSHPROPID_FirstVisibleChild:
                    property = FirstChild;
                    return VSConstants.S_OK;

                case __VSHPROPID.VSHPROPID_NextSibling:
                case __VSHPROPID.VSHPROPID_NextVisibleSibling:
                    property = NextSibling;
                    return VSConstants.S_OK;

                case __VSHPROPID.VSHPROPID_Expandable:
                    property = false;
                    return VSConstants.S_OK;

                case __VSHPROPID.VSHPROPID_Caption:
                    property = System.IO.Path.GetFileName(Path);
                    return VSConstants.S_OK;

                case __VSHPROPID.VSHPROPID_IconIndex:
                    property = ImageIndex;
                    return VSConstants.S_OK;

                default:
                    break;
            }
            property = null;
            return VSConstants.E_INVALIDARG;
        }

        protected abstract int ImageIndex { get; }

    }
}
