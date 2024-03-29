using System;
using System.Threading;
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
            Assert.IsAssignableFrom<IExpiringCache<string, string>>(sut);
        }

        [Fact]
        public void ThrowsExceptionIfCreateWithNegativeMaxCapacity()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new ConcurrentDictionaryExpiringCache<int, int>(-48));
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

        [Fact]
        public void UpdatesExistingValue()
        {
            // Arrange
            var sut = new ConcurrentDictionaryExpiringCache<string, string>();
            var expectedItem = "baz";

            // Act
            sut.Add("foo", "bar");
            sut.Add("foo", expectedItem);
            var result = sut.TryGet("foo", out var actualItem);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedItem, actualItem);
        }

        [Fact]
        public void ReturnsFalseIfTheItemIsExpired()
        {
            // Arrange
            var sut = new ConcurrentDictionaryExpiringCache<string, string>();
            var expired = "expired";

            // Act
            sut.Add("foo", expired, TimeSpan.FromSeconds(2));
            Thread.Sleep(3000);
            var result = sut.TryGet("foo", out var actualItem);

            // Assert
            Assert.False(result);
            Assert.Null(actualItem);
        }

        [Fact]
        public void UpdatesItemExpirationTime()
        {
            // Arrange
            var sut = new ConcurrentDictionaryExpiringCache<string, string>();
            var expired = "expired";
            var notExpired = "not expired";

            // Act
            sut.Add("foo", expired, TimeSpan.FromSeconds(2));
            sut.Add("foo", notExpired, TimeSpan.FromSeconds(8));
            Thread.Sleep(3000);

            var result = sut.TryGet("foo", out var actualItem);

            // Assert
            Assert.True(result);
            Assert.Equal(notExpired, actualItem);
        }

        [Fact]
        public void MaxCapacityNotExceeded()
        {
            // Arrange
            var expectedMaxCount = 2;
            var sut = new ConcurrentDictionaryExpiringCache<int, string>(expectedMaxCount);

            // Act
            sut.Add(1, "foo");
            sut.Add(2, "bar");
            sut.Add(3, "baz");
            var actualCount = sut.Count;

            // Assert
            Assert.Equal(expectedMaxCount, actualCount);
        }

        [Fact]
        public void EnsureLeastAccessedItemWasRemoved()
        {
            // Arrange
            var expectedMaxCount = 3;
            var sut = new ConcurrentDictionaryExpiringCache<int, string>(expectedMaxCount);
            var expectedItem = "i'm still here";

            // Act
            sut.Add(1, "foo");
            sut.Add(2, "deleted");
            sut.Add(3, "baz");
            sut.Add(1, expectedItem);
            sut.Add(4, "four");
            var actualCount = sut.Count;
            var actualResult = sut.TryGet(1, out var actualValue);
            var tryGetValueThatIsNotInTheCache = sut.TryGet(2, out var missingItem);

            // Assert
            Assert.Equal(expectedMaxCount, actualCount);
            Assert.True(actualResult);
            Assert.Equal(expectedItem, actualValue);
            
            Assert.False(tryGetValueThatIsNotInTheCache);
            Assert.Null(missingItem);
        }
    }
}