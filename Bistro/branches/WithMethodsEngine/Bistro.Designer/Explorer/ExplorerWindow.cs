using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Flavor;
//using Microsoft.Build.BuildEngine;

using Bistro;
using Bistro.Configuration;
using Bistro.Configuration.Logging;
using Bistro.Controllers;
using Bistro.MethodsEngine;
using Bistro.MethodsEngine.Reflection;
using Bistro.MethodsEngine.Subsets;
using Bistro.Designer.Projects;

namespace Bistro.Designer.Explorer
{
    //key=ctrlName,value={key=attribute,value=parameters' values}
    using ControllersTable = Dictionary<string, Dictionary<string, List<string>>>;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsWindowPane interface.
    /// </summary>
    [Guid("f9395d80-5ace-41c2-8c02-4be3582a6750")]
    public class ExplorerWindow : ToolWindowPane
    {
        // This is the user control hosted by the tool window; it is exposed to the base class 
        // using the Window property. Note that, even if this class implements IDispose, we are
        // not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
        // the object returned by the Window property.
        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public ExplorerWindow() :
            base(null)
        {
            this.Caption = Resources.ToolWindowTitle;
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            control = new DesignerControl();
            
        }
        public void NotifyRename(object sender, UpdateEventArgs e)
        {
            if (e.NewNode.oldtext != null)
            {
                TreeNode[] nodesToUpdate = control.BindingTree.Nodes.Find(e.NewNode.oldtext, e.NewNode.parent != null);
                nodesToUpdate[0].Tag = e.NewNode.tag;
                nodesToUpdate[0].Text = e.NewNode.newtext;
                nodesToUpdate[0].Name = e.NewNode.newtext;
            }
            else if (null == e.NewNode.parent)
            {
                TreeNode node = new TreeNode(e.NewNode.newtext);
                node.Name = e.NewNode.newtext;
                control.BindingTree.Nodes.Add(node);
            }
        }
        public void NotifyRemove(object sender, UpdateEventArgs e)
        {
            TreeNode[] nodesToUpdate = control.BindingTree.Nodes.Find(e.NewNode.oldtext, true);
            nodesToUpdate[0].Remove();
        }
        public void NotifyInsert(object sender, UpdateEventArgs e)
        {
            TreeNode[] nodesToUpdate = control.BindingTree.Nodes.Find(e.NewNode.parent, true);
            TreeNode node = new TreeNode();
            node.Text = e.NewNode.newtext;
            node.Tag = e.NewNode.tag;
            node.Name = e.NewNode.newtext;
            nodesToUpdate[0].Nodes.Add(node);
        }
        public DesignerControl Control
        {
            get { return control; }
        }
        /// <summary>
        /// This property returns the handle to the user control that should
        /// be hosted in the Tool Window.
        /// </summary>
        override public IWin32Window Window
        {
            get { return (IWin32Window)control; }
        }
        private DesignerControl control;

      
    }
}
