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
using Bistro.Interfaces;
using System.Reflection;
using System.Collections.Generic;
namespace Bistro.Controllers.Descriptor
{

    /// <summary>
    /// Interface, which contains methods and properties of the BindPointDescriptor 
    /// required by the Bistro.Core
    /// </summary>
    public interface IBindPointDescriptor
    {
        /// <summary>
        /// Gets the controller descriptor.
        /// </summary>
        /// <value>The controller descriptor.</value>
        IControllerDescriptor Controller { get; }

        /// <summary>
        /// Gets the BindType of the controller.
        /// </summary>
        /// <value>The BindType of the controller.(Before/Payload/After/Teardown)</value>
        BindType ControllerBindType { get; }

        /// <summary>
        /// Gets the parameter fields.
        /// </summary>
        /// <value>The parameter fields.</value>
        Dictionary<string, MemberInfo> ParameterFields { get; }

        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>The priority.</value>
        int Priority { get; }
        /// <summary>
        /// Gets the target bind url.
        /// </summary>
        /// <value>The target bind url.</value>
        string Target { get; }
    }
}
