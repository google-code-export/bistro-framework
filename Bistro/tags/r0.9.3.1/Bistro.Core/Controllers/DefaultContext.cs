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
using System.Collections;
using System.IO;
using System.Web.SessionState;
using System.Security.Principal;
using System.Threading;
using Bistro.Controllers.Security;
using Bistro.Http;
using System.Web;
using Bistro.Controllers.OutputHandling;
using Bistro.Controllers.Descriptor;

namespace Bistro.Controllers
{
    /// <summary>
    /// A default implementation of <see cref="IContext"/>
    /// </summary>
    public class DefaultContext : Dictionary<string, object>, IContext, IResponse
    {
        public const string contentTypeJSON = "application/json";
        public const string contentTypeXML = "text/xml";
        public const string contentTypeHTML = "text/html";
        public const string USER = "BistroUser";

        /// <summary>
        /// A reference to the ASP.NET session object
        /// </summary>
        private HttpSessionStateBase session;

        /// <summary>
        /// A reference to the HttpContext of the current request
        /// </summary>
        private HttpContextBase context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultContext"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public DefaultContext(HttpContextBase context)
        {
            this.session = context == null ? null : context.Session;
            this.context = context;
            this.Code = StatusCode.OK;
        }

        /// <summary>
        /// Transfers execution to the given url. All currently queued controllers will finish execution.
        /// </summary>
        /// <param name="target"></param>
        public virtual void Transfer(string target)
        {
            TransferTarget = target;
            TransferRequested = true;
        }

        /// <summary>
        /// Notifies the rendering engine how to render the results of the current request
        /// </summary>
        /// <param name="target"></param>
        public virtual void RenderWith(string target)
        {
            RenderWith(new Dictionary<RenderType, string>() { { RenderType.Full, target } });
        }

        /// <summary>
        /// Notifies the rendering engine how to render the results of the current request,
        /// providing a listing of different rendering options.
        /// </summary>
        /// <param name="targets"></param>
        public virtual void RenderWith(IDictionary<RenderType, string> targets)
        {
            ClearReturnValues();
            RenderTargets = targets;

            CurrentReturnType = ReturnType.Template;
        }

        /// <summary>
        /// Gets the render target.
        /// </summary>
        /// <value>The render target.</value>
        public virtual IDictionary<RenderType, string> RenderTargets
        { 
            get; 
            protected set; 
        }

        /// <summary>
        /// Gets the requested transfer target
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public virtual string TransferTarget
        {
            get;
            private set;
        }

        /// <summary>
        /// Determines whether the context has had a transfer request
        /// </summary>
        /// <value></value>
        public virtual bool TransferRequested
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the result that was supplied to one of the ReturnXXX methods.
        /// </summary>
        /// <value>The result.</value>
        public virtual object ExplicitResult
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the content type of the explicit result.
        /// </summary>
        /// <value></value>
        public virtual string ContentType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the encoding to use for the explicit result
        /// </summary>
        /// <value></value>
        public virtual Encoding Encoding
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the name that will be given to an attachment. Applicable to files
        /// returned with asAttachment set, or an explicit file names given to streams.
        /// </summary>
        /// <value></value>
        public virtual string AttachmentName
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the return type that is currently set on the context
        /// </summary>
        /// <value></value>
        public virtual ReturnType CurrentReturnType
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public virtual StatusCode Code
        {
            get;
            private set;
        }

