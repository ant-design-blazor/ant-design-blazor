// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>Segmented Controls. This component is available since `v0.12.0`.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>When displaying multiple options and user can select a single option</item>
        <item>When switching the selected option, the content of the associated area changes.</item>
    </list>
    </summary>
    <seealso cref="SegmentedItem{TValue}"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/bmw-prod/a3ff040f-24ba-43e0-92e9-c845df1612ad.svg", Title = "Segmented", SubTitle = "分段控制器")]
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TValue))]
#endif
    public partial class Segmented<TValue> : AntDomComponentBase
    {
        /// <summary>
        /// Default selected value
        /// </summary>
        [Parameter]
        public TValue DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValueSet = true;
                _defaultValue = value;
            }
        }

        /// <summary>
        /// Disable all segments
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Disabled
        {
            get => _disabled;
            set
            {
                if (_disabled == value)
                    return;

                _disabled = value;
                _items.ForEach(item => item.MarkStateHasChanged());
            }
        }

        /// <summary>
        /// The callback function that is triggered when the state changes
        /// </summary>
        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }

        /// <summary>
        /// Options for the segments. Takes priority over <see cref="Labels"/>
        /// </summary>
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

        /// <summary>
        /// Labels for the segments
        /// </summary>
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

        /// <summary>
        /// Size of the UI element
        /// </summary>
        [Parameter]
        public SegmentedSize Size { get; set; }

        /// <summary>
        /// Currently selected value
        /// </summary>
        [Parameter]
        public TValue Value
        {
            get => _value;
            set
            {
                if (_value is null && value is null)
                    return;

                _valueSet = true;
                if (_value?.Equals(value) == true)
                    return;

                _value = value;
                ChangeValue(_value);
            }
        }

        /// <summary>
        /// Callback executed when the selected segment changes
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Segments. Takes priority over <see cref="Labels"/> and <see cref="Options"/>
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Make the component the width of the parent
        /// </summary>
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
        private bool _valueSet;
        private bool _disabled;
        private TValue _defaultValue;
        private bool _defaultValueSet;

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

            if (_defaultValueSet)
            {
                _value = DefaultValue;
                ChangeValue(_value);
            }
        }

        internal void AddItem(SegmentedItem<TValue> item)
        {
            _items ??= new List<SegmentedItem<TValue>>();

            _items.Add(item);

            if (Labels == null && Options == null)
            {
                _optionValues.Add(item.Value);
                _optionsChanged = true;
            }

            if (!_valueSet && _optionValues?.Any() == true)
            {
                _value = _optionValues[0];
                _valueSet = true;
            }

            if (_valueSet)
            {
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
                ChangeValue(_value, true);
                await GetItemElememt();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private void ChangeValue(TValue value, bool noAnimation = false)
        {
            var item = _items.FirstOrDefault(x => x.Value.Equals(value));
            if (item != null)
            {
                Select(item, noAnimation);
            }
        }

        internal async Task Select(SegmentedItem<TValue> item, bool noAnimation = false)
        {
            _items[_activeIndex].SetSelected(false);
            _value = item.Value;

            if (OnChange.HasDelegate)
            {
                await OnChange.InvokeAsync(_value);
            }

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(_value);
            }

            if (!noAnimation)
            {
                await ThumbAnimation(item);
            }

            _activeIndex = _items.IndexOf(item);
            _items.ForEach(x => x.SetSelected(false));
            item.SetSelected(true);
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

            _sliding = true;
            _slidingCls = "ant-segmented-thumb ant-segmented-thumb-motion ant-segmented-thumb-motion-appear ant-segmented-thumb-motion-appear-start";
            _slidingStyle = $"transform: translateX({_itemRefs[_items[_activeIndex].Id].OffsetLeft}px); width: {_itemRefs[_items[_activeIndex].Id].ClientWidth}px;";

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
