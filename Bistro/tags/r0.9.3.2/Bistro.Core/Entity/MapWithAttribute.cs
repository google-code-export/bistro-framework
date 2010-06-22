using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bistro.Entity
{
    /// <summary>
    /// Associates a controller with a mapping class
    /// </summary>
    public class MapsWithAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the type of the mapper.
        /// </summary>
        /// <value>The type of the mapper.</value>
        public Type MapperType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapsWithAttribute"/> class.
        /// </summary>
        /// <param name="mapperType">Type of the mapper.</param>
        public  MapsWithAttribute(Type mapperType)
        {
            MapperType = mapperType;
        }
    }
}
