using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntCarousel : AntDomComponentBase
    {
        private string _trackStyle = "width: 5562px; opacity: 1; transform: translate3d(-618px, 0px, 0px);";
        private string _slickStyle = "outline: none; width: 618px;";
        private string _slickClonedStyle = "width: 618px;";
        private ElementReference _ref;

        private List<AntCarouselSlick> _slicks = new List<AntCarouselSlick>();

        private AntCarouselSlick _activeSlick;

        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        #endregion Parameters

        protected async override Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            DomRect carouselRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _ref);
            int width = (int)carouselRect.width;
            _trackStyle = $"width: {width * (_slicks.Count * 2 + 1)}px; opacity: 1; transform: translate3d({-width}px, 0px, 0px);";
            _slickStyle = $"outline: none; width: {width}px;";
            _slickClonedStyle = $"width: {width}px;";
        }

        internal void AddSlick(AntCarouselSlick slick)
        {
            _slicks.Add(slick);
            if (_activeSlick == null)
            {
                Activate(slick);
            }
        }

        private void Activate(AntCarouselSlick slick)
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
        }
    }
}
