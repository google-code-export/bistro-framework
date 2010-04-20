using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

namespace FSharp.ProjectExtender.Commands
{
    public class ProjectCTXCommand : OleMenuCommand
    {
        protected static readonly IVsMonitorSelection selectionMonitor = (IVsMonitorSelection)Package.GetGlobalService(typeof(SVsShellMonitorSelection));

        private static IVsTrackSelectionEx selectionTracker = get_selectionTracker();

        private static IVsTrackSelectionEx get_selectionTracker()
        {
            IVsTrackSelectionEx result;
            ErrorHandler.ThrowOnFailure(((IVsMonitorSelection2)selectionMonitor).GetEmptySelectionContext(out result));
            return result;
        }

        /// <summary>
        /// retrieves the IVsProject interface for currentll selected project
        /// </summary>
        /// <returns></returns>
        protected static IVsProject get_current_project()
        {
            IntPtr ppHier = IntPtr.Zero;
            uint pitemid;
            IVsMultiItemSelect ppMIS;
            IntPtr ppSC;
            IVsProject result = null;
            ErrorHandler.ThrowOnFailure(selectionTracker.GetCurrentSelection(out ppHier, out pitemid, out ppMIS, out ppSC));
            if (!IntPtr.Zero.Equals(ppHier))
            {
                result = Marshal.GetObjectForIUnknown(ppHier) as IVsProject;
                Marshal.Release(ppHier);
            }
            if (!IntPtr.Zero.Equals(ppSC))
                Marshal.Release(ppSC);
            return result;
        }

        protected ProjectCTXCommand(EventHandler eventHandler, CommandID cmdid)
            : base(eventHandler, cmdid) { }
    }
}
