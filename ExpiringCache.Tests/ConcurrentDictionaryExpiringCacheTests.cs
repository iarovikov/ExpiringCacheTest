using System;
using Xunit;

namespace ExpiringCache.Tests
{
    public class ConcurrentDictionaryExpiringCacheTests
    {
        [Fact]
        public void SutIsImplementingInterface()
        {
            var sut = new ConcurrentDictionaryExpiringCache<string, string>();
            Assert.IsAssignableFrom<IExpiringCache<string,string>>(sut);
        }
    }
}