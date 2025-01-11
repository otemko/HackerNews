using AutoMapper;
using HackerNews.BusinessLogic.Models;
using HackerNews.BusinessLogic.Services;
using HackerNews.Common.Constants;
using HackerNews.Common.Exceptions;
using HackerNews.Infrastructure.Configurations;
using HackerNews.SecondaryAdapters.Interfaces;
using HackerNews.SecondaryAdapters.Models;
using Microsoft.Extensions.Options;
using Moq;

namespace HackerNews.Tests.Services
{
    [TestFixture]
    public class StoriesServiceTests
    {
        private Mock<INewsAdapter> _newsAdapterMock;
        private Mock<IMapper> _mapperMock;
        private TestCustomMemoryCache _cache;
        private Mock<IOptions<CacheSettingsConfigurationSection>> _cacheSettingsMock;
        private StoriesService _storiesService;
        private CacheSettingsConfigurationSection _cacheSettings;

        [SetUp]
        public void SetUp()
        {
            _newsAdapterMock = new Mock<INewsAdapter>();
            _mapperMock = new Mock<IMapper>();
            _cache = new TestCustomMemoryCache();
            _cacheSettingsMock = new Mock<IOptions<CacheSettingsConfigurationSection>>();

            _cacheSettings = new CacheSettingsConfigurationSection { CacheDurationInMinutes = 60 };
            _cacheSettingsMock.Setup(x => x.Value).Returns(_cacheSettings);

            _storiesService = new StoriesService(_newsAdapterMock.Object, _mapperMock.Object, _cache, _cacheSettingsMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _cache.Dispose();
        }

        [Test]
        public async Task GetBestStoriesAsync_ShouldThrowArgumentOutOfRangeException_WhenCountIsLessThanOrEqualToZero()
        {
            // Arrange
            int count = 0;

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _storiesService.GetBestStoriesAsync(count));
            Assert.That(ex.ParamName, Is.EqualTo("count"));
        }

        [Test]
        public async Task GetBestStoriesAsync_ShouldThrowAppNotFoundApiException_WhenNoStoryIdsFound()
        {
            // Arrange
            int count = 10;
            _newsAdapterMock.Setup(x => x.GetBestStoryIdsAsync()).ReturnsAsync((IEnumerable<int>)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<AppNotFoundApiException>(() => _storiesService.GetBestStoriesAsync(count));
            Assert.That(ex.Message, Is.EqualTo("No story ids found."));
        }

        [Test]
        public async Task GetBestStoriesAsync_ShouldThrowArgumentOutOfRangeException_WhenCountIsGreaterThanStoryIdsCount()
        {
            // Arrange
            int count = 10;
            var storyIds = new List<int> { 1, 2, 3 };
            _newsAdapterMock.Setup(x => x.GetBestStoryIdsAsync()).ReturnsAsync(storyIds);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _storiesService.GetBestStoriesAsync(count));
            Assert.That(ex.ParamName, Is.EqualTo("count"));
        }

        [Test]
        public async Task GetBestStoriesAsync_ShouldReturnMappedStories_WhenValidCountIsProvided()
        {
            // Arrange
            int count = 2;
            var storyIds = new List<int> { 1, 2, 3 };
            var stories = new List<StoryApiModel> { new StoryApiModel(), new StoryApiModel() };
            var storyDtos = new List<StoryDto> { new StoryDto(), new StoryDto() };

            _newsAdapterMock.Setup(x => x.GetBestStoryIdsAsync()).ReturnsAsync(storyIds);
            _newsAdapterMock.Setup(x => x.GetStoriesByIdsAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(stories);
            _mapperMock.Setup(x => x.Map<IEnumerable<StoryDto>>(stories)).Returns(storyDtos);

            // Act
            var result = await _storiesService.GetBestStoriesAsync(count);

            // Assert
            Assert.That(result, Is.EqualTo(storyDtos));
        }

        [Test]
        public async Task GetBestStoriesAsync_ShouldCallRemoveStoriesFromCache_WhenMaxItemIdIsDifferent()
        {
            // Arrange
            int count = 2;
            var maxItemId = 100;
            var cachedMaxItemId = 50;
            var storyIds = new List<int> { 1, 2, 3 };
            var stories = new List<StoryApiModel> { new StoryApiModel(), new StoryApiModel() };
            var storyDtos = new List<StoryDto> { new StoryDto(), new StoryDto() };

            _newsAdapterMock.Setup(x => x.GetMaxItemIdAsync()).ReturnsAsync(maxItemId);
            _cache.CreateEntry(CacheKeys.MaxItemId).Value = cachedMaxItemId;
            _newsAdapterMock.Setup(x => x.GetBestStoryIdsAsync()).ReturnsAsync(storyIds);
            _newsAdapterMock.Setup(x => x.GetStoriesByIdsAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(stories);
            _mapperMock.Setup(x => x.Map<IEnumerable<StoryDto>>(stories)).Returns(storyDtos);

            // Act
            var result = await _storiesService.GetBestStoriesAsync(count);

            // Assert
            Assert.That(result, Is.EqualTo(storyDtos));
            Assert.That(_cache.TryGetValue(CacheKeys.MaxItemId, out var _), Is.True);
        }

        [Test]
        public async Task GetBestStoriesAsync_ShouldNotCallRemoveStoriesFromCache_WhenMaxItemIdIsSame()
        {
            // Arrange
            int count = 2;
            var maxItemId = 100;
            var cachedMaxItemId = 100;
            var storyIds = new List<int> { 1, 2, 3 };
            var stories = new List<StoryApiModel> { new StoryApiModel(), new StoryApiModel() };
            var storyDtos = new List<StoryDto> { new StoryDto(), new StoryDto() };

            _newsAdapterMock.Setup(x => x.GetMaxItemIdAsync()).ReturnsAsync(maxItemId);
            _cache.CreateEntry(CacheKeys.MaxItemId).Value = cachedMaxItemId;
            _newsAdapterMock.Setup(x => x.GetBestStoryIdsAsync()).ReturnsAsync(storyIds);
            _newsAdapterMock.Setup(x => x.GetStoriesByIdsAsync(It.IsAny<IEnumerable<int>>())).ReturnsAsync(stories);
            _mapperMock.Setup(x => x.Map<IEnumerable<StoryDto>>(stories)).Returns(storyDtos);

            // Act
            var result = await _storiesService.GetBestStoriesAsync(count);

            // Assert
            Assert.That(result, Is.EqualTo(storyDtos));
            Assert.That(_cache.TryGetValue(CacheKeys.MaxItemId, out var _), Is.True);
        }
    }
}