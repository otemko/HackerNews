using HackerNews.BusinessLogic.Models;

namespace HackerNews.Services.Interfaces
{
    /// <summary>
    /// The stories service interface.
    /// </summary>
    public interface IStoriesService
    {
        /// <summary>
        /// Gets the best stories async.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>The best storeis.</returns>
        Task<IEnumerable<StoryDto>> GetBestStoriesAsync(int count);
    }
}
