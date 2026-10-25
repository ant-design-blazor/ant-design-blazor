// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.Select.Internal;
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
        /// <summary>Whether to enable checkbox-based multiple selection.</summary>
        [Parameter]
        public bool Multiple { get; set; }

        /// <summary>Determines which checked nodes are rendered as tags in multiple mode.</summary>
        [Parameter]
        public CascaderCheckedStrategy ShowCheckedStrategy { get; set; } = CascaderCheckedStrategy.ShowParent;

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
        /// Set the validation status of the selector.
        /// </summary>
        [Parameter]
        public FormValidateStatus Status { get; set; } = FormValidateStatus.Default;

        /// <summary>
        /// Visual style of the selector.
        /// </summary>
        [Parameter]
        public InputVariant Variant { get; set; } = InputVariant.Outlined;

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
        /// Loads the children of a node on demand. The callback receives the selected path;
        /// add the loaded nodes to the last node's <see cref="CascaderNode.Children"/>.
        /// </summary>
        [Parameter]
        public EventCallback<CascaderNode[]> LoadData { get; set; }

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
        private readonly List<CascaderNode> _checkedLeafNodes = new();
        private readonly Dictionary<string, SelectOptionItem<string, string>> _triggerItems = new();
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
        private bool _focused;

        protected override string OverlayClassName => GetSemanticClass("popup.root", "ant-cascader-dropdown ant-select-css-var ant-cascader-css-var");

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

            if (Multiple)
            {
                Mode = SelectMode.Multiple;
                ApplyCheckedValues(Values ?? DefaultValues);
            }
            else
            {
                SetDefaultValue(Value ?? DefaultValue);
            }
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            var multipleChanged = parameters.IsParameterChanged(nameof(Multiple), Multiple);
            var valuesChanged = parameters.IsParameterChanged(nameof(Values), Values);
            await base.SetParametersAsync(parameters);

            if (Multiple)
            {
                Mode = SelectMode.Multiple;
                if (_nodelist.Count > 0 && (multipleChanged || valuesChanged))
                    ApplyCheckedValues(Values ?? DefaultValues);
            }
            else if (multipleChanged && Mode == SelectMode.Multiple)
            {
                Mode = SelectMode.Default;
                if (_nodelist.Count > 0)
                    SetDefaultValue(Value ?? DefaultValue);
            }
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
            if (Multiple)
            {
                ApplyCheckedValues(Array.Empty<string>());
                await NotifyMultipleValueChanged();
                _searchValue = string.Empty;
                await this.FocusAsync(_inputRef);
                return;
            }

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
        private async Task NodeOnClickAsync(CascaderNode node)
        {
            if (node.Disabled) return;
            _searchValue = string.Empty;
            if (Multiple)
            {
                if (node.HasChildren)
                    SetSelectedNode(node, SelectedTypeEnum.Click);
                return;
            }
            SetSelectedNode(node, SelectedTypeEnum.Click);

            if (LoadData.HasDelegate && node.IsLeaf == false && node.Children?.Any() != true)
            {
                node.IsLoading = true;
                await InvokeAsync(StateHasChanged);
                try
                {
                    await LoadData.InvokeAsync(GetNodePath(node));
                    if (node.Children?.Any() != true)
                    {
                        node.IsLeaf = true;
                    }
                    _searchList.Clear();
                    InitCascaderNodeState(_nodelist, null, 0);
                }
                finally
                {
                    node.IsLoading = false;
                    RefreshComponentState();
                }
            }
        }

        private CascaderNode[] GetNodePath(CascaderNode node)
        {
            var path = new List<CascaderNode>();
            SetSelectedNodeWithParent(node, ref path);
            path.Reverse();
            return path.ToArray();
        }

        private async Task NodeCheckboxChanged(CascaderNode node, bool isChecked)
        {
            if (node.Disabled || node.DisableCheckbox)
                return;

            SetNodeChecked(node, isChecked);
            await InvokeAsync(StateHasChanged);
            RefreshComponentState();
        }

        internal RenderFragment RenderCheckbox(CascaderNode node, bool isChecked, bool isIndeterminate) => builder =>
        {
            builder.OpenComponent<CascadingValue<string>>(0);
            builder.AddAttribute(1, nameof(CascadingValue<string>.Name), "PrefixCls");
            builder.AddAttribute(2, nameof(CascadingValue<string>.Value), "ant-cascader-checkbox");
            builder.AddAttribute(3, nameof(CascadingValue<string>.IsFixed), true);
            builder.AddAttribute(4, nameof(CascadingValue<string>.ChildContent), (RenderFragment)(checkboxBuilder =>
            {
                checkboxBuilder.OpenComponent<Checkbox>(0);
                checkboxBuilder.AddAttribute(1, nameof(Checkbox.Checked), isChecked);
                checkboxBuilder.AddAttribute(2, nameof(Checkbox.Indeterminate), isIndeterminate);
                checkboxBuilder.AddAttribute(3, nameof(Checkbox.Disabled), node.Disabled || node.DisableCheckbox);
                checkboxBuilder.AddAttribute(4, nameof(Checkbox.CheckedChanged), EventCallback.Factory.Create<bool>(this, value => NodeCheckboxChanged(node, value)));
                checkboxBuilder.CloseComponent();
            }));
            builder.CloseComponent();
        };

        private void SetNodeChecked(CascaderNode node, bool isChecked)
        {
            var leaves = GetCheckableLeaves(node).ToArray();
            if (leaves.Length == 0)
                return;

            var checkedValues = new HashSet<string>(Values ?? Enumerable.Empty<string>());
            foreach (var leaf in leaves)
            {
                if (isChecked) checkedValues.Add(leaf.Value);
                else checkedValues.Remove(leaf.Value);
            }
            ApplyCheckedValues(checkedValues);
            _ = NotifyMultipleValueChanged();
        }

        private async Task NotifyMultipleValueChanged()
        {
            var values = Values?.ToArray() ?? Array.Empty<string>();
            if (ValuesChanged.HasDelegate)
                await ValuesChanged.InvokeAsync(values);
            if (_initialized && SelectedNodesChanged.HasDelegate)
                await SelectedNodesChanged.InvokeAsync(_checkedLeafNodes.ToArray());
            OnChange?.Invoke(_checkedLeafNodes, string.Join(",", values), string.Join(", ", DisplayCheckedNodes.Select(x => x.Label)));
        }

        private IEnumerable<CascaderNode> GetCheckableLeaves(CascaderNode node)
        {
            if (node == null || node.Disabled)
                yield break;
            if (!node.HasChildren)
            {
                if (!node.DisableCheckbox)
                    yield return node;
                yield break;
            }
            foreach (var child in node.Children)
                foreach (var leaf in GetCheckableLeaves(child))
                    yield return leaf;
        }

        private IEnumerable<CascaderNode> GetCheckableLeavesFrom(IEnumerable<CascaderNode> nodes)
        {
            foreach (var node in nodes ?? Enumerable.Empty<CascaderNode>())
                foreach (var leaf in GetCheckableLeaves(node))
                    yield return leaf;
        }

        private bool IsNodeChecked(CascaderNode node)
        {
            var leaves = GetCheckableLeaves(node).ToArray();
            return leaves.Length > 0 && leaves.All(x => _checkedLeafNodes.Any(y => y.Value == x.Value));
        }

        private bool IsNodeIndeterminate(CascaderNode node)
        {
            var leaves = GetCheckableLeaves(node).ToArray();
            return leaves.Any(x => _checkedLeafNodes.Any(y => y.Value == x.Value)) && !leaves.All(x => _checkedLeafNodes.Any(y => y.Value == x.Value));
        }

        private IEnumerable<CascaderNode> DisplayCheckedNodes => ShowCheckedStrategy switch
        {
            CascaderCheckedStrategy.ShowAll => GetAllCheckedNodes(_nodelist),
            CascaderCheckedStrategy.ShowParent => GetParentCheckedNodes(_nodelist),
            _ => _checkedLeafNodes
        };

        private IEnumerable<CascaderNode> GetAllCheckedNodes(IEnumerable<CascaderNode> nodes)
        {
            foreach (var node in nodes ?? Enumerable.Empty<CascaderNode>())
            {
                if (IsNodeChecked(node)) yield return node;
                foreach (var child in GetAllCheckedNodes(node.Children)) yield return child;
            }
        }

        private IEnumerable<CascaderNode> GetParentCheckedNodes(IEnumerable<CascaderNode> nodes)
        {
            foreach (var node in nodes ?? Enumerable.Empty<CascaderNode>())
            {
                if (IsNodeChecked(node)) yield return node;
                else
                    foreach (var child in GetParentCheckedNodes(node.Children)) yield return child;
            }
        }

        internal override bool HasValue => Multiple ? _checkedLeafNodes.Count > 0 : _selectedNodes.Count > 0;

        internal override List<SelectOptionItem<string, string>> GetOrderedSelectedItems()
        {
            var nodes = Multiple ? DisplayCheckedNodes : _selectedNodes.LastOrDefault() is { } selected ? [selected] : [];
            var activeValues = new HashSet<string>();
            var items = new List<SelectOptionItem<string, string>>();
            foreach (var node in nodes)
            {
                activeValues.Add(node.Value);
                if (!_triggerItems.TryGetValue(node.Value, out var item))
                    _triggerItems[node.Value] = item = new SelectOptionItem<string, string> { Value = node.Value, Item = node.Value };
                item.Label = Multiple ? node.Label : string.Join(" / ", _selectedNodes.OrderBy(x => x.Level).Select(x => x.Label));
                item.IsSelected = true;
                items.Add(item);
            }
            foreach (var stale in _triggerItems.Keys.Where(key => !activeValues.Contains(key)).ToArray())
                _triggerItems.Remove(stale);
            return items;
        }

        private void ApplyCheckedValues(IEnumerable<string> values)
        {
            var selected = new HashSet<string>(values ?? Enumerable.Empty<string>());
            _checkedLeafNodes.Clear();
            _checkedLeafNodes.AddRange(GetCheckableLeavesFrom(_nodelist).Where(x => selected.Contains(x.Value)));
            base.Values = _checkedLeafNodes.Select(x => x.Value).ToArray();
        }

        internal override string GetSelectedItemLabel(SelectOptionItem<string, string> option) => option.Label;

        protected override async Task OnInputClearClickAsync(MouseEventArgs args)
        {
            if (Multiple)
                await ClearSelected();
            else
                await base.OnInputClearClickAsync(args);
        }

        protected override async Task OnRemoveSelectedAsync(SelectOptionItem<string, string> option)
        {
            if (!Multiple)
            {
                await base.OnRemoveSelectedAsync(option);
                return;
            }
            var node = GetNodeByValue(_nodelist, option.Value);
            if (node == null) return;
            var selected = new HashSet<string>(Values ?? Enumerable.Empty<string>());
            foreach (var leaf in GetCheckableLeaves(node)) selected.Remove(leaf.Value);
            ApplyCheckedValues(selected);
            await NotifyMultipleValueChanged();
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

                if (ChangeOnSelect || !cascaderNode.IsExpandable)
                {
                    SetValue(cascaderNode.Value);
                }
            }
            else
            {
                var index = _hoverSelectedNodes.FindIndex(x => x.Value == cascaderNode.Value);

                if (index != -1)
                {
                    //remove nodes after the hovered node while keeping the hovered node active
                    var firstNodeToRemoveIndex = index + 1;
                    var nodesToRemoveCount = _hoverSelectedNodes.Count - firstNodeToRemoveIndex;

                    if (nodesToRemoveCount > 0)//dont remove when empty
                    {
                        _hoverSelectedNodes.RemoveRange(firstNodeToRemoveIndex, nodesToRemoveCount);
                    }
                }
                else
                {
                    _hoverSelectedNodes.RemoveAll(x => x.Level >= cascaderNode.Level);
                    _hoverSelectedNodes.Add(cascaderNode);
                }

                _renderNodes = _hoverSelectedNodes;
            }
            _renderNodes.Sort((x, y) => x.Level.CompareTo(y.Level));  //Level 升序排序

