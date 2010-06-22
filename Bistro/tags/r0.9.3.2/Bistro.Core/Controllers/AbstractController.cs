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
using System.Web;
using System.Reflection;

namespace Bistro.Controllers
{
    /// <summary>
    /// An abstract base implementation of the <see cref="IController"/> interface
    /// </summary>
    public abstract class AbstractController : IController
    {
        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="requestContext"></param>
        public void ProcessRequest(HttpContextBase context, IContext requestContext)
        {
            DoProcessRequest(requestContext);
        }

        /// <summary>
        /// Actual implementation of controller logic
        /// </summary>
        public abstract void DoProcessRequest(IExecutionContext context);

        /// <summary>
        /// Gets a value indicating whether this <see cref="IController"/> is reusable. If reusable and Stateful,
        /// the recycle method will be called once request processing is complete.
        /// </summary>
        /// <value><c>true</c> if reusable; otherwise, <c>false</c>.</value>
        public virtual bool Reusable { get { return false; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IController"/> is stateful. If reusable and Stateful,
        /// the recycle method will be called once request processing is complete.
        /// </summary>
        /// <value><c>true</c> if stateful; otherwise, <c>false</c>.</value>
        public virtual bool Stateful { get { return true; } }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Recycles this instance. Recycle is called after ProcessRequest completes for stateful reusable controllers.
        /// This method is intended to put the controller in a state ready for a new request. This method may not be
        /// called from the request execution thread.
        /// </summary>
        public virtual void Recycle() { }

        /// <summary>
        /// Gets a global type-system identifier for this class of controllers. In most cases this is simply the Type of
        /// the controller class.
        /// </summary>
        /// <returns></returns>
        public MemberInfo GlobalHandle { get { return GetType(); } }
    }
}
