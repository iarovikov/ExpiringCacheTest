using System;

namespace ExpiringCache
{
    public class CacheItem<TKey, TItem>
    {
        public CacheItem(TKey key, TItem item, DateTimeOffset expirationTime)
        {
            Key = key;
            _item = item;
            ExpirationTime = expirationTime;
            LastAccessedTime = DateTimeOffset.Now;
        }

        public TKey Key { get; }

        private TItem _item;

        public TItem Item
        {
            get
            {
                LastAccessedTime = DateTimeOffset.Now;
                return _item;
            }
        }

        public DateTimeOffset ExpirationTime { get; }
        public DateTimeOffset LastAccessedTime { get; private set; }
    }
}