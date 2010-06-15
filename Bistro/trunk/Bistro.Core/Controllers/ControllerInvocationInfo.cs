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

namespace Bistro.Controllers
{
    /// <summary>
    /// Describes information about a particular controller instance, as used within a single match.
    /// </summary>
    public struct ControllerInvocationInfo
    {
        /// <summary>
        /// The bind point used in this invocation
        /// </summary>
        public ControllerDescriptor.BindPointDescriptor BindPoint;

        /// <summary>
        /// The parameters based on the requested url that are relevant
        /// to this invocation
        /// </summary>
        public Dictionary<string, string> Parameters;

        /// <summary>
        /// The depth of final match component within the url
        /// </summary>
        internal int MatchDepth;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerInstanceInfo"/> struct.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="matchDepth">The match depth.</param>
        public ControllerInvocationInfo(
            ControllerDescriptor.BindPointDescriptor controller,
            Dictionary<string, string> parameters,
            int matchDepth)
        {
            BindPoint = controller;
            Parameters = parameters;
            MatchDepth = matchDepth;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ControllerInvocationInfo c1, ControllerInvocationInfo c2)
        {
            return c1.BindPoint.Controller.ControllerType == c2.BindPoint.Controller.ControllerType;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="c1">The c1.</param>
        /// <param name="c2">The c2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ControllerInvocationInfo c1, ControllerInvocationInfo c2)
        {
            return c1.BindPoint.Controller.ControllerType != c2.BindPoint.Controller.ControllerType;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return (ControllerInvocationInfo)obj == this;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return BindPoint.Controller.ControllerType.GetHashCode();
        }
    }
}
