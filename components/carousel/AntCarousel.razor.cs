using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntCarousel : AntDomComponentBase
    {
        private const string PrefixCls = "ant-carousel";
        private string _trackStyle;
        private string _slickClonedStyle;
        private ElementReference _ref;
        private int _slickWidth = -1;
        private int _totalWidth = -1;
        private List<AntCarouselSlick> _slicks = new List<AntCarouselSlick>();
        private AntCarouselSlick _activeSlick;
        private Timer _timer;

        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The position of the dots, which can be one of Top, Bottom, Left or Right, <see cref="AntCarouselDotPosition"/>
        /// </summary>
        [Parameter]
        public string DotPosition { get; set; } = AntCarouselDotPosition.Bottom;

        /// <summary>
        /// Whether to scroll automatically
        /// </summary>
        [Parameter]
        public TimeSpan Autoplay { get; set; } = TimeSpan.Zero;

        /// <summary>
        /// Transition effect, <see cref="AntCarouselEffect"/>
        /// </summary>
        [Parameter]
        public string Effect { get; set; } = AntCarouselEffect.ScrollX;

        #endregion Parameters

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Effect != AntCarouselEffect.ScrollX && Effect != AntCarouselEffect.Fade)
            {
                throw new ArgumentOutOfRangeException($"{nameof(Effect)} must be one of {nameof(AntCarouselEffect)}.{nameof(AntCarouselEffect.ScrollX)} or {nameof(AntCarouselEffect)}.{nameof(AntCarouselEffect.Fade)}.");
            }

            _timer?.Dispose();
            if (Autoplay != TimeSpan.Zero)
            {
                _timer = new Timer(AutoplaySlick, null, (int)Autoplay.TotalMilliseconds, (int)Autoplay.TotalMilliseconds);
            }

            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-vertical", () => DotPosition == AntCarouselDotPosition.Left || DotPosition == AntCarouselDotPosition.Right);
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            DomRect listRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _ref);
            if (_slickWidth != (int)listRect.width)
            {
                _slickWidth = (int)listRect.width;
                _totalWidth = _slickWidth * (_slicks.Count * 2 + 1);
                if (Effect == AntCarouselEffect.ScrollX)
                {
                    _trackStyle = $"width: {_totalWidth}px; opacity: 1; transform: translate3d(-{_slickWidth}px, 0px, 0px); transition: -webkit-transform 500ms ease 0s;";
                }
                else
                {
                    _trackStyle = $"width: {_totalWidth}px; opacity: 1;";
                }
                _slickClonedStyle = $"width: {_slickWidth}px;";

                StateHasChanged();
            }
        }

        internal void AddSlick(AntCarouselSlick slick)
        {
            _slicks.Add(slick);
            if (_activeSlick == null)
            {
                Activate(0);
            }
        }

        private int Activate(int index, string transition = " transition: -webkit-transform 500ms ease 0s;")
        {
            if (Effect == AntCarouselEffect.ScrollX)
            {
                _trackStyle = $"width: {_totalWidth}px; opacity: 1; transform: translate3d(-{_slickWidth * (index + 1)}px, 0px, 0px);{transition}";
            }

            if (index == _slicks.Count)
            {
                index = 0;
            }

            AntCarouselSlick slick = _slicks[index];
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

            return index;
        }

        private async void AutoplaySlick(object state)
        {
            int newIndex = _slicks.IndexOf(_activeSlick) + 1;

            int realIndex = Activate(newIndex);

            // The current thread is not associated with the Dispatcher.
            // Use InvokeAsync() to switch execution to the Dispatcher when triggering rendering or component state
            await InvokeAsync(() => StateHasChanged());

            if (realIndex == 0 && Effect == AntCarouselEffect.ScrollX)
            {
                Thread.Sleep((int)Autoplay.TotalMilliseconds / 2);
                _trackStyle = $"width: {_totalWidth}px; opacity: 1; transform: translate3d(-{_slickWidth}px, 0px, 0px);";
            }

            await InvokeAsync(() => StateHasChanged());
        }
    }
}
