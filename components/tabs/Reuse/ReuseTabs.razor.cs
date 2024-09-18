// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign
{
    /// <summary>
    /// Reuse of multiple page components within an application
    /// </summary>
    public partial class ReuseTabs : Tabs
    {
        /// <summary>
        /// Class name of the inner tab pane.
        /// </summary>
        [Parameter]
        public string TabPaneClass { get; set; }

        /// <summary>
        /// Templates for customizing page content.
        /// </summary>
        [Parameter]
        public RenderFragment<ReuseTabsPageItem> TabPaneTemplate { get; set; } = context => context.Body;

        /// <summary>
        /// The content of the tab.
        /// </summary>
        [Parameter]
        public RenderFragment Body { get; set; }

        /// <summary>
        /// Localization Settings.
        /// </summary>
        [Parameter]
        public ReuseTabsLocale Locale { get; set; } = LocaleProvider.CurrentLocale.ReuseTabs;

        /// <summary>
        /// Whether to hide the page display and keep only the title tab. Then you can use <see cref="ReusePages" /> to show the page conent.
        /// </summary>
        [Parameter]
        public bool HidePages { get; set; }

        /// <summary>
        /// The routing information for the current page, which is a serializable version of <see cref="Microsoft.AspNetCore.Components.RouteData"/>.
        /// </summary>
        [Parameter]
        public ReuseTabsRouteData ReuseTabsRouteData { get; set; }

        [CascadingParameter]
        private RouteData RouteData { get; set; }

        [Inject]
        private NavigationManager Navmgr { get; set; }

        [Inject]
        private ReuseTabsService ReuseTabsService { get; set; }

        [CascadingParameter(Name = "AntDesign.InReusePageContent")]
        private bool InReusePageContent { get; set; }

        /// <summary>
        /// Can't be used with <see cref="ReuseTabs"/>.
        /// </summary>
        public override RenderFragment ChildContent { get; set; }

        private bool _showRender;

        public ReuseTabs()
        {
            Type = TabType.EditableCard;
            HideAdd = true;
        }

        protected override void OnInitialized()
        {
            if (InReusePageContent)
            {
                return;
            }
            base.OnInitialized();

            Navmgr.LocationChanged += OnLocationChanged;

            ChildContent = RenderPanes;

            ReuseTabsService.Init(true);
            ReuseTabsService.OnStateHasChanged += InvokeStateHasChanged;

            RouteData ??= ReuseTabsRouteData?.RouteData;

            ReuseTabsService.TrySetRouteData(RouteData, true);

            _showRender = true;
            ActiveKey = ReuseTabsService.ActiveKey;
        }

        protected override bool ShouldRender() => !InReusePageContent && base.ShouldRender();

        protected override void OnAfterRender(bool firstRender)
        {
            _showRender = false;
            base.OnAfterRender(firstRender);
        }
        protected override void Dispose(bool disposing)
        {
            ReuseTabsService.OnStateHasChanged -= InvokeStateHasChanged;
            Navmgr.LocationChanged -= OnLocationChanged;
            base.Dispose(disposing);
        }

        private void ClosePage(string key)
        {
            UpdateTabsPosition();
            ReuseTabsService.ClosePage(key);
        }

        protected override void OnRemoveTab(TabPane tab)
        {
            ReuseTabsService.ClosePage(tab.Key);
        }

        protected override void OnActiveTabChanged(TabPane tab)
        {
            if (tab.Key != ReuseTabsService.ActiveKey)
            {
                ReuseTabsService.ActiveKey = tab.Key;
            }
        }

        private void OnLocationChanged(object o, LocationChangedEventArgs _)
        {
            UpdateTabsPosition();

            RouteData ??= ReuseTabsRouteData?.RouteData;

            ReuseTabsService.TrySetRouteData(RouteData, true);

            ActivatePane(ReuseTabsService.ActiveKey);
        }
    }
}
