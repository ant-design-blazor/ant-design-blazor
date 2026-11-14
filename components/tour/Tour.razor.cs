using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /// <summary>
    /// A step-by-step guide that helps users understand a feature or complete a task.
    /// </summary>
    /// <seealso cref="TourStep" />
    /// <seealso cref="TourType" />
    /// <seealso cref="Placement" />
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/RT_USzA48/DatePicker.svg", Title = "Tour", SubTitle = "漫游式引导")]
    public partial class Tour : AntDomComponentBase
    {
        /// <summary>
        /// Steps displayed by the tour.
        /// </summary>
        [Parameter] public IReadOnlyList<TourStep> Steps { get; set; } = Array.Empty<TourStep>();
        /// <summary>
        /// Whether the tour is visible.
        /// </summary>
        [Parameter] public bool Open { get; set; }
        /// <summary>
        /// Raised when the visible state changes.
        /// </summary>
        [Parameter] public EventCallback<bool> OpenChanged { get; set; }
        /// <summary>
        /// Zero-based index of the current step.
        /// </summary>
        [Parameter] public int Current { get; set; }
        /// <summary>
        /// Raised when the current step changes.
        /// </summary>
        [Parameter] public EventCallback<int> CurrentChanged { get; set; }
        /// <summary>
        /// Raised after the current step changes.
        /// </summary>
        [Parameter] public EventCallback<int> OnChange { get; set; }
        /// <summary>
        /// Raised when the tour is closed.
        /// </summary>
        [Parameter] public EventCallback OnClose { get; set; }
        /// <summary>
        /// Raised when the last step is finished.
        /// </summary>
        [Parameter] public EventCallback OnFinish { get; set; }
        /// <summary>
        /// Custom renderer for the step indicators.
        /// </summary>
        [Parameter] public RenderFragment<(int Current, int Total)> IndicatorsRender { get; set; }
        /// <summary>
        /// Custom renderer for the action buttons.
        /// </summary>
        [Parameter] public RenderFragment<TourActionsRenderContext> ActionsRender { get; set; }
        /// <summary>
        /// Whether to render the modal mask.
        /// </summary>
        [Parameter] public bool Mask { get; set; } = true;
        /// <summary>
        /// Additional inline style for the mask.
        /// </summary>
        [Parameter] public string MaskStyle { get; set; }
        /// <summary>
        /// Mask color. Defaults to a translucent black.
        /// </summary>
        [Parameter] public string MaskColor { get; set; } = "rgba(0,0,0,0.5)";
        /// <summary>
        /// Whether clicking outside the highlighted area closes the tour.
        /// </summary>
        [Parameter] public bool MaskClosable { get; set; } = true;
        /// <summary>
        /// Whether the Escape key closes the tour.
        /// </summary>
        [Parameter] public bool Keyboard { get; set; } = true;
        /// <summary>
        /// Whether interaction with the highlighted target is disabled.
        /// </summary>
        [Parameter] public bool DisabledInteraction { get; set; }
        /// <summary>
        /// Visual style of the tour, default or primary.
        /// </summary>
        [Parameter] public TourType Type { get; set; } = TourType.Default;
        /// <summary>
        /// Shared gap, in pixels, around the highlighted target.
        /// </summary>
        [Parameter] public int GapOffset { get; set; } = 6;
        /// <summary>
        /// Horizontal gap, in pixels, around the highlighted target.
        /// </summary>
        [Parameter] public int? GapOffsetX { get; set; }
        /// <summary>
        /// Vertical gap, in pixels, around the highlighted target.
        /// </summary>
        [Parameter] public int? GapOffsetY { get; set; }
        /// <summary>
        /// Corner radius, in pixels, of the highlighted area.
        /// </summary>
        [Parameter] public int GapRadius { get; set; } = 2;
        /// <summary>
        /// CSS z-index of the tour.
        /// </summary>
        [Parameter] public int ZIndex { get; set; } = 1070;

        private DomRect _targetRect;
        private decimal _panelWidth = 520;
        private decimal _panelHeight = 180;
        private decimal _viewportWidth;
        private decimal _viewportHeight;
        private int _renderedCurrent = -1;
        private bool _needsPanelMeasure;
        private bool _bodyScrollLocked;

        protected TourStep CurrentStep => Steps?.ElementAtOrDefault(Current);
        protected bool HasTarget => CurrentStep != null &&
            (!string.IsNullOrWhiteSpace(CurrentStep.TargetSelector) || HasTargetValue(CurrentStep.Target));
        protected bool IsLastStep => CurrentStep != null && Current >= Steps.Count - 1;
        protected bool EffectiveMask => CurrentStep?.Mask ?? Mask;
        protected string EffectiveMaskStyle => CurrentStep?.MaskStyle ?? MaskStyle;
        protected string EffectiveMaskColor => CurrentStep?.MaskColor ?? MaskColor;
        protected bool EffectiveClosable => CurrentStep?.Closable ?? true;
        protected TourType EffectiveType => CurrentStep?.Type ?? Type;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (Open && !_bodyScrollLocked)
            {
                await JsInvokeAsync(JSInteropConstants.DisableBodyScroll);
                _bodyScrollLocked = true;
            }
            else if (!Open && _bodyScrollLocked)
            {
                await JsInvokeAsync(JSInteropConstants.EnableBodyScroll, false);
                _bodyScrollLocked = false;
            }

            if (!Open || CurrentStep == null) return;

            if (_needsPanelMeasure)
            {
                _needsPanelMeasure = false;
                var panelRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref);
                var viewport = await JsInvokeAsync<AntDesign.JsInterop.Window>(JSInteropConstants.GetWindow);
                _viewportWidth = viewport?.InnerWidth ?? 0;
                _viewportHeight = viewport?.InnerHeight ?? 0;
                if (panelRect != null && panelRect.Width > 0 && panelRect.Height > 0)
                {
                    _panelWidth = panelRect.Width;
                    _panelHeight = panelRect.Height;
                }
                await InvokeAsync(StateHasChanged);
                return;
            }

            if (_renderedCurrent == Current) return;

            if (!HasTarget)
            {
                _renderedCurrent = Current;
                return;
            }

            _renderedCurrent = Current;
            _targetRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, GetTargetArgument());
            _needsPanelMeasure = true;
            await InvokeAsync(StateHasChanged);
        }

        protected override void OnParametersSet()
        {
            if (Steps == null || Steps.Count == 0)
            {
                Current = 0;
                _renderedCurrent = -1;
                _targetRect = null;
            }
            else if (Current < 0)
            {
                Current = 0;
            }
            else if (Current >= Steps.Count)
            {
                Current = Steps.Count - 1;
            }

            if (!HasTarget)
            {
                _targetRect = null;
            }

            // Re-measure when the controlled tour is opened again at the same step.
            if (!Open)
            {
                _renderedCurrent = -1;
                _targetRect = null;
                _needsPanelMeasure = false;
            }
        }

        protected string RootClass => $"ant-tour {(EffectiveType == TourType.Primary ? "ant-tour-primary " : string.Empty)}{Class}".Trim();
        protected string PlacementClass => CurrentStep == null
            ? string.Empty
            : $"ant-tour-placement-{CurrentStep.Placement.ToString().ToLowerInvariant()}";

        protected string RootStyle
        {
            get
            {
                if (CurrentStep == null) return $"z-index:{ZIndex};{Style}";
                if (!HasTarget || _targetRect == null)
                {
                    return $"z-index:{ZIndex};left:50%;top:50%;transform:translate(-50%, -50%);{Style}";
                }
                var (left, top) = GetPopupPosition(_targetRect, CurrentStep.Placement);
                return $"z-index:{ZIndex};left:{left}px;top:{top}px;{Style}";
            }
        }

        protected string TargetPlaceholderStyle => _targetRect == null
            ? "display:none;"
            : $"left:{TargetLeft}px;top:{TargetTop}px;width:{TargetWidth}px;height:{TargetHeight}px;border-radius:{GapRadius}px;";

        protected string MaskId => $"{Id}-mask";
        protected string TargetPlaceholderClass => DisabledInteraction
            ? "ant-tour-target-placeholder ant-tour-placeholder-animated ant-tour-target-placeholder-disabled"
            : "ant-tour-target-placeholder ant-tour-placeholder-animated";
        protected decimal TargetLeft => _targetRect == null ? 0 : _targetRect.X - EffectiveGapOffsetX;
        protected int EffectiveGapOffsetX => GapOffsetX ?? GapOffset;
        protected int EffectiveGapOffsetY => GapOffsetY ?? GapOffset;
        protected decimal TargetTop => _targetRect == null ? 0 : _targetRect.Y - EffectiveGapOffsetY;
        protected decimal TargetRight => _targetRect == null ? 0 : _targetRect.X + _targetRect.Width + EffectiveGapOffsetX;
        protected decimal TargetBottom => _targetRect == null ? 0 : _targetRect.Y + _targetRect.Height + EffectiveGapOffsetY;
        protected decimal TargetWidth => _targetRect == null ? 0 : _targetRect.Width + EffectiveGapOffsetX * 2;
        protected decimal TargetHeight => _targetRect == null ? 0 : _targetRect.Height + EffectiveGapOffsetY * 2;
        protected string RemainingWidth => $"calc(100% - {TargetRight}px)";
        protected string RemainingHeight => $"calc(100% - {TargetBottom}px)";

        protected string MaskOverlayStyle => $"{EffectiveMaskStyle};z-index:{ZIndex - 10};pointer-events:none;";

        protected string ArrowStyle
        {
            get
            {
                if (_targetRect == null) return "left:50%;top:-4px;";

                var (left, top) = GetPopupPosition(_targetRect, CurrentStep.Placement);
                if (CurrentStep.Placement is Placement.Left or Placement.LeftTop or Placement.LeftBottom or
                    Placement.Right or Placement.RightTop or Placement.RightBottom)
                {
                    return $"top:{_targetRect.Y + _targetRect.Height / 2 - top}px;";
                }

                return $"left:{_targetRect.X + _targetRect.Width / 2 - left}px;";
            }
        }

        private object GetTargetArgument() => !string.IsNullOrWhiteSpace(CurrentStep?.TargetSelector)
            ? CurrentStep.TargetSelector
            : CurrentStep.Target;

        private static bool HasTargetValue(object target) => target switch
        {
            string selector => !string.IsNullOrWhiteSpace(selector),
            ElementReference element => element.Id != null,
            _ => false
        };

        private (decimal Left, decimal Top) GetPopupPosition(DomRect rect, Placement placement)
        {
            // The rotated 8px arrow extends 4px outside the panel. React Tour's
            // placement offset includes that arrow half-width plus the visible gap.
            const decimal Offset = 12;
            (decimal Left, decimal Top) position = placement switch
            {
                Placement.Top or Placement.TopLeft or Placement.TopRight => (GetHorizontal(rect, placement, _panelWidth), rect.Y - _panelHeight - Offset),
                Placement.Left or Placement.LeftTop or Placement.LeftBottom => (rect.X - _panelWidth - Offset, GetVertical(rect, placement, _panelHeight)),
                Placement.Right or Placement.RightTop or Placement.RightBottom => (rect.X + rect.Width + Offset, GetVertical(rect, placement, _panelHeight)),
                _ => (GetHorizontal(rect, placement, _panelWidth), rect.Y + rect.Height + Offset)
            };

            if (_viewportWidth > 0)
                position.Left = Math.Clamp(position.Left, 8, Math.Max(8, _viewportWidth - _panelWidth - 8));
            if (_viewportHeight > 0)
                position.Top = Math.Clamp(position.Top, 8, Math.Max(8, _viewportHeight - _panelHeight - 8));

            return position;
        }

        private static decimal GetHorizontal(DomRect rect, Placement placement, decimal width) => placement switch
        {
            Placement.TopRight or Placement.BottomRight => rect.X + rect.Width - width,
            Placement.Top or Placement.Bottom => rect.X + (rect.Width - width) / 2,
            _ => rect.X
        };

        private static decimal GetVertical(DomRect rect, Placement placement, decimal height) => placement switch
        {
            Placement.LeftBottom or Placement.RightBottom => rect.Y + rect.Height - height,
            Placement.Left or Placement.Right => rect.Y + (rect.Height - height) / 2,
            _ => rect.Y
        };

        protected async Task CloseAsync()
        {
            Open = false;
            // A completed/closed tour starts from its first step the next time it opens.
            Current = 0;
            _renderedCurrent = -1;
            _targetRect = null;
            _needsPanelMeasure = false;
            await CurrentChanged.InvokeAsync(0);
            await OpenChanged.InvokeAsync(false);
            await OnClose.InvokeAsync(null);
        }

        protected async Task NextAsync()
        {
            if (IsLastStep)
            {
                await OnFinish.InvokeAsync(null);
                await CloseAsync();
                return;
            }
            await SetCurrentAsync(Current + 1);
        }

        protected Task PreviousAsync() => SetCurrentAsync(Math.Max(0, Current - 1));

        private async Task SetCurrentAsync(int value)
        {
            Current = value;
            _renderedCurrent = -1;
            _targetRect = null;
            _needsPanelMeasure = false;
            await CurrentChanged.InvokeAsync(value);
            await OnChange.InvokeAsync(value);
        }

        protected async Task OnKeyDown(KeyboardEventArgs args)
        {
            if (Keyboard && args.Key == "Escape") await CloseAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _bodyScrollLocked)
            {
                _ = JsInvokeAsync(JSInteropConstants.EnableBodyScroll, false);
                _bodyScrollLocked = false;
            }

            base.Dispose(disposing);
        }
    }
}
