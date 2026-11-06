// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace AntDesign
{
    /// <summary>
    /// LocalStorage-based draft storage provider
    /// </summary>
    public class LocalStorageDraftProvider : IDraftStorageProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private const string StoragePrefix = "ant-design-blazor-draft-";

        public LocalStorageDraftProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SaveAsync(string key, string data)
        {
            var storageKey = GetStorageKey(key);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", storageKey, data);
        }

        public async Task<string> LoadAsync(string key)
        {
            var storageKey = GetStorageKey(key);
            try
            {
                return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", storageKey);
            }
            catch
            {
                return null;
            }
        }

        public async Task RemoveAsync(string key)
        {
            var storageKey = GetStorageKey(key);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", storageKey);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            var data = await LoadAsync(key);
            return !string.IsNullOrEmpty(data);
        }

        private static string GetStorageKey(string key)
        {
            return $"{StoragePrefix}{key}";
        }
    }
}
