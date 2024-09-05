// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign
{
    /**
    <summary>
        <para>A breadcrumb displays the current location within a hierarchy. It allows going back to states higher up in the hierarchy.</para>

        When To Use

        <list type="bullet">
            <item>When the system has more than two layers in a hierarchy.</item>
            <item>When you need to inform the user of where they are.</item>
            <item>When the user may need to navigate back to a higher level.</item>
        </list>
    </summary>
    <seealso cref="BreadcrumbItem" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Navigation, "https://gw.alipayobjects.com/zos/alicdn/9Ltop8JwH/Breadcrumb.svg", Title = "Breadcrumb", SubTitle = "面包屑")]
    public partial class Breadcrumb : AntDomComponentBase
    {
        /// <summary>
        /// Content of the Breadcrumb. Should contain BreadcrumbItem elements
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Not currently used. Planned for future development.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool AutoGenerate { get; set; } = false;

        /// <summary>
        /// Separator between items
        /// </summary>
        /// <default value="/" />
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
