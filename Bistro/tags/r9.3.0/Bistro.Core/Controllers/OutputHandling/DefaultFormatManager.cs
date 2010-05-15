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
    /// Default format manager implementation
    /// </summary>
    public class DefaultFormatManager: IFormatManager
    {
        /// <summary>
        /// The map of all available formatters
        /// </summary>
        Dictionary<string, IWebFormatter> formatters = new Dictionary<string, IWebFormatter>();

        /// <summary>
        /// The default formatter
        /// </summary>
        IWebFormatter defaultFormatter;

        /// <summary>
        /// The application object
        /// </summary>
        Application application;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFormatManager"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public DefaultFormatManager(Application application)
        {
            this.application = application;
        }

        /// <summary>
        /// Gets a formatter for the given format type
        /// </summary>
        /// <param name="formatName"></param>
        /// <returns></returns>
        public IWebFormatter GetFormatterByFormat(string formatName)
        {
            return formatters[formatName];
        }

        /// <summary>
        /// Gets the default formatter.
        /// </summary>
        /// <returns></returns>
        public IWebFormatter GetDefaultFormatter()
        {
            if (defaultFormatter == null)
                throw new InvalidOperationException("No default formatter specified");

            return defaultFormatter;
        }

        public void RegisterFormatter(IWebFormatter formatter, bool isDefault)
        {
            formatters.Add(formatter.FormatName, formatter);

            if (isDefault)
                defaultFormatter = formatter;
        }
    }
}
