using System;
using CandidateAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;

namespace CandidateAPI.Tests.Services
{
    public class CacheServiceTests
    {
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly ICacheService _cacheService;

        public CacheServiceTests()
        {
            // Mock the IMemoryCache
            _mockMemoryCache = new Mock<IMemoryCache>();
            // Initialize CacheService with mocked IMemoryCache
            _cacheService = new CacheService(_mockMemoryCache.Object);
        }

        [Fact]
        public void Get_ReturnsValue_FromCache()
        {
            // Arrange
            var key = "testKey";
            var expectedValue = "testValue";

            // Set up the TryGetValue method using a delegate to mock the out parameter
            object cacheValue = expectedValue;
            _mockMemoryCache
                .Setup(m => m.TryGetValue(key, out cacheValue))
                .Returns(true);

            // Act
            var result = _cacheService.Get<string>(key);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void Get_ReturnsNull_WhenKeyDoesNotExist()
        {
            // Arrange
            var key = "nonExistentKey";
            object cacheValue = null;

            // Setup the mock to simulate the behavior of TryGetValue returning false
            _mockMemoryCache
                .Setup(m => m.TryGetValue(key, out cacheValue))
                .Returns(false);

            // Act
            var result = _cacheService.Get<string>(key);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Remove_RemovesValue_FromCache()
        {
            // Arrange
            var key = "removeKey";

            // Act
            _cacheService.Remove(key);

            // Assert
            _mockMemoryCache.Verify(m => m.Remove(key), Times.Once);
        }
    }
}
