using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Flavor;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace Bistro.Designer.Projects.FSharp
{
    [Guid(Guids.guidFSharpProjectFactoryString)]
    public class Factory : FlavoredProjectFactoryBase
    {
        private DesignerPackage package;
        public Factory(DesignerPackage package)
            : base() 
        {
            this.package = package;
        }

        protected override object PreCreateForOuter(IntPtr outerProjectIUnknown)
        {
            var project = new ProjectManager(package);
            project.SetSite((IOleServiceProvider)((IServiceProvider)package).GetService(typeof(IOleServiceProvider)));
            return project;
        }

    }

    [Guid("C39A00F4-B2A4-478e-A0B2-C3E69B3BD899")]
    public class DummyWebFactory { }
}
