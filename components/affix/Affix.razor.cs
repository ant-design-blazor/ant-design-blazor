// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>Wrap Affix around another component to make it stick the viewport.</para>
    <h2>When To Use</h2>
    <list type="bullet">
        <item>On longer web pages, its helpful for some content to stick to the viewport. This is common for menus and actions.</item>
        <item>Please note that Affix should not cover other content on the page, especially when the size of the viewport is small.</item>
    </list>
    <para><strong>Important</strong>: Children of <c>Affix</c> must not have the property <c>position: absolute</c>, but you can set <c>position: absolute</c> on <c>Affix</c> itself</para>
    </summary> 
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Navigation, "https://gw.alipayobjects.com/zos/alicdn/tX6-md4H6/Affix.svg", Title = "Affix", SubTitle = "图钉")]
    public partial class Affix : AntDomComponentBase
    {
        private const string PrefixCls = "ant-affix";
        private const string RootScollSelector = "window";

        private bool _affixed;

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

                    if (OnChange.HasDelegate)
                    {
                        OnChange.InvokeAsync(_affixed);
                    }
                }
            }
        }

        private ElementReference _childRef;
        private string _hiddenStyle;
        private string _affixStyle;

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        #region Parameters

        /// <summary>
        /// Offset from the bottom of the viewport (in pixels)
        /// </summary>
        [Parameter]
        public int OffsetBottom { get; set; }

        /// <summary>
        /// Offset from the top of the viewport (in pixels)
        /// </summary>
        [Parameter]
        public int OffsetTop { get; set; }

        /// <summary>
        /// The CSS selector that specifies the scrollable area DOM node
        /// </summary>
        /// <default value="window" />
        [Parameter]
        public string TargetSelector { get; set; }

        /// <summary>
        /// Additional Content
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Callback for when Affix state is changed. A boolean indicating if the Affix is currently affixed is passed.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnChange { get; set; }

        #endregion Parameters

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper
               .If(PrefixCls, () => _affixed);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            if (ChildContent == null)
            {
                return;
            }

            var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _childRef);
            _hiddenStyle = $"width: {domRect.Width}px; height: {domRect.Height}px;";

            await RenderAffixAsync();
            if (!_rootListened && string.IsNullOrEmpty(TargetSelector))
            {
                DomEventListener.AddShared<JsonElement>(RootScollSelector, "scroll", OnWindowScroll);
                DomEventListener.AddShared<JsonElement>(RootScollSelector, "resize", OnWindowResize);
                _rootListened = true;
            }
            else if (!string.IsNullOrEmpty(TargetSelector))
            {
                DomEventListener.AddExclusive<JsonElement>(TargetSelector, "scroll", OnTargetScroll);
                DomEventListener.AddExclusive<JsonElement>(TargetSelector, "resize", OnTargetResize);
                _targetListened = true;
            }
        }

        private async Task OnWindowScroll(JsonElement obj) => await RenderAffixAsync();

        private async Task OnWindowResize(JsonElement obj) => await RenderAffixAsync();

        private async Task OnTargetScroll(JsonElement obj) => await RenderAffixAsync();

        private async Task OnTargetResize(JsonElement obj) => await RenderAffixAsync();

        private async Task RenderAffixAsync()
        {
            var originalAffixStyle = _affixStyle;
            DomRect domRect = null;
            Window window = null;

            async Task GetWindow()
            {
                window = await JsInvokeAsync<Window>(JSInteropConstants.GetWindow);
            }

            async Task GetDomReact()
            {
                domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref);
            }

            await Task.WhenAll(new[] { GetWindow(), GetDomReact() });
            if (domRect == null || window == null)
            {
                return;
            }

            _hiddenStyle = $"width: {domRect.Width}px; height: {domRect.Height}px;";

            DomRect containerRect;
            if (string.IsNullOrEmpty(TargetSelector))
            {
                containerRect = new DomRect()
                {
                    Top = 0,
                    Bottom = window.InnerHeight,
                    Height = window.InnerHeight,
                };
            }
            else
            {
                containerRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, TargetSelector);
            }

            var topDist = containerRect.Top + OffsetTop;
            var bottomDist = containerRect.Bottom - OffsetBottom;

            if (OffsetBottom > 0) // only affix bottom
            {
                if (domRect.Bottom > bottomDist)
                {
                    _affixStyle = _hiddenStyle + $"bottom: {window.InnerHeight - bottomDist}px; position: fixed;";
                    Affixed = true;
                }
                else
                {
                    _affixStyle = string.Empty;
                    Affixed = false;
                }
            }
            else if (domRect.Top < topDist)
            {
                _affixStyle = _hiddenStyle + $"top: {topDist}px; position: fixed;";
                Affixed = true;
            }
            else
            {
                _affixStyle = string.Empty;
                Affixed = false;
            }

            if (originalAffixStyle != _affixStyle)
            {
                StateHasChanged();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            DomEventListener?.Dispose();
        }
    }
}
