using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Rate : AntDomComponentBase
    {

        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// 是否允许再次点击后清除
        /// </summary>
        [Parameter] public bool AllowClear { get; set; } = true;

        /// <summary>
        /// 是否允许半选
        /// </summary>
        [Parameter] public bool AllowHalf { get; set; } = false;

        /// <summary>
        /// 是否禁止用户交互
        /// </summary>
        [Parameter] public bool Disabled { get; set; } = false;

        /// <summary>
        /// 是否可获得输入焦点
        /// </summary>
        [Parameter] public bool AutoFocus { get; set; } = true;

        /// <summary>
        /// 自定义字符,星星可以被自定义字符替代
        /// </summary>
        [Parameter] public RenderFragment<RateItemRenderContext> Character { get; set; }

        /// <summary>
        /// 组件要呈现的星星数目
        /// </summary>
        [Parameter] public int Count { get; set; } = 5;

        /// <summary>
        /// 当前值--被选中的星星数量
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

        [Parameter]
        public EventCallback<decimal> ValueChanged { get; set; }

        /// <summary>
        /// 默认当前被选中的星星数量,如果被设置为小数位不为0的，则组件默认含有半星并且允许半星
        /// </summary>
        [Parameter] public decimal DefaultValue { get; set; }

        /// <summary>
        /// 自定义每项的提示信息（存储每个子元素的提醒框内容文本）
        /// </summary>
        [Parameter] public string[] Tooltips { get; set; }

        [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }

        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }

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

            ClassMapper.Clear()
                .Add(clsPrefix)
            .If("ant-rate-disabled", () => Disabled);
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
