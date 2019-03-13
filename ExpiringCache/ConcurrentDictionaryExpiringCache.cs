using System;
using System.Collections.Concurrent;

namespace ExpiringCache
{
    /// <inheritdoc />
    public class ConcurrentDictionaryExpiringCache<TKey, TItem> : IExpiringCache<TKey, TItem>
    {
        private const int DefaultDurationInSeconds = 30;
        private const int DefaultMaxCapacity = 20;

        private TimeSpan _duration;
        private int _maxCapacity;

        private readonly ConcurrentDictionary<TKey, CacheItem<TKey, TItem>> _items
            = new ConcurrentDictionary<TKey, CacheItem<TKey, TItem>>();

        public ConcurrentDictionaryExpiringCache() : this(TimeSpan.FromSeconds(DefaultDurationInSeconds),
            DefaultMaxCapacity)
        {
        }

        public ConcurrentDictionaryExpiringCache(TimeSpan duration, int maxCapacity)
        {
            _duration = duration;
            _maxCapacity = maxCapacity;
        }

        public void Add(TKey key, TItem item)
        {
            var now = DateTimeOffset.UtcNow;
            if (!_items.TryAdd(key, new CacheItem<TKey, TItem>(key, item, now.Add(_duration))))
            {
                _items[key] = new CacheItem<TKey, TItem>(key, item, now.Add(_duration));
            }
        }

        public void Add(TKey key, TItem item, TimeSpan duration)
        {
            throw new NotImplementedException();
        }


        public bool TryGet(TKey key, out TItem item)
        {
            CacheItem<TKey, TItem> result;
            if (!_items.TryGetValue(key, out result))
            {
                item = default(TItem);
                return false;
            }
            else if (result.ExpirationTime < DateTimeOffset.Now)
            {
                item = default(TItem);
                return false;
            }
            else
            {
                item = result.Item;
                return true;
            }
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}