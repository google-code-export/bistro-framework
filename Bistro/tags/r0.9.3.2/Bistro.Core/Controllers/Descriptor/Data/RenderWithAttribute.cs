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
using Bistro.Controllers.OutputHandling;

namespace Bistro.Controllers.Descriptor.Data
{
    /// <summary>
    /// Marks a controller with the specified default template. Templates specified
    /// prior to this class being invoked are overriden. Templates specified
    /// after this class being invoked (including from within the class itself) will
    /// override this attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = true)]
    public class RenderWithAttribute: Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderWithAttribute"/> class.
        /// </summary>
        /// <param name="template">The template.</param>
        public RenderWithAttribute(string template)
        {
            Template = template;
        }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        public string Template { get; private set; }

        /// <summary>
        /// Gets or sets the type of the render.
        /// </summary>
        /// <value>The type of the render.</value>
        public RenderType RenderType { get; set; }
    }
}
