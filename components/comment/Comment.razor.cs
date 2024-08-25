// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

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

        [Parameter]
        public string Placement { get; set; } = "left";

        private bool RightAvatar => Placement?.Equals("right", StringComparison.InvariantCultureIgnoreCase) == true;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            this.ClassMapper.Clear()
                .Add("ant-comment")
                .If("ant-comment-right", () => RightAvatar)
                .If("ant-comment-rtl", () => RTL);
        }
    }
}
