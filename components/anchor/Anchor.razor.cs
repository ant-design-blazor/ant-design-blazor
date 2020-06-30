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
        private AnchorLink _activeLink;
        private AnchorLink _lastActiveLink;
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
                            await ActivateAsync(link, true);
                            // the offset does not matter, since the dictionary's value will not change any more in case user set up GetCurrentAnchor
                            _linkTops[link.Href] = hrefDom.top;
                            StateHasChanged();
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
            _activeLink = null;
            _flatLinks.ForEach(l => l.Activate(false));

            int offset = OffsetBottom.HasValue ? OffsetBottom.Value : -OffsetTop.Value;
            foreach (var link in _flatLinks)
            {
                try
                {
                    DomRect hrefDom = await link.GetHrefDom();
                    if (hrefDom != null)
                    {
                        _linkTops[link.Href] = hrefDom.top + offset;
                    }
                }
                catch (Exception ex)
                {
                    _linkTops[link.Href] = 1;
                }
            }

            string activeKey = _linkTops.Where(p => (int)p.Value <= 0).OrderBy(p => p.Value).LastOrDefault().Key;
            if (!string.IsNullOrEmpty(activeKey))
            {
                _activeLink = _flatLinks.Single(l => l.Href == activeKey);
                await ActivateAsync(_activeLink, true);
            }

            if (Affix && _activeLink != null)
            {
                _ballClass = "ant-anchor-ink-ball visible";
                decimal top = (_activeLink.LinkDom.top - _selfDom.top) + _activeLink.LinkDom.height / 2 - 2;
                _ballStyle = $"top: {top}px;";
            }
            else
            {
                _ballClass = "ant-anchor-ink-ball";
                _ballStyle = string.Empty;
            }

            StateHasChanged();
        }

        private async Task ActivateAsync(AnchorLink anchorLink, bool active)
        {
            anchorLink.Activate(active);

            if (active && _activeLink != _lastActiveLink)
            {
                _lastActiveLink = _activeLink;
                if (OnChange.HasDelegate)
                {
                    await OnChange.InvokeAsync(anchorLink.Href);
                }
            }
        }

        public async Task OnLinkClickAsync(MouseEventArgs args, AnchorLink anchorLink)
        {
            await JsInvokeAsync("window.eval", $"window.location.hash='{anchorLink._hash}'");
            
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(new Tuple<MouseEventArgs, AnchorLink>(args, anchorLink));
            }
        }
    }
}
