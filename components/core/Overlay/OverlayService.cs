// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Service for displaying render fragments at arbitrary positions
    /// </summary>
    public class OverlayService : IOverlayService
    {
        internal event Func<OverlayConfig, Task> OnOpening;
        internal event Func<string, Task> OnClosing;
        internal event Action OnCloseAll;

        private readonly ConcurrentDictionary<string, OverlayConfig> _overlays = new();
        private int _counter = 0;

        /// <summary>
        /// Open an overlay with the specified render fragment at the given position
        /// </summary>
        public async Task<OverlayReference> OpenAsync(RenderFragment content, double x, double y, string container = "body")
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var id = $"overlay_{System.Threading.Interlocked.Increment(ref _counter)}_{Guid.NewGuid():N}";
            
            var config = new OverlayConfig
            {
                Id = id,
                Content = content,
                X = x,
                Y = y,
                Container = container ?? "body"
            };

            _overlays.TryAdd(id, config);

            if (OnOpening != null)
            {
                await OnOpening.Invoke(config);
            }

            var reference = new OverlayReference(id, async () => await CloseAsync(id));
            return reference;
        }

        /// <summary>
        /// Open an overlay with the specified render fragment at the given position (synchronous)
        /// </summary>
        public OverlayReference Open(RenderFragment content, double x, double y, string container = "body")
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            var id = $"overlay_{System.Threading.Interlocked.Increment(ref _counter)}_{Guid.NewGuid():N}";
            
            var config = new OverlayConfig
            {
                Id = id,
                Content = content,
                X = x,
                Y = y,
                Container = container ?? "body"
            };

            _overlays.TryAdd(id, config);

            if (OnOpening != null)
            {
                _ = OnOpening.Invoke(config);
            }

            var reference = new OverlayReference(id, async () => await CloseAsync(id));
            return reference;
        }

        /// <summary>
        /// Close a specific overlay by ID
        /// </summary>
        internal async Task CloseAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return;

            if (_overlays.TryRemove(id, out var config))
            {
                config.Visible = false;
                
                if (OnClosing != null)
                {
                    await OnClosing.Invoke(id);
                }
            }
        }

        /// <summary>
        /// Close all open overlays
        /// </summary>
        public void CloseAll()
        {
            _overlays.Clear();
            OnCloseAll?.Invoke();
        }

        /// <summary>
        /// Get all current overlay configurations (for container component)
        /// </summary>
        internal OverlayConfig[] GetAll()
        {
            return _overlays.Values.Where(x => x.Visible).ToArray();
        }
    }
}
