using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class CarouselSlick : AntDomComponentBase
    {
        private const string PreFixCls = "slick";
        internal bool Active => Parent.ActiveSlick == this;
        internal new string Class => ClassMapper.Class;
        internal new string Style
        {
            get
            {
                if (Parent.Effect == CarouselEffect.Fade)
                {
                    return $"outline: none; width: {Parent.SlickWidth}px; position: relative; left: {-Parent.SlickWidth * Parent.IndexOfSlick(this)}px; opacity: {(Active ? 1 : 0)}; transition: opacity 500ms ease 0s, visibility 500ms ease 0s;";
                }
                else
                {
                    return $"outline: none; width: {Parent.SlickWidth}px;";
                }
            }
        }
        #region Parameters

        [CascadingParameter]
        internal Carousel Parent { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        #endregion Parameters

        protected override void Dispose(bool disposing)
        {
            Parent.RemoveSlick(this);
            base.Dispose(disposing);
        }

        protected override void OnInitialized()
        {
            Parent.AddSlick(this);
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            ClassMapper.Clear()
                .Add($"{PreFixCls}-slide")
                .If($"{PreFixCls}-active", () => Active)
                .If($"{PreFixCls}-current", () => Active);
        }
    }
}
