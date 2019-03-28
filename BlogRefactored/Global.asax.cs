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
			//using (var blogDbContext = new BlogRepository.MSSQL.BlogContext())
			//{
			//	blogDbContext.Database.CreateIfNotExists();
			//}

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new RazorViewEngine());
			ViewEngines.Engines.Add(new WebFormViewEngine());
		}
	}
}
