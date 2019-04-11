using System;
using System.Collections;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using BlogDAL;

namespace BlogRefactored
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			//自动建立数据库
			using (var blogdbcontext = new BlogContext())
			{
				blogdbcontext.Database.CreateIfNotExists();
			}

			//AutoFac依赖注入
			var container = App_Start.AutofacConfig.BlogContainer.GetContainer();
			//ControllerBuilder.Current.SetControllerFactory(new BlogControllerFactory(container));
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
			//DependencyResolver.SetResolver(new BlogDependencyResolver(container));

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new RazorViewEngine());

		}
		
    }
	
}
