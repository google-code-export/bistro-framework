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
    public class ValidationTests: TestingBase
    {
        [Test]
        public void PassedValidation()
        {
            var resp = handler.RunForTest("GET/validationTest/ab@mail.com?secondField=123123&thirdField=2008-01-01");
            var contexts = handler.AllContents;

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;

            Assert.That(messages.Count == 0, String.Format("The list of messages has {0} elements instead of 0", messages.Count));
        }

        [Test]
        public void RequiredFieldValidation()
        {
            var resp = handler.RunForTest("GET/validationTest");
            var contexts = handler.AllContents;

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;

            Assert.That(messages != null, "The list of messages is missing");

            // only the two required field rules should be complaining
            Assert.That(messages.Count == 3, String.Format("The list of messages has {0} elements instead of 2", messages.Count));
            Assert.That(containsValidation("someField is required", messages), "Field length validation didn't fire");
        }

        private bool containsValidation(string message, List<IValidationResult> messages)
        {
            foreach (IValidationResult res in messages)
                if (message.Equals(res.Message))
                    return true;

            return false;
        }

        [Test]
        public void FieldLengthFailsValidation()
        {
            var resp = handler.RunForTest("GET/validationTest/a");
            var contexts = handler.AllContents;

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;

            Assert.That(containsValidation("someField must be at least two characters in length", messages), "Field length validation didn't fire");
        }

        [Test]
        public void FieldLengthPassesValidation()
        {
            var resp = handler.RunForTest("GET/validationTest/ab");
            var contexts = handler.AllContents;

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;

            Assert.That(!containsValidation("someField must be at least two characters in length", messages), "Field length shouldn't have fired");
        }

        [Test]
        public void RegexFailsValidation()
        {
            var resp = handler.RunForTest("GET/validationTest/de");
            var contexts = handler.AllContents;

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;

            Assert.That(containsValidation("someField must be 'ab'", messages), "Regex validation didn't fire");
        }

        [Test]
        public void RegexPassesValidation()
        {
            var resp = handler.RunForTest("GET/validationTest/ab");
            var contexts = handler.AllContents;

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;

            Assert.That(!containsValidation("someField must be 'ab'", messages), "Regex shouldn't have fired");
        }

        [Test]
        public void RangeValidationFailsValidation()
        {
            var resp = handler.RunForTest("GET/validationTest/123");
            var contexts = handler.AllContents;

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;

            Assert.That(containsValidation("someField must be alpha", messages), "Range validation didn't fire");
        }

        [Test]
        public void RangeValidationPassesValidation()
        {
            var resp = handler.RunForTest("GET/validationTest/ab");
            var contexts = handler.AllContents;

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;

            Assert.That(!containsValidation("someField must be alpha", messages), "Range validation shouldn't have fired");
        }

        [Test]
        public void SecondFieldFailsValidation()
        {
            var resp = handler.RunForTest("GET/validationTest");
            var contexts = handler.AllContents;

            var messages = contexts["request"]["Messages"] as List<IValidationResult>;

            Assert.That(containsValidation("thirdField is required", messages), "Validation on second field did not fire.");
        }
    }
}
