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
    /// Marks a field as linked with a value on the Cookie collection
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class CookieFieldAttribute : Attribute, IDataFieldMarker
    {
        /// <summary>
        /// Gets or sets the name of the cookie. If not supplied, the name of the marked field is used.
        /// </summary>
        /// <value>The name of the field.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the value of this <see cref="CookieFieldAttribute"/> will be sent out on the Response cookie collection. Defaults to false.
        /// </summary>
        /// <value><c>true</c> if outbound; otherwise, <c>false</c>.</value>
        public bool Outbound { get; set; }
    }
}
