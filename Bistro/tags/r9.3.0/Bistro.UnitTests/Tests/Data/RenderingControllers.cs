using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers.OutputHandling;
using Bistro.UnitTests.Support;
using Bistro.Controllers.Descriptor;
using Bistro.Controllers;
using Bistro.Controllers.Descriptor.Data;

namespace Bistro.UnitTests.Tests.Data
{
    [Bind("/render", ControllerBindType = BindType.After)]
    [TemplateMapping("t")]
    public class TestRenderingController: RenderingController
    {
        protected override Type EngineType { get { return typeof(TestingRenderingEngine); } }
    }

    [Bind("/render/test1")]
    [RenderWith("template1.t")]
    public class Render1 : AbstractController
    {
        public override void DoProcessRequest(IExecutionContext context) { }
    }

    [Bind("/render/test2")]
    [RenderWith("template2.t", RenderType = RenderType.Partial)]
    public class Render2 : AbstractController
    {
        public override void DoProcessRequest(IExecutionContext context) { }
    }

    [Bind("/render/test3")]
    [RenderWith("template3.t", RenderType = RenderType.Full)]
    public class Render3 : AbstractController
    {
        public override void DoProcessRequest(IExecutionContext context) { }
    }
}
