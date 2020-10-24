// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign.Core.HashCodes
{
    /// <summary>
    /// 提供两参数值哈希比较
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    static class HashCode<TParameter>
    {
        private static readonly HashCodeProvider _provider = HashCodeProvider.Create(typeof(TParameter));

        /// <summary>
        /// 计算两参数值的哈希是否相等
        /// </summary>
        /// <param name="parameter1">参数值1</param>
        /// <param name="parameter2">参数值2</param>
        /// <returns></returns>
        public static bool HashCodeEquals(TParameter parameter1, TParameter parameter2)
        {
            return GetHashCode(parameter1) == GetHashCode(parameter2);
        }

        /// <summary>
        /// 计算参数的哈希值
        /// </summary>
        /// <param name="parameter">参数值1</param>
        /// <returns></returns>
        public static int GetHashCode(TParameter parameter)
        {
            return _provider.GetHashCode(parameter);
        }
    }
}
