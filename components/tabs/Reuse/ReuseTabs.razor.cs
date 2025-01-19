// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
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
        /// cover the base ChildContent
        /// </summary>
        private new RenderFragment ChildContent { get; set; }

        private RouteData ActualRouteData => RouteData ?? ReuseTabsRouteData?.RouteData;

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

            ReuseTabsService.Init(true);
            ReuseTabsService.OnStateHasChanged += OnStateHasChanged;

            LoadPage();

            base.ChildContent = RenderPanes;

            ActiveKey = ReuseTabsService.ActiveKey;
        }

        private void LoadPage()
        {
            ReuseTabsService.TrySetRouteData(ActualRouteData, true);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var isRouteDataChanged = parameters.IsParameterChanged(nameof(RouteData), RouteData, out var newValue) && IsRouteDataChanged(RouteData, newValue);
            var isReuseTabsRouteDataChanged = parameters.IsParameterChanged(nameof(ReuseTabsRouteData), ReuseTabsRouteData, out var newReuseTabsRouteData) && IsRouteDataChanged(ReuseTabsRouteData?.RouteData, newReuseTabsRouteData?.RouteData);

            await base.SetParametersAsync(parameters);
            if (isRouteDataChanged || isReuseTabsRouteDataChanged)
            {
                UpdateRouteData();
            }
        }

        protected override bool ShouldRender() => !InReusePageContent && base.ShouldRender();

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            // reload current page after ReloadPage() was called by ReuseTabsService
            if (ReuseTabsService.CurrentPage.Rendered == false)
            {
                LoadPage();
                StateHasChanged(); // update title need calling render again
            }
        }

        protected override void Dispose(bool disposing)
        {
            ReuseTabsService.OnStateHasChanged -= OnStateHasChanged;
            Navmgr.LocationChanged -= OnLocationChanged;
            base.Dispose(disposing);
        }

        private void ClosePage(string key)
        {
            UpdateTabsPosition();
            ReuseTabsService.ClosePageByKey(key);
        }

        protected override void OnRemoveTab(TabPane tab)
        {
            ReuseTabsService.ClosePageByKey(tab.Key);
        }

        protected override void OnActiveTabChanged(TabPane tab)
        {
            if (tab.Key != ReuseTabsService.ActiveKey)
            {
                ReuseTabsService.ActiveKey = tab.Key;
            }
        }

        private void OnLocationChanged(object o, LocationChangedEventArgs e)
        {
            // sometime the routeData is not updated in time
            // Here it is called almost only when Routedata is null because SetParametersAsync is called before OnLocationChanged
            if (RouteData == null || IsRouteDataChanged(ActualRouteData, ReuseTabsService.CurrentPage?.RouteData))
            {
                UpdateRouteData();
            }
        }

        private void UpdateRouteData()
        {
            UpdateTabsPosition();

            LoadPage();

            ActivatePane(ReuseTabsService.ActiveKey);
        }

        private void OnStateHasChanged()
        {
            SetShowRender();
            StateHasChanged();
        }

        private static bool IsRouteDataChanged(RouteData left, RouteData right)
        {
            if (left == null && right == null)
            {
                return false;
            }
            else if (left == null || right == null)
            {
                return true;
            }
            else if (left.PageType != right.PageType)
            {
                return true;
            }
            else if (left.RouteValues.Count != right.RouteValues.Count)
            {
                return true;
            }
            else
            {
                foreach (var kv in left.RouteValues)
                {
                    if (!right.RouteValues.ContainsKey(kv.Key) || !object.Equals(kv.Value, right.RouteValues[kv.Key]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
