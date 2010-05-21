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
using Bistro.MethodsEngine.Reflection;
using System.Reflection;

namespace Bistro.Controllers.Descriptor.Wrappers
{
    /// <summary>
    /// Class to store information from a MemberInfo.(properties and fields) Useful for methods engine.
    /// </summary>
    public class MemberWrapper : IMemberInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="memberInfo">MemberInfo to wrap around</param>
        public MemberWrapper(MemberInfo memberInfo)
        {
            Name = memberInfo.Name;
            Type = memberInfo.ReflectedType.FullName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberWrapper"/> class.
        /// </summary>
        /// <param name="_name">The _name.</param>
        /// <param name="_type">The _type.</param>
        public MemberWrapper(string _name, string _type)
        {
            Name = _name;
            Type = _type;
        }

        /// <summary>
        /// Member name
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Type name
        /// </summary>
        public string Type
        {
            get;
            private set;
        }

    }
}
