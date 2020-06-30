using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using Microsoft.AspNetCore.Components.Web;

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

        /// <summary>
        /// Fixed mode of Anchor
        /// </summary>
        [Parameter]
        public bool Affix { get; set; } = true;

        /// <summary>
        /// Bounding distance of anchor area
        /// </summary>
        [Parameter]
        public int Bounds { get; set; } = 5;

        /// <summary>
        /// Scrolling container
        /// </summary>
        [Parameter]
        public Func<string> GetContainer { get; set; } = () => "window";

        /// <summary>
        /// Pixels to offset from bottom when calculating position of scroll
        /// </summary>
        [Parameter]
        public int? OffsetBottom { get; set; }

        /// <summary>
        /// Pixels to offset from top when calculating position of scroll
        /// </summary>
        [Parameter]
        public int? OffsetTop { get; set; } = 0;

        /// <summary>
        /// Whether show ink-balls in Fixed mode
        /// </summary>
        [Parameter]
        public bool ShowInkInFixed { get; set; } = false;

        /// <summary>
        /// set the handler to handle click event
        /// </summary>
        [Parameter]
        public EventCallback<Tuple<MouseEventArgs, AnchorLink>> OnClick { get; set; }

        /// <summary>
        /// Customize the anchor highlight
        /// </summary>
        [Parameter]
        public Func<string> GetCurrentAnchor { get; set; }

        /// <summary>
        /// Anchor scroll offset, default as <see cref="OffsetTop"/>
        /// </summary>
        [Parameter]
        public int? TargetOffset { get; set; }

        [Parameter]
        public EventCallback<string> OnChange { get; set; }

        #endregion Parameters

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (GetCurrentAnchor is null)
            {
                DomEventService.AddEventListener("window", "scroll", OnScroll);
            }
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            _selfDom = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _ink);
            _linkTops = new Dictionary<string, decimal>();
            _flatLinks = FlatChildren();
            foreach (var link in _flatLinks)
            {
                _linkTops[link.Href] = 1;
            }

            if (GetCurrentAnchor != null)
            {
                AnchorLink link = _flatLinks.SingleOrDefault(l => l.Href == GetCurrentAnchor());
                if (link != null)
                {
                    try
                    {
                        DomRect hrefDom = await link.GetHrefDom(true);
                        if (hrefDom != null)
                        {
                            await ActivateAsync(link, (link.LinkDom.top - _selfDom.top) + link.LinkDom.height / 2 - 2);
                            // the offset does not matter, since the dictionary's value will not change any more in case user set up GetCurrentAnchor
                            _linkTops[link.Href] = hrefDom.top;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            await base.OnFirstAfterRenderAsync();
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
            int offset = OffsetBottom.HasValue ? OffsetBottom.Value : -OffsetTop.Value;
            foreach (var link in _flatLinks)
            {
                try
                {
                    DomRect hrefDom = await link.GetHrefDom();
                    if (hrefDom != null)
                    {
                        if (_linkTops[link.Href] * (hrefDom.top + offset) <= 0)
                        {
                            await ActivateAsync(link, (link.LinkDom.top - _selfDom.top) + link.LinkDom.height / 2 - 2);
                        }
                        _linkTops[link.Href] = hrefDom.top + offset;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private async Task ActivateAsync(AnchorLink anchorLink, decimal top)
        {
            foreach (var link in _flatLinks)
            {
                link.Activate(link == anchorLink);
            }

            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(anchorLink.Href);
            }

            if (Affix && anchorLink.Active)
            {
                _ballClass = "ant-anchor-ink-ball visible";
                _ballStyle = $"top: {top}px;";
            }
            else
            {
                _ballClass = "ant-anchor-ink-ball";
                _ballStyle = string.Empty;
            }

            StateHasChanged();
        }

        public async Task OnLinkClickAsync(MouseEventArgs args, AnchorLink anchorLink)
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(new Tuple<MouseEventArgs, AnchorLink>(args, anchorLink));
            }
        }

        public IAnchor GetParent()
        {
            return null;
        }
    }
}
