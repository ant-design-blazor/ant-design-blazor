using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Anchor : AntDomComponentBase, IAnchor
    {
        private string _ballClass = "ant-anchor-ink-ball";
        private string _ballStyle = string.Empty;
        private Dictionary<string, decimal> _linkTops;
        private List<AnchorLink> _flatLinks;
        private List<AnchorLink> _links = new List<AnchorLink>();

        [Inject]
        private DomEventService DomEventService { get; set; }


        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();

            DomEventService.AddEventListener("window", "scroll", OnScroll);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            _flatLinks = FlatChildren();
            foreach (var link in _flatLinks)
            {
                _linkTops[link.Href] = decimal.MaxValue;
            }
        }

        public void Add(AnchorLink anchorLink)
        {
            _links.Add(anchorLink);
        }

        public List<AnchorLink> FlatChildren()
        {
            List<AnchorLink> results = new List<AnchorLink>();

            foreach (IAnchor child in _links)
            {
                results.AddRange(child.FlatChildren());
            }

            return results;
        }

        private async void OnScroll(JsonElement obj)
        {
            foreach (var link in _flatLinks)
            {
                string id = "#" + link.Href.Split('#')[1];
                DomRect domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, id);

                if (_linkTops[link.Href] * domRect.top <= 0)
                {
                    DomRect containerRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, Id);
                    Activate(link, (domRect.top - containerRect.top) + domRect.height / 2);
                }
                _linkTops[link.Href] = domRect.top;
            }
        }

        private void Activate(AnchorLink anchorLink, decimal top)
        {
            foreach (var link in _flatLinks)
            {
                link.Activate(link == anchorLink);
            }

            _ballClass = "ant-anchor-ink-ball visible";
            _ballStyle = $"top: {top}px;";
        }

    }
}
