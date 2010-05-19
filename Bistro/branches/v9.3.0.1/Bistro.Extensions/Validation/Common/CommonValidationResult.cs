using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Validation;

namespace Bistro.Extensions.Validation.Common
{
    /// <summary>
    /// Default <see cref="IValidationResult"/> implementation
    /// </summary>
    public class CommonValidationResult: IValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommonValidationResult"/> class.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="target">The target.</param>
        /// <param name="message">The message.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        public CommonValidationResult(IValidator rule, IValidatable target, string message, bool success)
        {
            Rule = rule;
            Target = target;
            Message = message;
            Success = success;
        }

        /// <summary>
        /// Gets or sets the rule.
        /// </summary>
        /// <value>The rule.</value>
        public IValidator Rule
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public IValidatable Target
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommonValidationResult"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success
        {
            get;
            set;
        }
    }
}
