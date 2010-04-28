using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.Validation
{
    /// <summary>
    /// Shortcut attribute for telling the validation system that validation rules
    /// should be inferred by mapping
    /// </summary>
    public class InferValidationFromAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <value>The type of the target.</value>
        public Type TargetType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InferValidationFromAttribute"/> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        public InferValidationFromAttribute(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
