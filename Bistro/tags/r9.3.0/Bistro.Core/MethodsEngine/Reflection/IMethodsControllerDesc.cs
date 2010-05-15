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
    /// Interface, which contains methods and properties of the ControllerDescriptor 
    /// required by the Methods Engine
    /// </summary>
    public interface IMethodsControllerDesc
    {

        /// <summary>
        /// Gets the name of the controller type.
        /// </summary>
        /// <value>The name of the controller type.</value>
        string ControllerTypeName { get; }

        /// <summary>
        /// Gets the dependson resources names.
        /// </summary>
        /// <value>The dependson resources names.</value>
        List<string> DependsOn { get; }

        /// <summary>
        /// Gets the requires resources names.
        /// </summary>
        /// <value>The requires resources names.</value>
        List<string> Requires { get; }

        /// <summary>
        /// Gets the provides resources names.
        /// </summary>
        /// <value>The provides resources names.</value>
        List<string> Provides { get; }


        /// <summary>
        /// Gets a value indicating - whether controller is a security controller - useful for methods engine
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is a security controller; otherwise, <c>false</c>.
        /// </value>
        bool IsSecurity { get; }

        /// <summary>
        /// Returns name of the resource type for resource.
        /// </summary>
        /// <param name="resourceName">resource name</param>
        /// <returns>Resource type name</returns>
        string GetResourceType(string resourceName);

        /// <summary>
        /// Gets the list of bind points linked to this controller.
        /// </summary>
        /// <value>The list of bind points linked to this controller.</value>
        IEnumerable<IMethodsBindPointDesc> Targets { get; }

    }
}
