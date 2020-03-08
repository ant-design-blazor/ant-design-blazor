using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public class AntBreadcrumbBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] public bool autoGenerate { get; set; } = false;

        [Parameter] public string separator { get; set; } = "/";

        [Parameter] public string routeLabel { get; set; } = "breadcrumb";

        [Inject]
        public NavigationManager navigationManager { get; set; }

        protected BreadcrumbOption[] _breadcrumbs = { };

        protected void navigate(string url, MouseEventArgs e)
        {
            navigationManager.NavigateTo(url);
        }

        protected override void OnInitialized()
        {
            this.ClassMapper.Add("ant-breadcrumb");

            base.OnInitialized();
        }

        private void registerRouterChange()
        {
        }
    }

    public struct BreadcrumbOption
    {
        public string Label { get; set; }

        public Dictionary<string, object> Params { get; set; }

        public string Url { get; set; }
    }
}