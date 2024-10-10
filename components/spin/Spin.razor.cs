// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Timers;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
        <para>A spinner for displaying loading state of a page or a section.</para>

        <h2>When To Use</h2>

        <para>When part of the page is waiting for asynchronous data or during a rendering process, an appropriate loading animation can effectively alleviate users' inquietude.</para>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Feedback, "https://gw.alipayobjects.com/zos/alicdn/LBcJqCPRv/Spin.svg", Title = "Spin", SubTitle = "加载中")]
    public partial class Spin : AntDomComponentBase
    {
        /// <summary>
        /// Size of the spinner. Possible values: small, default, large
        /// </summary>
        /// <default value="default" />
        [Parameter]
        public string Size { get; set; } = "default";

        /// <summary>
        /// Customize description content when Spin has children
        /// </summary>
        [Parameter]
        public string Tip { get; set; } = null;

        /// <summary>
        /// Specifies a delay in milliseconds for loading state (prevent flush)
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int Delay { get; set; } = 0;

        /// <summary>
        /// Whether spin is active
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Spinning { get; set; } = true;

        /// <summary>
        /// Class name for the wrapper
        /// </summary>
        [Parameter]
        public string WrapperClassName { get; set; }

        /// <summary>
        /// Custom display for the spinning indicator
        /// </summary>
        [Parameter]
        public RenderFragment Indicator { get; set; }

        /// <summary>
        /// Content the spin will indicate loading for
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private ClassMapper WrapperClassMapper { get; set; } = new ClassMapper();

        private const string PrefixCls = "ant-spin";

        private bool _isLoading = true;

        private bool Simple => ChildContent == null;

        private string ContainerClass => _isLoading ? $"{PrefixCls}-blur" : "";

        private Timer _delayTimer;

        private void SetClass()
        {
            var hashId = UseStyle(PrefixCls, SpinStyle.UseComponentStyle);
            WrapperClassMapper
                .If(WrapperClassName, () => !string.IsNullOrWhiteSpace(WrapperClassName))
                .Add(hashId)
                .If($"{PrefixCls}-nested-loading", () => !Simple);

            ClassMapper
                .Add(PrefixCls)
                .Add(hashId)
                .If($"{PrefixCls}-spinning", () => _isLoading)
                .If($"{PrefixCls}-lg", () => Size == "large")
                .If($"{PrefixCls}-sm", () => Size == "small")
                .If($"{PrefixCls}-show-text", () => string.IsNullOrWhiteSpace(Tip))
                .If($"{PrefixCls}-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            SetClass();

            _isLoading = Spinning;

            if (Delay > 0)
            {
                _delayTimer = new Timer(Delay);
                _delayTimer.Elapsed += DelayElapsed;
            }

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (_delayTimer != null)
            {
                if (_isLoading != Spinning)
                {
                    _delayTimer.Stop();
                    _delayTimer.Start();
                }
                else
                {
                    _delayTimer.Stop();
                }
            }
            else
            {
                if (_isLoading != Spinning)
                {
                    _isLoading = Spinning;
                    InvokeAsync(StateHasChanged);
                }
            }
        }

        private void DelayElapsed(object sender, ElapsedEventArgs args)
        {
            _isLoading = Spinning;
            InvokeAsync(StateHasChanged);
        }

        protected override void Dispose(bool disposing)
        {
            _delayTimer?.Dispose();
            base.Dispose(disposing);
        }
    }
}
