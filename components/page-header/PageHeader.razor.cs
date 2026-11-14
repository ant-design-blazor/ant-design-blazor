// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;
using System.Threading.Tasks;

namespace AntDesign
{
    /**
    <summary>
    <para>A header with common actions and design elements built in.</para>

    <h2>When To Use</h2>

    <para>PageHeader can be used to highlight the page topic, display important information about the page, and carry the action items related to the current page (including page-level operations, inter-page navigation, etc.) It can also be used as inter-page navigation.</para>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Navigation, "https://gw.alipayobjects.com/zos/alicdn/6bKE0Cq0R/PageHeader.svg", Columns = 1, Title = "PageHeader", SubTitle = "页头")]
    public partial class PageHeader
    {
        /// <summary>
        /// Make background transparent
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Ghost { get; set; }

        /// <summary>
        /// Icon for back button
        /// </summary>
        [Parameter]
        public OneOf<bool?, string> BackIcon { get; set; }

        /// <summary>
        /// Back button RenderFragment
        /// </summary>
        [Parameter]
        public RenderFragment BackIconTemplate { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Sub Title
        /// </summary>
        [Parameter]
        public string Subtitle { get; set; }

        /// <summary>
        /// Sub Title
        /// </summary>
        [Parameter]
        public RenderFragment SubtitleTemplate { get; set; }

        /// <summary>
        /// Callback when clicking back
        /// </summary>
        [Parameter]
        public EventCallback OnBack { get; set; }

        /// <summary>
        /// Content section
        /// </summary>
        [Parameter]
        public RenderFragment PageHeaderContent { get; set; }

        /// <summary>
        /// Footer section
        /// </summary>
        [Parameter]
        public RenderFragment PageHeaderFooter { get; set; }

        /// <summary>
        /// Breadcrumb section
        /// </summary>
        [Parameter]
        public RenderFragment PageHeaderBreadcrumb { get; set; }

        /// <summary>
        /// Avatar section
        /// </summary>
        [Parameter]
        public RenderFragment PageHeaderAvatar { get; set; }

        /// <summary>
        /// Title section
        /// </summary>
        [Obsolete("Use TitleTemplate instead")]
        [Parameter]
        public RenderFragment PageHeaderTitle { get; set; }

        /// <summary>
        /// Sub title section
        /// </summary>
        [Obsolete("Use SubtitleTemplate instead")]
        [Parameter]
        public RenderFragment PageHeaderSubtitle { get; set; }

        /// <summary>
        /// Tags section after title
        /// </summary>
        [Parameter]
        public RenderFragment PageHeaderTags { get; set; }

        /// <summary>
        /// Operating area, at the end of the line of the title line
        /// </summary>
        [Parameter]
        public RenderFragment PageHeaderExtra { get; set; }

        private bool _isCompact = false;

        private void SetClassMap()
        {
            ClassMapper
                .Add("ant-page-header")
                .If("ant-page-header-compact", () => _isCompact)
                .If("has-footer", () => PageHeaderFooter != null)
                .If("ant-page-header-ghost", () => this.Ghost)
                .If("has-breadcrumb", () => PageHeaderBreadcrumb != null)
                .If("ant-page-header-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        private void OnResize(DomRect domRect)
        {
            _isCompact = domRect.Width < 768;
        }

        private async Task OnBackClick(MouseEventArgs eventArgs)
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
