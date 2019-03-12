using System;
using System.Collections.Concurrent;

namespace ExpiringCache
{
    
    public class ExpiringCache<TKey, TItem> : IExpiringCache<TKey, TItem>
    {
        private const int DefaultDurationInSeconds = 30;
        private const int DefaultMaxCapacity = 20;
        
        private TimeSpan _duration;
        private int _maxCapacity;

        private ConcurrentDictionary<TKey, TItem> _items;
        
        public ExpiringCache() : this(TimeSpan.FromSeconds(DefaultDurationInSeconds), DefaultMaxCapacity)
        {
        }

        public ExpiringCache(TimeSpan duration, int maxCapacity)
        {
            _duration = duration;
            _maxCapacity = maxCapacity;
        }
        
        public void Add(TKey key, TItem item)
        {
            throw new NotImplementedException();
        }

        public void Add(TKey key, TItem item, TimeSpan duration)
        {
            throw new NotImplementedException();
        }

        
        public bool TryGet(TKey key, out TItem item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}