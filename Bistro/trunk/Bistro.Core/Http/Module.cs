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
using System.Web;
using System.Reflection;
using Bistro.Controllers;
using System.Threading;
using Bistro.Configuration.Logging;
using Bistro.Controllers.Security;
using Bistro.Controllers.Descriptor;
using System.Web.SessionState;
using Bistro.Controllers.OutputHandling;
using Bistro.Controllers.Dispatch;
using Bistro.Configuration;
using System.Configuration;
using System.Diagnostics;

namespace Bistro.Http
{
    /// <summary>
    /// Interfaces with .NET framework to provide bistro functionality. This class uses the asp.net mvc url routing approach
    /// for url re-writing. The request is intercepted during the PostResolveRequestCache and saved off onto the context.
    /// It is then re-routed to a static page with a .bistro extension to bypass the StaticFileHandler. The pipeline is then
    /// again intercepted during PostMapRequestHandler and the original request and computed handler are put back in.
    /// </summary>
    public class Module : IHttpModule, IHttpHandler, IRequiresSessionState
	{
        enum Messages
        {
            [DefaultMessage("{0} is not a valid extension, and will be skipped")]
            InvalidExtension,
			[DefaultMessage("Method invocation completed in {0} ms. Request url: {1}")]
			InvocationCompleted
        }

        enum Exceptions
        {
            [DefaultMessage("The following permission(s) were not satisfied\r\n{0}"), SeverityLevel(Severity.Critical)]
            AccessDenied
        }

        /// <summary>
        /// The logger to use
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// The disptacher to use
        /// </summary>
        private MethodDispatcher methodDispatcher;

        /// <summary>
        /// The application object
        /// </summary>
        private Application application;

        /// <summary>
        /// mutex handle
        /// </summary>
        static object moduleLock = new object();

        /// <summary>
        /// List of directories that will be ignored
        /// </summary>
        private List<string> ignoredDirectories = null;

        /// <summary>
        /// List of extensions that will be processed
        /// </summary>
        private List<string> allowedExtensions = null;

        /// <summary>
        /// Flags whether a wild-card mapping is present in the allowed extensions list
        /// </summary>
        private bool allowAllExtensions = false;
        
		/// <summary>
		/// Initializes the http module
		/// </summary>
		/// <param name="context"></param>
		public virtual void Init(HttpApplication context) {

            context.PostResolveRequestCache += new EventHandler(context_PostResolveRequestCache);
			lock (moduleLock) {
                SectionHandler section = (ConfigurationManager.GetSection("bistro") as SectionHandler) ?? new SectionHandler();

                LoadFactories(section);
                LoadUrlRules(section);
			}
		}

        /// <summary>
        /// Loads the allowedExtensions and ignoredDirectories lists. To speed processing, if empty,
        /// these lists should be kept null. For allowedExtensions, if this list is empty, no extensions
        /// are allowed. For ignoredDirectories, if this list is empty, all directories are treated as
        /// bind points. If not empty, the path is taken as an absolute path, starting from the app root
        /// </summary>
        protected virtual void LoadUrlRules(SectionHandler section)
        {
            if (section.AllowedExtensions.Count > 0)
            {
                allowedExtensions = new List<string>(section.AllowedExtensions.Count);

                foreach (NameValueConfigurationElement elem in section.AllowedExtensions)
                {
                    string ext = elem.Value.Trim(' ', '.').ToUpper();

                    if (ext.Contains("."))
                    {
                        logger.Report(Messages.InvalidExtension, ext);
                        continue;
                    }

                    // the empty extension should be added as just that, because the virtual
                    // path tool will yield .extension for extensions, and "" for none
                    if (ext == String.Empty)
                        allowedExtensions.Add(System.String.Empty);
                    else
                        allowedExtensions.Add("." + ext);
                }

                allowAllExtensions = allowedExtensions.Contains(".*");
            }

            if (section.IgnoredDirectories.Count > 0)
            {
                ignoredDirectories = new List<string>(section.IgnoredDirectories.Count);

                foreach (NameValueConfigurationElement elem in section.IgnoredDirectories)
                    ignoredDirectories.Add(elem.Value.Replace('\\', '/').Trim(' ', '/'));
            }
        }
        
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            string requestPoint =
                BindPointUtilities.Combine(context.Request.HttpMethod, context.Request.RawUrl.Substring(context.Request.ApplicationPath.Length));
            try
            {
				Stopwatch sw = new Stopwatch();
				sw.Start();


                var contextWrapper = new HttpContextWrapper(context);
                IContext requestContext = CreateRequestContext(contextWrapper);
                context.User = requestContext.CurrentUser;

                methodDispatcher.InvokeMethod(contextWrapper, requestPoint, requestContext);
				sw.Stop();
				logger.Report(Messages.InvocationCompleted, sw.ElapsedMilliseconds.ToString(), context.Request.RawUrl);
				

			}
            catch (WebException webEx)
            {
                context.Response.Clear();
                context.Response.StatusCode = Convert.ToInt16(webEx.Code);
                if (!String.IsNullOrEmpty(webEx.Message))
                    context.Response.Write(webEx.Message);

                if (webEx.InnerException != null && webEx.Code == StatusCode.InternalServerError)
                    context.Response.Write("\r\n\r\n" + webEx.ToString());
            }
        }

