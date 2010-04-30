using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers.OutputHandling;
using System.Xml.Serialization;
using System.IO;

namespace Bistro.Extensions.Format.Xml
{
    /// <summary>
    /// Formatter implementation for using standard .net xml serialization mechanisms.
    /// </summary>
    public class XmlFormatter: IWebFormatter
    {
        #region IWebFormatter Members

        /// <summary>
        /// Serializes the specified object graph to the given stream.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="outputStream">The output stream.</param>
        public void Serialize(object graph, System.IO.Stream outputStream)
        {
            XmlSerializer xs = new XmlSerializer(graph.GetType());
            xs.Serialize(outputStream, graph);
        }

        /// <summary>
        /// Serialzies the specified object graph into a string.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public string Serialize(object graph)
        {
            XmlSerializer xs = new XmlSerializer(graph.GetType());
            StringWriter sw = new StringWriter();

            xs.Serialize(sw, graph);

            return sw.ToString();
        }

        /// <summary>
        /// Deserializes the specified target type.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public object Deserialize(Type targetType, string source)
        {
            XmlSerializer xs = new XmlSerializer(targetType);

            return xs.Deserialize(new StringReader(source));
        }

        /// <summary>
        /// Deserializes the specified target type.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="inputStream">The input stream.</param>
        /// <returns></returns>
        public object Deserialize(Type targetType, System.IO.Stream inputStream)
        {
            XmlSerializer xs = new XmlSerializer(targetType);

            return xs.Deserialize(inputStream);
        }

        /// <summary>
        /// Gets the value to set for the content-type header in the http response.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType
        {
            get { return "text/xml"; }
        }

        /// <summary>
        /// Gets the name of the format.
        /// </summary>
        /// <value>The name of the format.</value>
        public string FormatName
        {
            get { return "Xml"; }
        }

        #endregion
    }
}
