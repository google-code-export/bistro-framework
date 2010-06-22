using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Validation;

namespace Bistro.Extensions.Validation.Common
{
    public class RequiredValidator<T> : DefaultValidator<T> where T : IValidatable
    {
        public RequiredValidator(string message) : base(message) { }

        public override bool DoValidate(object target, out List<IValidationResult> messages)
        {
            messages = new List<IValidationResult>();
            bool fail = false;

            if (target == null)
                fail = true;
            else
            {
                var stringTarget = target as string;
                if (stringTarget != null && String.IsNullOrEmpty(stringTarget))
                    fail = true;
            }

            if (fail)
                messages.Add(new CommonValidationResult(this, target as IValidatable, Message, !fail));
 
            return !fail;
        }
    }
}