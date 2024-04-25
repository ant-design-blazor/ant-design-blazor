﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace AntDesign
{
    /// <summary>
    /// Reuse of multiple page components within an application
    /// </summary>
    public partial class ReuseTabs : AntDomComponentBase
    {
        /// <summary>
        /// Class name of the inner tab pane.
        /// </summary>
        [Parameter]
        public string TabPaneClass { get; set; }

        /// <summary>
        /// Whether Tab can be dragged and dropped.
        /// </summary>
        [Parameter]
        public bool Draggable { get; set; }

        /// <summary>
        /// The size of tabs.
        /// </summary>
        [Parameter]
        public TabSize Size { get; set; }

        /// <summary>
        /// Templates for customizing page content.
        /// </summary>
        [Parameter]
        public RenderFragment<ReuseTabsPageItem> Body { get; set; } = context => context.Body;

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

        protected override void OnInitialized()
        {
            if (InReusePageContent)
            {
                return;
            }
            base.OnInitialized();
            ReuseTabsService.Init(true);
            ReuseTabsService.OnStateHasChanged += OnStateHasChanged;

            if (RouteData != null)
            {
                ReuseTabsService.TrySetRouteData(RouteData, true);
            }
            else if (ReuseTabsRouteData != null)
            {
                ReuseTabsService.TrySetRouteData(ReuseTabsRouteData.RouteData, true);
            }

            Navmgr.LocationChanged += OnLocationChanged;
        }

        protected override bool ShouldRender() => !InReusePageContent;

        protected override void Dispose(bool disposing)
        {
            ReuseTabsService.OnStateHasChanged -= OnStateHasChanged;
            Navmgr.LocationChanged -= OnLocationChanged;
            base.Dispose(disposing);
        }

        private async Task<bool> OnTabEdit(string key, string action)
        {
            if (action != "remove")
                return false;

            return ReuseTabsService.ClosePage(key);
        }

        private void OnLocationChanged(object o, LocationChangedEventArgs _)
        {
            if (RouteData != null)
            {
                ReuseTabsService.TrySetRouteData(RouteData, true);
            }
            else if (ReuseTabsRouteData != null)
            {
                ReuseTabsService.TrySetRouteData(ReuseTabsRouteData.RouteData, true);
            }

            StateHasChanged();
        }

        private void OnStateHasChanged()
        {
            _ = InvokeStateHasChangedAsync();
        }
    }
}
