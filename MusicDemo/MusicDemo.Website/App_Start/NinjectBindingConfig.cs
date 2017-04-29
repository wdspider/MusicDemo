using AutoMapper;
using MusicDemo.Database;
using MusicDemo.Website.Backend;
using MusicDemo.Website.Backend.Database;
using Ninject;
using Ninject.Web.Common;

namespace MusicDemo.Website.App_Start
{
	public class NinjectBindingConfig
	{
		public static void Initialize(IKernel kernel)
		{
			// Initialize bindings
			kernel.Bind<IMapper>().ToMethod(m => AutoMapperConfig.Initialize()).InSingletonScope();

			kernel.Bind<MusicDemoDbContext>().ToSelf().InRequestScope();
			kernel.Bind<MusicDemoRepository>().ToSelf().InRequestScope();
			kernel.Bind<BackendProvider>().To<DBBackendProvider>().InRequestScope();
		}
	}
}