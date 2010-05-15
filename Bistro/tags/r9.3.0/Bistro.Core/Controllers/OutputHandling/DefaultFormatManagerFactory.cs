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
using Bistro.Configuration;
using Bistro.Configuration.Logging;

namespace Bistro.Controllers.OutputHandling
{
    /// <summary>
    /// Default format manager implementation
    /// </summary>
    public class DefaultFormatManagerFactory: IFormatManagerFactory
    {
        enum Messages
        {
            [DefaultMessage("{0} is not a known formatter")]
            UnknownFormatter
        }

        /// <summary>
        /// The format manager instance
        /// </summary>
        IFormatManager instance;

        /// <summary>
        /// The application class
        /// </summary>
        Application application;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFormatManagerFactory"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="configuration">The configuration.</param>
        public DefaultFormatManagerFactory(Application application, SectionHandler configuration)
        {
            this.application = application;
            instance = new DefaultFormatManager(application);

            foreach (string type in configuration.WebFormatters.AllKeys)
                instance.RegisterFormatter(Instantiate(configuration.WebFormatters[type].Value), type == configuration.DefaultFormatter);
        }

        /// <summary>
        /// Instantiates the specified type name.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        private IWebFormatter Instantiate(string typeName)
        {
            var formatter = Activator.CreateInstance(Type.GetType(typeName)) as IWebFormatter;

            if (formatter == null)
                application.LoggerFactory.GetLogger(GetType()).Report(Messages.UnknownFormatter, typeName);

            return formatter;
        }

        /// <summary>
        /// Gets the manager instance.
        /// </summary>
        /// <returns></returns>
        public IFormatManager GetManagerInstance()
        {
            return instance;
        }
    }
}
