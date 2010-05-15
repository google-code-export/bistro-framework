using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NUnit.Framework;
using Bistro.Controllers;
using Bistro.Controllers.Dispatch;
using System.Collections;
using Bistro.Controllers.OutputHandling;
using Bistro.Configuration.Logging;
using Bistro.Configuration;
using Bistro.Validation;
using Bistro.UnitTests.Support;

namespace Bistro.UnitTests.Tests
{
    [TestFixture]
    public class RuntimeTests : TestingBase
    {
        [Test]
        public void ApplicationCreated()
        {
            Assert.IsNotNull(Application.Instance, "Application not created.");
            Assert.AreEqual(1, StartupController.hitcount, String.Format("Application startup event called {0} time(s), instead of 1.", StartupController.hitcount));
        }

        [Test]
        public void DispatcherCreated()
        {
            Assert.IsNotNull(dispatcher);
        }

        [Test]
        public void BistroReflectionCorrect()
        {
            var dispatcher = new MethodDispatcher(Application.Instance);

            Assert.That(dispatcher.IsMethodDefined("GET/foo"), "Since there are wild-card mappings in the test apps, \"foo\" should return true");
            Assert.That(!dispatcher.IsMethodDefinedExplicitly("GET/foobar"), "There isn't an explicit controller binding to \"foobar\"");
            Assert.That(dispatcher.IsMethodDefinedExplicitly("EVENT/bistro/application/startup"), "The application startup method is defined explicitly. Something's amiss");
        }
    }
}
