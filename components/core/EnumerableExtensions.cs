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

            for (int i = 0; i < items.Count(); i++)
            {
                await func(items.ElementAt(i));
            }
        }

        public static bool IsIn<T>(this T source, params T[] array)
        {
            return array.Contains(source);
        }
    }
}
