using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Bistro.UnitTests.Support;

namespace Bistro.UnitTests.Tests
{
    [TestFixture]
    public class RenderTests : TestingBase
    {
        [Test]
        public void Render1()
        {
            var expected = "Rendering template1.t";

            var resp = handler.RunForTest("GET/render/test1");

            Assert.That(
                expected.Equals(resp),
                String.Format("Expected '{0}', Received {1}. Issue with standard RenderWith call.", expected, resp));
        }

        [Test]
        public void Render2()
        {
            var expected = "Rendering template2.t";

            var resp = handler.RunForTest("GET/render/test2");

            Assert.That(
                expected.Equals(resp),
                String.Format("Expected '{0}', Received {1}. Issue with extended RenderWith call.", expected, resp));
        }

        [Test]
        public void Render3()
        {
            var expected = "Rendering template3.t";

            var resp = handler.RunForTest("GET/render/test3");

            Assert.That(
                expected.Equals(resp),
                String.Format("Expected '{0}', Received {1}. Issue with RenderWith type fall-back call.", expected, resp));
        }
    }
}
