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
                _options = value;
                _optionValues = _options?.Select(x => x.Value).ToList() ?? new();
            }
        }

        [Parameter]
        public IEnumerable<TValue> Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                _optionValues = _labels?.ToList() ?? new();
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

        protected string PrefixCls => "ant-segmented";

        private bool _sliding;
        private string _slidingCls;
        private string _slidingStyle;
        private int _startLeft;
        private int _startWidth;
        private int _activeLeft;
        private int _activeWidth;

        private List<TValue> _optionValues = new List<TValue>();
        private IEnumerable<SegmentedOption<TValue>> _options;
        private IEnumerable<TValue> _labels;
        private TValue _value;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Add(PrefixCls)
                .If($"{PrefixCls}-lg", () => Size == SegmentedSize.Large)
                .If($"{PrefixCls}-sm", () => Size == SegmentedSize.Small)
                .If($"{PrefixCls}-disabled", () => Disabled)
                .If($"{PrefixCls}-block", () => Block)
                ;

            if (DefaultValue != null)
            {
                Value = DefaultValue;
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

            if (Labels == null && Options == null)
            {
                _optionValues.Add(item.Value);
            }

            if (Value == null && _optionValues?.Any() == true)
            {
                Value = _optionValues[0];
            }
        }

        internal void RemoveItem(SegmentedItem<TValue> item)
        {
            _items?.Remove(item);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (firstRender)
            //{
            //    var refs = _items.Select(x => x.Ref).ToArray();
            //    var elements = await JsInvokeAsync<Dictionary<string, HtmlElement>>(JSInteropConstants.GetElementsDomInfo, refs);
            //    foreach (var item in _items)
            //    {
            //        item.Width = elements[item.Id].ClientWidth;
            //    }
            //}

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

        internal void Select(SegmentedItem<TValue> item)
        {
            _items.ForEach(x => x.SetSelected(false));

            //_sliding = true;
            //_slidingCls = "ant-segmented-thumb";
            ////_slidingStyle = "--thumb-start-left:53px; --thumb-start-width:66px; --thumb-active-left:0px; --thumb-active-width:53px;";
            //_items.ForEach(x => x.SetSelected(false));
            //await Task.Delay(1000);

            //_slidingCls = "ant-segmented-thumb ant-segmented-thumb-motion-appear ant-segmented-thumb-motion-appear-prepare ant-segmented-thumb-motion";
            //StateHasChanged();
            //await Task.Delay(1000);

            //_slidingCls = "ant-segmented-thumb ant-segmented-thumb-motion-appear ant-segmented-thumb-motion-appear-start ant-segmented-thumb-motion";
            //_slidingStyle = "--thumb-start-left:53px; --thumb-start-width:66px; --thumb-active-left:0px; --thumb-active-width:53px; transform: translateX(var(--thumb-start-left)); width: var(--thumb-start-width);";
            //StateHasChanged();
            //await Task.Delay(1000);

            //_slidingCls = "ant-segmented-thumb ant-segmented-thumb-motion-appear ant-segmented-thumb-motion-appear-active ant-segmented-thumb-motion";
            //StateHasChanged();
            //await Task.Delay(1000);

            //_slidingCls = "";
            //_sliding = false;
            //StateHasChanged();

            Value = item.Value;

            item.SetSelected(true);

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(Value);
            }

            if (ValueChanged.HasDelegate)
            {
                ValueChanged.InvokeAsync(Value);
            }
        }
    }
}
