// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class SegmentedItem<TValue> : AntDomComponentBase
    {
        [Parameter]
        public TValue Value { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Icon { get; set; }

        [CascadingParameter]
        private Segmented<TValue> Parent { get; set; }

        protected string PrefixCls => "ant-segmented-item";

        internal int Index { get; set; }

        private bool _selected;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add(PrefixCls)
                .If($"{PrefixCls}-selected", () => _selected)
                .If($"{PrefixCls}-disabled", () => Disabled)
                ;

            Parent?.AddItem(this);
        }

        internal void SetSelected(bool selected)
        {
            _selected = selected;

            StateHasChanged();
        }

        private void OnClick()
        {
            if (Disabled)
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
