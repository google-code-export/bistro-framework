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
using System.Web;

using Bistro.Controllers.Descriptor;
using System.Reflection;
using Bistro.Controllers.Descriptor.Data;

namespace Bistro.Controllers.OutputHandling
{
    /// <summary>
    /// Rendering controller for using both Velocity and Django templates
    /// </summary>
    [Bind("?", ControllerBindType = BindType.After)]
    public abstract class RenderingController : IController
    {
        [Request, DependsOn]
        protected RenderType renderType = RenderType.Full;

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="requestContext"></param>
        public void ProcessRequest(HttpContextBase context, IContext requestContext)
        {
            // if transfer is requested, no rendering should be done
            if (requestContext.TransferRequested)
                return;

            string renderTarget;

            if (requestContext.Response.RenderTargets == null ||
                (!requestContext.Response.RenderTargets.TryGetValue(renderType, out renderTarget) &&
                requestContext.Response.RenderTargets.Count == 0))
            {
                if (requestContext.Response.CurrentReturnType == Bistro.Controllers.ReturnType.Template)
                    throw new ApplicationException("No template specified");

                return;
            }

            // if the requested render type doesn't have a corresponding target supplied,
            // default to the first non-empty target available.
            if (String.IsNullOrEmpty(renderTarget))
                renderTarget = requestContext.Response.RenderTargets.First(kvp => !String.IsNullOrEmpty(kvp.Value)).Value;

            var attrs = (TemplateMappingAttribute[])GetType().GetCustomAttributes(typeof(TemplateMappingAttribute), true);
            foreach (TemplateMappingAttribute attr in attrs)
                if (renderTarget.EndsWith(attr.Extension))
                {
                    ((Bistro.Http.Module)context.Handler).GetTemplateEngine(EngineType).Render(context, requestContext, renderTarget);
                    return;
                }
        }

        /// <summary>
        /// Gets the type of rendering engine this rendering controller uses
        /// </summary>
        /// <value>The type of the engine.</value>
        protected abstract Type EngineType { get; }

        /// <summary>
        /// Initializes this instance. This method is called before system-manipulated fields have been populated.
        /// </summary>
        public void Initialize() { }

        /// <summary>
        /// Recycles this instance. Recycle is called after ProcessRequest completes for stateful reusable controllers.
        /// This method is intended to put the controller in a state ready for a new request. This method may not be
        /// called from the request execution thread.
        /// </summary>
        public void Recycle() { }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IController"/> is reusable. If reusable and Stateful,
        /// the recycle method will be called once request processing is complete.
        /// </summary>
        /// <value><c>true</c> if reusable; otherwise, <c>false</c>.</value>
        public bool Reusable { get { return false; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IController"/> is stateful. If reusable and Stateful,
        /// the recycle method will be called once request processing is complete.
        /// </summary>
        /// <value><c>true</c> if stateful; otherwise, <c>false</c>.</value>
        public bool Stateful { get { return false; } }

        public MemberInfo GlobalHandle { get { return GetType(); } }

    }
}
