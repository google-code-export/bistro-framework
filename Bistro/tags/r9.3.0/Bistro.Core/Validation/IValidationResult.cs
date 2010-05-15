using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.Validation
{
    /// <summary>
    /// Result of running a validation
    /// </summary>
    public interface IValidationResult
    {
        IValidator Rule { get; }
        IValidatable Target { get; }
        string Message { get; }
        bool Success { get; }
    }
}
