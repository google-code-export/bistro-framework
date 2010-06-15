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
using System.Web;
namespace Bistro.Controllers
{
    /// <summary>
    /// Manages the state of a controller pre and post processing.
    /// </summary>
    public interface IControllerHandler
    {
        /// <summary>
        /// Gets an instance of a controller prepared to execute the associated invocation info.
        /// </summary>
        /// <param name="info">The invocation info for the controller.</param>
        /// <param name="context">The http context for the current request.</param>
        /// <returns>an IController instance</returns>
        IController GetControllerInstance(ControllerInvocationInfo info, HttpContextBase context, IContext requestContext);

        /// <summary>
        /// Returns the controller back to a ready state.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="context">The http context for the current request.</param>
        void ReturnController(IController controller, HttpContextBase context, IContext requestContext);
    }
}
