using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public partial class AntBreadcrumb : AntDomComponentBase
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

        protected BreadcrumbOption[] _breadcrumbs = { };

        protected void Navigate(string url, MouseEventArgs e)
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

    public struct BreadcrumbOption
    {
        public string Label { get; set; }

        public Dictionary<string, object> Params { get; set; }

        public string Url { get; set; }
    }
}
