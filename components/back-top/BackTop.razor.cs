using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class BackTop : AntDomComponentBase
    {
        private bool _visible = false;

        [Inject]
        public DomEventService DomEventService { get; set; }

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
                DomEventService.AddEventListener("window", "scroll", OnScroll);
            }
            else
            {
                DomEventService.AddEventListener(TargetSelector, "scroll", OnScroll);
            }
            await base.OnFirstAfterRenderAsync();
        }

        protected void SetClass()
        {
            string prefixCls = "ant-back-top";
            ClassMapper.Add(prefixCls);
            BackTopContentClassMapper.Add($"{prefixCls}-content");
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
            base.Dispose(disposing);
            if (string.IsNullOrWhiteSpace(TargetSelector))
            {
                DomEventService.RemoveEventListerner<JsonElement>("window", "scroll", OnScroll);
            }
            else
            {
                DomEventService.RemoveEventListerner<JsonElement>(TargetSelector, "scroll", OnScroll);
            }
        }
    }
}
