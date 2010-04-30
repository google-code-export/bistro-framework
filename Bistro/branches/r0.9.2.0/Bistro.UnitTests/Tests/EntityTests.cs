using System;
using System.Collections.Specialized;
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
using Bistro.UnitTests.Tests.Data;

namespace Bistro.UnitTests.Tests
{
    [TestFixture]
    public class EntityTests: TestingBase
    {
        [Test]
        public void MapsFromInputToEntity()
        {
            var resp = handler.RunForTest("GET/entityTest?Foo=hello&bar=world&thirdField=stuff&unwrap=false&extra=something&nullableInt=1234&nullableBool=true");
            var contexts = handler.AllContents;
            var entity = contexts["request"]["entity"] as SimpleEntity;

            Assert.NotNull(entity);

            if (entity != null)
            {
				Assert.AreEqual(1234, entity.nullableInt);
				Assert.AreEqual(true, entity.nullableBool);
				Assert.AreEqual("hello", entity.foo);
                Assert.AreEqual("world", entity.bar);
                Assert.AreEqual("stuff", entity.baz, String.Format("Expected 'stuff', received '{0}'. If the other tests passed, and this failed, the explicit mapping call is suspect", entity.baz));
                Assert.IsNull(entity.extra, "Field 'extra' should remain null. Issue with Except() method.");
            }
        }

        [Test]
        public void MapsFromEntityToInput()
        {
            NameValueCollection formFields = new NameValueCollection();
            formFields.Add("entity", "{foo: \"hello\", bar: \"world\", baz: \"stuff\"}");
            var resp = handler.RunForTest("GET/entityTest?unwrap=true", formFields);
            var contexts = handler.AllContents;

            Assert.AreEqual("hello", contexts["request"]["Foo"]);
            Assert.AreEqual("world", contexts["request"]["bar"]);
            Assert.AreEqual("stuff", contexts["request"]["thirdField"], String.Format("Expected 'stuff', received '{0}'. If the other tests passed, and this failed, the explicit mapping call is suspect", contexts["request"]["thirdField"]));
        }

        [Test]
        public void ValidatesByMapping()
        {
            var resp = handler.RunForTest("GET/entityTest?bar=world&thirdField=stuff&unwrap=false");
            var contexts = handler.AllContents;
            var entity = contexts["request"]["entity"] as SimpleEntity;

            Assert.Null(entity, "Entity should have been null - not enough data was supplied.");

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;
            Assert.AreEqual(1, messages.Count);
        }

        [Test]
        public void MapsFromInputToEntityStrict()
        {
            var resp = handler.RunForTest("GET/strictEntityTest?Foo=hello&bar=world&thirdField=stuff&unwrap=false");
            var contexts = handler.AllContents;
            var entity = contexts["request"]["entity"] as SimpleEntity;

            Assert.NotNull(entity);

            if (entity != null)
            {
                Assert.Null(entity.foo);
                Assert.AreEqual("world", entity.bar);
                Assert.AreEqual("stuff", entity.baz, String.Format("Expected 'stuff', received '{0}'. If the other tests passed, and this failed, the explicit mapping call is suspect", entity.baz));
            }
        }

        [Test]
        public void MapsFromInputToEntityAttributeInferred()
        {
            var resp = handler.RunForTest("GET/attributeInferredEntityTest?Foo=hello&bar=world&unwrap=false&extra=something");
            var contexts = handler.AllContents;
            var entity = contexts["request"]["entity"] as SimpleEntity;

            Assert.NotNull(entity);

            if (entity != null)
            {
                Assert.AreEqual("hello", entity.foo);
                Assert.AreEqual("world", entity.bar);
                Assert.IsNull(entity.extra, "Field 'extra' should remain null. Issue with Except() method.");
            }
        }

        [Test]
        public void ValidatesByInferredMapping()
        {
            var resp = handler.RunForTest("GET/attributeInferredEntityTest?bar=world&thirdField=stuff&unwrap=false");
            var contexts = handler.AllContents;
            var entity = contexts["request"]["entity"] as SimpleEntity;

            Assert.Null(entity, "Entity should have been null - not enough data was supplied.");

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;
            Assert.AreEqual(1, messages.Count);
        }

        [Test]
        public void MapsFromInputToInterfaceAttributeInferred()
        {
            var resp = handler.RunForTest("GET/attributeInterfaceEntityTest?Foo=hello&bar=world&unwrap=false&extra=something");
            var contexts = handler.AllContents;
            var entity = contexts["request"]["entity"] as SimpleEntity;

            Assert.NotNull(entity);

            if (entity != null)
            {
                Assert.AreEqual("hello", entity.foo);
                Assert.AreEqual("world", entity.bar);
                Assert.IsNull(entity.extra, "Field 'extra' should remain null. Issue with Except() method.");
            }
        }

		//[Test]
		//public void MapsFromDateTime()
		//{
		//    var resp = handler.RunForTest("GET/entityTest?MyDate=12%2F14%2F2009");

		//    var contexts = handler.AllContents;
		//    var dt = contexts["request"]["MyDate"];
		//    var dt2Compare = new DateTime(2009, 12, 14);
		//    Assert.AreEqual(dt2Compare, dt);
		//}
    }
}