using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using HackerNews.Infrastructure.Configurations;

namespace HackerNews.Infrastructure.Extensions
{
    /// <summary>
    /// The configuration extensions.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds the options to service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddConfigurationSections(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HackerNewsConfigurationSection>(options =>
                configuration.GetSection(HackerNewsConfigurationSection.SectionName).Bind(options));

            services.Configure<CacheSettingsConfigurationSection>(options =>
                configuration.GetSection(CacheSettingsConfigurationSection.SectionName).Bind(options));
        }
    }
}
