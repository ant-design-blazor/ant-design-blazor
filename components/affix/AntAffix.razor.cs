using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntAffix : AntDomComponentBase
    {
        private const string PrefixCls = "ant-affix";
        private bool _affixed;
        private bool _rendered;
        private bool _listened;
        private bool Affixed
        {
            get => _affixed;
            set
            {
                if (_affixed != value)
                {
                    _affixed = value;
                    StateHasChanged();
                    if (OnChange.HasDelegate)
                    {
                        OnChange.InvokeAsync(_affixed);
                    }
                }
            }
        }
        private ElementReference _ref;
        private ElementReference _childRef;
        private string _hiddenStyle;
        private string _affixStyle;

        [Inject]
        private DomEventService DomEventService { get; set; }

        #region Parameters

        /// <summary>
        /// Offset from the bottom of the viewport (in pixels)
        /// </summary>
        [Parameter]
        public uint? OffsetBottom { get; set; }

        /// <summary>
        /// Offset from the top of the viewport (in pixels)
        /// </summary>
        [Parameter]
        public uint? OffsetTop { get; set; } = 0;

        /// <summary>
        /// Specifies the scrollable area DOM node
        /// </summary>
        [Parameter]
        public ElementReference Target { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<bool> OnChange { get; set; }

        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClasses();
            //DomEventService.AddEventListener("#BodyContainer", "scroll", OnScroll);
            //DomEventService.AddEventListener("window", "resize", OnWindowResize);
        }

        protected async override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_rendered)
            {
                if (_listened)
                {

                    await RenderAffixAsync();
                }
                else
                {
                    DomEventService.AddEventListener("#BodyContainer", "scroll", OnScroll);
                    DomEventService.AddEventListener("#BodyContainer", "resize", OnWindowResize);
                    if (!string.IsNullOrEmpty(Target.Id))
                    {
                        DomEventService.AddEventListener(Target, "scroll", OnScroll);
                        DomEventService.AddEventListener(Target, "resize", OnWindowResize);
                    }

                    _listened = true;
                }
            }
        }

        protected async override Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();
            _rendered = true;

            DomRect domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _childRef);
            _hiddenStyle = $"width: {domRect.width}px; height: {domRect.height}px;";
        }

        private async void OnScroll(JsonElement obj)
        {
            await RenderAffixAsync();
        }

        private async void OnWindowResize(JsonElement obj)
        {
            await RenderAffixAsync();
        }

        private void SetClasses()
        {
            ClassMapper.Clear()
                .If(PrefixCls, () => _affixed);
        }

        private async Task RenderAffixAsync()
        {
            DomRect domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _ref);
            DomRect bodyContainerRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, "#BodyContainer");
            DomRect containerRect;
            if (string.IsNullOrEmpty(Target.Id))
            {
                containerRect = bodyContainerRect;
            }
            else
            {
                containerRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, Target);
            }
            // become affixed
            if (OffsetBottom.HasValue)
            {
                // domRect.bottom / domRect.top have the identical value here.
                if (domRect.top > containerRect.height + containerRect.top)
                {
                    _affixStyle = _hiddenStyle + $"bottom: { bodyContainerRect.height - containerRect.bottom + OffsetBottom}px; position: fixed;";
                    Affixed = true;
                }
                else
                {
                    _affixStyle = string.Empty;
                    Affixed = false;
                }
            }
            else if (OffsetTop.HasValue)
            {
                if (domRect.top < containerRect.top)
                {
                    _affixStyle = _hiddenStyle + $"top: {containerRect.top + OffsetTop}px; position: fixed;";
                    Affixed = true;
                }
                else
                {
                    _affixStyle = string.Empty;
                    Affixed = false;
                }
            }

            StateHasChanged();
        }
    }
}
