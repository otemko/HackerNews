namespace HackerNews.SecondaryAdapters.Models
{
    /// <summary>
    /// The story api model.
    /// </summary>
    public class StoryApiModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the url.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public int? Score { get; set; } = null;

        /// <summary>
        /// Gets or sets the by.
        /// </summary>
        public string By { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        public long? Time { get; set; } = null;

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        public IEnumerable<int> Kids { get; set; } = new List<int>();
    }
}
