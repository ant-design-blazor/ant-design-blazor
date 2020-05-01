using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntCarouselSlick : AntDomComponentBase
    {
        private const string PreFixCls = "slick";
        private AntCarousel _parent;
        internal bool Active { get; private set; }
        internal new string Class => ClassMapper.Class;

        #region Parameters

        [CascadingParameter]
        internal AntCarousel Parent
        {
            get => _parent;
            set
            {
                if (_parent == null)
                {
                    _parent = value;
                    OnParentSet();
                }
            }
        }

        private RenderFragment _childContent;

        [Parameter]
        public RenderFragment ChildContent
        {
            get => _childContent;
            set
            {
                _childContent = value;
            }
        }

        #endregion Parameters

        private void OnParentSet()
        {
            Parent.AddSlick(this);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            ClassMapper.Clear()
                .Add($"{PreFixCls}-slide")
                .If($"{PreFixCls}-active", () => Active)
                .If($"{PreFixCls}-current", () => Active);
        }

        internal void Activate()
        {
            Active = true;
            StateHasChanged();
        }

        internal void Deactivate()
        {
            Active = false;
            StateHasChanged();
        }
    }
}
