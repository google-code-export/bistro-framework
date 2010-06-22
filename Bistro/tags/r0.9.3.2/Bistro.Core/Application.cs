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
using Bistro.Controllers.Dispatch;
using Bistro.Controllers;
using Bistro.Configuration.Logging;
using Bistro.Configuration;
using System.Web;
using System.IO;
using System.Reflection;
using Bistro.Validation;
using Bistro.Controllers.OutputHandling;

namespace Bistro
{
    /// <summary>
    /// Application class for Bistro Runtime
    /// </summary>
    public class Application
    {
        /// <summary>
        /// Name of Bistro events
        /// </summary>
        public const string SystemEvents = "/bistro";

        /// <summary>
        /// Name application events
        /// </summary>
        public const string ApplicationEvents = "/application";

        /// <summary>
        /// Name of the startup event
        /// </summary>
        public const string ApplicationStartup = "EVENT" + SystemEvents + ApplicationEvents + "/startup";

        /// <summary>
        /// Name of the unhandled exception event
        /// </summary>
        public const string UnhandledException = "GET" + SystemEvents + ApplicationEvents + "/exception";

        /// <summary>
        /// Gets or sets the application instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Application Instance { get; protected set; }

        /// <summary>
        /// The application root directory
        /// </summary>
        private string rootDir;

        /// <summary>
        /// The application \bin directory
        /// </summary>
        private string binDir;

        /// <summary>
        /// The application logger
        /// </summary>
        protected ILogger logger;

        enum Messages
        {
            [DefaultMessage("Unable to load type '{0}'. Defaulting to {1}")]
            UnableToLoadType,
            [DefaultMessage("Type '{0}' is incompatible with {1}. Defaulting to {1}")]
            IncompatibleType,
            [DefaultMessage("Using '{0}' for {1}")]
            LoadingComponent,
            [DefaultMessage("Assembly '{0}' preloaded")]
            AssemblyPreloaded,
            [DefaultMessage("File '{0}' was not loaded: {1}")]
            AssemblySkipped
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        public Application(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            logger = loggerFactory.GetLogger(GetType());
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Application"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        public bool Initialized { get; protected set; }

        /// <summary>
        /// Initializes the application from the configuration section provided.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void Initialize(SectionHandler configuration)
        {
            ILoggerFactory loggerFactory = LoadComponent<ILoggerFactory>(null, configuration.LoggerFactory, typeof(DefaultLoggerFactory), new object[] { });
            ILogger logger = loggerFactory.GetLogger(typeof(Application));

            Application application = LoadComponent<Application>(logger, configuration.Application, typeof(Application), new object[] { loggerFactory });
            Instance = application;
            logger = application.LoggerFactory.GetLogger(application.GetType());

            // preload the assemblies prior to any other initialization, because they
            // may rely on that stuff being there.
            application.PreLoadAssemblies();

            application.FormatManagerFactory = LoadComponent<IFormatManagerFactory>(logger, configuration.FormatManager, typeof(DefaultFormatManagerFactory), new object[] { application, configuration });
            application.HandlerFactory = LoadComponent<IControllerHandlerFactory>(logger, configuration.ControllerHandlerFactory, typeof(ValidatingHandlerFactory), new object[] { application, configuration });
            application.DispatcherFactory = LoadComponent<IDispatcherFactory>(logger, configuration.DispatcherFactory, typeof(DispatcherFactory), new object[] { application, configuration });

            // manager factory requires handler and dispatcher factories to be in place
            application.ManagerFactory = LoadComponent<IControllerManagerFactory>(logger, configuration.ControllerManagerFactory, typeof(ControllerManagerFactory), new object[] { application, configuration });

            application.Initialized = true;

            var methodDispatcher = new MethodDispatcher(application);
            if (methodDispatcher.IsMethodDefined(ApplicationStartup))
                methodDispatcher.InvokeMethod(null, ApplicationStartup, new EventContext(null, false));
        }

        /// <summary>
        /// Preloads assemblies that are likely to be used by the bistro runtime
        /// </summary>
        protected virtual void PreLoadAssemblies()
        {
            try
            {
                rootDir = HttpRuntime.AppDomainAppPath;
                binDir = HttpRuntime.BinDirectory;
            }
            catch
            {
                rootDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                binDir = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath ?? rootDir;
            }

            foreach (string file in Directory.GetFiles(binDir, "*.dll"))
                try
                {
                    Assembly.Load(Path.GetFileNameWithoutExtension(file), null);
                    logger.Report(Messages.AssemblyPreloaded, file);
                }
                catch (Exception ex)
                {
                    logger.Report(Messages.AssemblySkipped, file, ex.Message);
                }
        }

        /// <summary>
        /// Loads the component based on the configuration parameters. This method will create a type based on the
        /// "type-name" attribute of the identified configuration node. If none is present, the default type is used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configuration">The configuration node. This is expected to be parent node of the node identified by tagName</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="defaultType">The default type.</param>
        /// <param name="constructorArgs">The constructor parameters to supply.</param>
        /// <param name="setter">The setter. This function will finalize any confi</param>
        private static T LoadComponent<T>(ILogger logger, string typeName, Type defaultType, object[] constructorArgs)
        {
            Type componentType = typeName == null ? null : Type.GetType(typeName, false);

            if (componentType == null)
            {
                if (logger != null)
                    logger.Report(Messages.UnableToLoadType, typeName, defaultType.FullName);
                componentType = defaultType;
            }

            List<Type> types = new List<Type>();
            foreach (object arg in constructorArgs)
                types.Add(arg.GetType());

            if (!typeof(T).IsAssignableFrom(componentType) || componentType.GetConstructor(types.ToArray()) == null)
            {
                if (logger != null)
                    logger.Report(Messages.IncompatibleType, typeName, defaultType.Name);
                componentType = defaultType;
            }

            if (logger != null)
                logger.Report(Messages.LoadingComponent, componentType.FullName, typeof(T).Name);
            return (T)Activator.CreateInstance(componentType, constructorArgs);
        }

        /// <summary>
        /// The manager factory to use
        /// </summary>
        public IControllerManagerFactory ManagerFactory { get; private set; }

        /// <summary>
        /// The dispatcher factory to use
        /// </summary>
        public IDispatcherFactory DispatcherFactory { get; private set; }

        /// <summary>
        /// Gets or sets the logger factory.
        /// </summary>
        /// <value>The logger factory.</value>
        public ILoggerFactory LoggerFactory { get; private set; }

        /// <summary>
        /// Gets or sets the handler factory.
        /// </summary>
        /// <value>The handler factory.</value>
        public IControllerHandlerFactory HandlerFactory { get; private set; }

        /// <summary>
        /// Gets or sets the format manager.
        /// </summary>
        /// <value>The format manager.</value>
        public IFormatManagerFactory FormatManagerFactory { get; private set; }
    }
}
