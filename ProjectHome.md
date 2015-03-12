## Bistro :: Don't hide the web ##

Out of the box, bistro lets you get to work with almost no configuration. You get

  * An attributed, default-first controller definition model (that means if you have a simple app, bistro'll understand on it's own. if you don't you can give bistro more info)
  * Integration with the first .NET port of the django templating language, NDjango
  * A comprehensive, up-to-date set of documentation and reference guides (right here!), with
    * Samples
    * Reference guides
    * Quickstarts
  * Preconfigured templates for Visual Studio.NET '08, including
    * Project templates for Bistro + NDjango, Bistro + NDjango + NHibernate
    * File templates for Bistro controllers and NDjango templates
  * Add-in for for MS Visual Studio (Supplied in a separate package with Workflow Server 5.0.1.2)

Pop over to the [the bistro wiki](http://www.bistroframework.org/) for more info.


## Getting started ##
One of the main design considerations for bistro is the "no-config hello world" - the idea that you should only need to set a minimum of options and jump through a minimum number of hoops to just get going. Here we'll show you the bare minimum you need.

Out of the box, you get bistro - the controller tier and the glue that lets you plug in a view and model. The default view in most of the examples on this wiki is http://www.ndjango.org.

Also, if you opt to use the MSI package to install Bistro, the "New Bistro Project" option that becomes available for C# does all of these steps for you.

### the bare minimum ###

To get going, all you have to do is plug in the bistro module

To get the module working with the Visual Studio Development server, add this to your web.config, under configuration/system.web (other configurations can be found [here](http://bistroframework.org/index.php?title=Reference:web.config#Modules_and_handlers))

```
<httpModules>
	<add type="Bistro.Http.Module, Bistro.Core" name="BistroModule" />
</httpModules>
```


... aaand you're done. Okay, now for the fun part. Fire up your favorite dev environment (assuming you weren't there to begin with), add a file called "Test.cs", and drop this into it:

```
using System;
using Bistro.Controller;
using Bistro.Controller.Descriptor.Data;

namespace Test
{
    [RenderWith("default.django")]
    public class Default : AbstractController
    {
        [Request]
        protected string message;

        public override void DoProcessRequest(IExecutionContext context)
        {
            message = "hello world!";
        }
    }
}
```


Keeping with the defaults for everything concept, this controller will, by convention, respond to /Test/Default (taking the full class name as the bind point). All we need now is our template. If you look at the code, the template it's asking for is "default.django". Go ahead and create a file with that name, and open it up. Inside put in the following

```
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>my first bistro app!!!!!1!!ONE!</title>
</head>
<body>
{{ message }}
</body>
```

### done! ###
But we're just scratching the surface. Here are some of the other things you can do...

**Think resources and actions, not URLs and controllers**

We like REST, and we don't care who knows it. We like it so much, that we think that when you build an application, you should think of it as a collection of resources that are consumed by the browser. The application exposes an interface that can be invoked through GET or POST HTTP methods, each URL it understands being a distinct resource. That puts more control in the browser, and less navigational complexity and interdependence in the server.

**Define smart URLs and small controllers**

Use [Bind](http://bistroframework.org/index.php?title=Reference:Bind) attributes to specify what incoming requests should trigger what controllers. We don't map different actions to different methods of a single class because we think that each action shouldn't be tightly coupled with another. That way, if you want to do a `/search` and also want to `/search/bytag/stuff`, your `bytag` controller does need to know how to perform a search, and your `search` controller does need to know how to figure out what tags you want to search by.

**Build your security policy where you use it**

Using a deny-first security policy and [Security Controllers](http://bistroframework.org/index.php?title=Reference:SecurityController), you can specify security policy alongside the code that will need protecting, not in an external config file.

**Build loosely coupled chains of controllers**

Since a single request can be processed by multiple controllers, we have a sophisticated, but easy to use system of specifying the order things run in. [Session](http://bistroframework.org/index.php?title=Reference:Session) and [Request](http://bistroframework.org/index.php?title=Reference:Request) scoping attributes along with [DependsOn](http://bistroframework.org/index.php?title=Reference:DependsOn) on fields tell the system what the implicit interdependencies are, without tightly coupling controllers. This way, the runtime knows what things need to happen in order, and wings it on the rest.