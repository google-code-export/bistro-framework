using System;
using NUnit.Framework;
using Bistro.UnitTests.Support;
using System.Collections.Specialized;

namespace Bistro.UnitTests.Tests
{
    [TestFixture]
    public class FormatTests : TestingBase
    {
        [Test]
        public void ValidOutput()
        {
            var expected = "{foo:bar,baz:qux}";
            var resp = handler.RunForTest("GET/format");
            resp = resp
                .Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\"", "");

            Assert.That(
                expected.Equals(resp),
                String.Format("Expected '{0}', Received {1}. Issue with Json formatting.", expected, resp));
        }

        [Test]
        public void ValidInput()
        {
            var expected = "\"hello world\"";
            var formData = new NameValueCollection();
            formData.Add("input", "{ foo: \"hello\", baz: \"world\" }");

            var resp = handler.RunForTest("POST/format", formData);

            Assert.That(
                expected.Equals(resp),
                String.Format("Expected '{0}', Received {1}. Issue with Json formatting.", expected, resp));
        }

        [Test]
        public void InvalidInput()
        {
            var expected = "";
            var formData = new NameValueCollection();
            formData.Add("input", "abracadabra");

            var resp = handler.RunForTest("POST/format", formData);

            Assert.That(
                expected.Equals(resp),
                String.Format("Expected '{0}', Received {1}. Issue with Json formatting.", expected, resp));
        }

        [Test]
        public void ValidXmlOutput()
        {
            var expected = "<?xmlversion=1.0encoding=utf-16?><SimpleTuplexmlns:xsi=http://www.w3.org/2001/XMLSchema-instancexmlns:xsd=http://www.w3.org/2001/XMLSchema><foo>bar</foo><baz>qux</baz></SimpleTuple>";
            var resp = handler.RunForTest("GET/format-xml");
            resp = resp
                .Replace(" ", "")
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\"", "");

            Assert.That(
                expected.Equals(resp),
                String.Format("Expected '{0}', Received {1}. Issue with Xml formatting.", expected, resp));
        }

        [Test]
        public void ValidXmlInput()
        {
            var expected = "\"hello world\"";
            var formData = new NameValueCollection();
            formData.Add(
                "input",
@"<?xml version=""1.0"" encoding=""utf-16""?>
<SimpleTuple xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <foo>hello</foo>
  <baz>world</baz>
</SimpleTuple>");

            var resp = handler.RunForTest("POST/format-xml", formData);

            Assert.That(
                expected.Equals(resp),
                String.Format("Expected '{0}', Received {1}. Issue with Xml formatting.", expected, resp));
        }

        [Test]
        public void InvalidXmlInput()
        {
            var expected = "";
            var formData = new NameValueCollection();
            formData.Add("input", "abracadabra");

            var resp = handler.RunForTest("POST/format-xml", formData);

            Assert.That(
                expected.Equals(resp),
                String.Format("Expected '{0}', Received {1}. Issue with Xml formatting.", expected, resp));
        }

        [Test]
        public void ResponseConfigurer()
        {
            var resp = handler.RunForTest("GET/format/custom");

            Assert.AreEqual(
                "true",
                handler.Headers["x-test-configurer"],
                "Missing 'x-test-configurer' header from response. Configurer must not have fired");
        }
    }
}


