// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DescriptionsItem : AntDomComponentBase, IDescriptionsItem
    {
        /// <summary>
        /// Title for the item
        /// </summary>
        [Parameter]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Title content for the item. Takes priority over <see cref="Title"/>
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Span of the item
        /// </summary>
        /// <default value="1"/>
        [Parameter]
        public int Span { get; set; } = 1;

        /// <summary>
        /// Content for the item
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Customize the style of the label
        /// </summary>
        [Parameter]
        public string LabelStyle { get; set; }

        /// <summary>
        /// Customize the style of the content
        /// </summary>
        [Parameter]
        public string ContentStyle { get; set; }

        /// <summary>
        /// Change default props <c>Colon</c> value of <see cref="DescriptionsItem"/>.
        /// </summary>
        [Parameter]
        public bool Colon { get; set; }

        /// <summary>
        /// Model for the item
        /// </summary>
        [Parameter]
        public bool? Vertical { get; set; }


        [CascadingParameter]
        private Descriptions Descriptions { get; set; }

        ElementReference IDescriptionsItem.Ref { get => base.Ref; set => base.Ref = value; }

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
