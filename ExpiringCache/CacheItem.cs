using System;

namespace ExpiringCache
{
    public class CacheItem<TKey, TItem>
    {
        public CacheItem(TKey key, TItem item, DateTimeOffset expirationTime)
        {
            Key = key;
            Item = item;
            ExpirationTime = expirationTime;
        }

        public TKey Key { get; }
        public TItem Item { get; }
        public DateTimeOffset ExpirationTime { get; }
    }
}