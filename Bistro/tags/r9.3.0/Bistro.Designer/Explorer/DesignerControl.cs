using System.Windows.Forms;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Diagnostics;
using Bistro.MethodsEngine;
using Bistro.MethodsEngine.Reflection;

namespace Bistro.Designer.Explorer
{
    public partial class DesignerControl : UserControl
    {
        public DesignerControl()
        {
            InitializeComponent();
            propertiesTree.ImageList = imageList1;
            bindingTree.ImageList = imageList1;
            
        }
        public TreeView BindingTree
        {
            get { return bindingTree; }
        }
        public TreeView PropertiesTree
        {
            get { return propertiesTree; }
        }
        /// <summary> 
        /// Let this control process the mnemonics.
        /// </summary>
        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogChar(char charCode)
        {
            // If we're the top-level form or control, we need to do the mnemonic handling
            if (charCode != ' ' && ProcessMnemonic(charCode))
            {
                return true;
            }
            return base.ProcessDialogChar(charCode);
        }

        /// <summary>
        /// Enable the IME status handling for this control.
        /// </summary>
        protected override bool CanEnableIme
        {
            get
            {
                return true;
            }
        }
        #region Private members
        private NodeObject curObject;
        private enum NodeObject
        {
            Project,
            UrlPattern,
            Controller,
            Resource
        }


        private void FillPropertiesTree()
        {
            propertiesTree.Nodes.Clear();
        }
        private void BindingTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
        }
        private void On_Click(object sender, System.EventArgs e)
        {
        }
        
        #endregion




    }
}
