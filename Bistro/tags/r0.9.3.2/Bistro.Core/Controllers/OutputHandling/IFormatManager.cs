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

namespace Bistro.Controllers.OutputHandling
{
    /// <summary>
    /// Manager of all available formatters
    /// </summary>
    public interface IFormatManager
    {
        /// <summary>
        /// Gets a formatter for the given format type
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        IWebFormatter GetFormatterByFormat(string formatName);

        /// <summary>
        /// Gets the default formatter.
        /// </summary>
        /// <returns></returns>
        IWebFormatter GetDefaultFormatter();

        /// <summary>
        /// Registers an instance of the formatter with the manager
        /// </summary>
        /// <param name="formatter"></param>
        void RegisterFormatter(IWebFormatter formatter, bool isDefault);
    }
}
