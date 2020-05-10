using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace AntBlazor.Docs.Services
{
    public class ConcurrentCache<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, Lazy<TValue>> dictionary;

        public ConcurrentCache()
        {
            this.dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>();
        }

        public ConcurrentCache(IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>(comparer);
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            return this.dictionary
                .GetOrAdd(key, k => new Lazy<TValue>(() => valueFactory(k), LazyThreadSafetyMode.ExecutionAndPublication))
                .Value;
        }

        public TValue GetOrAdd(TKey key, TValue value)
        {
            return this.dictionary
                .GetOrAdd(key, k => new Lazy<TValue>(() => value, LazyThreadSafetyMode.ExecutionAndPublication))
                .Value;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this.dictionary.TryGetValue(key, out Lazy<TValue> lazyValue))
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
                if (dictionary.ContainsKey(key))
                    return dictionary[key].Value;

                return default(TValue);
            }
            set
            {
                dictionary.AddOrUpdate(key,
                    k => new Lazy<TValue>(() => value, LazyThreadSafetyMode.ExecutionAndPublication),
                (k, v) => new Lazy<TValue>(() => value, LazyThreadSafetyMode.ExecutionAndPublication));
            }
        }

        public void AddOrUpdate(TKey key, TValue value, Func<TKey, TValue, TValue> action)
        {
            dictionary.AddOrUpdate(key, new Lazy<TValue>(() => value, LazyThreadSafetyMode.ExecutionAndPublication),
                (k, v) => new Lazy<TValue>(() => action(k, v.Value), LazyThreadSafetyMode.ExecutionAndPublication));
        }
    }
}