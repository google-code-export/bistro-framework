
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers;
using Bistro.Controllers.Descriptor.Data;
using Bistro.Controllers.Descriptor;
using Bistro.Controllers.OutputHandling;

namespace Bistro.UnitTests.Tests.Data
{
    public class SimpleTuple { public string foo, baz; }

    [Bind("get /format")]
    public class FormatController : AbstractController
    {
        public override void DoProcessRequest(IExecutionContext context)
        {
            context.Response.Return(new SimpleTuple { foo = "bar", baz = "qux" });
        }
    }

    [Bind("post /format")]
    public class FormatController2 : AbstractController
    {
        [FormField]
        protected SimpleTuple input;

        public override void DoProcessRequest(IExecutionContext context)
        {
            if (input == null)
                return;

            context.Response.Return(input.foo + " " + input.baz);
        }
    }

    [Bind("get /format-xml")]
    public class FormatXmlController : AbstractController
    {
        public override void DoProcessRequest(IExecutionContext context)
        {
            context.Response.Return(new SimpleTuple { foo = "bar", baz = "qux" }, "Xml");
        }
    }

    [Bind("post /format-xml")]
    public class FormatXmlController2 : AbstractController
    {
        [FormField, FormatAs(Format.Xml)]
        protected SimpleTuple input;

        public override void DoProcessRequest(IExecutionContext context)
        {
            if (input == null)
                return;

            context.Response.Return(input.foo + " " + input.baz);
        }
    }

    [Bind("post /format-xml")]
    public class FormatXmlControllerError : AbstractController
    {
        [FormField,Request, FormatAs(Format.Xml)]
        protected SimpleTuple input;

        public override void DoProcessRequest(IExecutionContext context)
        {
            if (input == null)
                return;

            context.Response.Return(input.foo + " " + input.baz);
        }
    }


    [Bind("get /format/custom")]
    public class ResponseConfigurerController : AbstractController
    {
        [Request]
        protected ResponseConfigurer responseConfigurer;

        public override void DoProcessRequest(IExecutionContext context)
        {
            responseConfigurer =
                response =>
                {
                    response.AppendHeader("x-test-configurer", "true");
                    return response;
                };
        }
    }
}
