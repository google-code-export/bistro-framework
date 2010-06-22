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
using System.Xml;
using System.IO;
using System.Security.Principal;
using Bistro.Http;

namespace Bistro.Controllers
{
    public enum ReturnType
    {
        Template,
        JSON,
        XML,
        File,
        Other
    }

    /// <summary>
    /// The execution context is used to allow controllers to pass execution instructions to the environment
    /// </summary>
    public interface IExecutionContext
    {
        /// <summary>
        /// Transfers execution to the given url. All currently queued controllers will finish execution.
        /// </summary>
        /// <param name="target"></param>
        void Transfer(string target);

        /// <summary>
        /// Gets the requested transfer target
        /// </summary>
        /// <returns></returns>
        string TransferTarget { get; }

        /// <summary>
        /// Determines whether the context has had a transfer request
        /// </summary>
        bool TransferRequested { get; }

        /// <summary>
        /// Clears the transfer request from the context
        /// </summary>
        void ClearTransferRequest();

        /// <summary>
        /// Gets the current user, or null if not authenticated
        /// </summary>
        /// <value>The current user.</value>
        IPrincipal CurrentUser { get; }

        /// <summary>
        /// Authenticates the specified user.
        /// </summary>
        /// <param name="token">The token identifiying a user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        void Authenticate(IPrincipal user);

        /// <summary>
        /// Abandons the session.
        /// </summary>
        void Abandon();

        /// <summary>
        /// Gets the response object.
        /// </summary>
        /// <value>The response.</value>
        IResponse Response { get; }

        /// <summary>
        /// Raises the given event. No expectation of when the actual event will be executed is provided. 
        /// </summary>
        /// <param name="eventUrl">The event URL.</param>
        void RaiseEvent(string eventUrl);
    }
}
