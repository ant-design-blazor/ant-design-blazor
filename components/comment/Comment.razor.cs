using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OneOf;

namespace AntDesign
{
    public partial class Comment : AntDomComponentBase
    {
        [Parameter] public OneOf<string, RenderFragment> Author { get; set; }

        [Parameter] public OneOf<string, RenderFragment> Avatar { get; set; }

        [Parameter] public OneOf<string, RenderFragment> Content { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public OneOf<string, RenderFragment> Datetime { get; set; }

        [Parameter] public IList<RenderFragment> Actions { get; set; } = new List<RenderFragment>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ClassMapper.Clear()
                .Add("ant-comment");
        }
    }
}
