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
    public partial class BackTop : AntDomComponentBase
    {
        [Parameter]
        public string Icon { get; set; } = "vertical-align-top";

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public double VisibilityHeight { get; set; } = 400;

        /// <summary>
        /// 回到顶部的目标控件
        /// </summary>
        [Parameter]
        public string TargetSelector { get; set; }

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

        private async void OnScroll(JsonElement obj)
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
