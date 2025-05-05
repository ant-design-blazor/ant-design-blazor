// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /** 
    <summary>
    <para>Makes it easy to go back to the top of the page.</para>
    
    <h2>When To Use</h2>
    <list type="bullet">
        <item>When the page content is very long.</item>
        <item>When you need to go back to the top very frequently in order to view the contents.</item>
    </list>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Other, "https://gw.alipayobjects.com/zos/alicdn/tJZ5jbTwX/BackTop.svg", Title = "BackTop", SubTitle = "回到顶部")]
    public partial class BackTop : AntDomComponentBase
    {
        /// <summary>
        /// Icon for the button
        /// </summary>
        /// <default value="vertical-align-top" />
        [Parameter]
        public string Icon { get; set; } = "vertical-align-top";

        /// <summary>
        /// Content for the button. Takes priority over icon.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Scroll offset at which the button becomes visible, in px
        /// </summary>
        /// <default value="400" />
        [Parameter]
        public double VisibilityHeight { get; set; } = 400;

        /// <summary>
        /// The scrollable area the button is for
        /// </summary>
        /// <default value="window" />
        [Parameter]
        public string TargetSelector { get; set; } = "window";

        /// <summary>
        /// Callback executed when BackTop gets clicked. Won't override default functionality.
        /// </summary>
        [Parameter]
        public EventCallback OnClick { get; set; }

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        protected ClassMapper BackTopContentClassMapper { get; set; } = new ClassMapper();

        protected ClassMapper BackTopIconClassMapper { get; set; } = new ClassMapper();

        private bool _visible = false;
        private bool _hidden = false;

        protected async Task OnClickHandler(MouseEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TargetSelector))
                await JsInvokeAsync<DomRect>(JSInteropConstants.BackTop);
            else
                await JsInvokeAsync<DomRect>(JSInteropConstants.BackTop, TargetSelector);

            if (OnClick.HasDelegate)
                await OnClick.InvokeAsync(e);
        }

        protected override void OnInitialized()
        {
            SetClass();
            base.OnInitialized();
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            if (string.IsNullOrWhiteSpace(TargetSelector))
            {
                DomEventListener.AddExclusive<JsonElement>("window", "scroll", OnScroll);
            }
            else
            {
                DomEventListener.AddExclusive<JsonElement>(TargetSelector, "scroll", OnScroll);
            }
            await base.OnFirstAfterRenderAsync();
        }

        protected void SetClass()
        {
            string prefixCls = "ant-back-top";
            ClassMapper.Add(prefixCls);

            BackTopContentClassMapper
                .If($"{prefixCls}-content", () => ChildContent == null)
                .If("ant-fade ant-fade-leave ant-fade-leave-active", () => !_visible)
                .If("ant-fade ant-fade-enter ant-fade-enter-active", () => _visible)
                ;

            BackTopIconClassMapper.Add($"{prefixCls}-icon");
        }

        private async Task OnScroll(JsonElement obj)
        {
            JsonElement scrollInfo = await JsInvokeAsync<JsonElement>(JSInteropConstants.GetScroll);
            double offset = scrollInfo.GetProperty("y").GetDouble();
            var visible = offset > VisibilityHeight;

            if (visible == _visible)
                return;

            _visible = visible;

            StateHasChanged();

            if (_visible)
            {
                _hidden = false;
            }
            else
            {
                await Task.Delay(300);
                _hidden = true;
            }

            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener.DisposeExclusive();
            base.Dispose(disposing);
        }
    }
}
