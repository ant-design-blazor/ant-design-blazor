// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class DrawerContainer
    {
        [Inject]
        private DrawerService DrawerService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            DrawerService.OnCloseEvent += DrawerService_OnClose;
            DrawerService.OnOpenEvent += DrawerService_OnCreate;
            DrawerService.OnUpdateEvent += DrawerService_OnUpdateEvent;

            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object sender, EventArgs e)
        {
            _drawerRefs.Clear();
            InvokeStateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            DrawerService.OnCloseEvent -= DrawerService_OnClose;
            DrawerService.OnOpenEvent -= DrawerService_OnCreate;
            DrawerService.OnUpdateEvent -= DrawerService_OnUpdateEvent;
            NavigationManager.LocationChanged -= OnLocationChanged;

            base.Dispose(disposing);
        }

        private readonly List<DrawerRef> _drawerRefs = new List<DrawerRef>();

        /// <summary>
        /// Create and Open a drawer
        /// </summary>
        private async Task DrawerService_OnCreate(DrawerRef drawerRef)
        {
            if (!_drawerRefs.Contains(drawerRef))
            {
                _drawerRefs.Add(drawerRef);
                await InvokeAsync(StateHasChanged);
                await Task.Delay(10);
            }
            drawerRef.Config.Visible = true;
            await InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Update drawer
        /// </summary>
        /// <param name="drawerRef"></param>
        /// <returns></returns>
        private async Task DrawerService_OnUpdateEvent(DrawerRef drawerRef)
        {
            if (_drawerRefs.Contains(drawerRef))
            {
                await InvokeStateHasChangedAsync();
            }
        }

        /// <summary>
        /// Close the drawer
        /// </summary>
        private async Task DrawerService_OnClose(DrawerRef drawerRef)
        {
            if (drawerRef.Config.Visible)
            {
                drawerRef.Config.Visible = false;
                await InvokeAsync(StateHasChanged);
                await Task.Delay(300);
                if (_drawerRefs.Contains(drawerRef))
                {
                    _drawerRefs.Remove(drawerRef);
                }
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
