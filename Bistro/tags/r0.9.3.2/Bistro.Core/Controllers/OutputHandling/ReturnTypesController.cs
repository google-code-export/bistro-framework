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
using System.Xml;
using System.IO;
using Bistro.Controllers.Descriptor;
using Bistro.Controllers.Descriptor.Data;

namespace Bistro.Controllers.OutputHandling
{
    /// <summary>
    /// Delegate for performing custom HttpResponse configuration. This delegate is 
    /// accepted by the ReturnTypesController to allow external customization of the 
    /// HttpResponseBase object that will be returned by the controller.
    /// </summary>
    public delegate HttpResponseBase ResponseConfigurer(HttpResponseBase response);

    /// <summary>
    /// Controller for handling non-rendered data
    /// </summary>
    [Bind("?", ControllerBindType = BindType.After)]
    public class ReturnTypesController : IController
    {
        const int BLOCK_SIZE = 8192;

        [Request, DependsOn]
        protected ResponseConfigurer responseConfigurer;

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="requestContext"></param>
        public void ProcessRequest(HttpContextBase context, IContext requestContext)
        {
            var response = requestContext.Response;

            // if transfer is requested, no return should be done
            if (requestContext.TransferRequested)
                return;

            // we explicitly want to only check for presence of data we care about
            // don't check whether render target has been set, that's not our concern
            if (response.CurrentReturnType == ReturnType.Template ||
                response.ExplicitResult == null)
                return;

            var contentType = response.ContentType;
            var returnValue = response.ExplicitResult;

            context.Response.Clear();

            context.Response.AppendHeader("content-type", contentType);
            context.Response.StatusCode = Convert.ToInt32(response.Code);

            if (responseConfigurer != null)
                responseConfigurer(context.Response);

            switch (response.CurrentReturnType)
            {
                case ReturnType.XML:
                case ReturnType.JSON:
                case ReturnType.Other:
                    context.Response.AppendHeader("Content-Length", returnValue.ToString().Length.ToString());
                    context.Response.Write(returnValue.ToString());
                    break;
                case ReturnType.File:
                    var stream = returnValue as Stream;
                    var fileName = response.AttachmentName as string;

                    if (fileName != null)
                        context.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);

                    // we weren't given a stream
                    if (stream == null)
                        context.Response.WriteFile(returnValue.ToString());
                    else
                    {
                        // since we were given a stream, we'll have to encode it somehow
                        using (stream)
                        {
                            context.Response.AppendHeader("Content-Length", stream.Length.ToString());
                            Encoding encoding =
                                response.Encoding != null ? response.Encoding : new UTF8Encoding();

                            byte[] buffer = new byte[BLOCK_SIZE];
                            int read = stream.Read(buffer, 0, BLOCK_SIZE);
                            while (read > 0)
                            {
                                context.Response.Write(encoding.GetChars(buffer, 0, read), 0, read);
                                read = stream.Read(buffer, 0, BLOCK_SIZE);
                            }

                            stream.Close();
                        }
                    }
                    break;
            }

            context.Response.Flush();
            context.Response.Close();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IController"/> is reusable. If reusable and Stateful,
        /// the recycle method will be called once request processing is complete.
        /// </summary>
        /// <value><c>true</c> if reusable; otherwise, <c>false</c>.</value>
        public bool Reusable
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IController"/> is stateful. If reusable and Stateful,
        /// the recycle method will be called once request processing is complete.
        /// </summary>
        /// <value><c>true</c> if stateful; otherwise, <c>false</c>.</value>
        public bool Stateful
        {
            get { return false; }
        }

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
        /// Gets a global type-system identifier for this class of controllers. In most cases this is simply the Type of
        /// the controller class.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public System.Reflection.MemberInfo GlobalHandle
        {
            get { return GetType(); }
        }
    }
}
