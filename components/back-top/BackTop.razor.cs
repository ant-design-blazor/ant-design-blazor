﻿using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class BackTop : AntDomComponentBase
    {
        private bool _visible = false;

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

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

        protected ClassMapper BackTopContentClassMapper { get; set; } = new ClassMapper();

        protected ClassMapper BackTopIconClassMapper { get; set; } = new ClassMapper();

        [Parameter]
        public EventCallback OnClick { get; set; }

        protected async Task OnClickHandle()
        {
            if (string.IsNullOrWhiteSpace(TargetSelector))
                await JsInvokeAsync<DomRect>(JSInteropConstants.BackTop);
            else
                await JsInvokeAsync<DomRect>(JSInteropConstants.BackTop, TargetSelector);

            if (OnClick.HasDelegate)
                await OnClick.InvokeAsync(null);
        }

        protected override void OnInitialized()
        {
            SetClass();
            base.OnInitialized();
        }

        protected async override Task OnFirstAfterRenderAsync()
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
            _visible = offset > VisibilityHeight;
            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener.DisposeExclusive();
            base.Dispose(disposing);
        }
    }
}
