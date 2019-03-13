using System;
using System.Collections.Concurrent;
using System.Linq;

namespace ExpiringCache
{
    /// <inheritdoc />
    public class ConcurrentDictionaryExpiringCache<TKey, TItem> : IExpiringCache<TKey, TItem>
    {
        private const int DefaultDurationInSeconds = 30;
        private const int DefaultMaxCapacity = 20;

        private readonly TimeSpan _duration;
        public readonly int MaxCapacity;
        public int Count => _items.Count;

        private readonly ConcurrentDictionary<TKey, CacheItem<TKey, TItem>> _items
            = new ConcurrentDictionary<TKey, CacheItem<TKey, TItem>>();

        public ConcurrentDictionaryExpiringCache() : this(TimeSpan.FromSeconds(DefaultDurationInSeconds),
            DefaultMaxCapacity)
        {
        }

        public ConcurrentDictionaryExpiringCache(int maxCapacity) : this(TimeSpan.FromSeconds(DefaultDurationInSeconds),
            maxCapacity)
        {
        }

        public ConcurrentDictionaryExpiringCache(TimeSpan duration, int maxCapacity)
        {
            if (maxCapacity <= 0)
                throw new ArgumentException($"Maximum capacity should be more than 0.");
            
            _duration = duration;
            MaxCapacity = maxCapacity;
        }

        public void Add(TKey key, TItem item)
        {
            var now = DateTimeOffset.UtcNow;
            if (Count == MaxCapacity)
            {
                var leastAccessed = _items.OrderBy(it => it.Value.LastAccessedTime).First();
                Remove(leastAccessed.Key);
            }

            if (!_items.TryAdd(key, new CacheItem<TKey, TItem>(key, item, now.Add(_duration))))
            {
                _items[key] = new CacheItem<TKey, TItem>(key, item, now.Add(_duration));
            }
        }

        public void Add(TKey key, TItem item, TimeSpan duration)
        {
            var now = DateTimeOffset.UtcNow;
            if (!_items.TryAdd(key, new CacheItem<TKey, TItem>(key, item, now.Add(duration))))
            {
                _items[key] = new CacheItem<TKey, TItem>(key, item, now.Add(duration));
            }
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
            return _items.TryRemove(key, out var result);
        }
    }
}