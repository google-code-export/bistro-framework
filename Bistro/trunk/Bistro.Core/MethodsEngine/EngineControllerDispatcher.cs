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
using Bistro.MethodsEngine.Subsets;
using Bistro.Configuration.Logging;
using Bistro.MethodsEngine.Reflection;
using Bistro.Controllers.Descriptor;
using BindPointDescriptor = Bistro.Controllers.Descriptor.ControllerDescriptor.BindPointDescriptor;
using Bistro.Controllers;
using Bistro.Interfaces;
using Bistro.Controllers.Dispatch;
using System.Diagnostics;

namespace Bistro.MethodsEngine
{
    /// <summary>
    /// Main methods engine class.
    /// </summary>
	public class EngineControllerDispatcher : IControllerDispatcher
    {

        enum Errors
        {
            [DefaultMessage("Binding not found in the map: {0} ")]
            BindingNotFound,
            [DefaultMessage("Controller type {0} does not support IMethodsBindPointDesc")]
            InterfaceNotSupported
        }

		enum Messages
		{
			[DefaultMessage("Extracted method by url in {0} ms over a set of {1} bind points")]
			MethodFound,
			[DefaultMessage("Found execution path in {0} ms over a set of {1} bind points")]
			PathCalculation
		}


        /// <summary>
        /// map of the binding urls to the list of the bind points.
        /// </summary>
        private Dictionary<string, List<IMethodsBindPointDesc>> map = new Dictionary<string, List<IMethodsBindPointDesc>>();


