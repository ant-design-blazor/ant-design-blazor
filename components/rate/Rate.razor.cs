// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
    <para>Rate component.</para>

    <h2>When To Use</h2>

    <list type="bullet">
        <item>Show evaluation.</item>
        <item>A quick rating operation on something.</item>
    </list>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/R5uiIWmxe/Rate.svg",Title = "Rate", SubTitle = "评分")]
    public partial class Rate : AntDomComponentBase
    {
        /// <summary>
        /// Whether to allow clear or not when clicking again
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool AllowClear { get; set; } = true;

        /// <summary>
        /// Whether to allow selection of halves
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool AllowHalf { get; set; } = false;

        /// <summary>
        /// Whether to disable the selection or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// Whether to focus on render or not
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool AutoFocus { get; set; } = true;

        /// <summary>
        /// Custom character for each rate
        /// </summary>
        [Parameter]
        public RenderFragment<RateItemRenderContext> Character { get; set; }

        /// <summary>
        /// Number of icons to display for rating
        /// </summary>
        /// <default value="5"/>
        [Parameter]
        public int Count { get; set; } = 5;

        /// <summary>
        /// Current value for rating
        /// </summary>
        [Parameter]
        public decimal Value
        {
            get { return _currentValue; }
            set
            {
                _valueWasSet = true;
                if (_currentValue != value)
                {
                    this._currentValue = value;
                    this._hasHalf = !(_currentValue == (int)_currentValue);
                    this._hoverValue = (int)Math.Ceiling(value);
                    ValueChanged.InvokeAsync(value);
                }
            }
        }

        /// <summary>
        /// Callback executed when the rating changes
        /// </summary>
        [Parameter]
        public EventCallback<decimal> ValueChanged { get; set; }

        /// <summary>
        /// Default value for when rating is rendered
        /// </summary>
        [Parameter]
        public decimal DefaultValue { get; set; }

        /// <summary>
        /// Tooltip to show for each increment of rating, in order of index of rating 0-n
        /// </summary>
        [Parameter]
        public string[] Tooltips { get; set; }

        /// <summary>
        /// Callback executed when the rate looses focus
        /// </summary>
        [Parameter]
        public EventCallback<FocusEventArgs> OnBlur { get; set; }

        /// <summary>
        /// Callback executed when the rate gains focus
        /// </summary>
        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        private IEnumerable<RateMetaData> RateMetaDatas { get; set; }

        /// <summary>
        /// 是否允许半选
        /// Whether to allow half-selection
        /// </summary>
        private bool _hasHalf = false;

        /// <summary>
        /// 鼠标悬停时从最左到光标位置的星星数。
        /// The number of stars from the far left to the cursor position when the hovered with mouse.
        /// </summary>
        private int _hoverValue = 0;

        /// <summary>
        /// 当前被选中的星星数量
        /// Number of stars currently selected
        /// </summary>
        private decimal _currentValue;

        /// <summary>
        /// 是否获取的输入焦点
        /// Wheter to get input focus.
        /// </summary>
        private bool _isFocused = false;

        /// <summary>
        /// Indicates if Value has been changed. Needed to avoid reseting to DefaultValue if exists.
        /// </summary>
        private bool _valueWasSet;

        private void Blur(FocusEventArgs e)
        {
            this._isFocused = false;
            OnBlur.InvokeAsync(e);
        }

        private void Focus(FocusEventArgs e)
        {
            this._isFocused = true;
            OnFocus.InvokeAsync(e);
        }

        private void KeyDown(KeyboardEventArgs e)
        {
            decimal oldVal = this.Value;

            if (e.Key == "ArrowRight" && this.Value < Count)
            {
                this.Value += this.AllowHalf ? 0.5M : 1M;
            }
            else if (e.Key == "ArrowLeft" && this.Value > 0)
            {
                this.Value -= this.AllowHalf ? 0.5M : 1M;
            }
            if (oldVal != this.Value)
            {
                this._hasHalf = !(this.Value == (int)this.Value);
                this._hoverValue = (int)Math.Ceiling(this.Value);
            }
        }

        private void MouseLeave()
        {
            this._hasHalf = !(this.Value == (int)this.Value);
            this._hoverValue = (int)Math.Ceiling(this.Value);
        }

        private void ItemHoverChange(int index, bool isHalf)
        {
            if (this.Disabled || (this._hoverValue == index + 1 && isHalf == _hasHalf))
                return;

            this._hoverValue = index + 1;
            this._hasHalf = isHalf;
        }

        private void ItemClick(int index, bool isHalf)
        {
            if (this.Disabled)
                return;

            this._hoverValue = index + 1;

            decimal actualValue = isHalf ? index + 0.5M : index + 1;

            if (this.Value == actualValue)
            {
                if (this.AllowClear)
                {
                    this.Value = 0;
                    this._hoverValue = 0;
                }
            }
            else
            {
                this.Value = actualValue;
            }
        }

        protected override void OnInitialized()
        {
            if (DefaultValue > 0 && !_valueWasSet)
            {
                this.Value = DefaultValue;
            }

            SetClass();
            RateMetaDatas = Enumerable.Range(1, Count).Select(c => new RateMetaData() { SerialNumber = c - 1, ToolTipText = this.Tooltips?[c - 1] });
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            if (RateMetaDatas == null)
                RateMetaDatas = Enumerable.Range(1, Count).Select(c => new RateMetaData() { SerialNumber = c - 1, ToolTipText = this.Tooltips?[c - 1] });
            SetClass();
            base.OnParametersSet();
        }

        protected void SetClass()
        {
            var clsPrefix = "ant-rate";
            var hashId = UseStyle(clsPrefix, RateStyle.UseComponentStyle);
            ClassMapper.Clear()
                .Add(clsPrefix)
                .Add(hashId)
                .If($"{clsPrefix}-disabled", () => Disabled)
                .If($"{clsPrefix}-rtl", () => RTL);
        }
    }

    public class RateMetaData
    {
        public int SerialNumber { get; set; }

        public string ToolTipText { get; set; }
    }

    public class RateItemRenderContext
    {
        /// <summary>
        ///  'AntIcon' | 'Text'
        /// </summary>
        public string Type { get; set; }

        public RenderFragment<RateItemRenderContext> DefaultRender { get; set; }
    }
}
