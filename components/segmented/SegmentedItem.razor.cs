// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class SegmentedItem<TValue> : AntDomComponentBase
    {
        /// <summary>
        /// Value of the segment item
        /// </summary>
        [Parameter]
        public TValue Value { get; set; }

        /// <summary>
        /// Label for the UI of the segment item
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        /// <summary>
        /// If the segment is disabled
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// UI content to display in the segment. Takes priority over <see cref="Label"/> and <see cref="Icon"/>
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Icon to display next to the label of the segment. Only used when <see cref="ChildContent"/> is not provided.
        /// </summary>
        [Parameter]
        public string Icon { get; set; }

        [CascadingParameter]
        private Segmented<TValue> Parent { get; set; }

        protected string PrefixCls => "ant-segmented-item";

        private bool _selected;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add(PrefixCls)
                .If($"{PrefixCls}-selected", () => _selected)
                .If($"{PrefixCls}-disabled", () => Disabled || Parent?.Disabled == true)
                ;

            Parent?.AddItem(this);
        }

        protected internal void SetSelected(bool selected)
        {
            _selected = selected;

            StateHasChanged();
        }

        protected internal void MarkStateHasChanged() => StateHasChanged();

        private void OnClick()
        {
            if (Disabled || Parent.Disabled)
            {
                return;
            }
            Parent.Select(this);
        }

        protected override void Dispose(bool disposing)
        {
            Parent?.RemoveItem(this);
            base.Dispose(disposing);
        }
    }
}
