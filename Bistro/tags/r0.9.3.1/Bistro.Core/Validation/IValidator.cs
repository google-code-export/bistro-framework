using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Bistro.Validation
{
    /// <summary>
    /// Interface for validator objects that can be used to validate an instance of type T
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Determines whether the specified target is valid.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>
        /// 	<c>true</c> if the specified target is valid; otherwise, <c>false</c>.
        /// </returns>
        bool IsValid(object target, out List<IValidationResult> messages);

        /// <summary>
        /// Gets the child validators of this instance
        /// </summary>
        /// <value>The children.</value>
        IEnumerable<IValidator> Children { get; }

        /// <summary>
        /// Merges the validator with the specified target. This method is required to be an
        /// idempotent factory method. It should produce a new instance of a validator which
        /// serves as a combination of all
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        IValidator Merge(IValidator target);

        /// <summary>
        /// Adds the specified child validator.
        /// </summary>
        /// <param name="child">The child.</param>
        /// <returns></returns>
        IValidator Add(IValidator child);

        /// <summary>
        /// Translates this validation to a different target
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        IValidator Translate(MemberInfo target);

        /// <summary>
        /// Gets the parameters that define this validator
        /// </summary>
        /// <value>The defining params.</value>
        Dictionary<string, object> DefiningParams { get; }

        /// <summary>
        /// Gets the name of the validation rule defined by this validator.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Builds a validator based on the mapping of the target object
        /// </summary>
        /// <returns></returns>
        IValidator ByMappingOnly();
    }
}
