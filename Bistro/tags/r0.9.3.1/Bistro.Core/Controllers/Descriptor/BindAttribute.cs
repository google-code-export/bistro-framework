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

namespace Bistro.Controllers.Descriptor
{

    /// <summary>
    /// Marks a class as bound to a particular meta-url
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class BindAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindAttribute"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        public BindAttribute(string target)
        {
            this.target = target;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindAttribute"/> class.
        /// </summary>
        /// <param name="verb">The verb.</param>
        /// <param name="target">The target.</param>
        public BindAttribute(BindVerb verb, string target)
        {
            if (BindPointUtilities.IsVerbQualified(target))
                throw new ApplicationException(
                    String.Format("Bind attributes may not use both the BindVerb and an HTTP Verb in the bind specification: '{0}'", target));

            this.target = verb.ToString() + " " + target.Trim();
        }

        /// <summary>
        /// The target url
        /// </summary>
        private string target;

        /// <summary>
        /// Gets or sets the target url structure
        /// </summary>
        /// <value>The target.</value>
        public string Target
        {
            get { return target; }
            set { target = value.Trim(); }
        }

        /// <summary>
        /// The priority of this bind point
        /// </summary>
        private int priority = -1;

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        /// <summary>
        /// The bind type of the controller.
        /// </summary>
        private BindType controllerBindType = BindType.Before;

        /// <summary>
        /// Determines when the controller will be executed in the request execution pipeline
        /// </summary>
        /// <value>The type of the controller bind.</value>
        public BindType ControllerBindType
        {
            get { return controllerBindType; }
            set { controllerBindType = value; }
        }
    }
}
