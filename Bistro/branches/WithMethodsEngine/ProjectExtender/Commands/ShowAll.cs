using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Runtime.InteropServices;

namespace FSharp.ProjectExtender.Commands
{
    public class ShowAll : ProjectCTXCommand, IVsSelectionEvents, IDisposable
    {
        uint pdwCookie = VSConstants.VSCOOKIE_NIL;

        public ShowAll()
            : base(Execute, new CommandID(Constants.guidProjectExtenderCmdSet, (int)Constants.cmdidProjectShowAll))
        {
            BeforeQueryStatus += new EventHandler(QueryStatus);
            selectionMonitor.AdviseSelectionEvents(this, out pdwCookie);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void QueryStatus(object sender, EventArgs e)
        {
            //Visible = get_current_project() is IProjectManager;
        }

        private static void Execute(object sender, EventArgs e)
        {
            var project = get_current_project();
            if (project != null)
                ((IProjectManager)project).FlipShowAll();
        }

        #region IVsSelectionEvents Members

        public int OnCmdUIContextChanged(uint dwCmdUICookie, int fActive)
        {
            return VSConstants.S_OK;
        }

        public int OnElementValueChanged(uint elementid, object varValueOld, object varValueNew)
        {
            return VSConstants.S_OK;
        }

        public int OnSelectionChanged(IVsHierarchy pHierOld, uint itemidOld, IVsMultiItemSelect pMISOld, ISelectionContainer pSCOld, IVsHierarchy pHierNew, uint itemidNew, IVsMultiItemSelect pMISNew, ISelectionContainer pSCNew)
        {
            Enabled = (get_current_project() is IProjectManager);
            return VSConstants.S_OK;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (selectionMonitor != null && pdwCookie != VSConstants.VSCOOKIE_NIL)
                selectionMonitor.UnadviseSelectionEvents(pdwCookie);
        }

        #endregion
    }
}
