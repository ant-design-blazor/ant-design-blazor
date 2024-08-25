// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign.Core.HashCodes
{
    /// <summary>
    /// Provides a hash comparison of two parameter values
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    static class HashCode<TParameter>
    {
        private static readonly HashCodeProvider _provider = HashCodeProvider.Create(typeof(TParameter));

        /// <summary>
        /// Calculate whether the hash of two parameter values is equal
        /// </summary>
        /// <param name="parameter1">Parameter 1</param>
        /// <param name="parameter2">Parameter 2</param>
        /// <returns></returns>
        public static bool HashCodeEquals(TParameter parameter1, TParameter parameter2)
        {
            return GetHashCode(parameter1) == GetHashCode(parameter2);
        }

        /// <summary>
        /// Calculate the hash value of the parameter
        /// </summary>
        /// <param name="parameter">Parameter</param>
        /// <returns></returns>
        public static int GetHashCode(TParameter parameter)
        {
            return _provider.GetHashCode(parameter);
        }
    }
}
