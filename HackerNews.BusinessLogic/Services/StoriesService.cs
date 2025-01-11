using AutoMapper;
using HackerNews.BusinessLogic.Models;
using HackerNews.Common.Constants;
using HackerNews.Common.Exceptions;
using HackerNews.Infrastructure.Configurations;
using HackerNews.SecondaryAdapters.Interfaces;
using HackerNews.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace HackerNews.BusinessLogic.Services
{
    /// <summary>
    /// The stories service.
    /// </summary>
    public class StoriesService : IStoriesService
    {
        private readonly INewsAdapter _newsAdapter;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoriesService"/> class.
        /// </summary>
        /// <param name="newsAdapter">The news adapter.</param>
        /// <param name="mapper">The mapper.</param>
        public StoriesService(INewsAdapter newsAdapter, IMapper mapper, IMemoryCache cache, IOptions<CacheSettingsConfigurationSection> cacheSettings)
        {
            _newsAdapter = newsAdapter;
            _mapper = mapper;
            _cache = cache;
            _cacheDuration = TimeSpan.FromMinutes(cacheSettings.Value.CacheDurationInMinutes);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StoryDto>> GetBestStoriesAsync(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }

            await CheckItemMaxId();

            var storyIds = await _newsAdapter.GetBestStoryIdsAsync();

            if (storyIds == null || !storyIds.Any())
            {
                throw new AppNotFoundApiException("No story ids found.");
            }

            if (count > storyIds.Count())
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be less than or equal to the number of stories available.");
            }

            var storyIdsToFetch = storyIds.Take(count);

            // We agreed that, based on the API we already have, stories are sorted by score in descending order
            var stories = await _newsAdapter.GetStoriesByIdsAsync(storyIdsToFetch);

            return _mapper.Map<IEnumerable<StoryDto>>(stories);
        }

        /// <summary>
        /// Checks the item max id.
        /// </summary>
        /// <returns>A Task.</returns>
        private async Task CheckItemMaxId()
        {
            var maxItemId = await _newsAdapter.GetMaxItemIdAsync();
            var cachedMaxItemId = _cache.Get<int?>(CacheKeys.MaxItemId);

            if (cachedMaxItemId == null || cachedMaxItemId != maxItemId)
            {
                // We can avoid clearing the cache for stories if it is not required to retrieve the actual count of comments
                RemoveStoriesFromCache();

                _cache.Remove(CacheKeys.BestStoryIds);
                _cache.Set(CacheKeys.MaxItemId, maxItemId, _cacheDuration);
            }
        }

        /// <summary>
        /// Removes the stories from cache.
        /// </summary>
        private void RemoveStoriesFromCache()
        {
            var possibleSavedStoryIds = _cache.Get<IEnumerable<int>>(CacheKeys.BestStoryIds);

            if (possibleSavedStoryIds == null) return;

            foreach (var id in possibleSavedStoryIds)
            {
                _cache.Remove(string.Format(CacheKeys.StoryIdFormat, id));
            }
        }
    }
}
