/****************************************************************************
 * 
 *  Bistro Framework Copyright © 2003-2009 Hill30 Inc
 *
 *  This file is part of Bistro Framework.
 *
 *  Bistro Framework is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Bistro Framework is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Bistro Framework.  If not, see <http://www.gnu.org/licenses/>.
 *  
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace Bistro.Controllers.Descriptor.Data
{
    /// <summary>
    /// Marks the controller as a supplier of the marked value onto the context
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class ProvidesAttribute : Attribute, IDataFieldMarker
    {
        /// <summary>
        /// Gets or sets the element.
        /// </summary>
        /// <value>The element.</value>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidesAttribute"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public ProvidesAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidesAttribute"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public ProvidesAttribute(string name)
        {
            Name = name;
        }
    }
}