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
using System.Collections.Generic;
using System.Web;

namespace Bistro.Controllers
{
    /// <summary>
    /// Interface for a controller lifecycle manager
    /// </summary>
    public interface IControllerManager
    {
        /// <summary>
        /// Gets the controller.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="context">The context.</param>
        /// <param name="requestContext">The request context.</param>
        /// <returns></returns>
		IController GetController(ControllerInvocationInfo invocationInfo, HttpContextBase context, IContext requestContext);

        /// <summary>
        /// Returns the controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="context">The context.</param>
        /// <param name="requestContext">The request context.</param>
        void ReturnController(IController controller, System.Web.HttpContextBase context, IContext requestContext);
    }
}
