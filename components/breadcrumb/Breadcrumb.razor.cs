using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Breadcrumb : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool AutoGenerate { get; set; } = false;

        [Parameter]
        public string Separator { get; set; } = "/";

        [Parameter]
        public string RouteLabel { get; set; } = "breadcrumb";

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private readonly BreadcrumbOption[] _breadcrumbs = Array.Empty<BreadcrumbOption>();

        private void Navigate(string url)
        {
            NavigationManager.NavigateTo(url);
        }

        protected override void OnInitialized()
        {
            this.ClassMapper.Add("ant-breadcrumb");

            base.OnInitialized();
        }

        private void RegisterRouterChange()
        {
        }
    }

    public class BreadcrumbOption
    {
        public string Label { get; set; }

        public Dictionary<string, object> Params { get; set; }

        public Uri Url { get; set; }
    }
}
