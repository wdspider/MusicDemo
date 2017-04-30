using AutoMapper;
using MusicDemo.Website.Backend.BackendProviders.Database;
using MusicDemo.Website.ViewModels;

namespace MusicDemo.Website.App_Start
{
	public class AutoMapperConfig
	{
		public static IMapper Initialize()
		{
			// Initialize the mapping profiles
			MapperConfiguration mappingConfig = new MapperConfiguration(config =>
			{
				config.AddProfile<ViewModelMappingProfile>();
				config.AddProfile<DBModelMappingProfile>();
			});

			// Verify config is correct
			mappingConfig.AssertConfigurationIsValid();

			// Return mapper
			return mappingConfig.CreateMapper();
		}
	}
}