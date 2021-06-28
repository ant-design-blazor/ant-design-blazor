using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace AntDesign.Docs.Services
{
    public class ConcurrentCache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, Lazy<TValue>> _dictionary;

        public ConcurrentCache()
        {
            this._dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>();
        }

        public ConcurrentCache(IEqualityComparer<TKey> comparer)
        {
            this._dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>(comparer);
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            return this._dictionary
                .GetOrAdd(key, k => new Lazy<TValue>(() => valueFactory(k), LazyThreadSafetyMode.ExecutionAndPublication))
                .Value;
        }

        public TValue GetOrAdd(TKey key, TValue value)
        {
            return this._dictionary
                .GetOrAdd(key, k => new Lazy<TValue>(() => value, LazyThreadSafetyMode.ExecutionAndPublication))
                .Value;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this._dictionary.TryGetValue(key, out Lazy<TValue> lazyValue))
            {
                value = lazyValue.Value;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (_dictionary.TryGetValue(key, out var lazy))
                {
                    return lazy.Value;
                }

                return default(TValue);
            }
            set
            {
                _dictionary.AddOrUpdate(key,
                    k => new Lazy<TValue>(() => value, LazyThreadSafetyMode.ExecutionAndPublication),
                (k, v) => new Lazy<TValue>(() => value, LazyThreadSafetyMode.ExecutionAndPublication));
            }
        }

        public void AddOrUpdate(TKey key, TValue value, Func<TKey, TValue, TValue> action)
        {
            _dictionary.AddOrUpdate(key, new Lazy<TValue>(() => value, LazyThreadSafetyMode.ExecutionAndPublication),
                (k, v) => new Lazy<TValue>(() => action(k, v.Value), LazyThreadSafetyMode.ExecutionAndPublication));
        }
    }
}
