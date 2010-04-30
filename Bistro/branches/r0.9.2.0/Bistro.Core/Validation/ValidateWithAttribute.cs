using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.Validation
{
    /// <summary>
    /// Maps a Validatable class to the class(s) that define its validation rules
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ValidateWithAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the type of the target class.
        /// </summary>
        /// <value>The type of the target.</value>
        public Type TargetType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateWithAttribute"/> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        public ValidateWithAttribute(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
