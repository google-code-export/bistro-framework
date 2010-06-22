using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Bistro.Controllers
{
    /// <summary>
    /// IContext implementation for use in event invocations. 
    /// </summary>
    public class EventContext: DefaultContext
    {
        private bool allowSessionAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventContext"/> class.
        /// Supply a null context to prevent request object access.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="allowSessionAccess">if set to <c>false</c> accessing the Session property will throw an <c>InvalidOperationException</c>.</param>
        /// <param name="allowRequestAccess">if set to <c>true</c> [allow request access].</param>
        public EventContext(HttpContextBase context, bool allowSessionAccess)
            : base(context)
        {
            this.allowSessionAccess = allowSessionAccess;
        }

        /// <summary>
        /// Transfers execution to the given url. All currently queued controllers will finish execution. This operation is not allowed for event contexts.
        /// </summary>
        /// <param name="target"></param>
        public override void Transfer(string target)
        {
            throw new InvalidOperationException("Transfers are not permitted for event handlers");
        }

        /// <summary>
        /// Notifies the rendering engine how to render the results of the current request. This operation is not allowed for event contexts.
        /// </summary>
        /// <param name="target"></param>
        public override void RenderWith(string target)
        {
            throw new InvalidOperationException("Event handlers are not permitted to produce user output");
        }

        /// <summary>
        /// Returns an arbitrary string as the http response, with the supplied content type. This operation is not allowed for event contexts.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="contentType">the content-type header value to supply.</param>
        public override void ReturnFreeForm(string value, string contentType)
        {
            throw new InvalidOperationException("Event handlers are not permitted to produce user output");
        }

        /// <summary>
        /// Gets the response object. This operation is not allowed for event contexts.
        /// </summary>
        /// <value>The response.</value>
        public override IResponse Response
        {
            get
            {
                throw new InvalidOperationException("Event handlers are not permitted to produce user output");
            }
        }

        /// <summary>
        /// Gets the session. If created with the <c>allowSessionAccess</c> parameter set to false, this property will throw an <c>InvalidOperationException</c>.
        /// </summary>
        /// <value>The session.</value>
        public override HttpSessionStateBase Session
        {
            get
            {
                if (!allowSessionAccess)
                    return null;

                return base.Session;
            }
        }
    }
}
