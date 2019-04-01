using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;

namespace BlogRefactored.IOC
{
	public class BlogControllerFactory:DefaultControllerFactory//Controller创建工厂
	{
		private readonly ILifetimeScope _container;
		public BlogControllerFactory(ILifetimeScope container)
		{
			_container = container;
		}
		protected override IController GetControllerInstance(RequestContext requestContext,Type controllerType)
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
	public class BlogDependencyResolver:IDependencyResolver
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
	}
}