using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Carousel : AntDomComponentBase
    {
        private const string PrefixCls = "ant-carousel";
        private string _trackStyle;
        private string _slickClonedStyle;
        private string _slickListStyle;
        private ElementReference _ref;
        private int _slickWidth = -1;
        private int _slickHeight = -1;
        private int _totalWidth = -1;
        private int _totalHeight = -1;
        private List<CarouselSlick> _slicks = new List<CarouselSlick>();
        private CarouselSlick _activeSlick;
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

        public void Next() => GoTo(_slicks.IndexOf(_activeSlick) + 1);

        public void Previous() => GoTo(_slicks.IndexOf(_activeSlick) - 1);

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

            Activate(index);
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

            DomRect listRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _ref);
            if ((_slickWidth != (int)listRect.width && IsHorizontal)
                || (_slickHeight != (int)listRect.height && !IsHorizontal)
                || IsHorizontal && !string.IsNullOrEmpty(_slickListStyle)
                || !IsHorizontal && string.IsNullOrEmpty(_slickListStyle))
            {
                _slickWidth = (int)listRect.width;
                _slickHeight = (int)listRect.height;
                _totalWidth = _slickWidth * (_slicks.Count * 2 + 1);
                _totalHeight = _slickHeight * (_slicks.Count * 2 + 1);
                if (Effect == CarouselEffect.ScrollX)
                {
                    if (IsHorizontal)
                    {
                        _trackStyle = $"width: {_totalWidth}px; opacity: 1; transform: translate3d(-{_slickWidth}px, 0px, 0px); transition: -webkit-transform 500ms ease 0s;";
                    }
                    else
                    {
                        _trackStyle = $"height: {_totalHeight}px; opacity: 1; transform: translate3d(0px, -{_slickHeight}px, 0px); transition: -webkit-transform 500ms ease 0s;";
                    }
                }
                else
                {
                    if (IsHorizontal)
                    {
                        _trackStyle = $"width: {_totalWidth}px; opacity: 1;";
                    }
                    else
                    {
                        _trackStyle = $"height: {_totalHeight}px; opacity: 1;";
                    }
                }

                _slickListStyle = IsHorizontal ? string.Empty : $"height: {_slickHeight}px";
                _slickClonedStyle = $"width: {_slickWidth}px;";

                Activate(_slicks.IndexOf(_activeSlick));
                StateHasChanged();
            }
        }

        internal void RemoveSlick(CarouselSlick slick)
        {
            _slicks.Remove(slick);
        }

        internal void AddSlick(CarouselSlick slick)
        {
            _slicks.Add(slick);
            if (_activeSlick == null)
            {
                Activate(0);
            }
        }

        private int Activate(int index, string transition = " transition: -webkit-transform 500ms ease 0s;")
        {
            if (Effect == CarouselEffect.ScrollX)
            {
                if (IsHorizontal)
                {
                    _trackStyle = $"width: {_totalWidth}px; opacity: 1; transform: translate3d(-{_slickWidth * (index + 1)}px, 0px, 0px);{transition}";
                }
                else
                {
                    _trackStyle = $"height: {_totalHeight}px; opacity: 1; transform: translate3d(0px, -{_slickHeight * (index + 1)}px, 0px);{transition}";
                }
            }

            if (index == _slicks.Count)
            {
                index = 0;
            }

            if (_slicks.Count > 0)
            {
                CarouselSlick slick = _slicks[index];
                _slicks.ForEach(s =>
                {
                    if (s == slick)
                    {
                        _activeSlick = s;
                        s.Activate();
                    }
                    else
                    {
                        s.Deactivate();
                    }
                });
            }

            return index;
        }

        private async void AutoplaySlick(object state)
        {
            int newIndex = _slicks.IndexOf(_activeSlick) + 1;

            int realIndex = Activate(newIndex);

            // The current thread is not associated with the Dispatcher.
            // Use InvokeAsync() to switch execution to the Dispatcher when triggering rendering or component state
            await InvokeAsync(() => StateHasChanged());

            if (realIndex == 0 && Effect == CarouselEffect.ScrollX)
            {
                await Task.Delay((int)Autoplay.TotalMilliseconds / 2);
                if (IsHorizontal)
                {
                    _trackStyle = $"width: {_totalWidth}px; opacity: 1; transform: translate3d(-{_slickWidth}px, 0px, 0px);";
                }
                else
                {
                    _trackStyle = $"height: {_totalHeight}px; opacity: 1; transform: translate3d(0px, -{_slickHeight}px, 0px);";
                }
            }

            await InvokeAsync(() => StateHasChanged());
        }
    }
}
