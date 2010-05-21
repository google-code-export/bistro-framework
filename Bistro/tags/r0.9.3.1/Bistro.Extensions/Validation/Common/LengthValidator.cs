using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Validation;
using System.Reflection;

namespace Bistro.Extensions.Validation.Common
{
    public class LengthValidator<T> : DefaultValidator<T> where T : IValidatable
    {
        int min, max;

        public LengthValidator(string message, int min, int max)
            : base(message)
        {
            this.min = min;
            this.max = max;

            DefiningParams.Add("min", min);
            DefiningParams.Add("max", max);
        }

        public override bool DoValidate(object target, out List<IValidationResult> messages)
        {
            messages = new List<IValidationResult>();
            bool fail = false;

            string stringTarget = target as string;
            if (stringTarget == null)
                return true;

            if (min > -1 && stringTarget.Length < min)
                fail = true;
            else if (max > -1 && stringTarget.Length > max)
                fail = true;

            if (fail)
                messages.Add(new CommonValidationResult(this, target as IValidatable, String.Format(Message,min,max), !fail));

            return !fail;
        }

        public override IValidator Translate(MemberInfo target)
        {
            return ValidatorForType(target.DeclaringType, new object[] { Message, min, max });
        }
    }
}
