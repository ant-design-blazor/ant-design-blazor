// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Segmented<TValue> : AntDomComponentBase
    {
        [Parameter]
        public TValue DefaultValue { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }

        [Parameter]
        public IEnumerable<SegmentedOption<TValue>> Options
        {
            get => _options;
            set
            {
                if (_options != null && _options.SequenceEqual(value))
                {
                    return;
                }
                _options = value;
                _optionValues = _options?.Select(x => x.Value).ToList() ?? new();
                _optionsChanged = true;
            }
        }

        [Parameter]
        public IEnumerable<TValue> Labels
        {
            get => _labels;
            set
            {
                if (_labels != null && _labels.SequenceEqual(value))
                {
                    return;
                }
                _labels = value;
                _optionValues = _labels?.ToList() ?? new();
                _optionsChanged = true;
            }
        }

        [Parameter]
        public SegmentedSize Size { get; set; }

        [Parameter]
        public TValue Value
        {
            get => _value;
            set
            {
                if (value != null && !value.Equals(_value))
                {
                    _value = value;
                    ChangeValue(_value);
                }
            }
        }

        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Block { get; set; }

        private IList<SegmentedItem<TValue>> _items = new List<SegmentedItem<TValue>>();
        private Dictionary<string, HtmlElement> _itemRefs;

        protected string PrefixCls => "ant-segmented";

        private bool _sliding;
        private string _slidingCls;
        private string _slidingStyle;

        private int _activeIndex;

        private List<TValue> _optionValues = new List<TValue>();
        private IEnumerable<SegmentedOption<TValue>> _options;
        private IEnumerable<TValue> _labels;
        private bool _optionsChanged;

        private bool _firstRendered;

        private TValue _value;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add(PrefixCls)
                .If($"{PrefixCls}-lg", () => Size == SegmentedSize.Large)
                .If($"{PrefixCls}-sm", () => Size == SegmentedSize.Small)
                .If($"{PrefixCls}-disabled", () => Disabled)
                .If($"{PrefixCls}-block", () => Block)
                .If($"{PrefixCls}-rtl", () => RTL)
                ;

            if (DefaultValue != null)
            {
                _value = DefaultValue;
                ChangeValue(_value);
            }
        }

        internal void AddItem(SegmentedItem<TValue> item)
        {
            _items ??= new List<SegmentedItem<TValue>>();

            item.Index = _items.Count;
            _items.Add(item);

            if (Labels == null && Options == null)
            {
                _optionValues.Add(item.Value);
                _optionsChanged = true;
            }

            if (_value == null && _optionValues?.Any() == true)
            {
                _value = _optionValues[0];
                ChangeValue(_value);
            }
        }

        internal void RemoveItem(SegmentedItem<TValue> item)
        {
            _items?.Remove(item);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _firstRendered = true;
            }

            if (_firstRendered && _optionsChanged)
            {
                _optionsChanged = false;
                await GetItemElememt();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private void ChangeValue(TValue value)
        {
            if (Disabled)
            {
                return;
            }

            var item = _items.FirstOrDefault(x => x.Value.Equals(value));
            if (item != null)
            {
                Select(item);
            }
        }

        internal async void Select(SegmentedItem<TValue> item)
        {
            _items.ForEach(x => x.SetSelected(false));

            await ThumbAnimation(item);

            _value = item.Value;

            item.SetSelected(true);

            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(_value);
            }

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(_value);
            }
        }

        private async Task GetItemElememt()
        {
            var refs = _items.Select(x => x.Ref).ToArray();
            _itemRefs = await JsInvokeAsync<Dictionary<string, HtmlElement>>(JSInteropConstants.GetElementsDomInfo, refs);
        }

        private async Task ThumbAnimation(SegmentedItem<TValue> item)
        {
            if (_itemRefs == null)
            {
                return;
            }

            if (item.Index == _activeIndex)
            {
                return;
            }

            _sliding = true;
            _slidingCls = "ant-segmented-thumb ant-segmented-thumb-motion ant-segmented-thumb-motion-appear ant-segmented-thumb-motion-appear-start";
            _slidingStyle = $"transform: translateX({_itemRefs[_items[_activeIndex].Id].OffsetLeft}px); width: {_itemRefs[_items[_activeIndex].Id].ClientWidth}px;";
            _activeIndex = item.Index;

            StateHasChanged();
            await Task.Delay(100);

            _slidingCls = "ant-segmented-thumb ant-segmented-thumb-motion ant-segmented-thumb-motion-appear ant-segmented-thumb-motion-appear-active ";
            _slidingStyle = $"transform: translateX({_itemRefs[item.Id].OffsetLeft}px); width: {_itemRefs[item.Id].ClientWidth}px;";

            StateHasChanged();
            await Task.Delay(300);

            _slidingCls = "";
            _sliding = false;
        }
    }
}
