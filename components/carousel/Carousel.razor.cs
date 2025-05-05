// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

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
                    return $"width: {TotalWidth}px; opacity: 1; transform: translate3d(-{SlickWidth * (IndexOfSlick(ActiveSlick) + 1)}px, 0px, 0px);transition: -webkit-transform 500ms ease 0s;";
                }
                else if (Effect == CarouselEffect.ScrollX && !IsHorizontal)
                {
                    return $"height: {TotalHeight}px; opacity: 1; transform: translate3d(0px, -{SlickHeight * (IndexOfSlick(ActiveSlick) + 1)}px, 0px);transition: -webkit-transform 500ms ease 0s;";
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

        internal string SlickClonedStyle => $"width: {SlickWidth}px;";
        private string SlickListStyle => IsHorizontal ? string.Empty : $"height: {SlickHeight}px";
        internal int SlickWidth { get; set; } = -1;
        private int SlickHeight { get; set; } = -1;
        private int TotalWidth => SlickWidth * (_slicks.Count * 2 + 1);
        private int TotalHeight => SlickHeight * (_slicks.Count * 2 + 1);
        private List<CarouselSlick> _slicks = new List<CarouselSlick>();
        internal CarouselSlick ActiveSlick { get; set; }
        private Timer _timer;
        private ClassMapper SlickSliderClassMapper { get; } = new ClassMapper();
        private bool IsHorizontal => DotPosition == CarouselDotPosition.Top || DotPosition == CarouselDotPosition.Bottom;

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
        /// Whether to scroll automatically
        /// </summary>
        [Parameter]
        public TimeSpan Autoplay { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Transition effect, <see cref="CarouselEffect"/>
        /// </summary>
        [Parameter]
        public CarouselEffect Effect { get; set; } = CarouselEffect.ScrollX;

        [Parameter]
        public bool Dots { get; set; }

        [Parameter]
        public string DotsClass { get; set; }

        #endregion Parameters

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

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
            if (index >= _slicks.Count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = _slicks.Count - 1;
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
                .If($"{PrefixCls}-rtl", () => RTL);
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
            if (Autoplay != TimeSpan.Zero)
            {
                _timer = new Timer(AutoplaySlick, null, (int)Autoplay.TotalMilliseconds, (int)Autoplay.TotalMilliseconds);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                Resize();
                DomEventListener.AddShared<JsonElement>("window", "resize", Resize);
            }
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
        }

        private async void AutoplaySlick(object state)
        {
            var index = IndexOfSlick(ActiveSlick) + 1;
            if (index == _slicks.Count)
            {
                index = 0;
            }
            if (_slicks == null || _slicks.Count == 0)
            {
                return;
            }
            this.Activate(_slicks[index]);

            // The current thread is not associated with the Dispatcher.
            // Use InvokeAsync() to switch execution to the Dispatcher when triggering rendering or component state
            await InvokeAsync(() => StateHasChanged());

            if (index == 0 && Effect == CarouselEffect.ScrollX)
            {
                await Task.Delay((int)Autoplay.TotalMilliseconds / 2);
            }

            await InvokeAsync(() => StateHasChanged());
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();
            _slicks.Clear();
            base.Dispose(disposing);
        }
    }
}
