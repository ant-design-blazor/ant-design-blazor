using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Comment : AntDomComponentBase
    {
        [Parameter]
        public string Author { get; set; }

        [Parameter]
        public RenderFragment AuthorTemplate { get; set; }

        [Parameter]
        public string Avatar { get; set; }

        [Parameter]
        public RenderFragment AvatarTemplate { get; set; }

        [Parameter]
        public string Content { get; set; }

        [Parameter]
        public RenderFragment ContentTemplate { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Datetime { get; set; }

        [Parameter]
        public RenderFragment DatetimeTemplate { get; set; }

        [Parameter]
        public IList<RenderFragment> Actions { get; set; } = new List<RenderFragment>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ClassMapper.Clear()
                .Add("ant-comment")
                .If("ant-comment-rtl", () => RTL);
        }
    }
}
