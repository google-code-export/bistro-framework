using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.Entity
{
    /// <summary>
    /// A shortcut attribute for automatically generating an inferred mapping for the given type.
    /// </summary>
    public class InferMappingForAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        /// <value>The type of the target.</value>
        public Type TargetType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="InferMappingForAttribute"/> follows a strict inferrence model.
        /// </summary>
        /// <value><c>true</c> if strict; otherwise, <c>false</c>.</value>
        public bool Strict { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InferMappingForAttribute"/> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        public InferMappingForAttribute(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
