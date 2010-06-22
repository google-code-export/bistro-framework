using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Validation;
using Bistro.Controllers.Descriptor;
using Bistro.Extensions.Validation;
using Bistro.Extensions.Validation.Common;
using System.Text.RegularExpressions;
using Bistro.Controllers.Descriptor.Data;
using Bistro.Controllers;

namespace Bistro.UnitTests.Tests.Data
{
    sealed class ControllerValidator : Validator<ValidatingController>
    {
        public ControllerValidator()
        {
            Define(
                    Value(c => c.someField)
                        .AliasedAs("otherField")
                        .IsRequired("someField is required")
                        .IsInRange("a", "zzzzzzzzzzzzzzzzzz", "someField must be alpha")
                        .IsMinLength(2, "someField must be at least two characters in length")
                        .IsOfType(ValidatableTypes.email, "someField must be type of email")
                        .MatchesRegex("ab", RegexOptions.None, "someField must be 'ab'"))
                .And(
                    Value(c => c.thirdField)
                        .IsRequired("thirdField is required")
                        .IsOfType(ValidatableTypes.date, "thirdField must be type of date")
                        .IsOfType(ValidatableTypes.dateISO, "thirdField must be type of dateISO"))
                .And(
                    Value(c => c.secondField)
                        .IsRequired("secondField is required")
                        .IsOfType(ValidatableTypes.number, "secondField must be type of number")
                        .IsOfType(ValidatableTypes.digits, "secondField must be type of digits"));
        }
    }

    [Bind("/validationTest/{someField}?{secondField}&{thirdField}")]
    [ValidateWith(typeof(ControllerValidator))]
    public class ValidatingController : AbstractController, IValidatable
    {
        public string someField;

        public string secondField;

        public string thirdField;

        [Request]
        public List<IValidationResult> Messages { get; set; }
        public bool IsValid { get; set; }

        public override void DoProcessRequest(IExecutionContext context)
        {
        }
    }

}
