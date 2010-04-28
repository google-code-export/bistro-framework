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
using System.Security.Principal;

namespace Bistro.Controllers.Security
{
    /// <summary>
    /// A set of actions that can be taken on a DemandAttribute failure
    /// </summary>
    [Flags]
    public enum FailAction
    {
        /// <summary>
        /// Throw an exception
        /// </summary>
        Fail = 1,
        /// <summary>
        /// Reditect to a supplied location
        /// </summary>
        Redirect = 2
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class SecurityAttribute : Attribute
    {
        /// <summary>
        /// The role given to all anonymous users
        /// </summary>
        public const string ANONYMOUS = "?";

        /// <summary>
        /// The role given to all authenticated users
        /// </summary>
        public const string AUTHENTICATED = "*";

        /// <summary>
        /// The name of the permission to enforce
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Action(s) to take if permssion is not present
        /// </summary>
        public FailAction OnFailure { get; set; }

        /// <summary>
        /// The redirect target
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Determines whether the specified user has access.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        /// 	<c>true</c> if the specified user has access; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool HasAccess(IPrincipal user);

        /// <summary>
        /// Gets a value indicating whether this permission failing implies that access should be explicitly denied
        /// </summary>
        /// <value><c>true</c> if hard fail; otherwise, <c>false</c>.</value>
        public abstract bool HardFail { get; }

        /// <summary>
        /// Gets a value indicating whether this attribute succeeding should override a prior attribute failing
        /// </summary>
        /// <value><c>true</c> if override prior; otherwise, <c>false</c>.</value>
        public abstract bool OverridePrior { get; }
    }
}
