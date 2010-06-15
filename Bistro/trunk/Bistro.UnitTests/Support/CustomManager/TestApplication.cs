using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Configuration.Logging;

namespace Bistro.UnitTests.Support.CustomManager
{
    public class TestApplication : Application
    {
        public TestApplication(ILoggerFactory loggerFactory) : base(loggerFactory) { }

        internal void ResetApp()
        {
            Instance = null;
            Initialized = false;
        }

    }
}
