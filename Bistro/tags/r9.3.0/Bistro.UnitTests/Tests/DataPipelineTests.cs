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
    public class DataPipelineTests : TestingBase
    {
        [Test]
        public void CorrectOrderWithSkip()
        {
            var resp = handler.RunForTest("GET/pipeline");
            var contexts = handler.AllContents;

            Assert.That(
                "FirstThird".Equals(contexts["request"]["Data"]),
                String.Format("Expected 'FirstThird', Received {0}. Pipeline processing order violated.", contexts["request"]["Data"]));
        }

        [Test]
        public void CorrectOrderFull()
        {
            var resp = handler.RunForTest("GET/pipeline/full");
            var contexts = handler.AllContents;

            Assert.That(
                "FirstSecondThird".Equals(contexts["request"]["Data"]),
                String.Format("Expected 'FirstSecondThird', Received {0}. Pipeline processing order violated.", contexts["request"]["Data"]));
        }
    }
}