        /// <summary>
        /// Loads the factories.
        /// </summary>
        protected virtual void LoadFactories(SectionHandler section)
        {
            if (Application.Instance == null || !Application.Instance.Initialized)
                Application.Initialize(section);

            application = Application.Instance;
            logger = application.LoggerFactory.GetLogger(GetType());
            methodDispatcher = new MethodDispatcher(application);
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
		public void Dispose() { }

        /// <summary>
        /// Handles the PostResolveRequestCache event of the context control. This point is used to 
        /// intercept the handling of a bistro url and remap them to the bistro HTTP handler
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_PostResolveRequestCache(object sender, EventArgs e)
        {
            if (!IsValidPath(HttpContext.Current.Request.Path))
                return;

            // Note: this requires .Net 2.0 SP2 or later
            ((HttpApplication)sender).Context.RemapHandler(this);
        }

        /// <summary>
        /// Determines whether the path has a valid extension for processing
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// 	<c>true</c> if the path has a valid extension; otherwise, <c>false</c>.
        /// </returns>
        private bool hasValidExtension(string path)
        {
            if (allowAllExtensions)
                return true;

            string extension = VirtualPathUtility.GetExtension(HttpContext.Current.Request.Path).ToUpper();
            if (allowedExtensions == null)
                return String.Empty.Equals(extension);
            else
                return allowedExtensions.Contains(extension);
        }

        /// <summary>
        /// Determines whether the path starts with an ignored directory
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// 	<c>true</c> if the path is on the ignored directory list; otherwise, <c>false</c>.
        /// </returns>
        private bool isIgnoredPath(string path)
        {
            if (ignoredDirectories == null)
                return false;
            else
            {
                string appRelativePath = path.Substring(HttpContext.Current.Request.ApplicationPath.Length).TrimStart('/');
                foreach (string directory in ignoredDirectories)
                    if (appRelativePath.StartsWith(directory, StringComparison.OrdinalIgnoreCase))
                        return true;

                return false;
            }
        }

        /// <summary>
        /// Determines whether the requested url should be treated as a bistro bind-point
        /// </summary>
        /// <param name="p">The path.</param>
        /// <returns>
        /// 	<c>true</c> if the requested url is valid for processing; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidPath(string path)
        {
            return hasValidExtension(path) && !isIgnoredPath(path);
        }

        #region IHttpHandler implementation
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return true; }
        }

        Dictionary<Type, TemplateEngine> engines = new Dictionary<Type, TemplateEngine>();
        /// <summary>
        /// Returns an instance of the template engine od the specified type associated with this particular 
        /// HttpHandler. Each instance of HttpHandler has to maintain its own list of engines
        /// </summary>
        /// <param name="engineType"></param>
        /// <returns></returns>
        internal TemplateEngine GetTemplateEngine(Type engineType)
        {
            TemplateEngine result;
            if (!engines.TryGetValue(engineType, out result))
            {
                if (typeof(TemplateEngine).IsAssignableFrom(engineType) && engineType.GetConstructor(new Type[] { GetType() }) != null)
                {
                    result = (TemplateEngine)Activator.CreateInstance(engineType, this);
                    // there is no need for synchronzation here because the dictionary of engines
                    // belongs to the handler. There will never be another request coming here before this one 
                    // is completed 
                    engines.Add(engineType, result);
                }
                else
                    throw new Exception(engineType.FullName + " is not a valid type for a rendering engine");
            }
            return result;
        }
        #endregion

        /// <summary>
        /// Creates an <see cref="T:Bistro.Controllers.IContext"/>
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected virtual IContext CreateRequestContext(HttpContextBase context)
        {
            return new DefaultContext(context);
        }
    }
}