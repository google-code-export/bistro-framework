namespace ExampleProject
{
	using System;
	using System.Web;
    using Castle.MonoRail.Framework.Routing;

	public class GlobalApplication : HttpApplication
	{
		public GlobalApplication()
		{
		}

		public void Application_OnStart()
		{
            RoutingModuleEx.Engine.Add(
            new PatternRoute("/<controller>/<id>/view.aspx")
                .DefaultForAction().Is("view"));

		}

		public void Application_OnEnd() 
		{
		}
	}
}
