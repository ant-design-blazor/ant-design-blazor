// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace AntDesign
{
    /// <summary>
    /// Draft service implementation
    /// </summary>
    public class DraftService : IDraftService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IDraftStorageProvider _defaultStorageProvider;
        private readonly Dictionary<string, CancellationTokenSource> _pendingSaves = new();
        private readonly object _lockObject = new();

        public DraftService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _defaultStorageProvider = new LocalStorageDraftProvider(jsRuntime);
        }

        public async Task SaveDraftAsync<T>(string key, T data, DraftOptions options = null)
        {
            options ??= new DraftOptions { Key = key };
            if (!options.Enabled) return;

            // Cancel previous pending save
            CancelPendingSave(key);

            // Create new delayed save task
            var cts = new CancellationTokenSource();
            lock (_lockObject)
            {
                _pendingSaves[key] = cts;
            }

            try
            {
                await Task.Delay(options.DelayMilliseconds, cts.Token);
                await SaveDraftImmediateAsync(key, data, options);
            }
            catch (TaskCanceledException)
            {
                // Delay was cancelled, ignore
            }
            finally
            {
                lock (_lockObject)
                {
                    if (_pendingSaves.TryGetValue(key, out var existingCts) && existingCts == cts)
                    {
                        _pendingSaves.Remove(key);
                        cts.Dispose();
                    }
                }
            }
        }

        public async Task SaveDraftImmediateAsync<T>(string key, T data, DraftOptions options = null)
        {
            options ??= new DraftOptions { Key = key };
            if (!options.Enabled) return;

            var draftData = new DraftData<T>
            {
                Data = data,
                Version = options.Version,
                SavedAt = DateTime.UtcNow,
                Key = key
            };

            var json = JsonSerializer.Serialize(draftData);
            var provider = options.StorageProvider ?? _defaultStorageProvider;
            await provider.SaveAsync(key, json);
        }

        public async Task<DraftData<T>> LoadDraftAsync<T>(string key, DraftOptions options = null)
        {
            options ??= new DraftOptions { Key = key };
            if (!options.Enabled) return null;

            var provider = options.StorageProvider ?? _defaultStorageProvider;
            var json = await provider.LoadAsync(key);

            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                var draftData = JsonSerializer.Deserialize<DraftData<T>>(json);

                // Version check
                if (options.EnableVersionCheck && draftData.Version != options.Version)
                {
                    return null;
                }

                return draftData;
            }
            catch
            {
                return null;
            }
        }

        public async Task RemoveDraftAsync(string key, DraftOptions options = null)
        {
            options ??= new DraftOptions { Key = key };
            var provider = options.StorageProvider ?? _defaultStorageProvider;
            await provider.RemoveAsync(key);
        }

        public async Task<bool> HasDraftAsync(string key, DraftOptions options = null)
        {
            options ??= new DraftOptions { Key = key };
            if (!options.Enabled) return false;

            var provider = options.StorageProvider ?? _defaultStorageProvider;
            var exists = await provider.ExistsAsync(key);

            if (!exists) return false;

            // If version check is enabled, need to validate version
            if (options.EnableVersionCheck)
            {
                var json = await provider.LoadAsync(key);
                if (string.IsNullOrEmpty(json))
                    return false;

                try
                {
                    var versionData = JsonSerializer.Deserialize<DraftData<object>>(json);
                    return versionData?.Version == options.Version;
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public void CancelPendingSave(string key)
        {
            lock (_lockObject)
            {
                if (_pendingSaves.TryGetValue(key, out var cts))
                {
                    cts.Cancel();
                    _pendingSaves.Remove(key);
                    cts.Dispose();
                }
            }
        }
    }
}