#if NET10_0_OR_GREATER
            // .NET 10: Refresh overlay to detect _renderNodes changes
            RefreshComponentState();
#endif

            if (!cascaderNode.IsExpandable)
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
            if (Multiple)
                ApplyCheckedValues(Values ?? DefaultValues);
            else
                SetDefaultValue(Value ?? DefaultValue);
        }

        private static string GetPathKey(CascaderNode node)
        {
            var values = new Stack<string>();
            for (var current = node; current != null; current = current.ParentNode)
            {
                values.Push(current.Value);
            }

            return string.Join("__RC_CASCADER_SPLIT__", values);
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
                .Add("ant-select ant-cascader ant-select-show-arrow")
                .Add($"{selectCls}-{Variant.ToString().ToLowerInvariant()}")
                .GetIf(() => "ant-select-multiple", () => Multiple)
                .GetIf(() => "ant-select-single", () => !Multiple)
                .Add("ant-select-css-var ant-cascader-css-var")
                .If("ant-select-open", () => _dropdownOpened)
                .If("ant-select-focused", () => _focused)
                .If("ant-select-show-search", () => ShowSearch)
                .If($"ant-select-lg", () => Size == InputSize.Large)
                .If($"ant-select-sm", () => Size == InputSize.Small)
                .If($"ant-select-disabled", () => Disabled)
                .If("ant-select-allow-clear", () => AllowClear)
                .If($"{selectCls}-in-form-item ", () => FormItem != null)
                .If($"{selectCls}-has-feedback", () => FormItem?.HasFeedback == true)
                .GetIf(() => $"{selectCls}-status-{GetValidationStatus().ToString().ToLowerInvariant()}", () => GetValidationStatus() != FormValidateStatus.Default)
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

        private FormValidateStatus GetValidationStatus()
        {
            return Status != FormValidateStatus.Default ? Status : FormItem?.ValidateStatus ?? FormValidateStatus.Default;
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
            _searchValue = e.Value?.ToString();
            if (!string.IsNullOrEmpty(_searchValue))
            {
                _matchList = _searchList.Where(x => x.Label.Contains(_searchValue, StringComparison.OrdinalIgnoreCase));
                _showClearIcon = true;
            }
            else
            {
                _matchList = null;
                _showClearIcon = false;
            }
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
