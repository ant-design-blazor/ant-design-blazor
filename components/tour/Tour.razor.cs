// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AntDesign
{
    /// <summary>
    /// Tour component for guided user experience
    /// </summary>
    public partial class Tour : AntDomComponentBase
    {
        private const string PrefixCls = "ant-tour";
        private static readonly string _getBoundingClientRectJs = JSInteropConstants.DomInfoHelper.GetBoundingClientRect;
        private static readonly string _disableBodyScrollJs = JSInteropConstants.DisableBodyScroll;
        private static readonly string _enableBodyScrollJs = JSInteropConstants.EnableBodyScroll;
        private List<TourStep> _steps;
        private TourStep _currentStep;
        private ElementReference? _targetElement;
        private double _panelX;
        private double _panelY;

        /// <summary>
        /// Steps for the tour
        /// </summary>
        [Parameter]
        public List<TourStep> Steps { get; set; } = new List<TourStep>();

        /// <summary>
        /// Whether the tour is open
        /// </summary>
        [Parameter]
        public bool Open { get; set; }

        /// <summary>
        /// Current step index
        /// </summary>
        [Parameter]
        public int Current { get; set; } = 0;

        /// <summary>
        /// Event callback when current step changes
        /// </summary>
        [Parameter]
        public EventCallback<int> CurrentChanged { get; set; }

        /// <summary>
        /// Event callback when current step changes
        /// </summary>
        [Parameter]
        public EventCallback<int> OnChange { get; set; }

        /// <summary>
        /// Event callback when tour is closed
        /// </summary>
        [Parameter]
        public EventCallback OnClose { get; set; }

        /// <summary>
        /// Event callback when tour is finished
        /// </summary>
        [Parameter]
        public EventCallback OnFinish { get; set; }

        /// <summary>
        /// Default type for all steps
        /// </summary>
        [Parameter]
        public TourType Type { get; set; } = TourType.Default;

        /// <summary>
        /// Whether to show mask by default
        /// </summary>
        [Parameter]
        public bool Mask { get; set; } = true;

        /// <summary>
        /// Default mask style
        /// </summary>
        [Parameter]
        public string MaskStyle { get; set; }

        /// <summary>
        /// z-index of the tour
        /// </summary>
        [Parameter]
        public int ZIndex { get; set; } = 1001;

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

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _steps = Steps ?? new List<TourStep>();
        }

        private bool _prevOpen;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (Open && _steps.Count > 0)
            {
                if (Current >= 0 && Current < _steps.Count)
                {
                    _currentStep = _steps[Current];
                    await ShowCurrentStep();
                }
                if (!_prevOpen)
                {
                    await JSRuntime.InvokeVoidAsync(_disableBodyScrollJs);
                }
            }
            else
            {
                if (_prevOpen)
                {
                    await JSRuntime.InvokeVoidAsync(_enableBodyScrollJs);
                }
                _currentStep = null;
            }
            _prevOpen = Open;
        }

        private async Task ShowCurrentStep()
        {
            if (_currentStep == null) return;

            var (x, y) = await GetTargetPosition();
            _panelX = x;
            _panelY = y;
        }

        private async Task<(double x, double y)> GetTargetPosition()
        {
            // 1. CSS selector 方式
            if (!string.IsNullOrWhiteSpace(_currentStep?.TargetSelector))
            {
                try
                {
                    var escaped = _currentStep.TargetSelector.Replace("\\", "\\\\").Replace("'", "\\'");
                    var rect = await JSRuntime.InvokeAsync<BoundingClientRect>(
                        "eval",
                        $"(function(){{var r=document.querySelector('{escaped}').getBoundingClientRect();" +
                        "return{{left:r.left,top:r.top,right:r.right,bottom:r.bottom,width:r.width,height:r.height,x:r.x,y:r.y}};}})()");
                    return CalculatePosition(rect, _currentStep.Placement);
                }
                catch (Exception)
                {
                    // selector 无效则降级到视口中心
                }
            }

            // 2. ElementReference 方式
            if (_currentStep?.Target != null)
            {
                try
                {
                    var elementRef = _currentStep.Target();
                    if (elementRef.HasValue && elementRef.Value.Id != null)
                    {
                        _targetElement = elementRef;
                        var rect = await JSRuntime.InvokeAsync<BoundingClientRect>(
                            _getBoundingClientRectJs,
                            elementRef);
                        return CalculatePosition(rect, _currentStep.Placement);
                    }
                }
                catch (Exception)
                {
                    // 目标元素未找到则降级到视口中心
                }
            }

            // 3. 默认居中，一次性获取视口尺寸
            try
            {
                var dims = await JSRuntime.InvokeAsync<int[]>("eval", "[window.innerWidth, window.innerHeight]");
                return (dims[0] / 2.0, dims[1] / 2.0);
            }
            catch
            {
                return (300, 200); // 兜底位置
            }
        }

        private (double x, double y) CalculatePosition(BoundingClientRect rect, Placement placement)
        {
            const int Offset = 12; // Arrow offset
            
            return placement switch
            {
                Placement.Top => (rect.Left + rect.Width / 2, rect.Top - Offset),
                Placement.TopLeft => (rect.Left, rect.Top - Offset),
                Placement.TopRight => (rect.Right, rect.Top - Offset),
                Placement.Bottom => (rect.Left + rect.Width / 2, rect.Bottom + Offset),
                Placement.BottomLeft => (rect.Left, rect.Bottom + Offset),
                Placement.BottomRight => (rect.Right, rect.Bottom + Offset),
                Placement.Left => (rect.Left - Offset, rect.Top + rect.Height / 2),
                Placement.LeftTop => (rect.Left - Offset, rect.Top),
                Placement.LeftBottom => (rect.Left - Offset, rect.Bottom),
                Placement.Right => (rect.Right + Offset, rect.Top + rect.Height / 2),
                Placement.RightTop => (rect.Right + Offset, rect.Top),
                Placement.RightBottom => (rect.Right + Offset, rect.Bottom),
                _ => (rect.Left + rect.Width / 2, rect.Bottom + Offset)
            };
        }

        private async Task HandleClose()
        {
            await JSRuntime.InvokeVoidAsync(_enableBodyScrollJs);
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync(null);
            }
        }

        private async Task HandlePrev()
        {
            if (Current > 0)
            {
                await SetCurrent(Current - 1);
            }
        }

        private async Task HandleNext()
        {
            if (Current < _steps.Count - 1)
            {
                await SetCurrent(Current + 1);
            }
        }

        private async Task HandleFinish()
        {
            await JSRuntime.InvokeVoidAsync(_enableBodyScrollJs);
            if (OnFinish.HasDelegate)
            {
                await OnFinish.InvokeAsync(null);
            }
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync(null);
            }
        }

        private async Task SetCurrent(int newCurrent)
        {
            if (newCurrent >= 0 && newCurrent < _steps.Count)
            {
                Current = newCurrent;
                _currentStep = _steps[Current];

                if (CurrentChanged.HasDelegate)
                {
                    await CurrentChanged.InvokeAsync(Current);
                }
                if (OnChange.HasDelegate)
                {
                    await OnChange.InvokeAsync(Current);
                }

                await ShowCurrentStep();
                StateHasChanged();
            }
        }

        private void OnMaskClick()
        {
            // Optional: close on mask click
        }

        private string GetMaskClass()
        {
            return ClassMapper
                .Add($"{PrefixCls}-mask")
                .ToString();
        }

        private string GetMaskStyle()
        {
            var style = $"z-index: {ZIndex - 1};";
            if (!string.IsNullOrEmpty(MaskStyle))
            {
                style += MaskStyle;
            }
            if (!string.IsNullOrEmpty(_currentStep?.MaskStyle))
            {
                style += _currentStep.MaskStyle;
            }
            return style;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Bounding client rect for element positioning
    /// </summary>
    public class BoundingClientRect
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
