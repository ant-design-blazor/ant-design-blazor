// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace AntDesign
{
    /// <summary>
    /// Draft service interface.
    /// </summary>
    public interface IDraftService
    {
        /// <summary>
        /// Save draft with delay.
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="key">The draft key</param>
        /// <param name="data">The data</param>
        /// <param name="options">The configuration options</param>
        public Task SaveDraftAsync<T>(string key, T data, DraftOptions options = null);

        /// <summary>
        /// Save draft immediately without delay.
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="key">The draft key</param>
        /// <param name="data">The data</param>
        /// <param name="options">The configuration options</param>
        public Task SaveDraftImmediateAsync<T>(string key, T data, DraftOptions options = null);

        /// <summary>
        /// Load draft.
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="key">The draft key</param>
        /// <param name="options">The configuration options</param>
        /// <returns>Draft data, or null if not exists or version mismatch</returns>
        public Task<DraftData<T>> LoadDraftAsync<T>(string key, DraftOptions options = null);

        /// <summary>
        /// Remove draft.
        /// </summary>
        /// <param name="key">The draft key</param>
        /// <param name="options">The configuration options</param>
        public Task RemoveDraftAsync(string key, DraftOptions options = null);

        /// <summary>
        /// Check if draft exists.
        /// </summary>
        /// <param name="key">The draft key</param>
        /// <param name="options">The configuration options</param>
        /// <returns>True if exists and version matches</returns>
        public Task<bool> HasDraftAsync(string key, DraftOptions options = null);

        /// <summary>
        /// Cancel pending delay save.
        /// </summary>
        /// <param name="key">The draft key</param>
        public void CancelPendingSave(string key);
    }
}