        /// <summary>
        /// Clears the transfer request from the context
        /// </summary>
        public void ClearTransferRequest()
        {
            TransferRequested = false;
            TransferTarget = null;
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        public virtual HttpSessionStateBase Session
        {
            get { return session; }
        }

        /// <summary>
        /// Gets the current user, or null if not authenticated
        /// </summary>
        /// <value>The current user.</value>
        public IPrincipal CurrentUser
        {
            get { return (IPrincipal)session[USER] ?? AnonymousUser.Instance; }
        }

        /// <summary>
        /// Authenticates the specified user.
        /// </summary>
        /// <param name="token">The token identifiying a user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public void Authenticate(IPrincipal user)
        {
            context.User = user;
            session[USER] = user;
        }

        /// <summary>
        /// Returns XML as the http response, closing the output stream.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        public void ReturnXml(string xml)
        {
            ReturnFreeForm(xml, contentTypeXML);
            CurrentReturnType = ReturnType.XML;
        }

        /// <summary>
        /// Returns XML as the http response, closing the output stream.
        /// </summary>
        /// <param name="xml">The XML fragment.</param>
        public void ReturnXml(System.Xml.XmlNode xml)
        {
            ReturnFreeForm(xml.ToString(), contentTypeXML);
            CurrentReturnType = ReturnType.XML;
        }

        /// <summary>
        /// Returns the JSON as the http response, closing the output stream.
        /// </summary>
        /// <param name="json">The json.</param>
        public void ReturnJSON(string json)
        {
            ReturnFreeForm(json, contentTypeJSON);
            CurrentReturnType = ReturnType.JSON;
        }

        /// <summary>
        /// Returns the file as the http reponse, closing the output stream
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="contentType">the content-type header value to supply.</param>
        /// <param name="fileName">Name of the file. If supplied, a content-disposition header of "attachment" will be appended.</param>
        /// <param name="encoding">The encoding. If not supplied, UTF8 will be used</param>
        public void ReturnFile(System.IO.Stream fileStream, string contentType, string fileName, Encoding encoding)
        {
            ClearReturnValues();
            ExplicitResult = fileName;
            ContentType = contentType;

            if (!String.IsNullOrEmpty(fileName))
                AttachmentName = fileName;

            if (encoding != null)
                Encoding = encoding;

            CurrentReturnType = ReturnType.File;
        }

        /// <summary>
        /// Returns the file as the http reponse, closing the output stream
        /// </summary>
        /// <param name="fileName">The absolute path to the file.</param>
        /// <param name="contentType">the content-type header value to supply.</param>
        /// <param name="asAttachment">whether the file should be returned as an attachment</param>
        public void ReturnFile(string fileName, string contentType, bool asAttachment)
        {
            ClearReturnValues();
            ExplicitResult = fileName;
            ContentType = contentType;

            if (asAttachment)
                AttachmentName = Path.GetFileName(fileName);

            CurrentReturnType = ReturnType.File;
        }

        /// <summary>
        /// Returns an arbitrary string as the http response, with the supplied content type
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="contentType">the content-type header value to supply.</param>
        public virtual void ReturnFreeForm(string value, string contentType)
        {
            ClearReturnValues();
            ExplicitResult = value;
            ContentType = contentType;

            CurrentReturnType = ReturnType.Other;
        }

        /// <summary>
        /// Clears the return values.
        /// </summary>
        protected virtual void ClearReturnValues()
        {
            ExplicitResult = null;
            ContentType = null;
            AttachmentName = null;
            Encoding = null;
            RenderTargets = null;

            CurrentReturnType = ReturnType.Other;
        }

        /// <summary>
        /// Abandons the session.
        /// </summary>
        public virtual void Abandon()
        {
            session.Abandon();
        }

        /// <summary>
        /// Returns an http status code and a message. Note that this will behave the same way as
        /// the rest of the ReturnXXX methods, allowing the controller chain to finish execution.
        /// If an immediate return is required, use <see cref="WebException"/>.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public void ReturnStatus(StatusCode status, string message)
        {
            Code = status;
            ReturnFreeForm(message, contentTypeHTML);
            CurrentReturnType = ReturnType.Other;
        }

        /// <summary>
        /// Gets the response object.
        /// </summary>
        /// <value>The response.</value>
        public virtual IResponse Response { get { return this; } }

        /// <summary>
        /// Returns the content of the current context. This method is called 
        /// by the NDjango debug tag
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder("\r\n------ Bistro Context ------\r\nVariables:\r\n");
            foreach (KeyValuePair<string, object> var in this)
                result.AppendFormat("{0} = {1}\r\n", var.Key, var.Value.ToString());

            return result.ToString();
        }

        /// <summary>
        /// Returns the specified object graph.
        /// </summary>
        /// <param name="objectGraph">The object graph.</param>
        public virtual void Return(object objectGraph)
        {
            IWebFormatter formatter =
                Application.Instance.FormatManagerFactory.GetManagerInstance().GetDefaultFormatter();

            ReturnFreeForm(formatter.Serialize(objectGraph), formatter.ContentType);
        }

        /// <summary>
        /// Returns the specified object graph.
        /// </summary>
        /// <param name="objectGraph">The object graph.</param>
        /// <param name="formatName">Name of the format.</param>
        public virtual void Return(object objectGraph, string formatName)
        {
            IWebFormatter formatter =
                Application.Instance.FormatManagerFactory.GetManagerInstance().GetFormatterByFormat(formatName);

            ReturnFreeForm(formatter.Serialize(objectGraph), formatter.ContentType);
        }

        /// <summary>
        /// Raises the given event. No expectation of when the actual event will be executed is provided.
        /// </summary>
        /// <param name="eventUrl">The event URL.</param>
        public virtual void RaiseEvent(string eventUrl)
        {
            new MethodDispatcher(Application.Instance)
                .InvokeMethod(
                    context, 
                    BindPointUtilities.VerbQualify(eventUrl, "EVENT"), 
                    new EventContext(context, true));
        }
    }
}
