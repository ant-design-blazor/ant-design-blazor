using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class PageHeader
    {
        #region Parameters

        [Parameter] public bool Ghost { get; set; }

        [Parameter] public OneOf<string, RenderFragment> BackIcon { get; set; }

        [Parameter] public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter] public OneOf<string, RenderFragment> Subtitle { get; set; }

        [Parameter] public EventCallback OnBack { get; set; }

        [Parameter] public RenderFragment PageHeaderContent { get; set; }

        [Parameter] public RenderFragment PageHeaderFooter { get; set; }

        [Parameter] public RenderFragment PageHeaderBreadcrumb { get; set; }

        [Parameter] public RenderFragment PageHeaderAvatar { get; set; }

        [Parameter] public RenderFragment PageHeaderTitle { get; set; }

        [Parameter] public RenderFragment PageHeaderSubtitle { get; set; }

        [Parameter] public RenderFragment PageHeaderTags { get; set; }

        [Parameter] public RenderFragment PageHeaderExtra { get; set; }

        #endregion Parameters

        private void SetClassMap()
        {
            ClassMapper.Clear().Add("ant-page-header")
                .If("has-footer", () => PageHeaderFooter != null)
                .If("ant-page-header-ghost", () => this.Ghost)
                .If("has-breadcrumb", () => PageHeaderBreadcrumb != null);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        private async void OnBackClick(MouseEventArgs eventArgs)
        {
            if (OnBack.HasDelegate)
            {
                await OnBack.InvokeAsync(eventArgs);
            }
            else
            {
                await JsInvokeAsync("history.back");
            }
        }
    }
}
