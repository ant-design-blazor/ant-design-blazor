using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Resizable : AntDomComponentBase
    {
        [Parameter]
        public string Tag { get; set; } = "div";

        [Parameter]
        public bool Disabled { get; set; }

        internal double Height { get; set; }

        internal double Width { get; set; }

        public bool EnableRender { get; set; } = true;

        private bool _resizing = false;

        private IList<ResizeHandle> _handles = new List<ResizeHandle>();

        private void SetClass()
        {
            var prefixCls = "ant-resizable";
            ClassMapper.Add($"{prefixCls}")
                .If($"{prefixCls}-resizing", () => _resizing)
                .If($"{prefixCls}-disabled", () => Disabled)
                 ;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();
        }

        internal void AddHandle(ResizeHandle handle)
        {
            _handles.Add(handle);
        }

        internal void RemoveHandle(ResizeHandle handle)
        {
            _handles.Remove(handle);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (EnableRender && builder != null)
            {
                base.BuildRenderTree(builder);

                int i = 0;
                builder.OpenElement(i++, Tag);
                builder.AddAttribute(i++, "id", Id);
                builder.AddAttribute(i++, "class", ClassMapper.Class);
                builder.AddAttribute(i++, "style", GetStyle());
                builder.AddAttribute(i++, "onmouseenter", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseEnter));
                builder.AddAttribute(i++, "onmouseleave", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseLeave));

                builder.OpenComponent<CascadingValue<Resizable>>(i++);
                builder.AddAttribute(i++, "Value", this);
                builder.CloseComponent();

                builder.AddElementReferenceCapture(i++, (value) => { Ref = value; });
                builder.CloseElement();

                EnableRender = false;
            }
        }

        protected override bool ShouldRender()
        {
            return EnableRender;
        }

        private void OnMouseEnter()
        {
        }

        private void OnMouseLeave()
        {
        }

        private void OnHandleMouseDown(string direction, MouseEventArgs evnet)
        {
        }

        private string GetStyle()
        {
            var sb = new StringBuilder();
            if (Height > 0)
            {
                sb.Append($"height: {Height}px;");
            }

            return sb.ToString();
        }
    }
}
