// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
        <para>A carousel component. Scales with its container.</para>

        <h2>When To Use</h2>

        <list type="bullet">
            <item>When there is a group of content on the same level.</item>
            <item>When there is insufficient content space, it can be used to save space in the form of a revolving door.</item>
            <item>Commonly used for a group of pictures/cards.</item>
        </list>
    </summary>
    <seealso cref="CarouselSlick"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/antfincdn/%24C9tmj978R/Carousel.svg", Title = "Carousel", SubTitle = "走马灯")]
    public partial class Carousel : AntDomComponentBase
    {
        private const string PrefixCls = "ant-carousel";

        private string TrackStyle
        {
            get
            {
                if (Effect == CarouselEffect.ScrollX && IsHorizontal)
                {
                    return $"width: {TotalWidth}px; opacity: 1; transform: translate3d(-{SlickWidth * (IndexOfSlick(ActiveSlick) + (Infinite ? 1 : 0))}px, 0px, 0px);transition: -webkit-transform 500ms ease 0s;";
                }
                else if (Effect == CarouselEffect.ScrollX && !IsHorizontal)
                {
                    return $"height: {TotalHeight}px; opacity: 1; transform: translate3d(0px, -{SlickHeight * (IndexOfSlick(ActiveSlick) + (Infinite ? 1 : 0))}px, 0px);transition: -webkit-transform 500ms ease 0s;";
                }
                else if (Effect == CarouselEffect.Fade && IsHorizontal)
                {
                    return $"width: {TotalWidth}px; opacity: 1;";
                }
                else if (Effect == CarouselEffect.Fade && !IsHorizontal)
                {
                    return $"height: {TotalHeight}px; opacity: 1;";
                }

                return string.Empty;
            }
        }

        internal string SlickClonedStyle => IsHorizontal ? $"width: {SlickWidth}px;" : $"height: {SlickHeight}px;";
        private string SlickListStyle => IsHorizontal ? string.Empty : $"height: {SlickHeight}px";
        internal int SlickWidth { get; set; } = -1;
        private int SlickHeight { get; set; } = -1;
        private int TotalWidth => SlickWidth * (_slicks.Count + (Infinite ? 2 : 0));
        private int TotalHeight => SlickHeight * (_slicks.Count + (Infinite ? 2 : 0));
        private List<CarouselSlick> _slicks = new List<CarouselSlick>();
        internal CarouselSlick ActiveSlick { get; set; }
        private Timer _timer;
        private ClassMapper SlickSliderClassMapper { get; } = new ClassMapper();
        private bool IsHorizontal => EffectiveDotPlacement == CarouselDotPosition.Top || EffectiveDotPlacement == CarouselDotPosition.Bottom;
        private bool IsAutoplayEnabled => Autoplay.IsT0
            ? Autoplay.AsT0
            : Autoplay.IsT1 ? Autoplay.AsT1 != TimeSpan.Zero : true;
        private bool IsDotDurationEnabled => DotDuration || (Autoplay.IsT2 && Autoplay.AsT2.DotDuration);
        private TimeSpan AutoplayInterval => Autoplay.IsT1 && Autoplay.AsT1 > TimeSpan.Zero
            ? Autoplay.AsT1
            : TimeSpan.FromMilliseconds(AutoplaySpeed);
        private string RootStyle
        {
            get
            {
                var style = Style;
                if (IsDotDurationEnabled && IsAutoplayEnabled)
                {
                    style += $"--dot-duration:{AutoplayInterval.TotalMilliseconds}ms;";
                }

                var componentStyle = GetComponentStyle("Carousel", style);
                return string.IsNullOrWhiteSpace(componentStyle) ? null : componentStyle;
            }
        }
        private CarouselDotPosition EffectiveDotPlacement => DotPlacement ?? DotPosition;
        internal string DotPositionClass => EffectiveDotPlacement switch
        {
            CarouselDotPosition.Left or CarouselDotPosition.Start => "start",
            CarouselDotPosition.Right or CarouselDotPosition.End => "end",
            _ => EffectiveDotPlacement.ToString().ToLowerInvariant(),
        };
        internal string DotsClassName => $"slick-dots slick-dots-{DotPositionClass}{(string.IsNullOrWhiteSpace(DotsClass) ? string.Empty : $" {DotsClass}")}";
        internal bool IsPreviousDisabled => !Infinite && IndexOfSlick(ActiveSlick) <= 0;
        internal bool IsNextDisabled => !Infinite && IndexOfSlick(ActiveSlick) >= _slicks.Count - 1;

        private DotNetObjectReference<Carousel> _dotNetHelper;
        #region Parameters

        /// <summary>
        /// Content of the carousel. Typically <see cref="CarouselSlick"/> elements
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The position of the dots, which can be one of Top, Bottom, Left or Right, <see cref="CarouselDotPosition"/>
        /// </summary>
        [Parameter]
        public CarouselDotPosition DotPosition { get; set; } = CarouselDotPosition.Bottom;

        /// <summary>
        /// The position of the dots: Top, Bottom, Start, or End.
        /// </summary>
        [Parameter]
        public CarouselDotPosition? DotPlacement { get; set; }

        /// <summary>
        /// Whether to scroll automatically
        /// </summary>
        [Parameter]
        public OneOf<bool, TimeSpan, CarouselAutoplayOptions> Autoplay { get; set; } = false;

        /// <summary>
        /// Autoplay interval in milliseconds.
        /// </summary>
        [Parameter]
        public int AutoplaySpeed { get; set; } = 3000;

        /// <summary>
        /// Whether to animate the active dot for the autoplay interval.
        /// </summary>
        [Parameter]
        public bool DotDuration { get; set; }

        /// <summary>
        /// Whether to show switch arrows.
        /// </summary>
        [Parameter]
        public bool Arrows { get; set; }

        /// <summary>
        /// Whether to infinitely wrap around at the start or end.
        /// </summary>
        [Parameter]
        public bool Infinite { get; set; } = true;

        /// <summary>
        /// Transition effect, <see cref="CarouselEffect"/>
        /// </summary>
        [Parameter]
        public CarouselEffect Effect { get; set; } = CarouselEffect.ScrollX;

        [Parameter]
        public bool Dots { get; set; } = true;

        [Parameter]
        public string DotsClass { get; set; }

        /// <summary>
        /// Whether to enable swipe gestures for the carousel
        /// </summary>
        [Parameter]
        [PublicApi("1.5.0")]
        public bool EnableSwipe { get; set; }

        #endregion Parameters

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        [JSInvokable]
        public void HandleSwipe(string direction)
        {
            switch (direction)
            {
                case "left":
                    if (IsHorizontal) Next();
                    break;
                case "right":
                    if (IsHorizontal) Previous();
                    break;
                case "up":
                    if (!IsHorizontal) Next();
                    break;
                case "down":
                    if (!IsHorizontal) Previous();
                    break;
            }
        }

        /// <summary>
        /// Slides the carousel to the next slide
        /// </summary>
        public void Next() => GoTo(_slicks.IndexOf(ActiveSlick) + 1);

        /// <summary>
        /// Slides the carousel to the previous slide
        /// </summary>
        public void Previous() => GoTo(_slicks.IndexOf(ActiveSlick) - 1);

        /// <summary>
        /// Slides the carousel to the specified slide
        /// </summary>
        /// <param name="index"></param>
        public void GoTo(int index)
        {
            if (_slicks.Count == 0)
            {
                return;
            }

            if (Infinite && index >= _slicks.Count)
            {
                index = 0;
            }
            else if (Infinite && index < 0)
            {
                index = _slicks.Count - 1;
            }
            else
            {
                index = Math.Clamp(index, 0, _slicks.Count - 1);
            }

            Activate(_slicks[index]);
        }

        private void SetClass()
        {
            SlickSliderClassMapper.Clear()
                .Add("slick-slider slick-initialized")
                .If("slick-vertical", () => !IsHorizontal);

            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-vertical", () => !IsHorizontal)
                .If($"{PrefixCls}-rtl", () => RTL)
                .Add("css-var-root");
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            SetClass();

            if (Effect != CarouselEffect.ScrollX && Effect != CarouselEffect.Fade)
            {
                throw new ArgumentOutOfRangeException($"{nameof(Effect)} must be one of {nameof(CarouselEffect)}.{nameof(CarouselEffect.ScrollX)} or {nameof(CarouselEffect)}.{nameof(CarouselEffect.Fade)}.");
            }

            _timer?.Dispose();
            if (AutoplaySpeed <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(AutoplaySpeed), "AutoplaySpeed must be greater than zero.");
            }

            if (IsAutoplayEnabled)
            {
                var interval = (int)AutoplayInterval.TotalMilliseconds;
                _timer = new Timer(AutoplaySlick, null, interval, interval);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await Resize();

                _dotNetHelper = DotNetObjectReference.Create(this);
                if (EnableSwipe)
                {
                    await Js.InvokeVoidAsync("AntDesign.interop.touchHelper.initializeTouch", new { element = Ref, dotNetHelper = _dotNetHelper, minSwipeDistance = 50, vertical = !IsHorizontal });
                }

                DomEventListener.AddShared<JsonElement>("window", "resize", Resize);
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (EnableSwipe)
                {
                    await Js.InvokeVoidAsync("AntDesign.interop.touchHelper.dispose", Ref);
                    _dotNetHelper?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disposing carousel: {ex.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();
            _slicks.Clear();
            base.Dispose(disposing);
        }

        private async Task Resize(JsonElement e = default)
        {
            DomRect listRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref);
            if ((SlickWidth != (int)listRect.Width && IsHorizontal)
                || (SlickHeight != (int)listRect.Height && !IsHorizontal)
                || IsHorizontal && !string.IsNullOrEmpty(SlickListStyle)
                || !IsHorizontal && string.IsNullOrEmpty(SlickListStyle))
            {
                SlickWidth = (int)listRect.Width;
                SlickHeight = (int)listRect.Height;
                StateHasChanged();
            }
        }

        internal void RemoveSlick(CarouselSlick slick)
        {
            var slicks = new List<CarouselSlick>(_slicks ?? new List<CarouselSlick>());
            slicks.Remove(slick);
            _slicks = slicks;
            InvokeAsync(async () =>
            {
                await InvokeAsync(StateHasChanged).ConfigureAwait(false);
            });
        }

        internal void AddSlick(CarouselSlick slick)
        {
            var slicks = new List<CarouselSlick>(_slicks ?? new List<CarouselSlick>());
            slicks.Add(slick);
            _slicks = slicks;
            if (ActiveSlick == null)
            {
                Activate(_slicks[0]);
            }
            InvokeStateHasChanged();
        }

        internal int IndexOfSlick(CarouselSlick slick)
        {
            return _slicks.IndexOf(slick);
        }

        private void Activate(CarouselSlick slick)
        {
            this.ActiveSlick = slick;
            InvokeStateHasChanged();
        }

        private async void AutoplaySlick(object state)
        {
            if (_slicks == null || _slicks.Count == 0)
            {
                return;
            }

            var index = IndexOfSlick(ActiveSlick) + 1;
            if (index == _slicks.Count && Infinite)
            {
                index = 0;
            }
            else if (index == _slicks.Count)
            {
                _timer?.Change(Timeout.Infinite, Timeout.Infinite);
                return;
            }
            this.Activate(_slicks[index]);

            // The current thread is not associated with the Dispatcher.
            // Use InvokeAsync() to switch execution to the Dispatcher when triggering rendering or component state
            await InvokeAsync(() => StateHasChanged());

            if (index == 0 && Effect == CarouselEffect.ScrollX)
            {
                await Task.Delay((int)AutoplayInterval.TotalMilliseconds / 2);
            }

            await InvokeAsync(() => StateHasChanged());
        }
    }
}
