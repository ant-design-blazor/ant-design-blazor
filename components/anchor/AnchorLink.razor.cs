using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class AnchorLink : AntDomComponentBase, IAnchor
    {
        private const string PrefixCls = "ant-anchor-link";
        private bool _active;
        private bool _hrefDomExist;
        private ClassMapper _titleClass = new ClassMapper();
        private ElementReference _self;
        private List<AnchorLink> _links = new List<AnchorLink>();
        public DomRect LinkDom { get; private set; }

        #region Parameters

        private IAnchor _parent;

        [CascadingParameter]
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

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Clear()
                .Add($"{PrefixCls}")
                .If($"{PrefixCls}-active", () => _active);

            _titleClass.Clear()
                .Add($"{PrefixCls}-title")
                .If($"{PrefixCls}-title-active", () => _active);
        }

        protected async override Task OnFirstAfterRenderAsync()
        {
            await base.OnFirstAfterRenderAsync();

            LinkDom = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, _self);
            try
            {
                await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, "#" + Href.Split('#')[1]);
                _hrefDomExist = true;
            }
            catch { }
        }

        public void Add(AnchorLink anchorLink)
        {
            _links.Add(anchorLink);
        }

        public List<AnchorLink> FlatChildren()
        {
            List<AnchorLink> results = new List<AnchorLink>();

            if (!string.IsNullOrEmpty(Href))
            {
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
            _active = active;
        }

        internal async Task<DomRect> GetHrefDom()
        {
            DomRect domRect = null;
            if (_hrefDomExist)
            {
                domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.getBoundingClientRect, "#" + Href.Split('#')[1]);
            }
            return domRect;
        }
    }
}
