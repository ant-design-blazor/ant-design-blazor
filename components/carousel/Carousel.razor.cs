using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
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

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The position of the dots, which can be one of Top, Bottom, Left or Right, <see cref="CarouselDotPosition"/>
        /// </summary>
        [Parameter]
        public string DotPosition { get; set; } = CarouselDotPosition.Bottom;

        /// <summary>
        /// Whether to scroll automatically
        /// </summary>
        [Parameter]
        public TimeSpan Autoplay { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Transition effect, <see cref="CarouselEffect"/>
        /// </summary>
        [Parameter]
        public string Effect { get; set; } = CarouselEffect.ScrollX;

        #endregion Parameters

        [Inject] public DomEventService DomEventService { get; set; }

        public void Next() => GoTo(_slicks.IndexOf(ActiveSlick) + 1);

        public void Previous() => GoTo(_slicks.IndexOf(ActiveSlick) - 1);

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
                .If($"{PrefixCls}-vertical", () => !IsHorizontal);
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

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                Resize();
                DomEventService.AddEventListener("window", "resize", Resize, false);
            }
        }

        private async void Resize(JsonElement e = default)
        {
            DomRect listRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref);
            if ((SlickWidth != (int)listRect.width && IsHorizontal)
                || (SlickHeight != (int)listRect.height && !IsHorizontal)
                || IsHorizontal && !string.IsNullOrEmpty(SlickListStyle)
                || !IsHorizontal && string.IsNullOrEmpty(SlickListStyle))
            {
                SlickWidth = (int)listRect.width;
                SlickHeight = (int)listRect.height;
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
            DomEventService.RemoveEventListerner<JsonElement>("window", "resize", Resize);

            _slicks.Clear();

            base.Dispose(disposing);
        }
    }
}
