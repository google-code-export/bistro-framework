using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Validation;
using System.Text.RegularExpressions;

namespace Bistro.Extensions.Validation.Common
{
    public enum ValidatableTypes
    {
        email,
        date,
        dateISO,
        number,
        digits,
        url,
        alpha,
        alphanumeric,
        /// <summary>
        /// Alphanumeric, with underscores and dashes
        /// </summary>
        extendedalphanumeric
    }
    public class TypeValidator<T> : DefaultValidator<T> where T : IValidatable
    {
        ValidatableTypes type;
        Regex re;

        public TypeValidator(string message, ValidatableTypes type): base(message)
        {
            this.type = type;
            DefiningParams.Add("type", type);
        }

        public override IValidator Translate(System.Reflection.MemberInfo target)
        {
            return ValidatorForType(target.DeclaringType, new object[] {Message, type});
        }

        public override bool DoValidate(object target, out List<IValidationResult> messages)
        {
            messages = new List<IValidationResult>();
            bool fail = false;

            string stringTarget = target as string;
            if (stringTarget == null)
                return true;
            switch (type) 
            {
                case ValidatableTypes.number:
                    re = new Regex(@"^-?(?:\d+|\d{1,3}(?:,\d{3})+)(?:\.\d+)?$");
                    break;
                case ValidatableTypes.email:
                    re = new Regex(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$");
                    break;
                case ValidatableTypes.alpha:
                    re = new Regex(@"^[a-zA-Z]*$");
                    break;
                case ValidatableTypes.alphanumeric:
                    re = new Regex(@"^[\w]*$");
                    break;
                case ValidatableTypes.extendedalphanumeric:
                    re = new Regex(@"^[\w-]*$");
                    break;
                case ValidatableTypes.date:
                    DateTime d;
                    fail=!DateTime.TryParse(stringTarget, out d);
                    break;
                case ValidatableTypes.dateISO:
                    re = new Regex(@"^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$");
                    break;
                case ValidatableTypes.digits:
                    re = new Regex(@"^\d+$");
                    break;
                case ValidatableTypes.url:
                    re = new Regex(@"^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$");
                    break;
            }
            if (re!=null)
                fail = !re.IsMatch(stringTarget);

            if (fail)
                messages.Add(new CommonValidationResult(this, target as IValidatable, Message, !fail));

            return !fail;
        }
    }
}