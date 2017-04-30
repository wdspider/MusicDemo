using MusicDemo.Database;
using Ninject;
using Ninject.Web.Common;

namespace MusicDemo.Website.Backend.BackendProviders.Database
{
	public class DBKernelBindings
	{
		public static void Initialize(IKernel kernel)
		{
			// Initialize bindings for database backend provider
			kernel.Bind<MusicDemoDbContext>().ToSelf().InRequestScope();
			kernel.Bind<MusicDemoRepository>().ToSelf().InRequestScope();
			kernel.Bind<BackendProvider>().To<DBBackendProvider>().InRequestScope();
		}
	}
}
