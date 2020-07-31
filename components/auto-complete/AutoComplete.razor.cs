using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using System.Data;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;

namespace AntDesign
{
    public partial class AutoComplete : AntInputComponentBase<string>
    {
        #region parameters

        /// <summary>
        /// 选项数据
        /// </summary>
        [Parameter] public IEnumerable<string> Options { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Parameter] public IEnumerable<string> FormatList { get; set; }

        /// <summary>
        /// 支持清除, 单选模式有效
        /// </summary>
        [Parameter] public bool AllowClear { get; set; }

        /// <summary>
        /// 自动获取焦点
        /// </summary>
        [Parameter] public bool AutoFocus { get; set; }

        /// <summary>
        /// 使用键盘选择选项的时候把选中项回填到输入框中
        /// </summary>
        [Parameter] public bool BackFill { get; set; }

        /// <summary>
        /// 自定义输入框
        /// </summary>
        [Parameter] public RenderFragment CustomInput { get; set; }

        /// <summary>
        /// 是否默认高亮第一个选项
        /// </summary>
        [Parameter] public bool DefaultActiveFirstOption { get; set; } = true;

        /// <summary>
        /// 默认的选中项
        /// </summary>
        [Parameter] public string DefaultValue { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [Parameter] public bool Disabled { get; set; }

        /// <summary>
        /// 输入框提示
        /// </summary>
        [Parameter] public string PlaceHolder { get; set; }

        /// <summary>
        /// 获得焦点时的回调
        /// </summary>
        [Parameter] public Action<string> OnFocus { get; set; }

        /// <summary>
        /// 失去焦点时的回调
        /// </summary>
        [Parameter] public Action<string> OnBlur { get; set; }

        /// <summary>
        /// 选中 option，或 input 的 value 变化时，调用此函数
        /// </summary>
        [Parameter] public Action<string> OnChange { get; set; }

        /// <summary>
        /// 被选中时调用，参数为选中项的 value 值
        /// </summary>
        [Parameter] public Action<string> OnSelect { get; set; }

        [Parameter]
        public Func<string, string, bool> FilterOption { get; set; }

        #endregion parameters

        #region variable

        /// <summary>
        /// 浮层 数据
        /// </summary>
        private IList<string> _options = new List<string>();


        private bool _toggleState;
        /// <summary>
        /// 浮层 展开/折叠状态
        /// </summary>
        private bool ToggleState
        {
            get => _toggleState;
            set
            {
                _toggleState = value;
                if (_toggleState == false) _activeOption = null;
            }
        }

        /// <summary>
        /// active 状态的 Option
        /// </summary>
        private string _activeOption;

        /// <summary>
        /// 鼠标是否在 Option 上
        /// </summary>
        private bool _isOnOptions;

        #endregion variable

        #region init

        protected override void OnInitialized()
        {
            FilterOption ??= (value, option) => option.Contains(value, StringComparison.InvariantCulture);
            base.OnInitialized();
        }

        #endregion init

        #region event

        private void OnInputFocus()
        {
            if (Value != null || Options != null && Options.Any())
            {
                _activeOption = null;
                ToggleState = true;
            }

            if (!string.IsNullOrWhiteSpace(Value))
                OnFocus?.Invoke(Value);
        }

        private void OnInputBlur()
        {
            if (_isOnOptions) return;

            ToggleState = false;

            if (!string.IsNullOrWhiteSpace(Value))
                OnBlur?.Invoke(Value);
        }

        private void OnInputChange(ChangeEventArgs args)
        {
            var v = args?.Value.ToString();
            CurrentValue = v;

            if (Options != null)   // Options 参数不为空时，本地过滤选项
            {
                _options.Clear();
                _options = !string.IsNullOrWhiteSpace(v) ? Options.Where(option => FilterOption(v, option)).ToList() : Options.ToList();

                // 默认选中第一个
                if (_options.Count > 0)
                {
                    ToggleState = true;
                }
            }
            else if (FormatList != null)   // FormatList 参数不为空时，按照指定 Format 格式添加选项
            {
                _options.Clear();
                if (!string.IsNullOrWhiteSpace(v))
                {
                    FormatList.ForEach(f => _options.Add(string.Format(f, v)));
                }
                // 默认选中第一个
                if (_options.Count > 0)
                {
                    ToggleState = true;
                }
            }
            else  // 一般模式，从远程获取数据添加选项
            {
                // 此处暂无需处理
            }

            OnChange?.Invoke(v);
        }

        private void OnOptionMouseOver(string option)
        {
            _activeOption = option;
        }

        private void OnOptionClick(string option)
        {
            ToggleState = false;
            _isOnOptions = false;

            if (Value != option)
            {
                CurrentValue = option;
                ValueChanged.InvokeAsync(option);
            }

            OnSelect?.Invoke(option);
        }

        private void OnOptionsMouseOver()
        {
            _isOnOptions = true;
        }

        private void OnOptionsMouseOut()
        {
            _isOnOptions = false;
        }

        public void OnKeyDown(KeyboardEventArgs args)
        {
            if (!ToggleState)
                return;

            if (args.Code == "NumpadEnter" || args.Code == "Enter") //Enter
            {
                if (!string.IsNullOrWhiteSpace(_activeOption))
                {
                    CurrentValue = _activeOption;
                    ValueChanged.InvokeAsync(_activeOption);
                    ToggleState = false;

                }
                else if (_options.IndexOf(Value) != -1)
                {
                    ValueChanged.InvokeAsync(CurrentValue);
                    ToggleState = false;
                }
            }

            if (args.Code == "ArrowUp") //上键
            {
                if (_options.Count == 0) return;

                int index = _options.IndexOf(_activeOption);
                if (string.IsNullOrWhiteSpace(_activeOption) || index <= 0)
                    index = _options.Count;

                _activeOption = _options.ElementAt(index - 1);
            }

            if (args.Code == "ArrowDown") //下键
            {
                if (_options.Count == 0) return;

                int index = _options.IndexOf(_activeOption);
                if (index == -1)
                {
                    index = _options.IndexOf(Value);
                }
                if (index >= _options.Count - 1 || index < 0)
                    index = -1;

                _activeOption = _options.ElementAt(index + 1);
            }
        }

        #endregion event

        #region public

        public void LoadData(IEnumerable<string> list)
        {
            if (Options != null)   // Options 参数不为空时，本地过滤
            {
                // 此处暂无需处理
            }
            else if (FormatList != null)   // FormatList 参数不为空时，按照指定 Format 格式添加选项
            {
                // 此处暂无需处理
            }
            else  // 一般模式，从远程获取数据添加选项
            {
                list ??= Enumerable.Empty<string>();

                ToggleState = list.Any();

                _options = list.ToList();
            }
        }

        #endregion public
    }
}
