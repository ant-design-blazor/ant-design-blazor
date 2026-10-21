// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <default value="SpinSize.Default" />
        [Parameter]
        public SpinSize Size { get; set; } = SpinSize.Default;

        /// <summary>
        /// Customize description content when Spin has children
        /// </summary>
        [Obsolete("Use Description instead.")]
        [Parameter]
        public string Tip { get; set; } = null;

        /// <summary>Customize the loading description.</summary>
        [Parameter]
        public string Description { get; set; }

        /// <summary>CSS classes keyed by Spin semantic DOM parts.</summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> ClassNames { get; set; }

        /// <summary>Inline CSS declarations keyed by Spin semantic DOM parts.</summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> Styles { get; set; }

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

        private string EffectiveDescription => Description ?? Tip;

        private string ContainerClass => _isLoading ? $"{PrefixCls}-blur" : "";

        internal string SemanticClass(string slot, string baseClass = null, string legacySlot = null)
        {
            var classNames = new[]
            {
                baseClass,
                GetSemanticValue(ClassNames, legacySlot),
                GetSemanticValue(ClassNames, slot)
            };
            return string.Join(" ", classNames.Where(value => !string.IsNullOrWhiteSpace(value)));
        }

        internal string SemanticStyle(string slot, string baseStyle = null, string legacySlot = null)
        {
            var styleParts = new[]
            {
                baseStyle,
                GetSemanticValue(Styles, legacySlot),
                GetSemanticValue(Styles, slot)
            };
            return string.Join(";", styleParts
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value!.Trim().TrimEnd(';')));
        }

        private static string GetSemanticValue(IReadOnlyDictionary<string, string> values, string key)
        {
            return key != null && values != null && values.TryGetValue(key, out var value) ? value : null;
        }

        private Timer _delayTimer;

        private void SetClass()
        {
            WrapperClassMapper
                .If(WrapperClassName, () => !string.IsNullOrWhiteSpace(WrapperClassName))
                .If($"{PrefixCls}-nested-loading", () => !Simple);

            ClassMapper
                .Add(PrefixCls)
                .If($"{PrefixCls}-spinning", () => _isLoading)
                .If($"{PrefixCls}-lg", () => Size == SpinSize.Large)
                .If($"{PrefixCls}-sm", () => Size == SpinSize.Small)
                .If($"{PrefixCls}-show-text", () => !string.IsNullOrWhiteSpace(EffectiveDescription))
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
