// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;

namespace AntDesign.Internal
{
    public class NavMgrService
    {
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;

        public event EventHandler<LocationChangedEventArgs> OnLocationChanged;

        public NavMgrService(NavigationManager navigationManager, IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _navigationManager = navigationManager;
            _navigationManager.LocationChanged += NavigationManager_LocationChanged;
        }

        private async void NavigationManager_LocationChanged(object sender, LocationChangedEventArgs e)
        {
            Console.WriteLine("NavigationManager_LocationChanged 1");
            await _jsRuntime.InvokeVoidAsync(JSInteropConstants.OnLocationChanged);
            Console.WriteLine("NavigationManager_LocationChanged 2");
            if (OnLocationChanged != null)
            {
                OnLocationChanged(this, e);
            }
        }
    }
}
