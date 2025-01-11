using HackerNews.Infrastructure.Configurations;
using HackerNews.SecondaryAdapters.Adapters;
using HackerNews.SecondaryAdapters.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HackerNews.SecondaryAdapters.DI
{
    /// <summary>
    /// The secondary adapters dependency injection extension.
    /// </summary>
    public static class SecondaryAdaptersDependencyInjectionExtension
    {
        /// <summary>
        /// Adds the secondary adapters services.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        public static void AddSecondaryAdaptersServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHttpClient<INewsAdapter, HackerNewsAdapter>((serviceProvider, client) =>
            {
                var hackerNewsConfiguration = serviceProvider.GetRequiredService<IOptions<HackerNewsConfigurationSection>>().Value;

                if (hackerNewsConfiguration != null && !string.IsNullOrEmpty(hackerNewsConfiguration.BaseAddress))
                {
                    client.BaseAddress = new Uri(hackerNewsConfiguration.BaseAddress);
                }
            });
        }
    }
}
