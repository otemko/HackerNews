using HackerNews.SecondaryAdapters.Models;

namespace HackerNews.SecondaryAdapters.Interfaces
{
    /// <summary>
    /// The news service.
    /// </summary>
    public interface INewsAdapter
    {
        /// <summary>
        /// Gets the best story ids.
        /// </summary>
        /// <returns>Best story ids.</returns>
        Task<IEnumerable<int>> GetBestStoryIdsAsync();

        /// <summary>
        /// Gets the stories by ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>Stories.</returns>
        Task<IEnumerable<StoryApiModel>> GetStoriesByIdsAsync(IEnumerable<int> ids);

        /// <summary>
        /// Gets the max item id.
        /// </summary>
        /// <returns>A Task.</returns>
        Task<int> GetMaxItemIdAsync();
    }
}
