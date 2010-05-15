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
using System.Reflection;
using System.Collections.Generic;
using Bistro.Controllers.OutputHandling;
using Bistro.Controllers.Descriptor;

namespace Bistro.Interfaces
{
    /// <summary>
    /// Interface, which contains methods and properties of the ControllerDescriptor 
    /// required by the Bistro.Core
    /// </summary>
    public interface IControllerDescriptor :IComparable
    {

        /// <summary>
        /// Gets the MemberInfo containing information on the controller type.
        /// </summary>
        /// <value>The type of the controller.</value>
        MemberInfo ControllerType { get; }

        /// <summary>
        /// Gets the name of the controller type.
        /// </summary>
        /// <value>The name of the controller type.</value>
        string ControllerTypeName { get; }

        /// <summary>
        /// Gets the dictionary with the cookie fields.
        /// </summary>
        /// <value>The cookie fields dictionary.</value>
        Dictionary<string, ControllerDescriptor.CookieFieldDescriptor> CookieFields { get; }


        /// <summary>
        /// Gets the dictionary with default templates.
        /// </summary>
        /// <value>The default templates dictionary.</value>
        Dictionary<RenderType, string> DefaultTemplates { get; }

        /// <summary>
        /// Gets the dependson resources names.
        /// </summary>
        /// <value>The dependson resources names.</value>
        List<string> DependsOn { get; }

        /// <summary>
        /// Gets resources marked as FormField.
        /// </summary>
        /// <value>Resources marked as FormField.</value>
        Dictionary<string, MemberInfo> FormFields { get; }


        /// <summary>
        /// Gets the provides resources names.
        /// </summary>
        /// <value>The provides resources names.</value>
        List<string> Provides { get; }

        /// <summary>
        /// Gets resources marked as Request.
        /// </summary>
        /// <value>Resources marked as Request.</value>
        Dictionary<string, MemberInfo> RequestFields { get; }

        /// <summary>
        /// Gets the requires resources names.
        /// </summary>
        /// <value>The requires resources names.</value>
        List<string> Requires { get; }

        /// <summary>
        /// Gets resources marked as Session.
        /// </summary>
        /// <value>Resources marked as Session.</value>
        Dictionary<string, MemberInfo> SessionFields { get; }

        /// <summary>
        /// Gets the BindPointDescriptors.
        /// </summary>
        /// <value>The BindPointDescriptors.</value>
        IEnumerable<IBindPointDescriptor> Targets { get; }

        /// <summary>
        /// Populates the descriptor contents.
        /// </summary>
        void PopulateDescriptor();

    }
}
