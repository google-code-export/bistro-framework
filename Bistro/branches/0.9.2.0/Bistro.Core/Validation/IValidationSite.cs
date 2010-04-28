using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Bistro.Validation;

namespace Bistro.Validation
{
    /// <summary>
    /// A non-generic validation site representation
    /// </summary>
    public interface IValidationSite: IValidator
    {
        /// <summary>
        /// Gets the member represented by this validation site.
        /// </summary>
        /// <value>The member.</value>
        MemberInfo Member { get; }
    }
}
