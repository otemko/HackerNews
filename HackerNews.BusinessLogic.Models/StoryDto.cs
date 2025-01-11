namespace HackerNews.BusinessLogic.Models
{
    /// <summary>
    /// The story dto.
    /// </summary>
    public class StoryDto
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        public string Uri { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the posted by.
        /// </summary>
        public string PostedBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        public DateTime? Time { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// Gets or sets the comment count.
        /// </summary>
        public int? CommentCount { get; set; }
    }
}
