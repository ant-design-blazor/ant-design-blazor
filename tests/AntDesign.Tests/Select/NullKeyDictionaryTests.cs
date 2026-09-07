// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using AntDesign.Select.Internal;
using Xunit;

namespace AntDesign.Tests.Select
{
    public class NullKeyDictionaryTests
    {
        [Fact]
        public void Null_key_can_be_added_read_updated_and_removed()
        {
            var dictionary = new NullKeyDictionary<string, int>();

            dictionary.Add(null!, 1);
            Assert.Equal(1, dictionary.Count);
            Assert.True(dictionary.TryGetValue(null!, out var nullValue));
            Assert.Equal(1, nullValue);
            Assert.Equal(1, dictionary[null!]);

            dictionary[null!] = 2;
            Assert.Equal(2, dictionary[null!]);

            Assert.True(dictionary.Remove(null!));
            Assert.False(dictionary.TryGetValue(null!, out _));
            Assert.False(dictionary.Remove(null!));
            Assert.Equal(0, dictionary.Count);
        }

        [Fact]
        public void Null_key_cannot_be_added_twice_and_missing_null_key_throws_on_indexer()
        {
            var dictionary = new NullKeyDictionary<string, int>();

            dictionary.Add(null!, 1);
            Assert.Throws<ArgumentException>(() => dictionary.Add(null!, 2));

            dictionary.Remove(null!);
            Assert.Throws<KeyNotFoundException>(() => _ = dictionary[null!]);
        }

        [Fact]
        public void Enumeration_and_values_include_the_null_entry_first()
        {
            var dictionary = new NullKeyDictionary<string, int>
            {
                [null!] = 1,
                ["a"] = 2,
            };

            Assert.Equal(new[] { 1, 2 }, dictionary.Values.ToArray());
            Assert.Equal(2, dictionary.ToArray().Length);
            dictionary.Clear();
            Assert.Empty(dictionary);
            Assert.Equal(0, dictionary.Count);
        }

        [Fact]
        public void Non_null_keys_delegate_to_inner_dictionary()
        {
            var dictionary = new NullKeyDictionary<string, int>
            {
                ["a"] = 1,
            };

            Assert.Equal(1, dictionary["a"]);
            Assert.True(dictionary.Remove("a"));
            Assert.Empty(dictionary);
        }
    }
}
