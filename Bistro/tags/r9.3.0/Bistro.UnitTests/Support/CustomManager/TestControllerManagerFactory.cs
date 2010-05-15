using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers;
using Bistro.Configuration;

namespace Bistro.UnitTests.Support.CustomManager
{
    public class TestControllerManagerFactory : ControllerManagerFactory
    {
        public TestControllerManagerFactory(Application application, SectionHandler configuration) : base(application, configuration) { }

        public override IControllerManager GetInstanceImpl(Application application)
        {
            var mgr = new TestControllerManager(application);
            mgr.Load();

            return mgr;
        }
    }
}
