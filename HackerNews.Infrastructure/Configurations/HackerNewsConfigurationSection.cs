namespace HackerNews.Infrastructure.Configurations
{
    /// <summary>
    /// The hacker news configuration section.
    /// </summary>
    public class HackerNewsConfigurationSection
    {
        /// <summary>
        /// The section name.
        /// </summary>
        public const string SectionName = "HackerNews";

        /// <summary>
        /// Gets or sets the base address.
        /// </summary>
        required public string BaseAddress { get; set; }
    }
}