// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.Core.Extensions;
using AntDesign.JsInterop;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Tree selection control.</para>

    <h2>When To Use</h2>

    <para>
        <c>TreeSelect</c> is similar to <c>Select</c>, but the values are provided in a tree like structure. 
        Any data whose entries are defined in a hierarchical manner is fit to use this control. 
        Examples of such case may include a corporate hierarchy, a directory structure, and so on.
    </para>
    </summary>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/Ax4DA0njr/TreeSelect.svg", Title = "TreeSelect", SubTitle = "树选择")]
#if NET6_0_OR_GREATER
    [CascadingTypeParameter(nameof(TItem))]
    [CascadingTypeParameter(nameof(TItemValue))]
#endif

    public partial class TreeSelect<TItemValue, TItem> : SelectBase<TItemValue, TItem>, ITreeSelect
    {
        /// <summary>
        /// Whether to allow multiple selections or not
        /// </summary>
        /// <default value="false unless TreeCheckable is true"/>
        [Parameter]
        public bool Multiple
        {
            get => _multiple;
            set
            {
                _multiple = value;
                if (_multiple)
                {
                    Mode = SelectMode.Multiple;
                }
            }
        }

        /// <summary>
        /// Whether tree nodes are able to be selected or not, which would select all leafs under that node.
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool TreeCheckable
        {
            get => _treeCheckable;
            set
            {
                _treeCheckable = value;
                if (_treeCheckable)
                {
                    Mode = SelectMode.Multiple;
                }
            }
        }

        /// <summary>
        /// Check treeNode precisely; parent treeNode and children treeNodes are not associated
        /// </summary>
        [Parameter]
        public bool TreeCheckStrictly { get; set; }

        /// <summary>
        /// Specify how to show checked values when TreeCheckable is true and TreeCheckStrictly is false 
        /// </summary>
        [Parameter]
        public TreeCheckedStrategy ShowCheckedStrategy { get; set; } = TreeCheckedStrategy.ShowChild;

        /// <summary>
        /// Whether to check the checkbox when user click the tree node.  
        /// </summary>
        [Parameter] public bool CheckOnClickNode { get; set; } = true;

        /// <summary>
        /// Callback executed when the component looses focus
        /// </summary>
        [Parameter]
        public Action OnBlur { get; set; }

        /// <summary>
        /// Get or set the template to render the title of the tree node
        /// </summary>
        [Parameter] public RenderFragment<TreeNode<TItem>> TitleTemplate { get; set; }

        /// <summary>
        ///  Customize the icon templates
        /// </summary>
        [Parameter]
        public RenderFragment<TreeNode<TItem>> TitleIconTemplate { get; set; }

        /// <summary>
        /// Nodes to render in the tree. Use either this or <see cref="DataSource"/>
        /// </summary>
        [Parameter] public TreeNode<TItem>[] Nodes { get; set; }

        /// <summary>
        /// Datasource for the tree. Can be a list of any custom object type by providing the expressions to get children, leafs, titles, etc. Use either this or <see cref="ChildContent"/>
        /// </summary>
        [Parameter]
        public IEnumerable<TItem> DataSource { get; set; }

        /// <summary>
        /// Whether to expand all nodes by default
        /// </summary>
        [Parameter] public bool TreeDefaultExpandAll { get; set; }

        /// <summary>
        /// Whether to expand parent nodes by default
        /// </summary>
        [Parameter]
        public bool TreeDefaultExpandParent { get; set; }

        /// <summary>
        /// Set the keys of the default expanded tree nodes
        /// </summary>
        [Parameter]
        public string[] TreeDefaultExpandedKeys { get; set; }

        /// <summary>
        /// Whether to expand the parent node when clicking on a tree node
        /// </summary>
        [Parameter] public bool ExpandOnClickNode { get; set; } = false;

        /// <inheritdoc cref="Tree{TItem}.SearchExpression"/>
        [Parameter] public Func<TreeNode<TItem>, bool> SearchExpression { get; set; }

        /// <summary>
        /// Callback executed when the user searches for a value
        /// </summary>
        [Parameter] public EventCallback<string> OnSearch { get; set; }

        /// <summary>
        /// Set the style of the matched text
        /// </summary>
        [Parameter] public string MatchedStyle { get; set; } = string.Empty;

        /// <summary>
        /// Set the class of the matched text
        /// </summary>
        [Parameter] public string MatchedClass { get; set; }

        /// <summary>
        /// The value of the root node
        /// </summary>
        /// <default value="0"/>
        [Parameter] public string RootValue { get; set; } = "0";

        /// <summary>
        /// Determine whether the dropdown menu and the select input are the same width. Default set min-width same as input. Will ignore when value less than select width. false will disable virtual scroll
        /// </summary>
        [Parameter]
        public OneOf<bool, string> DropdownMatchSelectWidth { get; set; } = true;

        /// <summary>
        /// Maximum width of the dropdown
        /// </summary>
        /// <default value="auto"/>
        [Parameter] public string DropdownMaxWidth { get; set; } = "auto";

        /// <summary>
        /// Maximum height of the dropdown
        /// </summary>
        /// <default value="256px"/>
        [Parameter] public string PopupContainerMaxHeight { get; set; } = "256px";

        /// <summary>
        /// show treeNode icon icon
        /// </summary>
        [Parameter]
        public bool ShowIcon { get; set; }

        /// <summary>
        /// Whether to show leaf icon
        /// </summary>
        [Parameter] public bool ShowLeafIcon { get; set; }

        /// <summary>
        /// Set the attributes of the tree
        /// </summary>
        [Parameter] public IDictionary<string, object> TreeAttributes { get; set; }

        /// <summary>
        /// Callback executed when the tree node is loaded
        /// </summary>
        [Parameter] public EventCallback<TreeEventArgs<TItem>> OnNodeLoadDelayAsync { get; set; }

        /// <summary>
        /// Callback executed when the tree node is selected
        /// </summary>
        [Parameter]
        public EventCallback<TreeEventArgs<TItem>> OnTreeNodeSelect { get; set; }

        /// <inheritdoc cref="Tree{TItem}.TitleExpression"/>
        [Parameter]
        public Func<TreeNode<TItem>, string> TitleExpression { get; set; }

        /// <summary>
        /// Set the style of the dropdown menu
        /// </summary>
        [Parameter]
        public string DropdownStyle { get; set; }

        /// <summary>
        /// Whether to show lines in the tree or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool ShowTreeLine { get; set; }

        /// <inheritdoc cref="Tree{TItem}.KeyExpression"/>
        [Parameter]
        public Func<TreeNode<TItem>, string> KeyExpression { get; set; }

        /// <inheritdoc cref="Tree{TItem}.IconExpression"/>
        [Parameter]
        public Func<TreeNode<TItem>, string> IconExpression { get; set; }

        /// <inheritdoc cref="Tree{TItem}.IsLeafExpression"/>
        [Parameter]
        public Func<TreeNode<TItem>, bool> IsLeafExpression { get; set; }

        /// <inheritdoc cref="Tree{TItem}.ChildrenExpression"/>
        [Parameter]
        public Func<TreeNode<TItem>, IEnumerable<TItem>> ChildrenExpression { get; set; }

        /// <inheritdoc cref="Tree{TItem}.DisabledExpression"/>
        [Parameter]
        public Func<TreeNode<TItem>, bool> DisabledExpression { get; set; }

        /// <summary>
        /// Specifies a method to return a checkable node
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, bool> CheckableExpression { get; set; }

        /// <summary>
        /// Specifies a method to return a selectable node
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, bool> SelectableExpression { get; set; }

        /// <summary>
        /// (Controlled) expands the specified tree node
        /// </summary>
        [Parameter]
        public string[] ExpandedKeys { get; set; }

        /// <summary>
        /// Render fragment to customize the nodes in the tree.
        /// </summary>
        [Parameter]
        [PublicApi("1.2.0")]
        public RenderFragment ChildContent { get; set; }

        private bool IsMultiple => Multiple || TreeCheckable;

        private bool IsTemplatedNodes => ChildContent != null;

        private bool _multiple;
        private bool _treeCheckable;
        private readonly string _dir = "ltr";
        private Tree<TItem> _tree;
        private bool _checkedEventDisabled = false;

        protected override bool UseChildContentAsTrigger => false;

        /// <summary>
        /// 
        /// </summary>
        internal Tree<TItem> TreeComponent { get => _tree; }

        private TItemValue _cachedValue;

        /// <summary>
        /// The selected value
        /// </summary>
        [Parameter]
        public override TItemValue Value
        {
            get => base.Value;
            set
            {
                if (_cachedValue.AllNullOrEquals(value))
                    return;

                _cachedValue = value;

                UpdateValueAndSelection();
            }
        }

        private TItemValue[] _cachedValues;
        private List<TItemValue> _newValues = [];

        /// <summary>
        /// The selected values
        /// </summary>
        [Parameter]
        public override IEnumerable<TItemValue> Values
        {
            get => base.Values;
            set
            {
                if (value != null && _cachedValues != null)
                {
                    var hasChanged = !value.SequenceEqual(_cachedValues);

                    if (!hasChanged)
                        return;

                    _cachedValues = value.ToArray();
                }
                else if (value != null && _cachedValues == null)
                {
                    _cachedValues = value.ToArray();
                }
                else if (value == null && _cachedValues != null)
                {
                    _cachedValues = default;
                    ClearOptions();
                }

                _cachedValues ??= [];

                UpdateValuesSelection();
            }
        }

        /// <summary>
        /// Check all nodes
        /// </summary>
        [PublicApi("1.0.0")]
        public void CheckAll()
        {
            TreeComponent?.CheckAll();
        }

        /// <summary>
        /// Uncheck all nodes
        /// </summary>
        [PublicApi("1.0.0")]
        public void UncheckAll()
        {
            TreeComponent?.UncheckAll();
        }

        /// <summary>
        /// Select all nodes
        /// </summary>
        [PublicApi("1.0.0")]
        public void SelectAll()
        {
            TreeComponent?.SelectAll();
        }

        /// <summary>
        /// Deselect all nodes
        /// </summary>
        [PublicApi("1.0.0")]
        public void DeselectAll()
        {
            TreeComponent?.DeselectAll();
        }

        /// <summary>
        /// Expand all nodes
        /// </summary>
        [PublicApi("1.0.0")]
        public void ExpandAll(Func<TreeNode<TItem>, bool> predicate = null, bool recursive = true)
        {
            TreeComponent?.ExpandAll(predicate, recursive);
        }

        /// <summary>
        /// Collapse all nodes
        /// </summary>
        [PublicApi("1.0.0")]
        public void CollapseAll(Func<TreeNode<TItem>, bool> predicate = null, bool recursive = true)
        {
            TreeComponent?.CollapseAll(predicate, recursive);
        }

        /// <summary>
        /// Get TreeNode by Key
        /// </summary>
        /// <param name="key">Key</param>
        [PublicApi("1.0.0")]
        public TreeNode<TItem> GetNode(TItemValue key)
        {
            return TreeComponent?.GetNode(GetTreeKeyFormValue(key));
        }

        private void ClearOptions()
        {
            SelectOptionItems.Clear();
            SelectedOptionItems.Clear();
            if (TreeCheckable)
                _tree?._allNodes.ForEach(x => x.SetChecked(false));
            else
                _tree?._allNodes.ForEach(x => x.SetSelected(false));
        }

        private void CreateOptions(IEnumerable<TItemValue> values)
        {
            values.ForEach(value =>
            {
                var d = _tree?._allNodes.FirstOrDefault(m => GetValueFromNode(m).Equals(value));
                if (d != null)
                {
                    _ = CreateOption(d, true);
                }
            });
        }

        private SelectOptionItem<TItemValue, TItem> CreateOption(TreeNode<TItem> node, bool append = false)
        {
            if (!TreeCheckable || TreeCheckStrictly || (ShowCheckedStrategy == TreeCheckedStrategy.ShowAll)
                    || ((ShowCheckedStrategy == TreeCheckedStrategy.ShowParent) && ((node.ParentNode == null) || (!_cachedValues.Contains(_getValue(node.ParentNode.DataItem)))))
                    || ((ShowCheckedStrategy == TreeCheckedStrategy.ShowChild) && !node.HasChildNodes))
            {
                var o = new SelectOptionItem<TItemValue, TItem>()
                {
                    Label = node.Title,
                    LabelTemplate = node.TitleTemplate,
                    Value = GetValueFromNode(node),
                    Item = node.DataItem,
                    IsAddedTag = Mode != SelectMode.Default,
                };
                if (append && !SelectOptionItems.Any(m => m.Value.AllNullOrEquals(o.Value)))
                    SelectOptionItems.Add(o);
                if (IsMultiple)
                {
                    _newValues.Add(o.Value);
                }
                return o;
            }
            return null;
        }

        protected override Task OnFirstAfterRenderAsync()
        {
            if (_cachedValue != null)
            {
                UpdateValueAndSelection();
            }

            if (_cachedValues != null)
            {
                UpdateValuesSelection();
            }

            return base.OnFirstAfterRenderAsync();
        }

        protected override async Task OnInputAsync(ChangeEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!IsSearchEnabled)
            {
                return;
            }

            if (!_dropDown.IsOverlayShow())
            {
                await _dropDown.Show();
            }

            _prevSearchValue = _searchValue;

            if (OnSearch.HasDelegate)
                await OnSearch.InvokeAsync(e.Value?.ToString());

            _searchValue = e.Value?.ToString();
            StateHasChanged();
            FocusIfInSearch();
        }

        protected override async Task SetInputBlurAsync()
        {
            if (Focused)
            {
                Focused = false;

                SetClassMap();

                await JsInvokeAsync(JSInteropConstants.Blur, _inputRef);

                OnBlur?.Invoke();
            }
        }

        protected override async Task OnOverlayVisibleChangeAsync(bool visible)
        {
            if (visible)
            {
                OnOverlayShow();

                await SetDropdownStyleAsync();
                FocusIfInSearch();
            }
            else
            {
                OnOverlayHide();
                await SetInputBlurAsync();
            }
        }

        protected override async Task OnRemoveSelectedAsync(SelectOptionItem<TItemValue, TItem> selectOption)
        {
            if (selectOption == null) throw new ArgumentNullException(nameof(selectOption));
            await SetValueAsync(selectOption);
            var key = GetTreeKeyFormValue(selectOption.Value);
            var item = _tree?._allNodes.Where(x => x.Key == key).FirstOrDefault();
            if (item == null)
                return;
            if (TreeCheckable)
                item.SetChecked(false);
            else
                item.SetSelected(false);
            FocusIfInSearch();
        }

        private TItemValue GetValueFromNode(TreeNode<TItem> node)
        {
            if (node == null) return default;
            if (_getValue == null || node.DataItem == null)
            {
                return THelper.ChangeType<TItemValue>(node.Key);
            }
            return _getValue(node.DataItem);
        }

        private async Task DoTreeNodeClick(TreeEventArgs<TItem> args)
        {
            if (TreeCheckable)
                return;
            var node = args.Node;

            var key = node.Key;
            if (Multiple)
            {
                if (_tree.SelectedKeys is not { Length: > 0 })
                {
                    Values = [];
                    return;
                }
                var selectedNodes = _tree._allNodes.Where(x => _tree.SelectedKeys.Contains(x.Key));
                Values = selectedNodes.Select(GetValueFromNode).ToArray();
                if (IsSearchEnabled && AutoClearSearchValue && !string.IsNullOrWhiteSpace(_searchValue))
                {
                    ClearSearch();
                }
            }
            else
            {
                if (node.Selected)
                {
                    Value = GetValueFromNode(node);
                }
            }

            if (Mode == SelectMode.Default)
            {
                await CloseAsync();
            }
            else
            {
                FocusIfInSearch();
            }
        }


        private void DoTreeCheckedKeysChanged(string[] checkedKeys)
        {
            if (_checkedEventDisabled) return;
            if (!TreeCheckable)
                return;

            if (checkedKeys is not { Length: > 0 })
            {
                Values = [];
                return;
            }
            var checkedNodes = _tree._allNodes.Where(x => checkedKeys.Contains(x.Key));
            Values = checkedNodes.Select(GetValueFromNode).ToArray();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            bool checkStricklyChanged = parameters.IsParameterChanged("TreeCheckStrictly", TreeCheckStrictly);
            bool checkedStrategyChanged = parameters.IsParameterChanged("ShowCheckedStrategy", ShowCheckedStrategy);
            await base.SetParametersAsync(parameters);
            if ((Values != null) && TreeCheckable && (checkStricklyChanged || checkedStrategyChanged))
            {
                UpdateTreeCheckedKeys(Values.Select(x => GetTreeKeyFormValue(x)));
                var checkedNodes = _tree._allNodes.Where(x => _tree.CheckedKeys.Contains(x.Key));
                // Force to update values
                _cachedValues = [];
                Values = checkedNodes.Select(GetValueFromNode).ToArray();
            }
        }

        protected async Task SetDropdownStyleAsync()
        {
            string maxWidth = "", minWidth = "", definedWidth = "";
            var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, _selectContent.Ref);
            var width = domRect.Width.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            minWidth = $"min-width: {width}px;";
            if (DropdownMatchSelectWidth.IsT0 && DropdownMatchSelectWidth.AsT0)
            {
                definedWidth = $"width: {width}px;";
            }
            else if (DropdownMatchSelectWidth.IsT1)
            {
                definedWidth = $"width: {DropdownMatchSelectWidth.AsT1};";
            }
            if (!DropdownMaxWidth.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                maxWidth = $"max-width: {DropdownMaxWidth};";
            _dropdownStyle = minWidth + definedWidth + maxWidth + DropdownStyle ?? "";

            if (IsMultiple)
            {
                if (Values == null)
                    return;
                var checkedKeys = Values.Select(x => GetTreeKeyFormValue(x));
                if (TreeCheckable)
                {
                    UpdateTreeCheckedKeys(checkedKeys);
                }
                else
                {
                    _tree?._allNodes.ForEach(n => n.DoSelect(checkedKeys.Contains(n.Key), true, false));
                    _tree?.UpdateSelectedKeys();
                }
            }
            else
            {
                if (Value == null)
                    return;
                _tree?.FindFirstOrDefaultNode(node => node.Key == GetTreeKeyFormValue(Value))?.SetSelected(true);
            }
        }

        private void UpdateTreeCheckedKeys(IEnumerable<string> checkedKeys)
        {
            _checkedEventDisabled = true;
            _tree?._allNodes.ForEach(n => n.DoCheck(false, true, false));
            _tree?._allNodes.Where(n => checkedKeys.Contains(n.Key)).ForEach(n => n.DoCheck(true, false, false));
            _tree?.UpdateCheckedKeys();
            _checkedEventDisabled = false;
        }

        protected override void SetClassMap()
        {
            ClassMapper
                .Add("ant-tree-select")
                .Add($"{ClassPrefix}")
                .If($"{ClassPrefix}-open", () => _dropDown?.IsOverlayShow() ?? false)
                .If($"{ClassPrefix}-focused", () => Focused)
                .If($"{ClassPrefix}-single", () => Mode == SelectMode.Default)
                .If($"{ClassPrefix}-multiple", () => Mode != SelectMode.Default)
                .If($"{ClassPrefix}-sm", () => Size == InputSize.Small)
                .If($"{ClassPrefix}-lg", () => Size == InputSize.Large)
                .If($"{ClassPrefix}-show-arrow", () => ShowArrowIcon)
                .If($"{ClassPrefix}-show-search", () => IsSearchEnabled)
                .If($"{ClassPrefix}-loading", () => Loading)
                .If($"{ClassPrefix}-disabled", () => Disabled)
                .If($"{ClassPrefix}-rtl", () => RTL)
                .If($"{ClassPrefix}-allow-clear", () => AllowClear)
                ;
        }

        // bind the option once after fetching the data source asynchronously
        // fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/3446
        internal void UpdateValueAfterDataSourceChanged()
        {
            if (_cachedValue != null && !_cachedValue.Equals(Value))
            {
                UpdateValueAndSelection();
                StateHasChanged();
            }

            if (_cachedValues != null && Values != null && !_cachedValues.SequenceEqual(Values))
            {
                UpdateValuesSelection();
                StateHasChanged();
            }
        }

        private void UpdateValueAndSelection()
        {
            if (_tree == null)
                return;
            if (_tree._allNodes.Count == 0)
                return;

            if (_cachedValue == null)
            {
                ClearOptions();
            }
            else
            {
                if (SelectOptionItems.Any(o => o.Value.AllNullOrEquals(_cachedValue)))
                {
                    _ = SetValueAsync(SelectOptionItems.First(o => o.Value.AllNullOrEquals(_cachedValue)));
                }
                else
                {
                    var data = _tree?._allNodes.FirstOrDefault(x => x.Key == GetTreeKeyFormValue(_cachedValue));
                    if (data != null)
                    {
                        var o = CreateOption(data, true);
                        _ = SetValueAsync(o);
                    }
                    else
                    {
                        ClearOptions();
                    }
                }
            }
            base.Value = _cachedValue;
        }

        private void UpdateValuesSelection()
        {
            if (_tree == null)
                return;
            if (_tree._allNodes.Count == 0)
                return;

            if (_cachedValues?.Any() != true)
            {
                ClearOptions();
            }
            _newValues.Clear();
            CreateOptions(_cachedValues);
            base.Values = _newValues.ToArray();
            if (_isNotifyFieldChanged && (Form?.ValidateOnChange == true))
            {
                EditContext?.NotifyFieldChanged(FieldIdentifier);
            }
        }

        void ITreeSelect.UpdateValueAfterDataSourceChanged()
        {
            UpdateValueAfterDataSourceChanged();
        }

        private string GetTreeKeyFormValue(TItemValue value)
        {
            if (value is string key)
                return key;

            if (DataSource == null)
                return null;

            var node = _tree?._allNodes.Where(x => _getValue(x.DataItem).Equals(value)).FirstOrDefault();

            if (node == null)
                return null;

            KeyExpression ??= node => node.DataItem.ToString();

            return KeyExpression.Invoke(node);
        }
    }
}
