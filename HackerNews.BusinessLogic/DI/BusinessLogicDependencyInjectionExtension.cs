using HackerNews.BusinessLogic.Services;
using HackerNews.Services.Interfaces;
using HackerNews.SecondaryAdapters.DI;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.BusinessLogic.DI
{
    /// <summary>
    /// The business logic dependency injection extension.
    /// </summary>
    public static class BusinessLogicDependencyInjectionExtension
    {

        /// <summary>
        /// Adds the secondary adapters services.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        public static void AddBusinessLogicServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStoriesService, StoriesService>();

            serviceCollection.AddSecondaryAdaptersServices();
        }
    }
}
