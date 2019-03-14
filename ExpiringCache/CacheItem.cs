using System;

namespace ExpiringCache
{
    /// <summary>
    /// Generic cache item.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class CacheItem<TKey, TItem>
    {
        public CacheItem(TKey key, TItem item, DateTimeOffset expirationTime)
        {
            Key = key;
            Item = item;
            ExpirationTime = expirationTime;
        }

        public TKey Key { get; }

        private TItem _item;

        public TItem Item
        {
            get
            {
                // By 'accessing' CacheItem element we assume that the actual 
                // Item was accessed (get or set), not the Key or ExpirationTime
                LastAccessedTime = DateTimeOffset.Now;
                return _item;
            }
            private set
            {
                LastAccessedTime = DateTimeOffset.Now;
                _item = value;
            }
        }

        public DateTimeOffset ExpirationTime { get; }
        public DateTimeOffset LastAccessedTime { get; private set; }
    }
}