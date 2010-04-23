using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;

namespace FSharp.ProjectExtender.Project.Excluded
{
    [ComVisible(true)]
    abstract class FakeNode : ItemNode, IOleCommandTarget
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


        #region IOleCommandTarget Members

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            if (pguidCmdGroup.Equals(Constants.guidStandardCommandSet2K) && nCmdID == (uint)VSConstants.VSStd2KCmdID.INCLUDEINPROJECT)
                return IncludeItem();

            //if (pguidCmdGroup.Equals(Constants.guidStandardCommandSet97) && nCmdID == (uint)VSConstants.VSStd97CmdID.PropSheetOrProperties)
            //    IncludeItem();

            return VSConstants.S_OK;
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            if (pguidCmdGroup.Equals(Constants.guidStandardCommandSet2K) && prgCmds[0].cmdID == (uint)VSConstants.VSStd2KCmdID.INCLUDEINPROJECT)
                prgCmds[0].cmdf = (uint)OLECMDF.OLECMDF_SUPPORTED | (uint)OLECMDF.OLECMDF_ENABLED;

            return VSConstants.S_OK;
        }

        #endregion

        private int IncludeItem()
        {
            return Items.IncludeItem(this, Path);
        }

    }
}
