using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers.OutputHandling;
using System.Web;
using Bistro.Controllers;

namespace Bistro.UnitTests.Support
{
    public class TestingRenderingEngine: TemplateEngine
    {
        public TestingRenderingEngine(IHttpHandler handler) { }

        public override void Render(HttpContextBase httpContext, IContext requestContext, string target)
        {
            httpContext.Response.Write(string.Format("Rendering {0}", target));
        }
    }
}
