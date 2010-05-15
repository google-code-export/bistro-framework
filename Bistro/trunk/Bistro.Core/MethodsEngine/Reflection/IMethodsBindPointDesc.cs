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
using Bistro.Controllers.Descriptor;

namespace Bistro.MethodsEngine.Reflection
{
    /// <summary>
    /// Interface, which contains methods and properties of the BindPointDescriptor 
    /// required by the Methods Engine
    /// </summary>
    public interface IMethodsBindPointDesc
    {
        /// <summary>
        /// Gets the BindType of the controller.
        /// </summary>
        /// <value>The BindType of the controller.(Before/Payload/After/Teardown)</value>
        BindType ControllerBindType { get; }

        /// <summary>
        /// Gets the target bind url.
        /// </summary>
        /// <value>The target bind url.</value>
        string Target { get; }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>The priority.</value>
        int Priority { get; }


        /// <summary>
        /// Gets the length of the bind in facets.
        /// </summary>
        /// <value>The length of the bind in facets.</value>
        int BindLength { get; }


        /// <summary>
        /// Gets the controller descriptor with information required by the methods engine.
        /// </summary>
        /// <value>The controller descriptor.</value>
        IMethodsControllerDesc Controller { get; }
    }
}
