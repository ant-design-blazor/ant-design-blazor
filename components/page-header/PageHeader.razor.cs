// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class PageHeader
    {
        [Parameter]
        public bool Ghost { get; set; }

        [Parameter]
        public OneOf<bool?, string> BackIcon { get; set; }

        [Parameter]
        public RenderFragment BackIconTemplate { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        [Parameter]
        public string Subtitle { get; set; }

        [Parameter]
        public RenderFragment SubtitleTemplate { get; set; }

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

        [Obsolete("Use TitleTemplate instead")]
        [Parameter]
        public RenderFragment PageHeaderTitle { get; set; }

        [Obsolete("Use SubtitleTemplate instead")]
        [Parameter]
        public RenderFragment PageHeaderSubtitle { get; set; }

        [Parameter]
        public RenderFragment PageHeaderTags { get; set; }

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
