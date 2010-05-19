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
    /// Determines when the controller will be executed in the request execution pipeline
    /// </summary>
    public enum BindType
    {
        /// <summary>
        /// Controller will be executed on the way down the tree. This is default behavior
        /// </summary>
        Before,
        /// <summary>
        /// Controller will be executed as the payload of the tree (i.e. once the final leaf is reached)
        /// </summary>
        Payload,
        /// <summary>
        /// Controller will be executed on the way up the tree.
        /// </summary>
        After,
        /// <summary>
        /// Controller will be executed after all other processing. Attempts to return data from a Teardown
        /// controller will cause a run-time exception
        /// </summary>
        Teardown
    }
}
