using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;

namespace BlogRefactored.IOC
{
	public static class BlogContainer
	{
		public static IContainer GetContainer()//创建容器
		{
			var builder = new ContainerBuilder();

			//将组件加入容器
			builder.RegisterType<BlogBLL.BlogBLL>();
			builder.RegisterType<BlogBLL.BlogManager>();
            builder.RegisterType<BlogBLL.BlogGuests>();

			builder.RegisterType<Controllers.ManageController>();
			builder.RegisterType<Controllers.HomeController>();

			var containner = builder.Build();
			return containner;
		}
	}
}