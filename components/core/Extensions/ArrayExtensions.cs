// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;

namespace AntDesign.Core.Extensions
{
    internal static class ArrayExtensions
    {
        /// <summary>
        /// scroll left the elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceArray"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static T[] Scroll<T>(this T[] sourceArray, int offset)
        {
            return offset switch
            {
                0 => sourceArray,
                > 0 => sourceArray[offset..].Concat(sourceArray[..offset]).ToArray(),
                < 0 => sourceArray[^-offset..].Concat(sourceArray[..^-offset]).ToArray()
            };
        }
    }
}
