using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace AntDesign
{
    public partial class Cascader : AntInputComponentBase<string>
    {
        [Parameter] public bool Readonly { get; set; } = true;

        /// <summary>
        /// 是否支持清除
        /// </summary>
        [Parameter] public bool AllowClear { get; set; } = true;

        /// <summary>
        /// 是否显示关闭图标
        /// </summary>
        private bool ShowClearIcon { get; set; }

        /// <summary>
        /// 当此项为 true 时，点选每级菜单选项值都会发生变化
        /// </summary>
        [Parameter] public bool ChangeOnSelect { get; set; }

        /// <summary>
        /// 默认的选中项
        /// </summary>
        [Parameter] public string DefaultValue { get; set; }

        /// <summary>
        /// 次级菜单的展开方式，可选 'click' 和 'hover'
        /// </summary>
        [Parameter] public string ExpandTrigger { get; set; }

        /// <summary>
        /// 当下拉列表为空时显示的内容
        /// </summary>
        [Parameter] public string NotFoundContent { get; set; } = "Not Found";

        /// <summary>
        /// 输入框占位文本
        /// </summary>
        [Parameter] public string PlaceHolder { get; set; } = "Please Select";

        /// <summary>
        /// 在选择框中显示搜索框
        /// </summary>
        [Parameter] public bool ShowSearch { get; set; }

        /// <summary>
        /// 输入框大小，可选 'large', 'middle', 'small'
        /// </summary>
        [Parameter] public string Size { get; set; } = "middle";

        /// <summary>
        /// 选择完成后的回调(参数为选中的节点集合及选中值)
        /// </summary>
        [Parameter] public Action<List<CascaderNode>, string, string> OnChange { get; set; }

        [Parameter]
        public IReadOnlyCollection<CascaderNode> Options
        {
            get
            {
                if (_nodelist != null)
                    return _nodelist;
                return Array.Empty<CascaderNode>();
            }
            set
            {
                if (value == null || value.Count == 0)
                {
                    _nodelist = null;
                    return;
                }
                if (_nodelist == null) _nodelist = new List<CascaderNode>();
                else if (_nodelist.Count != 0) _nodelist.Clear();
                _nodelist.AddRange(value);
            }
        }

        private List<CascaderNode> _nodelist;

        /// <summary>
        /// 选中节点集合(click)
        /// </summary>
        internal List<CascaderNode> _selectedNodes = new List<CascaderNode>();

        /// <summary>
        /// 选中节点集合(hover)
        /// </summary>
        internal List<CascaderNode> _hoverSelectedNodes = new List<CascaderNode>();

        /// <summary>
        /// 用于渲染下拉节点集合(一级节点除外)
        /// </summary>
        internal List<CascaderNode> _renderNodes = new List<CascaderNode>();

        private string _pickerSizeClass = string.Empty;
        private string _inputSizeClass = string.Empty;

        /// <summary>
        /// 浮层 展开/折叠状态
        /// </summary>
        private bool ToggleState { get; set; }

        /// <summary>
        /// 鼠标是否处于 浮层 之上
        /// </summary>
        private bool IsOnCascader { get; set; }

        /// <summary>
        /// 选择节点类型
        /// Click: 点击选中节点, Hover: 鼠标移入选中节点
        /// </summary>
        private SelectedTypeEnum SelectedType { get; set; }

        private string _displayText;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitCascaderNodeState(_nodelist, null, 0);

            if (Size == "large")
            {
                _pickerSizeClass = "ant-cascader-picker-large";
                _inputSizeClass = "ant-input-lg";
            }
            else if (Size == "small")
            {
                _pickerSizeClass = "ant-cascader-picker-small";
                _inputSizeClass = "ant-input-sm";
            }

            SetDefaultValue(Value ?? DefaultValue);
        }

        #region event

        /// <summary>
        /// 输入框单击(显示/隐藏浮层)
        /// </summary>
        private void InputOnToggle()
        {
            SelectedType = SelectedTypeEnum.Click;
            _hoverSelectedNodes.Clear();
            ToggleState = !ToggleState;
        }

        /// <summary>
        /// 输入框/浮层失去焦点(隐藏浮层)
        /// </summary>
        private void CascaderOnBlur()
        {
            if (!IsOnCascader)
            {
                ToggleState = false;
                _renderNodes = _selectedNodes;
            }
        }

        /// <summary>
        /// 输入框鼠标移入
        /// </summary>
        private void InputOnMouseOver()
        {
            if (!AllowClear) return;

            ShowClearIcon = true;
        }

        /// <summary>
        /// 输入框鼠标移出
        /// </summary>
        private void InputOnMouseOut()
        {
            if (!AllowClear) return;

            ShowClearIcon = false;
        }

        /// <summary>
        /// 清除已选择项
        /// </summary>
        private void ClearSelected()
        {
            _selectedNodes.Clear();
            _hoverSelectedNodes.Clear();
            Value = string.Empty;
        }

        /// <summary>
        /// 浮层移入
        /// </summary>
        private void NodesOnMouseOver()
        {
            if (!AllowClear) return;

            IsOnCascader = true;
        }

        /// <summary>
        /// 浮层移出
        /// </summary>
        private void NodesOnMouseOut()
        {
            if (!AllowClear) return;

            IsOnCascader = false;
        }

        /// <summary>
        /// 下拉节点单击
        /// </summary>
        /// <param name="node"></param>
        private void NodeOnClick(CascaderNode node)
        {
            if (node.Disabled) return;

            SetSelectedNode(node, SelectedTypeEnum.Click);
        }

        /// <summary>
        /// 下拉节点移入
        /// </summary>
        /// <param name="node"></param>
        private void NodeOnMouseOver(CascaderNode node)
        {
            if (ExpandTrigger != "hover") return;

            if (node.Disabled) return;
            if (!node.HasChildren) return;

            SetSelectedNode(node, SelectedTypeEnum.Hover);
        }

        #endregion event

        /// <summary>
        /// 选中节点
        /// </summary>
        /// <param name="cascaderNode"></param>
        /// <param name="selectedType"></param>
        internal void SetSelectedNode(CascaderNode cascaderNode, SelectedTypeEnum selectedType)
        {
            if (cascaderNode == null) return;

            SelectedType = selectedType;
            if (selectedType == SelectedTypeEnum.Click)
            {
                _selectedNodes.Clear();
                SetSelectedNodeWithParent(cascaderNode, ref _selectedNodes);
                _renderNodes = _selectedNodes;

                if (ChangeOnSelect || !cascaderNode.HasChildren)
                    SetValue(cascaderNode.Value);
            }
            else
            {
                _hoverSelectedNodes.Clear();
                SetSelectedNodeWithParent(cascaderNode, ref _hoverSelectedNodes);
                _renderNodes = _hoverSelectedNodes;
            }
            _renderNodes.Sort((x, y) => x.Level.CompareTo(y.Level));  //Level 升序排序

            if (!cascaderNode.HasChildren)
            {
                ToggleState = false;
                IsOnCascader = false;
            }
        }

        /// <summary>
        /// 设置选中所有父节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="list"></param>
        private void SetSelectedNodeWithParent(CascaderNode node, ref List<CascaderNode> list)
        {
            if (node == null) return;

            list.Add(node);
            SetSelectedNodeWithParent(node.ParentNode, ref list);
        }

        /// <summary>
        /// 初始化节点属性(Level, ParentNode)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentNode"></param>
        /// <param name="level"></param>
        private void InitCascaderNodeState(List<CascaderNode> list, CascaderNode parentNode, int level)
        {
            if (list == null) return;

            foreach (var node in list)
            {
                node.Level = level;
                node.ParentNode = parentNode;

                if (node.HasChildren)
                    InitCascaderNodeState(node.Children.ToList(), node, level + 1);
            }
        }

        /// <summary>
        /// 设置默认选中
        /// </summary>
        /// <param name="defaultValue"></param>
        private void SetDefaultValue(string defaultValue)
        {
            if (string.IsNullOrWhiteSpace(defaultValue))
                return;

            var node = GetNodeByValue(_nodelist, defaultValue);
            SetSelectedNodeWithParent(node, ref _selectedNodes);
            _renderNodes = _selectedNodes;
            SetValue(node.Value);
        }

        /// <summary>
        /// 设置输入框选中值
        /// </summary>
        /// <param name="value"></param>
        private void SetValue(string value)
        {
            _selectedNodes.Sort((x, y) => x.Level.CompareTo(y.Level));  //Level 升序排序
            _displayText = string.Empty;
            int count = 0;
            foreach (var node in _selectedNodes)
            {
                if (node == null) continue;

                if (count < _selectedNodes.Count - 1)
                    _displayText += node.Label + " / ";
                else
                    _displayText += node.Label;
                count++;
            }

            if (Value != value)
            {
                Value = value;
                ValueChanged.InvokeAsync(Value);
            }

            OnChange?.Invoke(_selectedNodes, value, _displayText);
        }

        /// <summary>
        /// 根据指定值获取节点
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private CascaderNode GetNodeByValue(List<CascaderNode> list, string value)
        {
            if (list == null) return null;
            CascaderNode result = null;

            foreach (var node in list)
            {
                if (node.Value == value)
                    return node;

                if (node.HasChildren)
                {
                    var nd = GetNodeByValue(node.Children.ToList(), value);
                    if (nd != null)
                        result = nd;
                }
            }
            return result;
        }
    }
}
