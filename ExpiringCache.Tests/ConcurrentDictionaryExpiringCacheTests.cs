using System;
using Xunit;

namespace ExpiringCache.Tests
{
    public class ConcurrentDictionaryExpiringCacheTests
    {
        [Fact]
        public void ImplementsInterface()
        {
            // Arrange
            // Act
            var sut = new ConcurrentDictionaryExpiringCache<string, string>();
            
            // Assert
            Assert.IsAssignableFrom<IExpiringCache<string,string>>(sut);
        }

        [Fact]
        public void ReturnsFalseIfThereIsNoValue()
        {
            // Arrange
            var sut = new ConcurrentDictionaryExpiringCache<string, string>();
            
            // Act
            var actual = sut.TryGet("foo", out var result);
            
            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void ReturnsAddedValue()
        {
            // Arrange
            var sut = new ConcurrentDictionaryExpiringCache<string, string>();
            var expectedItem = "bar";
            
            // Act
            sut.Add("foo", expectedItem);
            var result = sut.TryGet("foo", out var actualItem);
            
            // Assert
            Assert.True(result);
            Assert.Equal(expectedItem, actualItem);
        }
    }
}