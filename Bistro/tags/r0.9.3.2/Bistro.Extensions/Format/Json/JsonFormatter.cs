using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers.OutputHandling;
using Newtonsoft.Json;
using System.IO;

namespace Bistro.Extensions.Format.Json
{
    /// <summary>
    /// Formatter implementation that provides Json serialization through Newtonsoft libraries.
    /// </summary>
    public class JsonFormatter: IWebFormatter
    {
        /// <summary>
        /// The underlying serializer
        /// </summary>
        JsonSerializer serializer = new JsonSerializer();

        /// <summary>
        /// Serializes the specified object graph to the given stream.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="outputStream">The output stream.</param>
        public void Serialize(object graph, System.IO.Stream outputStream)
        {
            serializer.Serialize(new StreamWriter(outputStream), graph);
        }

        /// <summary>
        /// Serialzies the specified object graph into a string.
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <returns></returns>
        public string Serialize(object graph)
        {
            StringWriter sw = new StringWriter();

            serializer.Serialize(sw, graph);

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
            return serializer.Deserialize(new StringReader(source), targetType);
        }

        /// <summary>
        /// Deserializes the specified target type.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="inputStream">The input stream.</param>
        /// <returns></returns>
        public object Deserialize(Type targetType, System.IO.Stream inputStream)
        {
            return serializer.Deserialize(new StreamReader(inputStream), targetType);
        }

        /// <summary>
        /// Gets the value to set for the content-type header in the http response.
        /// </summary>
        /// <value>The type of the content.</value>
        public string ContentType
        {
            get { return "application/json"; }
        }

        /// <summary>
        /// Gets the name of the format.
        /// </summary>
        /// <value>The name of the format.</value>
        public string FormatName
        {
            get { return "Json"; }
        }
    }
}
