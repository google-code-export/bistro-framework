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
using System.Linq;
using System.Text;

namespace Bistro.Controllers.Descriptor.Data
{
    /// <summary>
    /// List of standard formats
    /// </summary>
    public enum Format {
        /// <summary>
        /// Specifies that the format should be provided by the default "Json" provider
        /// </summary>
        Json,
        /// <summary>
        /// Specifies that the format should be provided by the default "Xml" provider
        /// </summary>
        Xml
    }

    /// <summary>
    /// Specifies formatting for the object graph through an enum or a string literal.
    /// </summary>
    public class FormatAsAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the name of the format.
        /// </summary>
        /// <value>The name of the format.</value>
        public string FormatName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatAsAttribute"/> class.
        /// </summary>
        /// <param name="format">The format.</param>
        public FormatAsAttribute(Format format)
        {
            FormatName = format.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatAsAttribute"/> class.
        /// </summary>
        /// <param name="formatName">Name of the format.</param>
        public FormatAsAttribute(string formatName)
        {
            FormatName = formatName;
        }
    }
}
