﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using AntDesign.JsInterop;

namespace AntDesign
{
    public partial class Cascader : AntInputComponentBase<string>
    {
        [Parameter] public bool AllowClear { get; set; } = true;

        /// <summary>
        /// Overlay adjustment strategy (when for example browser resize is happening)
        /// </summary>
        [Parameter] public TriggerBoundaryAdjustMode BoundaryAdjustMode { get; set; } = TriggerBoundaryAdjustMode.None;

        [Parameter] public bool ChangeOnSelect { get; set; }

        [Parameter] public string DefaultValue { get; set; }

        [Parameter] public string ExpandTrigger { get; set; }

        [Parameter] public string NotFoundContent { get; set; } = LocaleProvider.CurrentLocale.Empty.Description;

        [Parameter] public string Placeholder { get => _placeHolder; set => _placeHolder = value; }

        [Parameter] public string PopupContainerSelector { get; set; } = "body";

        [Parameter] public bool ShowSearch { get; set; }

        /// <summary>
        /// Please use SelectedNodesChanged instead.
        /// </summary>
        [Obsolete("Instead use SelectedNodesChanged.")]
        [Parameter] public Action<List<CascaderNode>, string, string> OnChange { get; set; }

        [Parameter] public EventCallback<CascaderNode[]> SelectedNodesChanged { get; set; }

        [Parameter]
        public IEnumerable<CascaderNode> Options
        {
            get
            {
                if (_nodelist != null)
                    return _nodelist;
                return Array.Empty<CascaderNode>();
            }
            set
            {
                if (value?.Any() != true)
                {
                    _nodelist = null;
                }
                if (_nodelist == null) _nodelist = new List<CascaderNode>();
                else if (_nodelist.Count != 0) _nodelist.Clear();
                _nodelist.AddRange(value);

                ProcessParentAndDefault();
            }
        }

        private List<CascaderNode> _nodelist = new List<CascaderNode>();
        private List<CascaderNode> _selectedNodes = new List<CascaderNode>();
        private List<CascaderNode> _hoverSelectedNodes = new List<CascaderNode>();
        private List<CascaderNode> _renderNodes = new List<CascaderNode>();
        private List<CascaderNode> _searchList = new List<CascaderNode>();
        private IEnumerable<CascaderNode> _matchList;

        private ClassMapper _menuClassMapper = new ClassMapper();
        private ClassMapper _inputClassMapper = new ClassMapper();

        private EventCallbackFactory _callbackFactory = new EventCallbackFactory();

        private bool _dropdownOpened;
        private bool _isOnCascader;
        private SelectedTypeEnum _selectedType;

        private bool _showClearIcon;
        private string _displayText;
        private bool _initialized;
        private string _searchValue;
        private ElementReference _inputRef;
        private string _placeHolder = LocaleProvider.CurrentLocale.Global.Placeholder;

        private bool _focused;
        private string _menuStyle;

        private static Dictionary<string, string> _sizeMap = new Dictionary<string, string>()
        {
            ["large"] = "lg",
            ["small"] = "sm"
        };

        protected override void OnInitialized()
        {
            base.OnInitialized();
            string prefixCls = "ant-cascader";

            ClassMapper
                .Add($"{prefixCls}-picker")
                .GetIf(() => $"{prefixCls}-picker-{Size}", () => _sizeMap.ContainsKey(Size))
                .If($"{prefixCls}-picker-show-search", () => ShowSearch)
                .If($"{prefixCls}-picker-with-value", () => !string.IsNullOrEmpty(_searchValue))
                .If($"{prefixCls}-picker-rtl", () => RTL);

            _inputClassMapper
                .Add("ant-input")
                .GetIf(() => $"ant-input-{_sizeMap[Size]}", () => _sizeMap.ContainsKey(Size))
                .Add($"{prefixCls}-input")
                .If($"{prefixCls}-input-rtl", () => RTL);

            _menuClassMapper
                .Add($"{prefixCls}-menu")
                .If($"{prefixCls}-menu-rtl", () => RTL);

            SetDefaultValue(Value ?? DefaultValue);
        }

        protected override void OnValueChange(string value)
        {
            base.OnValueChange(value);
            RefreshNodeValue(value);
        }

        /// <summary>
        /// 输入框单击(显示/隐藏浮层)
        /// </summary>
        private async Task InputOnToggle()
        {
            _selectedType = SelectedTypeEnum.Click;
            _hoverSelectedNodes.Clear();
            if (!_dropdownOpened)
            {
                var inputElemnet = await Js.InvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _inputRef);
                _menuStyle = $"width:{inputElemnet.ClientWidth}px;";
                if (!_nodelist.Any())
                {
                    _menuStyle += "height:auto;";
                }

                _dropdownOpened = true;
            }
        }

        /// <summary>
        /// 输入框/浮层失去焦点(隐藏浮层)
        /// </summary>
        private void CascaderOnBlur()
        {
            if (!_isOnCascader)
            {
                _dropdownOpened = false;
                _renderNodes = _selectedNodes;
            }
        }

        /// <summary>
        /// 输入框鼠标移入
        /// </summary>
        private void InputOnMouseOver()
        {
            if (!AllowClear) return;

            _showClearIcon = !string.IsNullOrWhiteSpace(Value) || !string.IsNullOrEmpty(_searchValue);
        }

        /// <summary>
        /// 输入框鼠标移出
        /// </summary>
        private void InputOnMouseOut()
        {
            if (!AllowClear) return;

            _showClearIcon = false;
        }

