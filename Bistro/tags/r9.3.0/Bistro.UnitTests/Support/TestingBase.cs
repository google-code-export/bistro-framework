using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Bistro.Configuration;
using Bistro.Controllers.Dispatch;
using Bistro.Controllers;
using System.Configuration;
using Bistro.UnitTests.Support.CustomManager;

namespace Bistro.UnitTests.Support
{
    /// <summary>
    /// Base class for bistro test fixtures. Provides necessary components
    /// </summary>
    public class TestingBase
    {
        /// <summary>
        /// Configures the bistro enviroment for test runs, and populates component references
        /// </summary>
        [TestFixtureSetUp]
        public virtual void Init()
        {
            SectionHandler sh = new SectionHandler();

            sh.DefaultFormatter = "Json";
            sh.WebFormatters.Add(new NameValueConfigurationElement("Json", "Bistro.Extensions.Format.Json.JsonFormatter, Bistro.Extensions"));
            sh.WebFormatters.Add(new NameValueConfigurationElement("Xml", "Bistro.Extensions.Format.Xml.XmlFormatter, Bistro.Extensions"));
			sh.Application = "Bistro.UnitTests.Support.CustomManager.TestApplication, Bistro.UnitTests";

            if (Application.Instance == null)
                Application.Initialize(sh);

			if (Application.Instance.ManagerFactory.GetType() != typeof(ControllerManagerFactory))
				(Application.Instance as TestApplication).InitAfter(sh);
			
         
            application = Application.Instance;
            manager = application.ManagerFactory.GetManagerInstance();
            dispatcher = application.DispatcherFactory.GetDispatcherInstance();
            handler = new TestingHandler();
        }

        /// <summary>
        /// Cleanups this instance.
        /// </summary>
        [TestFixtureTearDown]
        public virtual void Cleanup() { }

        protected IControllerDispatcher dispatcher;
        protected Application application;
        protected IControllerManager manager;
        protected TestingHandler handler;
    }
}
