// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// Tour panel component for displaying step content
    /// </summary>
    public partial class TourPanel : AntDomComponentBase
    {
        private const string PrefixCls = "ant-tour";

        /// <summary>
        /// Current step configuration
        /// </summary>
        [Parameter]
        public TourStep Step { get; set; }

        /// <summary>
        /// Current step index
        /// </summary>
        [Parameter]
        public int Current { get; set; }

        /// <summary>
        /// Total number of steps
        /// </summary>
        [Parameter]
        public int Total { get; set; }

        /// <summary>
        /// Tour type
        /// </summary>
        [Parameter]
        public TourType Type { get; set; } = TourType.Default;

        /// <summary>
        /// Custom close icon
        /// </summary>
        [Parameter]
        public RenderFragment CloseIcon { get; set; }

        /// <summary>
        /// Custom indicator render
        /// </summary>
        [Parameter]
        public RenderFragment<(int current, int total)> IndicatorsRender { get; set; }

        /// <summary>
        /// Close callback
        /// </summary>
        [Parameter]
        public EventCallback OnClose { get; set; }

        /// <summary>
        /// Previous button callback
        /// </summary>
        [Parameter]
        public EventCallback OnPrev { get; set; }

        /// <summary>
        /// Next button callback
        /// </summary>
        [Parameter]
        public EventCallback OnNext { get; set; }

        /// <summary>
        /// Finish button callback
        /// </summary>
        [Parameter]
        public EventCallback OnFinish { get; set; }

        /// <summary>
        /// Panel left position (px)
        /// </summary>
        [Parameter]
        public double X { get; set; }

        /// <summary>
        /// Panel top position (px)
        /// </summary>
        [Parameter]
        public double Y { get; set; }

        private bool IsLastStep => Current == Total - 1;

        private string GetPanelClass()
        {
            return ClassMapper
                .Add($"{PrefixCls}-panel")
                .If($"{PrefixCls}-primary", () => Type == TourType.Primary)
                .ToString();
        }

        private string GetPanelStyle()
        {
            return $"position:fixed;left:{X.ToString(System.Globalization.CultureInfo.InvariantCulture)}px;top:{Y.ToString(System.Globalization.CultureInfo.InvariantCulture)}px;z-index:1002;";
        }

        private string GetSectionClass()
        {
            return $"{PrefixCls}-section";
        }

        private string GetIndicatorClass(int index)
        {
            return ClassMapper
                .Add($"{PrefixCls}-indicator")
                .If($"{PrefixCls}-indicator-active", () => index == Current)
                .ToString();
        }

        private string GetPrevButtonClass()
        {
            var buttonClass = $"{PrefixCls}-prev-btn";
            if (!string.IsNullOrEmpty(Step?.PrevButtonProps?.Class))
            {
                buttonClass += " " + Step.PrevButtonProps.Class;
            }
            return buttonClass;
        }

        private string GetNextButtonClass()
        {
            var buttonClass = $"{PrefixCls}-next-btn";
            if (!string.IsNullOrEmpty(Step?.NextButtonProps?.Class))
            {
                buttonClass += " " + Step.NextButtonProps.Class;
            }
            return buttonClass;
        }

        private string GetPrevButtonText()
        {
            if (!string.IsNullOrEmpty(Step?.PrevButtonProps?.Text))
            {
                return Step.PrevButtonProps.Text;
            }
            return "Previous"; // TODO: Add localization
        }

        private string GetNextButtonText()
        {
            if (IsLastStep)
            {
                return "Finish"; // TODO: Add localization
            }
            if (!string.IsNullOrEmpty(Step?.NextButtonProps?.Text))
            {
                return Step.NextButtonProps.Text;
            }
            return "Next"; // TODO: Add localization
        }

        private ButtonType GetMainButtonType()
        {
            return Type == TourType.Primary ? ButtonType.Default : ButtonType.Primary;
        }

        private ButtonType GetSecondaryButtonType()
        {
            return ButtonType.Default;
        }

        private async Task HandleClose()
        {
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync(null);
            }
        }

        private async Task HandlePrev()
        {
            // Execute custom handler first if provided
            if (Step?.PrevButtonProps?.OnClick.HasDelegate ?? false)
            {
                await Step.PrevButtonProps.OnClick.InvokeAsync(null);
            }

            if (OnPrev.HasDelegate)
            {
                await OnPrev.InvokeAsync(null);
            }
        }

        private async Task HandleNext()
        {
            // Execute custom handler first if provided
            if (Step?.NextButtonProps?.OnClick.HasDelegate ?? false)
            {
                await Step.NextButtonProps.OnClick.InvokeAsync(null);
            }

            if (OnNext.HasDelegate)
            {
                await OnNext.InvokeAsync(null);
            }
        }

        private async Task HandleFinish()
        {
            // Execute custom handler first if provided
            if (Step?.NextButtonProps?.OnClick.HasDelegate ?? false)
            {
                await Step.NextButtonProps.OnClick.InvokeAsync(null);
            }

            if (OnFinish.HasDelegate)
            {
                await OnFinish.InvokeAsync(null);
            }
        }
    }
}
