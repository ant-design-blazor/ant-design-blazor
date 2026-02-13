// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Container component for rendering overlays
    /// </summary>
    public partial class OverlayContainer : AntDomComponentBase
    {
        [Inject] private OverlayService OverlayService { get; set; }
        [Inject] private InteropService InteropService { get; set; }

        private List<OverlayConfig> _configs = new();
        private int _baseZIndex = 1000;
        private bool _jsModuleLoaded = false;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (OverlayService != null)
            {
                OverlayService.OnOpening += HandleOpeningAsync;
                OverlayService.OnClosing += HandleClosingAsync;
                OverlayService.OnCloseAll += HandleCloseAll;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                _jsModuleLoaded = true;
            }

            // Position all newly rendered overlays
            if (_jsModuleLoaded && InteropService != null)
            {
                foreach (var config in _configs)
                {
                    try
                    {
                        await InteropService.JsInvokeAsync(
                            "AntDesign.overlayService.positionOverlay",
                            config.Id,
                            config.X,
                            config.Y,
                            config.Container);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error positioning overlay {config.Id}: {ex.Message}");
                    }
                }
            }
        }

        private async Task HandleOpeningAsync(OverlayConfig config)
        {
            if (config == null) return;

            // Add or update config
            var existingIndex = _configs.FindIndex(c => c.Id == config.Id);
            if (existingIndex >= 0)
            {
                _configs[existingIndex] = config;
            }
            else
            {
                _configs.Add(config);
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task HandleClosingAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return;

            var config = _configs.FirstOrDefault(c => c.Id == id);
            if (config != null)
            {
                _configs.Remove(config);

                await InvokeAsync(StateHasChanged);
            }
        }

        private void HandleCloseAll()
        {
            _configs.Clear();
            InvokeAsync(StateHasChanged);
        }

        protected override void Dispose(bool disposing)
        {
            if (OverlayService != null)
            {
                OverlayService.OnOpening -= HandleOpeningAsync;
                OverlayService.OnClosing -= HandleClosingAsync;
                OverlayService.OnCloseAll -= HandleCloseAll;
            }

            base.Dispose(disposing);
        }
    }
}
