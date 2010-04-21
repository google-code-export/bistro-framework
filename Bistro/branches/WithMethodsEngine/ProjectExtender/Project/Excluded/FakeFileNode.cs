using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FSharp.ProjectExtender.Project.Excluded
{
    class FakeFileNode : FakeNode
    {
        public FakeFileNode(ItemList items, ItemNode parent, string path)
            : base(items, parent, ItemNodeType.ExcludedFile, path)
        { }

        protected override string SortOrder
        {
            get { return "e"; }
        }

        protected override int ImageIndex
        {
            get { return (int)Constants.ImageName.ExcludedFile; }
        }

    }
}
