namespace HackerNews.Infrastructure.Configurations
{
    /// <summary>
    /// The cache settings configuration section.
    /// </summary>
    public class CacheSettingsConfigurationSection
    {
        /// <summary>
        /// The section name.
        /// </summary>
        public const string SectionName = "CacheSettings";

        /// <summary>
        /// Gets or sets the cache duration in minutes.
        /// </summary>
        required public int CacheDurationInMinutes { get; set; }
    }
}