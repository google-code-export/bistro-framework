using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Configuration.Logging;
using Bistro.Configuration;

namespace Bistro.UnitTests.Support.CustomManager
{
    public class TestApplication : Application
    {
        public TestApplication(ILoggerFactory loggerFactory) : base(loggerFactory) { }

		public void InitAfter(SectionHandler configuration)
		{
			InitializeAfter(configuration);
		}

    }
}
