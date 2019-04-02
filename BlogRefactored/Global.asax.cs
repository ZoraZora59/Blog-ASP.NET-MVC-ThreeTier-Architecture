using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

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

			//自动建立数据库，慎用
			//using (var blogdbcontext = new BlogDAL.BlogContext())
			//{
			//	blogdbcontext.Database.CreateIfNotExists();
			//}

			//AutoFac依赖注入
			var container = IOC.BlogContainer.GetContainer();
			ControllerBuilder.Current.SetControllerFactory(new IOC.BlogControllerFactory(container));
			DependencyResolver.SetResolver(new IOC.BlogDependencyResolver(container));

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new RazorViewEngine());

		}
	}
}
