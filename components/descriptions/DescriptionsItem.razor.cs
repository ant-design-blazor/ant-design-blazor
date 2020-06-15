using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class DescriptionsItem : AntDomComponentBase, IDescriptionsItem
    {
        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; } = "";

        [Parameter]
        public int Span { get; set; } = 1;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        public Descriptions Descriptions { get; set; }

        protected override void OnInitialized()
        {
            this.Descriptions?.Items.Add(this);
            base.OnInitialized();
        }
    }
}
