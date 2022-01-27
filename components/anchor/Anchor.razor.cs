﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Anchor : AntDomComponentBase, IAnchor
    {
        private string _ballClass = "ant-anchor-ink-ball";
        private string _ballStyle = string.Empty;
        private ElementReference _ink;
        private DomRect _selfDom;
        private AnchorLink _activeLink;
        private bool _activatedByClick = false;
        private AnchorLink _lastActiveLink;
        private Dictionary<string, decimal> _linkTops;
        private List<AnchorLink> _flatLinks;
        private List<AnchorLink> _links = new List<AnchorLink>();
        private bool _linksChanged = false;

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        #region Parameters

        private string _key;

        /// <summary>
        /// used to refresh links list when the key changed.
        /// </summary>
        [Parameter]
        public string Key
        {
            get => _key;
            set
            {
                if (_key != value)
                {
                    _key = value;
                    _linksChanged = true;
                    Clear();
                }
            }
        }

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

            string prefixCls = "ant-anchor";
            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-rtl", () => RTL);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (GetCurrentAnchor is null)
                {
                    DomEventListener.AddShared<JsonElement>("window", "scroll", OnScroll);
                }
            }

            if (firstRender || _linksChanged)
            {
                _linksChanged = false;

                _selfDom = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _ink);
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
                                _activatedByClick = false;
                                await ActivateAsync(link, true);
                                // the offset does not matter, since the dictionary's value will not change any more in case user set up GetCurrentAnchor
                                _linkTops[link.Href] = hrefDom.Top;
                                StateHasChanged();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }

        public void Remove(AnchorLink anchorLink)
        {
            _links.Remove(anchorLink);
        }

        public void Add(AnchorLink anchorLink)
        {
            if (!_links.Where(l => !string.IsNullOrEmpty(l.Href))
                .Select(l => l.Href)
                .Contains(anchorLink.Href))
            {
                _links.Add(anchorLink);
            }
        }

        public void Clear()
        {
            foreach (IAnchor link in _links)
            {
                link.Clear();
            }
            _links.Clear();
        }

        public List<AnchorLink> FlatChildren()
        {
            List<AnchorLink> results = new List<AnchorLink>();

            foreach (IAnchor child in _links)
            {
                results.AddRange(child.FlatChildren());
            }

            results.Distinct(new AnchorLinkEqualityComparer());
            return results;
        }

        private async void OnScroll(JsonElement obj)
        {
            if (!_activatedByClick && _flatLinks != null)
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
                            _linkTops[link.Href] = hrefDom.Top + offset;
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
                    _activeLink = _flatLinks.FirstOrDefault(l => l.Href == activeKey);
                    await ActivateAsync(_activeLink, true);
                }

                if (Affix && _activeLink != null)
                {
                    _ballClass = "ant-anchor-ink-ball visible";
                    decimal top = (_activeLink.LinkDom.Top - _selfDom.Top) + _activeLink.LinkDom.Height / 2 - 2;
                    _ballStyle = $"top: {top}px;";
                }
                else
                {
                    _ballClass = "ant-anchor-ink-ball";
                    _ballStyle = string.Empty;
                }

                StateHasChanged();
            }

            _activatedByClick = false;
        }

        private async Task ActivateAsync(AnchorLink anchorLink, bool active)
        {
            if (anchorLink == null)
                return;

            anchorLink?.Activate(active);

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

            // forced to activate the link even the dom is not at the top of the browser
            // occurs when the dom is located at the bottom of the page
            _activatedByClick = true;
            _activeLink = anchorLink;
            // deactivate everythin else
            _flatLinks.ForEach(l => l.Activate(false));
            await ActivateAsync(anchorLink, true);

            // forced to render when user click on another link that is at the same height with the previously activated dom
            StateHasChanged();
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener.Dispose();
            base.Dispose(disposing);
        }
    }
}
