// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
        <para>Cascade selection box.</para>

        <h2>When To Use</h2>

        <list type="bullet">
            <item>When you need to select from a set of associated data set. Such as province/city/district, company level, things classification.</item>
            <item>When selecting from a large data set, with multi-stage classification separated for easy selection.</item>
            <item>Chooses cascade items in one float layer for better user experience.</item>
        </list>
    </summary>
    <seealso cref="CascaderNode"/>
    <seealso cref="TriggerBoundaryAdjustMode"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/UdS8y8xyZ/Cascader.svg", Title = "Cascader", SubTitle = "级联选择")]
    public partial class Cascader : SelectBase<string, string>
    {
        /// <summary>
        /// Change value on each selection if set to true, only chage on final selection if false.
        /// </summary>
        [Parameter]
        public bool ChangeOnSelect { get; set; }

        /// <summary>
        /// Initially selected value
        /// </summary>
        [Parameter]
        public string DefaultValue { get; set; }

        /// <summary>
        /// When to expand the current item - click or hover
        /// </summary>
        /// <default value="click" />
        [Parameter]
        public string ExpandTrigger { get; set; }

        /// <summary>
        /// Empty description for when not found
        /// </summary>
        /// <default value="No Data (in current locale)" />
        [Parameter]
        public string NotFoundContent { get; set; } = LocaleProvider.CurrentLocale.Empty.Description;

        /// <summary>
        /// Allow searching or not
        /// </summary>
        [Parameter]
        public bool ShowSearch { get; set; }

        /// <summary>
        /// Callback executed when the selected value changes
        /// </summary>
        [Obsolete("Instead use SelectedNodesChanged.")]
        [Parameter]
        public Action<List<CascaderNode>, string, string> OnChange { get; set; }

        /// <summary>
        /// Callback executed when the selected value changes
        /// </summary>
        [Parameter]
        public EventCallback<CascaderNode[]> SelectedNodesChanged { get; set; }

        /// <summary>
        /// Options for the overlay
        /// </summary>
        [Parameter]
        public IEnumerable<CascaderNode> Options
        {
            get => _nodelist ?? Enumerable.Empty<CascaderNode>();
            set
            {
                if (value == null && _nodelist == null) return;
                if (value != null && _nodelist?.SequenceEqual(value) == true) return;

                _nodelist = _nodelist ?? new List<CascaderNode>();
                _nodelist?.Clear();
                _searchList?.Clear();
                if (value != null)
                {
                    _nodelist.AddRange(value);
                }
                ProcessParentAndDefault();
            }
        }

        /// <summary>
        /// Child content to be rendered inside the <see cref="Cascader"/>.
        /// </summary>
        [Parameter]
        [PublicApi("1.2.0")]
        public RenderFragment ChildContent { get; set; }

        protected override RenderFragment TriggerContent => ChildContent;

        private List<CascaderNode> _nodelist = new List<CascaderNode>();
        private List<CascaderNode> _selectedNodes = new List<CascaderNode>();
        private List<CascaderNode> _hoverSelectedNodes = new List<CascaderNode>();
        private List<CascaderNode> _renderNodes = new List<CascaderNode>();
        private readonly List<CascaderNode> _searchList = new List<CascaderNode>();
        private IEnumerable<CascaderNode> _matchList;

        private readonly ClassMapper _menuClassMapper = new ClassMapper();
        private readonly ClassMapper _inputClassMapper = new ClassMapper();

        private readonly EventCallbackFactory _callbackFactory = new EventCallbackFactory();

        private bool _dropdownOpened;
        private bool _isOnCascader;
        private SelectedTypeEnum _selectedType;

        private bool _showClearIcon;
        private string _displayText;
        private bool _initialized;
        private string _searchValue;
        private ElementReference _inputRef;

        private bool _focused;

        protected override string OverlayClassName => "ant-cascader-dropdown";

        protected override string DefaultWidth => "";

        private Trigger[] OpenTriggers => ExpandTrigger switch
        {
            _ when Disabled => [],
            "hover" => [Trigger.Hover],
            _ => [Trigger.Click]
        };

        private static Dictionary<InputSize, string> _sizeMap = new Dictionary<InputSize, string>()
        {
            [InputSize.Large] = "lg",
            [InputSize.Small] = "sm"
        };

        protected override void OnInitialized()
        {
            base.OnInitialized();
            
            SetDefaultValue(Value ?? DefaultValue);
        }

        protected override void OnValueChange(string value)
        {
            base.OnValueChange(value);
            RefreshNodeValue(value);
            SetValue(value);
        }

        /// <summary>
        /// 输入框单击(显示/隐藏浮层)
        /// </summary>
        private void InputOnToggle()
        {
            if (Disabled)
                return;

            _selectedType = SelectedTypeEnum.Click;
            _hoverSelectedNodes.Clear();
            _focused = true;
        }

        /// <summary>
        /// 输入框/浮层失去焦点(隐藏浮层)
        /// </summary>
        private void CascaderOnBlur()
        {
            if (!_isOnCascader)
            {
                _renderNodes = _selectedNodes;
                _searchValue = string.Empty;
            }

            _focused = false;
        }

        /// <summary>
        /// 清除已选择项
        /// </summary>
        private async Task ClearSelected()
        {
            _selectedNodes.Clear();
            _hoverSelectedNodes.Clear();
            _displayText = string.Empty;
            SetValue(string.Empty);
            _ = CloseAsync();
            _searchValue = string.Empty;
            await this.FocusAsync(_inputRef);
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

        private void OnSearchKeyUp(KeyboardEventArgs e)
        {
            if (string.IsNullOrEmpty(_searchValue))
            {
                _showClearIcon = false;
                return;
            }

            _matchList = _searchList.Where(x => x.Label.Contains(_searchValue, StringComparison.OrdinalIgnoreCase));
            _showClearIcon = true;
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
                var index = _selectedNodes.FindIndex(x => x.Value == cascaderNode.Value);
                if (index != -1)
                {
                    _selectedNodes.RemoveRange(index, _selectedNodes.Count - index);
                }
                else
                {
                    _selectedNodes.RemoveAll(x => x.Level >= cascaderNode.Level);
                    _selectedNodes.Add(cascaderNode);
                }

                _renderNodes = _selectedNodes;

                if (ChangeOnSelect || !cascaderNode.HasChildren)
                {
                    SetValue(cascaderNode.Value);
                }
            }
            else
            {
                var index = _hoverSelectedNodes.FindIndex(x => x.Value == cascaderNode.Value);

                if (index != -1)
                {
                    _hoverSelectedNodes.RemoveRange(index, _hoverSelectedNodes.Count - index);
                }
                else
                {
                    _hoverSelectedNodes.RemoveAll(x => x.Level >= cascaderNode.Level);
                    _hoverSelectedNodes.Add(cascaderNode);
                }

                _renderNodes = _hoverSelectedNodes;
            }
            _renderNodes.Sort((x, y) => x.Level.CompareTo(y.Level));  //Level 升序排序

            if (!cascaderNode.HasChildren)
            {
                CloseAsync();
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
        /// <param name="recursive"></param>
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

        protected override void SetClassMap()
        {
            string prefixCls = "ant-cascader";
            string selectCls = "ant-select";

            ClassMapper
                .Add("ant-select ant-cascader ant-select-single ant-select-show-arrow")
                .Add($"{prefixCls}-picker")
                .GetIf(() => $"{prefixCls}-picker-{Size}", () => _sizeMap.ContainsKey(Size))
                .If("ant-select-open", () => _dropdownOpened)
                .If("ant-select-focused", () => _focused)
                .If($"{prefixCls}-picker-show-search ant-select-show-search", () => ShowSearch)
                .If($"{prefixCls}-picker-with-value", () => !string.IsNullOrEmpty(_searchValue))
                .If($"ant-select-lg", () => Size == InputSize.Large)
                .If($"ant-select-sm", () => Size == InputSize.Small)
                .If($"ant-select-disabled", () => Disabled)
                .If("ant-select-allow-clear ", () => AllowClear)
                .If($"{selectCls}-in-form-item ", () => FormItem != null)
                .If($"{selectCls}-has-feedback", () => FormItem?.HasFeedback == true)
                .GetIf(() => $"{selectCls}-status-{FormItem?.ValidateStatus.ToString().ToLowerInvariant()}", () => FormItem is { ValidateStatus: not FormValidateStatus.Default })
                .If($"{prefixCls}-picker-rtl", () => RTL);

            _inputClassMapper
                .Add("ant-input")
                .GetIf(() => $"ant-input-{_sizeMap[Size]}", () => _sizeMap.ContainsKey(Size))
                .Add($"{prefixCls}-input")
                .If($"{prefixCls}-input-rtl", () => RTL);

            _menuClassMapper
                .Add($"{prefixCls}-menu")
                .If($"{prefixCls}-menu-rtl", () => RTL);
        }

        protected override async Task OnOverlayVisibleChangeAsync(bool visible)
        {
            _dropdownOpened = visible;

            if (OpenChanged.HasDelegate)
            {
                await OpenChanged.InvokeAsync(visible);
            }

            if (visible)
            {
                InputOnToggle();
            }
            else
            {
                CascaderOnBlur();
            }
        }

        protected override Task OnInputAsync(ChangeEventArgs e)
        {
            return Task.CompletedTask;
        }

        protected override async Task OnKeyUpAsync(KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case "ArrowUp":
                    ActivePrevNode();
                    break;
                case "ArrowDown":
                    ActiveNextNode();
                    break;
                case "ArrowLeft":
                    ActiveParentNode();
                    break;
                case "ArrowRight":
                    ActiveChildNode();
                    break;
                default:
                    OnSearchKeyUp(e);
                    break;
            }

        }

        private void ActiveNextNode()
        {
            if (!_dropdownOpened)
            {
                _ = OpenAsync();
                return;
            }
            var node = _renderNodes.LastOrDefault();
            if (node == null)
            {
                SetSelectedNode(_nodelist.FirstOrDefault(), SelectedTypeEnum.Click);
                return;
            }

            var siblingNodes = (node.ParentNode == null ? _nodelist.Where(x => x.ParentNode == null) : node.ParentNode.Children).ToList();
            var currentIndex = siblingNodes.FindIndex(x => x.Value == node.Value);

            var nextIndex = currentIndex + 1 > siblingNodes.Count - 1 ? 0 : currentIndex + 1;
            var nextNode = siblingNodes.ElementAt(nextIndex);
            SetSelectedNode(nextNode, SelectedTypeEnum.Click);
        }

        private void ActivePrevNode()
        {
            var node = _renderNodes.LastOrDefault();
            if (node == null)
            {
                SetSelectedNode(_nodelist.FirstOrDefault(), SelectedTypeEnum.Click);
                return;
            }

            var siblingNodes = (node.ParentNode == null ? _nodelist.Where(x => x.ParentNode == null) : node.ParentNode.Children).ToList();
            var currentIndex = siblingNodes.FindIndex(x => x.Value == node.Value);

            var prevIndex = currentIndex - 1 < 0 ? siblingNodes.Count - 1 : currentIndex - 1;
            var prevNode = siblingNodes.ElementAt(prevIndex);
            SetSelectedNode(prevNode, SelectedTypeEnum.Click);
        }

        private void ActiveChildNode()
        {
            var node = _renderNodes.LastOrDefault();
            if (node == null)
            {
                SetSelectedNode(_nodelist.FirstOrDefault(), SelectedTypeEnum.Click);
                return;
            }

            if (node != null && node.HasChildren)
            {
                var childNode = node.Children.FirstOrDefault();
                SetSelectedNode(childNode, SelectedTypeEnum.Click);
            }
        }

        private void ActiveParentNode()
        {
            var node = _renderNodes.LastOrDefault();
            if (node == null)
            {
                SetSelectedNode(_nodelist.FirstOrDefault(), SelectedTypeEnum.Click);
                return;
            }

            if (node != null)
            {
                SetSelectedNode(node, SelectedTypeEnum.Click);
            }
        }
    }
}
