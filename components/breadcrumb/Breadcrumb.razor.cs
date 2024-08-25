// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign
{
    public partial class Breadcrumb : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }


        [Parameter]
        public bool AutoGenerate { get; set; } = false;

        /// <summary>
        /// Custom separator
        /// </summary>
        [Parameter]
        public string Separator { get; set; } = "/";

        [Inject] private MenuService MenuService { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; }

        private List<BreadcrumbOption> _existingOptions = [];

        protected override void OnInitialized()
        {
            var prefixCls = "ant-breadcrumb";

            ClassMapper
                .Add(prefixCls)
                .If($"{prefixCls}-rtl", () => RTL);

            if (AutoGenerate)
            {
                MenuService.MenuItemLoaded += InvokeStateHasChanged;
                NavigationManager.LocationChanged += NavigationManager_LocationChanged;
            }

            base.OnInitialized();
        }

        private void NavigationManager_LocationChanged(object sender, LocationChangedEventArgs e)
        {
            InvokeStateHasChanged();
        }

        internal void AddOption(BreadcrumbOption option)
        {
            if (!_existingOptions.Contains(option))
            {
                _existingOptions.Add(option);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (AutoGenerate)
            {
                MenuService.MenuItemLoaded -= InvokeStateHasChanged;
                NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
            }

            base.Dispose(disposing);
        }
    }
}
