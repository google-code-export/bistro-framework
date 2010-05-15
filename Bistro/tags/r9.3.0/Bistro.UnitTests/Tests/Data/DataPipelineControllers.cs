using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers;
using Bistro.Controllers.Descriptor;
using Bistro.Controllers.Descriptor.Data;

namespace Bistro.UnitTests
{
    [Bind("get /pipeline")]
    public class First : AbstractController
    {
        [Request]
        public string Data;

        public override void DoProcessRequest(IExecutionContext context)
        {
            Data += "First";
        }
    }

    [Bind("get /pipeline/full")]
    public class Second : AbstractController
    {
        [Request, DependsOn, Provides("SecondData")]
        public string Data;

        public override void DoProcessRequest(IExecutionContext context)
        {
            Data += "Second";
        }
    }

    [Bind("get /pipeline")]
    public class Third : AbstractController
    {
        [Request, DependsOn, DependsOn("SecondData")]
        public string Data;

        public override void DoProcessRequest(IExecutionContext context)
        {
            Data += "Third";
        }
    }
}
