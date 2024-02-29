using HackerNews.Core.Domain;
using HackerNews.Core.Mapping;
using HackerNews.Core.ServiceContracts;
using HackerNews.Core.Services;

namespace HackerNewsApi.StartupExtensions
{
	public static class ConfigureServicesExtension
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			//Add AutoMapper
			services.AddAutoMapper(typeof(MappingProfile).Assembly);

			// Add HttpClient
			services.AddHttpClient<IHackerNewsService, HackerNewsService>();

			//Using IOption pattern to get it from settings
			services.Configure<StoryOptions>(configuration.GetSection("Story"));

			// Add caching
			services.AddMemoryCache();

			// Add services into IoC container
			services.AddScoped<IHackerNewsService, HackerNewsService>();

			return services;
		}
	}
}
