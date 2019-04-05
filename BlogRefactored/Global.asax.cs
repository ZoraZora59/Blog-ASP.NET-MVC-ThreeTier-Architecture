using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
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

			//自动建立数据库，慎用
			using (var blogdbcontext = new BlogContext())
			{
				blogdbcontext.Database.CreateIfNotExists();
			}

			//AutoFac依赖注入
			var container = BlogContainer.GetContainer();
			ControllerBuilder.Current.SetControllerFactory(new BlogControllerFactory(container));
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
			//DependencyResolver.SetResolver(new BlogDependencyResolver(container));

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new RazorViewEngine());

		}

        protected void Session_End()
        {
            Hashtable SingleOnline = (Hashtable)System.Web.HttpContext.Current.Application["Online"];
            if (SingleOnline != null && SingleOnline[User.Identity.Name] != null)
            {
                SingleOnline.Remove(Session.SessionID);
                System.Web.HttpContext.Current.Application.Lock();
                System.Web.HttpContext.Current.Application["Online"] = SingleOnline;
                System.Web.HttpContext.Current.Application.UnLock();
            }
            Session.Abandon();
        }
    }
	public static class BlogContainer
	{
		public static IContainer GetContainer()//创建容器
		{
			var builder = new ContainerBuilder();

			//将组件加入容器
			builder.RegisterType<BlogBLL.BlogManager>();
			builder.RegisterType<BlogBLL.BlogGuests>();
			builder.RegisterType<BlogBLL.BlogSide>();
			builder.RegisterControllers(Assembly.GetExecutingAssembly());
			Console.WriteLine("成功运行依赖注入AutoFac");
			var containner = builder.Build();
			return containner;
		}
	}
	public class BlogControllerFactory : DefaultControllerFactory//Controller创建工厂
	{
		private readonly ILifetimeScope _container;
		public BlogControllerFactory(ILifetimeScope container)
		{
			_container = container;
		}
		protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
		{
			try
			{
				return (IController)_container.Resolve(controllerType);
			}
			catch
			{ }
			return base.GetControllerInstance(requestContext, controllerType);
		}
	}

   
	/*public class BlogDependencyResolver : IDependencyResolver//弃用的DependencyResolver
	{
		private readonly ILifetimeScope _container;

		//生存周期
		//InstancePerLifetimeScope：同一个Lifetime生成的对象是同一个实例
		//SingleInstance：单例模式，每次调用，都会使用同一个实例化的对象；每次都用同一个对象；
		//InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；

		public BlogDependencyResolver(ILifetimeScope container)
		{
			_container = container;
		}
		public object GetService(Type serviceType)
		{
			try
			{
				var instance = _container.Resolve(serviceType);
				return instance;
			}
			catch
			{
				return null;
			}
		}
		public IEnumerable<object> GetServices(Type serviceType)
		{
			try
			{
				var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
				var instance = _container.Resolve(enumerableServiceType);
				return (IEnumerable<object>)instance;
			}
			catch//不确定
			{
				return null;
			}
		}
	}*/
}
