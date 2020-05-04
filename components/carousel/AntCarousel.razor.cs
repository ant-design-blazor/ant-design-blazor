using System;
using System.Collections.Generic;
using System.Linq;
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
        private string _slickStyle;
        private string _slickClonedStyle;
        private ElementReference _ref;
        private int _slickWidth = -1;
        private int _totalWidth = -1;
        private List<AntCarouselSlick> _slicks = new List<AntCarouselSlick>();
        private AntCarouselSlick _activeSlick;

        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The position of the dots, which can be one of Top, Bottom, Left or Right.
        /// </summary>
        [Parameter]
        public string DotPosition { get; set; } = AntCarouselDotPosition.Bottom;

        #endregion Parameters

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-vertical", () => DotPosition == AntCarouselDotPosition.Left || DotPosition == AntCarouselDotPosition.Right);
        }

        protected async override Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            DomRect carouselRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _ref);
            Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _ref);
            _slickWidth = (int)carouselRect.width;
            _totalWidth = _slickWidth * (_slicks.Count * 2 + 1);
            _trackStyle = $"width: {_totalWidth}px; opacity: 1; transform: translate3d(-{_slickWidth}px, 0px, 0px); transition: -webkit-transform 500ms ease 0s;";
            _slickStyle = $"outline: none; width: {_slickWidth}px;";
            _slickClonedStyle = $"width: {_slickWidth}px;";
        }

        internal void AddSlick(AntCarouselSlick slick)
        {
            _slicks.Add(slick);
            if (_activeSlick == null)
            {
                Activate(slick);
            }
        }

        private async void Activate(AntCarouselSlick slick)
        {
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

            if (_slickWidth > 0)
            {
                int count = _slicks.IndexOf(_activeSlick) + 1;
                _trackStyle = $"width: {_totalWidth}px; opacity: 1; transform: translate3d(-{_slickWidth * count - 1}px, 0px, 0px); transition: -webkit-transform 500ms ease 0s;";
            }
        }
    }
}
