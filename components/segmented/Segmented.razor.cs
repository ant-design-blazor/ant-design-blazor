// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Segmented<TValue> : AntDomComponentBase
    {
        [Parameter]
        public bool Block { get; set; }

        [Parameter]
        public TValue DefaultValue { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }

        [Parameter]
        public IEnumerable<TValue> Options { get; set; }

        [Parameter]
        public SegmentedSize Size { get; set; }

        [Parameter]
        public TValue Value { get; set; }

        [Parameter]
        public EventCallback<TValue> OnValueChanged { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private IList<SegmentedItem<TValue>> _items = new List<SegmentedItem<TValue>>();

        protected string PrefixCls => "ant-segmented";

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add(PrefixCls)
                .If($"{PrefixCls}-lg", () => Size == SegmentedSize.Large)
                .If($"{PrefixCls}-sm", () => Size == SegmentedSize.Small)
                ;

            if (DefaultValue != null && DefaultValue.IsIn(Options))
            {
                Value = DefaultValue;
            }

            if (Value == null && Options.Any())
            {
                Value = Options.ElementAt(0);
            }
        }

        internal void AddItem(SegmentedItem<TValue> item)
        {
            if (Value != null && item.Value.ToString() == Value.ToString())
            {
                item.SetSelected(true);
            }

            _items ??= new List<SegmentedItem<TValue>>();
            _items.Add(item);
        }

        internal void RemoveItem(SegmentedItem<TValue> item)
        {
            _items?.Remove(item);
        }

        internal async Task Select(SegmentedItem<TValue> item)
        {
            if (Value == null || Value.Equals(item.Value))
            {
                return;
            }

            Value = item.Value;

            _items.ForEach(x => x.SetSelected(false));
            item.SetSelected(true);

            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(Value);
            }
        }
    }
}
