using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace AntDesign
{
    public partial class Anchor : AntDomComponentBase, IAnchor
    {
        private string _ballClass = "ant-anchor-ink-ball";
        private string _ballStyle = string.Empty;
        private ElementReference _self;
        private ElementReference _ink;
        private DomRect _selfDom;
        private Dictionary<string, decimal> _linkTops;
        private List<AnchorLink> _flatLinks;
        private List<AnchorLink> _links = new List<AnchorLink>();

        [Inject]
        private DomEventService DomEventService { get; set; }

        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        #endregion Parameters

        protected override void OnInitialized()
        {
            base.OnInitialized();

            DomEventService.AddEventListener("window", "scroll", OnScroll);
        }

        protected async override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
            {
                _selfDom = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _ink);
                _linkTops = new Dictionary<string, decimal>();
                _flatLinks = FlatChildren();
                foreach (var link in _flatLinks)
                {
                    _linkTops[link.Href] = 1;
                }
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
                try
                {
                    DomRect hrefDom = await link.GetHrefDom();

                    if (_linkTops[link.Href] * hrefDom.top <= 0)
                    {
                        Activate(link, (link.LinkDom.top - _selfDom.top) + link.LinkDom.height / 2);
                        Debug.WriteLine((link.LinkDom.top - _selfDom.top) + link.LinkDom.height / 2);
                    }
                    _linkTops[link.Href] = hrefDom.top;
                }
                catch (Exception ex)
                {
                }
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

            StateHasChanged();
        }
    }
}
