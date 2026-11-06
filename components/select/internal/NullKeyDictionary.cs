// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace AntDesign.Select.Internal
{
    /// <summary>
    /// A dictionary that accepts a single <see langword="null"/> key.
    /// </summary>
    /// <remarks>
    /// <see cref="Dictionary{TKey, TValue}"/> rejects null keys, including a null
    /// nullable value type. Select values may legitimately be null, so selected
    /// options need an index which can represent that value.
    /// </remarks>
    internal sealed class NullKeyDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new();
        private bool _hasNullKey;
        private TValue _nullValue;

        public int Count => _dictionary.Count + (_hasNullKey ? 1 : 0);

        public IEnumerable<TValue> Values
        {
            get
            {
                if (_hasNullKey)
                {
                    yield return _nullValue;
                }

                foreach (var value in _dictionary.Values)
                {
                    yield return value;
                }
            }
        }

        public TValue this[TKey key]
        {
            get => IsNull(key) ? _hasNullKey ? _nullValue : throw new KeyNotFoundException() : _dictionary[key];
            set
            {
                if (IsNull(key))
                {
                    _hasNullKey = true;
                    _nullValue = value;
                }
                else
                {
                    _dictionary[key] = value;
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (IsNull(key))
            {
                if (_hasNullKey)
                {
                    throw new ArgumentException("An item with the same key has already been added.", nameof(key));
                }

                _hasNullKey = true;
                _nullValue = value;
                return;
            }

            _dictionary.Add(key, value);
        }

        public void Clear()
        {
            _dictionary.Clear();
            _hasNullKey = false;
            _nullValue = default;
        }

        public bool Remove(TKey key)
        {
            if (!IsNull(key))
            {
                return _dictionary.Remove(key);
            }

            if (!_hasNullKey)
            {
                return false;
            }

            _hasNullKey = false;
            _nullValue = default;
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!IsNull(key))
            {
                return _dictionary.TryGetValue(key, out value);
            }

            value = _nullValue;
            return _hasNullKey;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            if (_hasNullKey)
            {
                yield return new KeyValuePair<TKey, TValue>(default, _nullValue);
            }

            foreach (var item in _dictionary)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static bool IsNull(TKey key) => key is null;
    }
}
