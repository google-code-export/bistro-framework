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

namespace Bistro.Http
{
    /// <summary>
    /// An application exception that should be reported to the end user with an http status code
    /// </summary>
    public class WebException : ApplicationException
    {
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>The code.</value>
        public StatusCode Code { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebException"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        public WebException(StatusCode statusCode, string message)
            : base(message)
        {
            Code = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebException"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public WebException(StatusCode statusCode, string message, Exception exception)
            : base(message, exception)
        {
            Code = statusCode;
        }
    }
}