// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntDesign
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
            {
                return;
            }

            var temp = items.ToList(); // Get a copy so that we can enumerate it safely.
            foreach (var item in temp)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
        {
            if (items == null)
            {
                return;
            }

            var temp = items.ToList(); // Get a copy so that we can enumerate it safely.
            for (var i = 0; i < temp.Count; i++)
            {
                action(temp[i], i);
            }
        }

        public static async Task ForEachAsync<T>(this IEnumerable<T> items, Func<T, Task> func)
        {
            if (items == null)
            {
                return;
            }

            foreach (var item in items) await func(item);
        }

        public static bool IsIn<T>(this T source, params T[] array)
        {
            if (array == null)
            {
                return false;
            }

            return source.IsIn(array.AsSpan());
        }

        public static bool IsIn<T>(this T source, params ReadOnlySpan<T> array)
        {
            if (array.IsEmpty)
            {
                return false;
            }

            if (source is IEquatable<T> eq)
            {
                for (var i = 0; i < array.Length; i++)
                {
                    if (eq.Equals(array[i]))
                    {
                        return true;
                    }
                }
            }
            else
            {
                var comparer = EqualityComparer<T>.Default;
                for (var i = 0; i < array.Length; i++)
                {
                    if (comparer.Equals(source, array[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //public static bool IsIn<T>(this T source, IEnumerable<T> array)
        //{
        //    if (array == null)
        //    {
        //        return false;
        //    }

        //    return array.Contains(source);
        //}

        public static T[] Append<T>(this T[] array, T item)
        {
            if (array == null)
            {
                return [item];
            }
            Array.Resize(ref array, array.Length + 1);
            array[^1] = item;

            return array;
        }

        public static T[] Remove<T>(this T[] array, T item)
        {
            if (array == null)
            {
                return [];
            }

            if (item is null)
            {
                return Array.FindAll(array, x => x is not null);
            }

            return Array.FindAll(array, x => !item.Equals(x));
        }

        /// <summary>
        /// add item to items when condition is true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="condition"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IList<T> AddIf<T>(this IList<T> items, bool condition, T item)
        {
            items ??= [];
            if (condition)
            {
                items.Add(item);
            }

            return items;
        }
    }
}
