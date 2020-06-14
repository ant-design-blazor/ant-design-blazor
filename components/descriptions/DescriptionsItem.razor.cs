using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class DescriptionsItem : AntDomComponentBase, IDescriptionsItem
    {
        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; } = "";

        [Parameter]
        public int Span { get; set; }

        [Parameter]
        public RenderFragment Content { get; set; }


        private Descriptions _parent;

        [CascadingParameter]
        internal Descriptions Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                if (_parent == null)
                {
                    _parent = value;
                    _parent.AddDescriptionsItem(this);
                }
            }
        }

    }
}
