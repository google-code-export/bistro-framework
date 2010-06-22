using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.Controllers.OutputHandling
{
    /// <summary>
    /// Defines the render type.
    /// </summary>
    public enum RenderType
    {
        /// <summary>
        /// Output will be rendered in full
        /// </summary>
        Full,
        /// <summary>
        /// Only a portion of the output will be rendered
        /// </summary>
        Partial
    }
}