        /// <summary>
        /// 清除已选择项
        /// </summary>
        private void ClearSelected()
        {
            _selectedNodes.Clear();
            _hoverSelectedNodes.Clear();
            _displayText = string.Empty;
            SetValue(string.Empty);
            _dropdownOpened = false;
        }

        /// <summary>
        /// 浮层移入
        /// </summary>
        private void NodesOnMouseOver()
        {
            if (!AllowClear) return;

            _isOnCascader = true;
        }

        /// <summary>
        /// 浮层移出
        /// </summary>
        private void NodesOnMouseOut()
        {
            if (!AllowClear) return;

            _isOnCascader = false;
        }

        /// <summary>
        /// 下拉节点单击
        /// </summary>
        /// <param name="node"></param>
        private void NodeOnClick(CascaderNode node)
        {
            if (node.Disabled) return;
            _searchValue = string.Empty;
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

        private void OnSearchInput(ChangeEventArgs e)
        {
            _searchValue = e.Value?.ToString();
        }

        private async Task OnSearchKeyUp(KeyboardEventArgs e)
        {
            if (string.IsNullOrEmpty(_searchValue))
            {
                _showClearIcon = false;
                return;
            }

            var inputElemnet = await Js.InvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _inputRef);
            _menuStyle = $"width:{inputElemnet.ClientWidth}px;";
            _matchList = _searchList.Where(x => x.Label.Contains(_searchValue, StringComparison.OrdinalIgnoreCase));
            _showClearIcon = true;
            if (!_matchList.Any())
            {
                _menuStyle += "height:auto;";
            }
        }

        /// <summary>
        /// Selected nodes
        /// </summary>
        /// <param name="cascaderNode"></param>
        /// <param name="selectedType"></param>
        private void SetSelectedNode(CascaderNode cascaderNode, SelectedTypeEnum selectedType)
        {
            if (cascaderNode == null) return;

            _selectedType = selectedType;
            if (selectedType == SelectedTypeEnum.Click)
            {
                _selectedNodes.Clear();
                SetSelectedNodeWithParent(cascaderNode, ref _selectedNodes);
                _renderNodes = _selectedNodes;

                if (ChangeOnSelect || !cascaderNode.HasChildren)
                {
                    SetValue(cascaderNode.Value);
                }
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
                _dropdownOpened = false;
                _isOnCascader = false;
            }
        }

        /// <summary>
        /// Set all parent nodes to be selected
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
        /// handles parent nodes and defaults after Options updating
        /// </summary>
        private void ProcessParentAndDefault()
        {
            InitCascaderNodeState(_nodelist, null, 0);
            SetDefaultValue(Value ?? DefaultValue);
        }

        /// <summary>
        /// Initialize nodes (Level, ParentNode)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentNode"></param>
        /// <param name="level"></param>
        private void InitCascaderNodeState(List<CascaderNode> list, CascaderNode parentNode, int level, bool recursive = false)
        {
            if (list == null) return;

            foreach (var node in list)
            {
                node.Level = level;
                node.ParentNode = parentNode;

                if (node.HasChildren)
                {
                    InitCascaderNodeState(node.Children.ToList(), node, level + 1, true);
                }
                else
                {
                    _searchList.Add(new CascaderNode
                    {
                        Label = node.Label,
                        ParentNode = node.ParentNode,
                        Value = node.Value,
                        Disabled = node.Disabled,
                        Level = node.Level
                    });
                }
            }

            if (!recursive)
            {
                _searchList.ForEach(node =>
                {
                    var pathList = new List<CascaderNode>();
                    SetSelectedNodeWithParent(node, ref pathList);
                    pathList.Reverse();
                    node.Label = string.Join(" / ", pathList.Select(x => x.Label));
                });
            }
        }

        /// <summary>
        /// Refresh the selected value
        /// </summary>
        /// <param name="value"></param>
        private void RefreshNodeValue(string value)
        {
            _selectedNodes.Clear();
            var node = GetNodeByValue(_nodelist, value);
            SetSelectedNodeWithParent(node, ref _selectedNodes);
            _renderNodes = _selectedNodes;
        }

        /// <summary>
        /// Set the default value
        /// </summary>
        /// <param name="defaultValue"></param>
        private void SetDefaultValue(string defaultValue)
        {
            if (!string.IsNullOrWhiteSpace(defaultValue))
            {
                RefreshNodeValue(defaultValue);
                SetValue(defaultValue);
            }

            _initialized = true;
        }

        /// <summary>
        /// Set the binding value
        /// </summary>
        /// <param name="value"></param>
        private void SetValue(string value)
        {
            RefreshDisplayText();

            if (Value != value)
            {
                CurrentValueAsString = value;

                if (_initialized && SelectedNodesChanged.HasDelegate)
                {
                    SelectedNodesChanged.InvokeAsync(_selectedNodes.ToArray());
                }

                OnChange?.Invoke(_selectedNodes, value, _displayText);
            }
        }

        /// <summary>
        /// rebuild the display text
        /// </summary>
        private void RefreshDisplayText()
        {
            _selectedNodes.Sort((x, y) => x.Level.CompareTo(y.Level));  //Level 升序排序
            _displayText = string.Join(" / ", _selectedNodes.Where(x => x != null).Select(x => x.Label));
        }

        /// <summary>
        /// Get the node based on the specified value
        /// </summary>
        /// <param name="list"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private CascaderNode GetNodeByValue(IEnumerable<CascaderNode> list, string value)
        {
            if (list == null) return null;

            foreach (var node in list)
            {
                if (node.Value == value)
                    return node;

                if (node.HasChildren)
                {
                    var nd = GetNodeByValue(node.Children, value);
                    if (nd != null)
                    {
                        return nd;
                    }
                }
            }
            return null;
        }
    }
}
