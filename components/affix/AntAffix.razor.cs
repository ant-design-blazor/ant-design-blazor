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
        private const string RootScollSelector = "window";
        private const string RootRectSelector = "app";
        private bool _affixed;
        private bool _rendered;
        private bool _rootListened;
        private bool _targetListened;

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

        #endregion Parameters

        protected override void OnInitialized()
        {
            base.OnInitialized();

            SetClasses();
        }

        public async override Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);

            if (!_targetListened && !string.IsNullOrEmpty(Target.Id))
            {
                DomEventService.AddEventListener(Target, "scroll", OnScroll);
                DomEventService.AddEventListener(Target, "resize", OnWindowResize);

                await RenderAffixAsync();
                _targetListened = true;
            }
        }

        protected async override Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();
            _rendered = true;

            DomRect domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _childRef);
            _hiddenStyle = $"width: {domRect.width}px; height: {domRect.height}px;";

            if (_rootListened)
            {
                await RenderAffixAsync();
            }
            else
            {
                DomEventService.AddEventListener(RootScollSelector, "scroll", OnScroll);
                DomEventService.AddEventListener(RootScollSelector, "resize", OnWindowResize);
                _rootListened = true;
            }
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
            DomRect childRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _childRef);
            _hiddenStyle = $"width: {childRect.width}px; height: {childRect.height}px;";

            DomRect domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _ref);
            DomRect appRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, RootRectSelector);
            // reset appRect.top / bottom, so its position is fixed.
            appRect.top = 0;
            appRect.bottom = appRect.height;
            DomRect containerRect;
            if (string.IsNullOrEmpty(Target.Id))
            {
                containerRect = appRect;
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
                    _affixStyle = _hiddenStyle + $"bottom: { appRect.height - containerRect.bottom + OffsetBottom}px; position: fixed;";
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
