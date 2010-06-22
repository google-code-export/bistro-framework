using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers;
using Bistro.Controllers.Descriptor;
using Bistro.Controllers.Descriptor.Data;
using Bistro.Validation;
using Bistro.Extensions.Validation.Common;
using Bistro.Extensions.Validation;
using System.Text.RegularExpressions;

namespace Bistro.UnitTests
{
    [Bind(Application.ApplicationStartup)]
    public class StartupController : AbstractController
    {
        public static int hitcount = 0;

        public override void DoProcessRequest(IExecutionContext context)
        {
            hitcount++;
        }
    }

    [Bind(Application.UnhandledException)]
    public class ExceptionController : AbstractController
    {
        [Request]
        protected Exception unhandledException;

        public override void DoProcessRequest(IExecutionContext context)
        {
            context.Response.Return("exception " + unhandledException.Message);
        }
    }

    [Bind("get /exception")]
    public class ExceptionTestController : AbstractController
    {
        public override void DoProcessRequest(IExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }

    [Bind("event /eventtest")]
    public class EventController : AbstractController
    {
        public static int hitcount = 0;

        public override void DoProcessRequest(IExecutionContext context)
        {
            hitcount++;
        }
    }



    [Bind("/")]
    public class HomeUrlController1 : AbstractController
    {
        public override void DoProcessRequest(IExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }

    [Bind("?/parameters?{parm1}&{parm2}")]
    public class ParameterTest : AbstractController
    {
        public string parm1, parm2;

        [Request]
        public string output;

        public override void DoProcessRequest(IExecutionContext context)
        {
            output = parm1 + ":" + parm2;
        }
    }

    [Bind("?/parameters2/{parm1}?{parm2}")]
    public class ParameterTest2 : AbstractController
    {
        public string parm1, parm2;

        [Request]
        public string output;

        public override void DoProcessRequest(IExecutionContext context)
        {
            output = parm1 + ":" + parm2;
        }
    }

    [Bind("/parameters3/??{parm1}&{parm2}")]
    public class ParameterTest3 : AbstractController
    {
        public string parm1, parm2;

        [Request]
        public string output;

        public override void DoProcessRequest(IExecutionContext context)
        {
            output = parm1 + ":" + parm2;
        }
    }
    //
    // This test will invalidate other tests by appending another global controller
    // to the execution chain. uncomment only when necessary
    //
    //[Bind("??{parm1}&{parm2}")]
    //public class ParameterTest4 : AbstractController
    //{
    //    public string parm1, parm2;

    //    [Request]
    //    public string outputForRoot;

    //    public override void DoProcessRequest(IExecutionContext context)
    //    {
    //        outputForRoot = parm1 + ":" + parm2;
    //    }
    //}

    [Bind("/")]
    public class HomeUrlController2 : AbstractController
    {
        public override void DoProcessRequest(IExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }

    [Bind("/hello/?/you")]
    public class HelloYouController1 : AbstractController
    {
        public override void DoProcessRequest(IExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }

    [Bind("/hello/*/you")]
    public class HelloYouController2 : AbstractController
    {
        public override void DoProcessRequest(IExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
    [Bind("/order/world/new")]
    public class OrderController1 : AbstractController
    {   [Session]
        public string c1;
        [Session, Requires]
        public string c2;
        
        public override void DoProcessRequest(IExecutionContext context)
        {
            c1 = c2 + "1";
        }
    }
    [Bind("/order/world/new")]
    public class OrderController2 : AbstractController
    {
        [Session]
        public string c2;
        [Session, Requires]
        public string c5;

        public override void DoProcessRequest(IExecutionContext context)
        {
            c2 = c5 + "2";
        }
    }
    [Bind("/order/world/new")]
    public class OrderController3 : AbstractController
    {
        [Session]
        public string c3;

        [Session, Requires]
        public string c2;
        [Session, Requires]
        public string c4;
        [Session, Requires]
        public string c5;

        public override void DoProcessRequest(IExecutionContext context)
        {
            c3 = c2 + c4 + c5 + "3";
        }
    }
    [Bind("/order/world/new")]
    public class OrderController4 : AbstractController
    {
        [Session]
        public string c4;

        [Session, Requires]
        public string c1;
        [Session, Requires]
        public string c2;
        [Session, Requires]
        public string c5;

        public override void DoProcessRequest(IExecutionContext context)
        {
            c4 = c1 + c2 + c5 + "4";
        }
    }
    [Bind("/order/world/new")]
    public class OrderController5 : AbstractController
    {
        [Session]
        public string c5;

        public override void DoProcessRequest(IExecutionContext context)
        {
            c5 = "5";
        }
    }
    [Bind("/order/world/new")]
    public class OrderController6 : AbstractController
    {
        [Session]
        public string c6;
        [Session, Requires]
        public string c3;
        [Session, Requires]
        public string c7;


        public override void DoProcessRequest(IExecutionContext context)
        {
            c6 = c3 + c7 + "6";
        }
    }
    [Bind("/order/world/new")]
    public class OrderController7 : AbstractController
    {
        [Session]
        public string c7;

        public override void DoProcessRequest(IExecutionContext context)
        {
            c7 = "7";
        }
    }
    
    [Bind("/one_little_url")]
    public class littleController1 : AbstractController
    {
        [Request]
        public string l1;
        [Request, Requires]
        public string l2;

        public override void DoProcessRequest(IExecutionContext context)
        {
            l1 = "1" + l2;
        }
    }
    [Bind("/one_little_url")]
    public class littleController2 : AbstractController
    {
        [Request]
        public string l2;

        public override void DoProcessRequest(IExecutionContext context)
        {
            l2 = "2";
        }
    }
    [Bind("/little_url/more")]
    public class littleController3 : AbstractController
    {
        [Request]
        public string l3;

        public override void DoProcessRequest(IExecutionContext context)
        {
            l3 = "3";
        }
    }
    [Bind("/little_url/more")]
    public class littleController4 : AbstractController
    {
        [Request]
        public string l4;
        [Request, Requires]
        public string l3;
        [Request, Requires]
        public string l5;

        public override void DoProcessRequest(IExecutionContext context)
        {
            l4 = "4" + l3 + l5;
        }
    }
    [Bind("/little_url/more")]
    public class littleController5 : AbstractController
    {
        [Request]
        public string l5;
        [Request, Requires]
        public string l3;

        public override void DoProcessRequest(IExecutionContext context)
        {
            l5 = "5" + l3;
        }
    }
}
