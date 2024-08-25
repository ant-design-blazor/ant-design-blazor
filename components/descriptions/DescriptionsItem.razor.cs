// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class DescriptionsItem : AntDomComponentBase, IDescriptionsItem
    {
        [Parameter]
        public string Title { get; set; } = "";

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        [Parameter]
        public int Span { get; set; } = 1;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string LabelStyle { get; set; }

        [Parameter]
        public string ContentStyle { get; set; }

        [CascadingParameter]
        public Descriptions Descriptions { get; set; }

        protected override void Dispose(bool disposing)
        {
            this.Descriptions?.Items.Remove(this);
            base.Dispose(disposing);
        }

        protected override void OnInitialized()
        {
            this.Descriptions?.Items.Add(this);
            base.OnInitialized();
        }
    }
}
