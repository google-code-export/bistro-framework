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
using System.Xml;
using System.IO;
using Bistro.Controllers.OutputHandling;

namespace Bistro.Controllers
{
    /// <summary>
    /// The Bistro response interface. 
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Returns XML as the http response, closing the output stream.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        [Obsolete("This method is being deprecated in favor of the Return(objectGraph[, format]) methods")]
        void ReturnXml(string xml);

        /// <summary>
        /// Returns XML as the http response, closing the output stream.
        /// </summary>
        /// <param name="xml">The XML fragment.</param>
        [Obsolete("This method is being deprecated in favor of the Return(objectGraph[, format]) methods")]
        void ReturnXml(XmlNode xml);

        /// <summary>
        /// Returns the JSON as the http response, closing the output stream.
        /// </summary>
        /// <param name="json">The json.</param>
        [Obsolete("This method is being deprecated in favor of the Return(objectGraph[, format]) methods")]
        void ReturnJSON(string json);

        /// <summary>
        /// Returns the file as the http reponse, closing the output stream
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="contentType">the content-type header value to supply.</param>
        /// <param name="fileName">Name of the file. If supplied, a content-disposition header of "attachment" will be appended.</param>
        /// <param name="encoding">The encoding. If not supplied, UTF8 will be used</param>
        void ReturnFile(Stream fileStream, string contentType, string fileName, Encoding encoding);

        /// <summary>
        /// Returns the file as the http reponse, closing the output stream
        /// </summary>
        /// <param name="fileName">The absolute path to the file.</param>
        /// <param name="contentType">the content-type header value to supply.</param>
        /// <param name="asAttachment">whether the file should be returned as an attachment</param>
        void ReturnFile(string fileName, string contentType, bool asAttachment);

        /// <summary>
        /// Returns an arbitrary string as the http response, with the supplied content type
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="contentType">the content-type header value to supply.</param>
        void ReturnFreeForm(string value, string contentType);

        /// <summary>
        /// Returns an http status code and a message. Note that this will behave the same way as
        /// the rest of the ReturnXXX methods, allowing the controller chain to finish execution.
        /// If an immediate return is required, use <see cref="WebException"/>.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        void ReturnStatus(StatusCode status, string message);

        /// <summary>
        /// Returns the specified object graph.
        /// </summary>
        /// <param name="objectGraph">The object graph.</param>
        void Return(Object objectGraph);

        /// <summary>
        /// Returns the specified object graph.
        /// </summary>
        /// <param name="objectGraph">The object graph.</param>
        /// <param name="formatName">Name of the format.</param>
        void Return(Object objectGraph, string formatName);

        /// <summary>
        /// Gets the result that was supplied to one of the ReturnXXX methods.
        /// </summary>
        /// <value>The result.</value>
        object ExplicitResult { get; }

        /// <summary>
        /// Gets the content type of the explicit result. 
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Gets the encoding to use for the explicit result
        /// </summary>
        Encoding Encoding { get; }

        /// <summary>
        /// Gets the name that will be given to an attachment. Applicable to files 
        /// returned with asAttachment set, or an explicit file names given to streams.
        /// </summary>
        string AttachmentName { get; }

        /// <summary>
        /// Gets the http status code.
        /// </summary>
        /// <value>The code.</value>
        StatusCode Code { get; }

        /// <summary>
        /// Notifies the rendering engine how to render the results of the current request
        /// </summary>
        void RenderWith(string target);

        /// <summary>
        /// Notifies the rendering engine how to render the results of the current request, 
        /// providing a listing of different rendering options.
        /// </summary>
        void RenderWith(IDictionary<RenderType, string> targets);

        /// <summary>
        /// Gets the render target.
        /// </summary>
        /// <value>The render target.</value>
        IDictionary<RenderType, string> RenderTargets { get; }

        /// <summary>
        /// Gets the return type that is currently set on the context
        /// </summary>
        ReturnType CurrentReturnType { get; }
    }
}
