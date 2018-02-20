using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using XCentium.Challenge.Services;

namespace XCentium.Challenge
{
	/// <summary>
	/// Register this site's DI mappings here. You can then use constructor injection to use them in controllers.
	/// </summary>
	public class NinjectDependencyResolver : IDependencyResolver
	{
		readonly IKernel kernel;
		public NinjectDependencyResolver()
		{
			kernel = new StandardKernel();
			AddBindings();
		}

		public object GetService(Type serviceType)
		{
			return kernel.TryGet(serviceType);
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			return kernel.GetAll(serviceType);
		}

		private void AddBindings()
		{
			kernel.Bind<IScraperService>().To<ScraperService>().InTransientScope();
		}
	}
}