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
using Bistro.Http;
using Bistro.Controllers.Security;
using System.Web;
using Bistro.Controllers.Descriptor;
using Bistro.Configuration.Logging;
using Bistro.Controllers.Dispatch;
using System.Diagnostics;

namespace Bistro.Controllers
{
    public class MethodDispatcher
    {
        enum Messages
        {
            [DefaultMessage("Application has not been properly initialized")]
            NotInitialized,
            [DefaultMessage("No web session factory defined")]
            NoSessionFactory,
            [DefaultMessage("Application is being unloaded")]
            Unloading,
            [DefaultMessage("Exception processing URL ({0}) {1}\r\n\tSession: N/A. For additional information, reference {2}. ")]
            ExceptionNoSession,
            [DefaultMessage("Exception processing URL ({0}) {1}\r\n\tSession: ID={2} Activity: {3} ({4}). For additional information, reference {5}.")]
            Exception,
			[DefaultMessage("Unhandled exception: {0}\r\n\t Stack trace: {1}")]
			UnhandledException,
            [DefaultMessage("Headers are \r\n{0}")]
            Headers,
            [DefaultMessage("Extended information for trace {0}. Review attached parameters.")]
            ExtendedInfo,
            [DefaultMessage("Initializing Application")]
            Initializing,
            [DefaultMessage("Waiting for parallel initialization to complete")]
            Waiting,
            [DefaultMessage("Parallel initialization completed")]
            Initialized,
            [DefaultMessage("{0} could not be associated with a bind point")]
            ControllerNotFound,
            [DefaultMessage("Redirecting to {0} based on requirement(s) for \r\n{1}")]
            SecurityRedirect,
            [DefaultMessage("Processing request {0}")]
            ProcessingRequest,
            [DefaultMessage("{0} is not a valid extension, and will be skipped")]
            InvalidExtension,
			[DefaultMessage("Controller '{2}' has been invoked. Time elapsed: {0} ms. Processing time: {1}")]
			ControllerInvoked,
			[DefaultMessage("Execution path for {0} is \r\n{1}")]
            ExecutionPath
        }

        /// <summary>
        /// The logger to use
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// The controller manager to use
        /// </summary>
        private IControllerManager manager;

        /// <summary>
        /// The disptacher to use
        /// </summary>
        private IControllerDispatcher dispatcher;

        /// <summary>
        /// The application object
        /// </summary>
        private Application application;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDispatcher"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        public MethodDispatcher(Application application)
        {
            this.application = application;
            manager = application.ManagerFactory.GetManagerInstance();
            dispatcher = application.DispatcherFactory.GetDispatcherInstance();
            logger = application.LoggerFactory.GetLogger(GetType());
        }

        /// <summary>
        /// Determines whether <c>method</c> is defined on the runtime, i.e. if there is at least one
        /// controller that would fire for the given request
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>
        /// 	<c>true</c> if [is method supported] [the specified method]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMethodDefined(string method)
        {
            return dispatcher.IsDefined(method);
        }

