using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.Entity
{
    /// <summary>
    /// Marks a controller as one that can accept a mapper
    /// </summary>
    public interface IMappable
    {
        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        /// <value>The mapper.</value>
        EntityMapperBase Mapper { get; set; }
    }
}
