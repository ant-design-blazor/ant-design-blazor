// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>
    /// Draft storage provider interface.
    /// </summary>
    public interface IDraftStorageProvider
    {
        /// <summary>
        /// Save draft data.
        /// </summary>
        /// <param name="key">The storage key</param>
        /// <param name="data">The data</param>
        public Task SaveAsync(string key, string data);

        /// <summary>
        /// Load draft data.
        /// </summary>
        /// <param name="key">The storage key</param>
        /// <returns>The data, or null if not exists</returns>
        public Task<string> LoadAsync(string key);

        /// <summary>
        /// Remove draft data.
        /// </summary>
        /// <param name="key">The storage key</param>
        public Task RemoveAsync(string key);

        /// <summary>
        /// Check if draft exists.
        /// </summary>
        /// <param name="key">The storage key</param>
        /// <returns>True if exists</returns>
        public Task<bool> ExistsAsync(string key);
    }
}
