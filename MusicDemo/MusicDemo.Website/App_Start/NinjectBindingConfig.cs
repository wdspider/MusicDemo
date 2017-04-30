using AutoMapper;
using MusicDemo.Website.Backend.BackendProviders.Database;
using Ninject;

namespace MusicDemo.Website.App_Start
{
	public class NinjectBindingConfig
	{
		public static void Initialize(IKernel kernel)
		{
			// Initialize bindings
			kernel.Bind<IMapper>().ToMethod(m => AutoMapperConfig.Initialize()).InSingletonScope();

			// Add database backend provider bindings
			DBKernelBindings.Initialize(kernel);
		}
	}
}