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
using System.Configuration;

namespace Bistro.Configuration
{
    /// <summary>
    /// Configuration section handler for bistro settings.
    /// </summary>
    public class SectionHandler : ConfigurationSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionHandler"/> class.
        /// </summary>
        public SectionHandler() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionHandler"/> class.
        /// </summary>
        /// <param name="dispatcherFactory">The dispatcher factory.</param>
        /// <param name="handlerFactory">The handler factory.</param>
        /// <param name="managerFactory">The manager factory.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public SectionHandler(string dispatcherFactory, string handlerFactory, string managerFactory, string loggerFactory)
        {
            DispatcherFactory = dispatcherFactory;
            ControllerHandlerFactory = handlerFactory;
            ControllerManagerFactory = managerFactory;
            LoggerFactory = loggerFactory;
        }

        /// <summary>
        /// Gets or sets the Application class name.
        /// </summary>
        /// <value>The dispatcher factory.</value>
        [ConfigurationProperty("application", DefaultValue = "Bistro.Application")]
        public string Application
        {
            get { return (string)this["application"]; }
            set { this["application"] = value; }
        }

        /// <summary>
        /// Gets or sets the dispatcher factory.
        /// </summary>
        /// <value>The dispatcher factory.</value>
        [ConfigurationProperty("dispatcher-factory", DefaultValue = "Bistro.Controllers.Dispatch.DispatcherFactory")]
        public string DispatcherFactory
        {
            get { return (string)this["dispatcher-factory"]; }
            set { this["dispatcher-factory"] = value; }
        }

        /// <summary>
        /// Gets or sets the controller handler factory.
        /// </summary>
        /// <value>The controller handler factory.</value>
        [ConfigurationProperty("handler-factory", DefaultValue = "Bistro.Validation.ValidatingHandlerFactory")]
        public string ControllerHandlerFactory
        {
            get { return (string)this["handler-factory"]; }
            set { this["handler-factory"] = value; }
        }

        /// <summary>
        /// Gets or sets the controller manager factory.
        /// </summary>
        /// <value>The controller manager factory.</value>
        [ConfigurationProperty("manager-factory", DefaultValue = "Bistro.Controllers.ControllerManagerFactory")]
        public string ControllerManagerFactory
        {
            get { return (string)this["manager-factory"]; }
            set { this["manager-factory"] = value; }
        }

        /// <summary>
        /// Gets or sets the controller manager factory.
        /// </summary>
        /// <value>The controller manager factory.</value>
        [ConfigurationProperty("logger-factory", DefaultValue = "Bistro.Configuration.Logging.DefaultLoggerFactory")]
        public string LoggerFactory
        {
            get { return (string)this["logger-factory"]; }
            set { this["logger-factory"] = value; }
        }

        /// <summary>
        /// Gets or sets the controller manager factory.
        /// </summary>
        /// <value>The controller manager factory.</value>
        [ConfigurationProperty("format-manager-factory", DefaultValue = "Bistro.OutputHandling.DefaultFormatManagerFactory")]
        public string FormatManager
        {
            get { return (string)this["format-manager-factory"]; }
            set { this["format-manager-factory"] = value; }
        }

        /// <summary>
        /// Gets or sets the extensions that will be processed by bistro.
        /// </summary>
        /// <value>The allowed extensions.</value>
        [ConfigurationProperty("allowed-extensions")]
        public NameValueConfigurationCollection AllowedExtensions
        {
            get { return (NameValueConfigurationCollection)this["allowed-extensions"]; }
            set { this["allowed-extensions"] = value; }
        }

        /// <summary>
        /// Gets or sets the directories that will be ignored by bistro.
        /// </summary>
        /// <value>The ignored directories.</value>
        [ConfigurationProperty("ignored-directories")]
        public NameValueConfigurationCollection IgnoredDirectories
        {
            get { return (NameValueConfigurationCollection)this["ignored-directories"]; }
            set { this["ignored-directories"] = value; }
        }

        /// <summary>
        /// Gets or sets the formatters supported by bistro.
        /// </summary>
        /// <value>The formatters.</value>
        [ConfigurationProperty("web-formatters")]
        public NameValueConfigurationCollection WebFormatters
        {
            get { return (NameValueConfigurationCollection)this["web-formatters"]; }
            set { this["web-formatters"] = value; }
        }

        /// <summary>
        /// Gets or sets the default formatter for bistro.
        /// </summary>
        /// <value>The default formatter.</value>
        [ConfigurationProperty("default-formatter")]
        public string DefaultFormatter
        {
            get { return (string)this["default-formatter"]; }
            set { this["default-formatter"] = value; }
        }
    }
}
