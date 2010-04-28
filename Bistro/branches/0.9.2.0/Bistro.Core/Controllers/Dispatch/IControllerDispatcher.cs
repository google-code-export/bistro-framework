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
using Bistro.Controllers.Descriptor;
namespace Bistro.Controllers.Dispatch
{
    /// <summary>
    /// Manages controller application to urls. This interface should be implemented only if
    /// a drastic departure from default dispatch functionality is required
    /// </summary>
    public interface IControllerDispatcher
    {
        /// <summary>
        /// Gets a sorted list of controllers that will process the given url
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns>A sorted list of controllers</returns>
        ControllerInvocationInfo[] GetControllers(string requestUrl);

        /// <summary>
        /// Determines whether the specified url has a controller explicitly bound to it
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns>
        /// 	<c>true</c> if an exact binding exists; otherwise, <c>false</c>.
        /// </returns>
        bool HasExactBind(string requestUrl);

        /// <summary>
        /// Registers the controller with the dispatcher.
        /// </summary>
        /// <param name="info">The info.</param>
        void RegisterController(ControllerDescriptor info);
    }
}