        /// <summary>
        /// Determines whether the specified method has at least one controller bound directly to it
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>
        /// 	<c>true</c> if there is an explicit binding; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMethodDefinedExplicitly(string method)
        {
            return dispatcher.HasExactBind(method);
        }

        
        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        /// <summary>
        /// Obtains and processes the chain of controllers servicing this request. If a transfer is requested
        /// during the processing of a controller, the computed chain of controllers finishes, and then a
        /// recursive call is made to process the new contrller.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="requestPoint">The request point.</param>
        /// <param name="requestContext">The request context.</param>
        public virtual void InvokeMethod(HttpContextBase context, string requestPoint, IContext requestContext, bool handleException)
        {
            logger.Report(Messages.ProcessingRequest, requestPoint);

            try
            {
				List<ControllerInvocationInfo> invocationInfos = dispatcher.GetControllers(requestPoint);

				// TODO: this code should be replaced with call to the check method 
				// when it will be implemented on the dispatcher correctly
                if (invocationInfos.Count() == 0)
                {
                    logger.Report(Messages.ControllerNotFound, requestPoint);
                    throw new WebException(StatusCode.NotFound, String.Format("'{0} could not be found", requestPoint));
                }

				StringBuilder path = new StringBuilder();
				foreach (ControllerInvocationInfo info in invocationInfos)
					path.Append(info.BindPoint.Controller.ControllerTypeName).Append(" based on ").Append(info.BindPoint.Target).Append("\r\n");

				logger.Report(Messages.ExecutionPath, requestPoint, path.ToString());


				bool securityCheckComplete = false;
				bool securityCheckFailed = false;
				var failedPermissions = new Dictionary<string, KeyValuePair<FailAction, string>>();

				Stopwatch sw = new Stopwatch();
				Stopwatch sw1 = new Stopwatch();

				foreach (ControllerInvocationInfo invocationInfo in invocationInfos)
                {
					sw.Reset();
					sw.Start(); 
					
					
					IController controller = manager.GetController(invocationInfo, context, requestContext);

					try
					{
						// all security controllers are guaranteed to be at the top of the list, in proper order.
						// we need to run through all of them, because an inner controller may override the decision 
						// of an outer controller. therefore, the final decision is deferred until all security checks
						// have passed
						if (!securityCheckComplete)
						{
							ISecurityController securityController = controller as ISecurityController;
							if (securityController == null)
								securityCheckComplete = true;
							else
							{
								// this needs to run prior to the check
								controller.ProcessRequest(context, requestContext);

								// we only care about the actual return value of the last controller, as intermediate
								// results can be overriden by subsequent controllers. discard intermediate return values.
								securityCheckFailed = !securityController.HasAccess(requestContext, failedPermissions);
							}
						}

						// we have to do the check a second time, because the securityCheckComplete flag
						// gets set inside the top if. doing this in an else up top would skip the first
						// non-security controller
						if (securityCheckComplete)
						{
							if (securityCheckFailed || failedPermissions.Count != 0)
							{
								StringBuilder builder = new StringBuilder();
								foreach (string perm in failedPermissions.Keys)
									builder.AppendLine(perm);

								foreach (KeyValuePair<FailAction, string> kvp in failedPermissions.Values)
									if (kvp.Key == FailAction.Redirect)
									{
										// currently you can only house one transfer request per context, 
										// however, that may change in the future.
										requestContext.ClearTransferRequest();

										// give the fail action target a chance to redirect after re-validating
										requestContext.Transfer(
											kvp.Value +
											(kvp.Value.Contains("?") ? "&" : "?") +
											"originalRequest=" +
											HttpUtility.UrlEncode(requestPoint));

										logger.Report(Messages.SecurityRedirect, kvp.Value, builder.ToString());
										break;
									}

                                if (!requestContext.TransferRequested)
                                    throw new WebException(StatusCode.Unauthorized, "Access denied");
                                else
                                    // break out of the controller loop. we shouldn't be processing any more
                                    // controllers for this request, and need to get into whatever the security
                                    // guys requested
                                    break;
                            }
                            else
                            {
								if (invocationInfo.BindPoint.Controller.DefaultTemplates.Count > 0)
									requestContext.Response.RenderWith(invocationInfo.BindPoint.Controller.DefaultTemplates);

								sw1.Reset();
								sw1.Start();
								controller.ProcessRequest(context, requestContext);
								sw1.Stop();

							}
						}
					}
					finally
					{
						manager.ReturnController(controller, context, requestContext);
						sw.Stop();
						logger.Report(Messages.ControllerInvoked, sw.ElapsedMilliseconds.ToString(),sw1.ElapsedMilliseconds.ToString(), invocationInfo.BindPoint.Controller.ControllerTypeName);

					}
				}

				if (requestContext.TransferRequested)
				{
					string transferRequestPoint = BindPointUtilities.VerbQualify(requestContext.TransferTarget, "get");
					requestContext.ClearTransferRequest();

                    InvokeMethod(context, transferRequestPoint, requestContext);
                }
            }
            catch (Exception ex)
            {
				try
				{
					logger.Report(Messages.UnhandledException,ex.Message,ex.StackTrace);
				}
				catch
				{}

                //Assume that there are some other ctrls which match UnhandledException url and cause an exception.
                //In this case, removing this check may cause an infinite recursion of InvokeMethod and StackOverflow at the end.
                if (handleException)
                    throw new ApplicationException("Cannot process UnhandledException url, maybe some other controllers also match this url and cause an exception", ex);

				if (!IsMethodDefinedExplicitly(Application.UnhandledException))
				{
					throw new ApplicationException("Unhandled exception, and no binding to " + Application.UnhandledException + " found.", ex);
				}

                requestContext.Clear();
                requestContext.Add("unhandledException", ex);

				InvokeMethod(context, Application.UnhandledException, requestContext, true);
                
			}
        }
        public virtual void InvokeMethod(HttpContextBase context, string requestPoint, IContext requestContext)
        {
            InvokeMethod(context, requestPoint, requestContext, false);
        }
    }
}
