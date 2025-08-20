// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace AntDesign.Core.Extensions
{
    internal static class DataConversionExtensions
    {
        /// <summary>
        /// Converts the generic type TFrom to the specified TTo type
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="fromValue"></param>
        /// <returns></returns>
        public static TTo Convert<TFrom, TTo>(TFrom fromValue)
        {
            return Unsafe.As<TFrom, TTo>(ref fromValue);
        }
    }
}
