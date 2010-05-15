using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers;
using System.Web;
using Bistro.Controllers.OutputHandling;
using Bistro.Controllers.Descriptor.Data;
using Bistro.Controllers.Descriptor;
using System.Reflection;

namespace Bistro.Extensions.OutputHandling
{
    /// <summary>
    /// Controller for determining whether an incoming GET or POST is an AJAX 
    /// request or not. This controller will populate <code>isAjaxRequest</code>
    /// and <code>renderType</code> onto the request context based on the value 
    /// of the <code>X-Requested-With</code> http header.
    /// </summary>
    [Bind("get ?")]
    [Bind("post ?")]
    public class AjaxDeterminer: IController
    {
        [Request]
        protected bool isAjaxRequest;

        [Request]
        protected RenderType renderType;

        public void ProcessRequest(HttpContextBase context, IContext requestContext)
        {
            isAjaxRequest = context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            renderType = isAjaxRequest ? RenderType.Partial : RenderType.Full;
        }

        public bool Reusable { get { return true; } }
        public bool Stateful { get { return true; } }
        public void Initialize() { }
        public void Recycle() { }
        public MemberInfo GlobalHandle { get { return GetType(); } }
    }
}
