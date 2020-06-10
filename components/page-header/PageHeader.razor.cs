using System;
using System.Collections.Generic;
using System.Text;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class PageHeader
    {
        #region Parameters

        [Parameter]
        public string BackIcon { get; set; } = "";

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Subtitle { get; set; }

        [Parameter]
        public EventCallback OnBack { get; set; }


        [Parameter]
        public RenderFragment PageHeaderContent { get; set; }

        [Parameter]
        public RenderFragment PageHeaderFooter { get; set; }

        [Parameter]
        public RenderFragment PageHeaderBreadcrumb { get; set; }

        [Parameter]
        public RenderFragment PageHeaderAvatar { get; set; }

        [Parameter]
        public RenderFragment PageHeaderTitle { get; set; }

        [Parameter]
        public RenderFragment PageHeaderSubtitle { get; set; }

        [Parameter]
        public RenderFragment PageHeaderTags { get; set; }

        [Parameter]
        public RenderFragment PageHeaderExtra { get; set; }

        #endregion


        [Inject]
        public NavigationManager NavigationManager { get; set; }


        private async void OnBackClick(MouseEventArgs eventArgs)
        {
            if (OnBack.HasDelegate)
            {
                await OnBack.InvokeAsync(eventArgs);
            }
            else
            {
            }

        }
    }
}