        /// <summary>
        /// url sebsets processor.
        /// </summary>
        private MethodsProcessor processor;
        public Dictionary<string, List<IMethodsBindPointDesc>> Map
        {
            get { return map; }
        }
        public MethodsProcessor Processor
        {
            get { return processor; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public EngineControllerDispatcher(Application application)
        {
			Logger = application.LoggerFactory.GetLogger(GetType());
            processor = new MethodsProcessor(this);
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        /// <value>The logger.</value>
        internal ILogger Logger { get; private set; }



        public void RegisterController(IControllerDescriptor info)
        {
            IMethodsControllerDesc infoNew = info as IMethodsControllerDesc;
            if (infoNew == null)
            {
                Logger.Report(Errors.InterfaceNotSupported, info.ControllerTypeName);
            }

            RegisterController(infoNew);
        }

        /// <summary>
        /// Registers the controller in the engine.
        /// </summary>
        /// <param name="info">The controller descriptor.</param>
        public void RegisterController(IMethodsControllerDesc info)
        {
            List<string> newBindUrls = new List<string>();
            foreach (IMethodsBindPointDesc bindPoint in info.Targets)
            {
                List<IMethodsBindPointDesc> descriptors;
                //Manually written "?" at the end of binding indicates that there whould be 
                //at least one item on this position.
                //In this case we replace "?" with "?/*".
                if (bindPoint.Target.EndsWith("?"))
                    descriptors = RegisterBindPointTarget(newBindUrls, bindPoint.Target.Remove(bindPoint.Target.Length - 1).Insert(bindPoint.Target.Length - 1, "*/?"));
                else
                    //common case
                    descriptors = RegisterBindPointTarget(newBindUrls, bindPoint.Target);

                int i = 0;
                foreach (IMethodsBindPointDesc comparedBindPoint in descriptors)
                {
                    if (comparedBindPoint.Priority > bindPoint.Priority)
                        break;

                    i++;
                }

                descriptors.Insert(i, bindPoint);
            }
            foreach (string bindUrl in newBindUrls)
            {
                processor.AddNewBinding(bindUrl);
            }

        }

        /// <summary>
        /// Adds bind point target to corresponding dictionary.
        /// The method will return either new list of descriptors or null depending on 
        /// existing bindPointTarget in the dictionary. New targets will also be added to newBindUrls dictionary
        /// to be given later to the processor.
        /// </summary>
        /// <param name="newBindUrls"></param>
        /// <param name="bindPointTarget"></param>
        /// <returns></returns>
        private List<IMethodsBindPointDesc> RegisterBindPointTarget(List<string> newBindUrls, string bindPointTarget)
        {
            List<IMethodsBindPointDesc> descriptors = null;

            if (!map.TryGetValue(bindPointTarget, out descriptors))
            {
                descriptors = new List<IMethodsBindPointDesc>();
                map.Add(bindPointTarget, descriptors);
                newBindUrls.Add(bindPointTarget);
            }
            return descriptors;
        }


		/// <summary>
		/// Forces the update of bind points in all methods.
		/// </summary>
		public void ForceUpdateBindPoints()
		{
			processor.UpdateBindPoints();
		}


        /// <summary>
        /// Determines whether the request url has exact bind in the <c>map</c>
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns>
        /// 	<c>true</c> if the request url has exact bind in the map; otherwise, <c>false</c>.
        /// </returns>
        public bool HasExactBind(string requestUrl)
        {
            return map.ContainsKey(requestUrl);

        }

        /// <summary>
        /// This method should be used by SubSetsProcessor to retrieve Bind Points associated with GenBindings
        /// </summary>
        /// <param name="binding">binding to use for search</param>
        /// <returns>List of associated bind points found in the map</returns>
        internal List<IMethodsBindPointDesc> GetTypesByBinding(MethodBinding binding)
        {
            if (map.ContainsKey(binding.InitialUrl))
            {
                return map[binding.InitialUrl];
            }
            Logger.Report(Errors.BindingNotFound, binding.InitialUrl);
            throw new ApplicationException("Binding not found");
            
        }




        /// <summary>
        /// Gets the array of ControllerInvocationInfo objects for the url.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns></returns>
        public List<ControllerInvocationInfo> GetControllers(string requestUrl)
        {
			Stopwatch sw = new Stopwatch();
			sw.Start();

			
			var methodSubSet = processor.GetMethodByUrl(requestUrl);
			var getParams = methodSubSet.ExtractParameters(requestUrl);
			Logger.Report(Messages.MethodFound, sw.ElapsedMilliseconds.ToString(), map.Count.ToString());


			List<ControllerInvocationInfo> retList = new List<ControllerInvocationInfo>(methodSubSet.BindingsList.Count);
			// Here we should get the bindings list and create controller invocation with params and binding for every controller.
			foreach (var bindPoint in methodSubSet.BindPointsList)
			{
				retList.Add(new ControllerInvocationInfo((BindPointDescriptor)bindPoint, getParams[bindPoint]));
			}

			Logger.Report(Messages.PathCalculation, sw.ElapsedMilliseconds.ToString(), map.Count.ToString());

			return retList;
        }

		/// <summary>
		/// Determines whether the specified method url returns at least one controller.
		/// TODO: Analyze how often we call this method and implement some caching dictionary.
		/// </summary>
		/// <param name="requestUrl">The method URL.</param>
		/// <returns>
		/// 	<c>true</c> if specified method url returns at least one controller; otherwise, <c>false</c>.
		/// </returns>
		public bool IsDefined(string requestUrl)
		{
			return GetControllers(requestUrl).Count > 0;
		}



        internal virtual void RaiseResourceLoop(string methodUrl, IEnumerable<IMethodsControllerDesc> controllers, params string[] args)
        {
        }

        internal virtual void RaiseMissingProvider(string resName, IEnumerable<IMethodsControllerDesc> controllers, params string[] args)
        {
        }

        internal virtual void RaiseInconsistentResourceType(string methodUrl, string resName, IEnumerable<IMethodsControllerDesc> controllers, params string[] args)
        {
        }
        public void Clean()
        {
            if (map != null) map.Clear();
            processor = null;
            processor = new MethodsProcessor(this);
        }




    }
}
