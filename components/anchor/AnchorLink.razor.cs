// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    [DebuggerDisplay("Href: {Href}")]
    public partial class AnchorLink : AntDomComponentBase, IAnchor
    {
        private const string PrefixCls = "ant-anchor-link";
        internal bool Active { get; private set; }

        private bool _hrefDomExist;
        private ClassMapper _titleClass = new ClassMapper();
        private List<AnchorLink> _links = new List<AnchorLink>();
        public DomRect LinkDom { get; private set; }

        #region Parameters

        [CascadingParameter(Name = "Root")]
        public Anchor Root { get; set; }

        private IAnchor _parent;

        [CascadingParameter(Name = "Parent")]
        public IAnchor Parent
        {
            get => _parent;
            set
            {
                //Debug.WriteLine($"link:{Title} {GetHashCode()}\tparent:{value.GetHashCode()}");
                _parent = value;
                _parent?.Add(this);
            }
        }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// target of hyperlink
        /// </summary>
        [Parameter]
        public string Href { get; set; }

        /// <summary>
        /// content of hyperlink
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Specifies where to display the linked URL
        /// </summary>
        [Parameter]
        public string Target { get; set; }

        #endregion Parameters

        internal string _hash = string.Empty;

        protected override void Dispose(bool disposing)
        {
            _parent?.Remove(this);
            base.Dispose(disposing);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Clear()
                .Add($"{PrefixCls}")
                .If($"{PrefixCls}-active", () => Active);

            _titleClass.Clear()
                .Add($"{PrefixCls}-title")
                .If($"{PrefixCls}-title-active", () => Active);
        }

        protected async override Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            LinkDom = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref);
            try
            {
                await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, "#" + Href.Split('#')[1]);
                _hrefDomExist = true;
            }
            catch { }
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

            if (!string.IsNullOrEmpty(Href))
            {
                _hash = Href.Contains('#') ? Href.Split('#')[1] : Href;
                results.Add(this);
            }

            foreach (IAnchor child in _links)
            {
                results.AddRange(child.FlatChildren());
            }

            return results;
        }

        internal void Activate(bool active)
        {
            Active = active;
        }

        internal async Task<DomRect> GetHrefDom(bool forced = false)
        {
            DomRect domRect = null;
            if (forced || _hrefDomExist)
            {
                domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, "#" + Href.Split('#')[1]);
            }
            return domRect;
        }

        private async void OnClick(MouseEventArgs args)
        {
            await Root.OnLinkClickAsync(args, this);
        }
    }
}
