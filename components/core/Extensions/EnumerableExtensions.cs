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

            foreach (T obj in items)
            {
                action(obj);
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

            return array.Contains(source);
        }

        public static bool IsIn<T>(this T source, IEnumerable<T> array)
        {
            if (array == null)
            {
                return false;
            }

            return array.Contains(source);
        }

        public static T[] Append<T>(this T[] array, T item)
        {
            if (array == null)
            {
                return new[] { item };
            }
            Array.Resize(ref array, array.Length + 1);
            array[^1] = item;

            return array;
        }

        public static T[] Remove<T>(this T[] array, T item)
        {
            if (array == null)
            {
                return Array.Empty<T>();
            }

            if (item == null)
            {
                return array.Where(x => x != null).ToArray();
            }

            return array.Where(x => !item.Equals(x)).ToArray();
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
            items ??= new List<T>();
            if (condition)
            {
                items.Add(item);
            }

            return items;
        }
    }
}
