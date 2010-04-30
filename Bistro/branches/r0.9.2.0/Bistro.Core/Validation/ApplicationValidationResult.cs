using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.Validation
{
    /// <summary>
    /// Validation result that was generated outside of the validation framework, by an application rule
    /// </summary>
    public class ApplicationValidationResult : IValidationResult
    {
        /// <summary>
        /// Gets or sets the rule.
        /// </summary>
        /// <value>The rule.</value>
        public IValidator Rule { get { return null; } }

        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>The target.</value>
        public IValidatable Target { get; protected set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ApplicationValidationResult"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get { return false; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationValidationResult"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="message">The message.</param>
        public ApplicationValidationResult(IValidatable target, string message)
        {
            Target = target;
            Message = message;
        }
    }
}