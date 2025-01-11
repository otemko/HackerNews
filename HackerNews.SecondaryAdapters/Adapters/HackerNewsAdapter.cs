using HackerNews.Common.Constants;
using HackerNews.Common.Exceptions;
using HackerNews.Infrastructure.Configurations;
using HackerNews.SecondaryAdapters.Interfaces;
using HackerNews.SecondaryAdapters.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HackerNews.SecondaryAdapters.Adapters
{
    /// <summary>
    /// The hacker news adapter.
    /// </summary>
    internal class HackerNewsAdapter : INewsAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="HackerNewsAdapter"/> class.
        /// </summary>
        /// <param name="httpClient">The http client.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="cacheSettings">The cache settings.</param>
        public HackerNewsAdapter(HttpClient httpClient, IMemoryCache cache, IOptions<CacheSettingsConfigurationSection> cacheSettings)
        {
            _httpClient = httpClient;
            _cache = cache;
            _cacheDuration = TimeSpan.FromMinutes(cacheSettings.Value.CacheDurationInMinutes);
        }

        /// <summary>
        /// Gets the max item id.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task<int> GetMaxItemIdAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("maxitem.json");

                if (string.IsNullOrEmpty(response))
                {
                    throw new AppExteranalApiException("No max id data returned from the hacker news api.");
                }

                return JsonConvert.DeserializeObject<int>(response);
            }
            catch (HttpRequestException ex)
            {
                throw new AppExteranalApiException("Error occurred while fetching best stories.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<int>> GetBestStoryIdsAsync()
        {
            try
            {
                return await _cache.GetOrCreateAsync(CacheKeys.BestStoryIds, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                    var response = await _httpClient.GetStringAsync("beststories.json");

                    if (string.IsNullOrEmpty(response))
                    {
                        throw new AppExteranalApiException("No data returned from the hacker news api.");
                    }

                    return JsonConvert.DeserializeObject<IEnumerable<int>>(response);
                });
            }
            catch (HttpRequestException ex)
            {
                throw new AppExteranalApiException("Error occurred while fetching best stories.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<StoryApiModel>> GetStoriesByIdsAsync(IEnumerable<int> ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            if (!ids.Any())
            {
                throw new AppNotFoundApiException("No story ids found.");
            }

            var tasks = ids.Select(GetStoryById);
            return await Task.WhenAll(tasks);
        }


        /// <summary>
        /// Gets the story by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>A story.</returns>
        private async Task<StoryApiModel> GetStoryById(int id)
        {
            var cacheKey = string.Format(CacheKeys.StoryIdFormat, id);
            try
            {
                return await _cache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = _cacheDuration;
                    var response = await _httpClient.GetStringAsync($"item/{id}.json");
                    return JsonConvert.DeserializeObject<StoryApiModel>(response);
                });
            }
            catch (HttpRequestException ex)
            {
                throw new AppExteranalApiException($"Error occurred while fetching story with id: {id}.", ex);
            }
        }
    }
}
