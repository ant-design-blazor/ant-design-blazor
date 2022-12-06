// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/Ax4DA0njr/TreeSelect.svg")]
    public partial class TreeSelect<TItem> : SelectBase<string, TItem> where TItem : class
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
                    Mode = SelectMode.Multiple.ToString("G");
                }
            }
        }

        /// <summary>
        /// Whether tree nodes are able to be selected or not, which would select all leafs under that node.
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool TreeCheckable { get; set; }

        /// <summary>
        /// Container for displaying the popup
        /// </summary>
        /// <default value="body"/>
        [Parameter]
        public string PopupContainerSelector { get; set; } = "body";

        /// <summary>
        /// Callback executed when the mouse enters the component
        /// </summary>
        [Parameter]
        public Action OnMouseEnter { get; set; }

        /// <summary>
        /// Callback executed when the mouse leaves the component
        /// </summary>
        [Parameter]
        public Action OnMouseLeave { get; set; }

        /// <summary>
        /// Callback executed when the component looses focus
        /// </summary>
        [Parameter]
        public Action OnBlur { get; set; }

        /// <summary>
        /// Content to display as label for the lead nodes
        /// </summary>
        [Parameter]
        public RenderFragment<TItem> LabelTemplate { get; set; }

        /// <inheritdoc cref="Tree{TItem}.TitleTemplate"/>
        [Parameter]
        public RenderFragment<TreeNode<TItem>> TitleTemplate { get; set; }

        /// <summary>
        /// Whether to show search icon or not
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool ShowSearchIcon { get; set; } = true;

        /// <summary>
        /// Whether to show arrow icons or not
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool ShowArrowIcon { get; set; } = true;

        /// <summary>
        /// Datasource for the tree. Can be a list of any custom object type by providing the expressions to get children, leafs, titles, etc. Use either this or <see cref="ChildContent"/>
        /// </summary>
        [Parameter]
        public IEnumerable<TItem> DataSource { get; set; }

        /// <summary>
        /// Nodes for the tree. Use either this or <see cref="DataSource"/>
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <inheritdoc cref="Tree{TItem}.DefaultExpandAll"/>
        [Parameter]
        public bool TreeDefaultExpandAll { get; set; }

        /// <inheritdoc cref="Tree{TItem}.SearchExpression"/>
        [Parameter]
        public Func<TreeNode<TItem>, bool> SearchExpression { get; set; }

        /// <inheritdoc cref="Tree{TItem}.MatchedStyle"/>
        [Parameter]
        public string MatchedStyle { get; set; } = string.Empty;

        /// <inheritdoc cref="Tree{TItem}.MatchedClass"/>
        [Parameter]
        public string MatchedClass { get; set; }
        
        /// <summary>
        /// Determine whether the dropdown menu and the select input are the same width. Default set min-width same as input. Will ignore when value less than select width. false will disable virtual scroll
        /// </summary>
        [Parameter]
        public OneOf<bool, string> DropdownMatchSelectWidth { get; set; } = true;

        /// <summary>
        /// Maximum width of the dropdown
        /// </summary>
        /// <default value="auto"/>
        [Parameter]
        public string DropdownMaxWidth { get; set; } = "auto";

        /// <summary>
        /// Maximum height of the dropdown
        /// </summary>
        /// <default value="256px"/>
        [Parameter]
        public string PopupContainerMaxHeight { get; set; } = "256px";

        /// <summary>
        /// Style to apply to the dropdown
        /// </summary>
        [Parameter]
        public string DropdownStyle { get; set; }

        /// <summary>
        /// Whether to show lines in the tree or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool ShowTreeLine { get; set; }

        /// <summary>
        /// Whether to show an icon on leaf nodes or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool ShowLeafIcon { get; set; }

        /// <inheritdoc cref="Tree{TItem}.TitleExpression"/>
        [Parameter]
        public Func<TreeNode<TItem>, string> TitleExpression { get; set; }

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

        private const string ClassPrefix = "ant-select";

        private bool IsMultiple => Multiple || TreeCheckable;

        private bool IsTemplatedNodes => ChildContent != null;

        internal override SelectMode SelectMode => IsMultiple ? SelectMode.Multiple : base.SelectMode;

        private string[] SelectedKeys => Values?.ToArray();

        private string _dropdownStyle = string.Empty;
        private bool _multiple;
        private readonly string _dir = "ltr";
        private Tree<TItem> _tree;

        /// <summary>
        /// The selected value in the tree
        /// </summary>
        public override string Value
        {
            get => base.Value;
            set
            {
                if (base.Value == value)
                    return;

                base.Value = value;

                if (value == null)
                {
                    ClearOptions();
                }

                UpdateValueAndSelection();
            }
        }

        /// <summary>
        /// The selected values in the tree
        /// </summary>
        public override IEnumerable<string> Values
        {
            get => base.Values;
            set
            {
                if (value != null && _selectedValues != null)
                {
                    var hasChanged = !value.SequenceEqual(_selectedValues);

                    if (!hasChanged)
                        return;

                    _selectedValues = value;
                }
                else if (value != null && _selectedValues == null)
                {
                    _selectedValues = value;
                }
                else if (value == null && _selectedValues != null)
                {
                    _selectedValues = default;
                    ClearOptions();
                }

                UpdateValuesSelection();

                if (_isNotifyFieldChanged && (Form?.ValidateOnChange == true))
                {
                    EditContext?.NotifyFieldChanged(FieldIdentifier);
                }
            }
        }

        private void ClearOptions()
        {
            SelectOptionItems.Clear();
            SelectedOptionItems.Clear();
            _tree?._allNodes.ForEach(x => x.SetSelected(false));
        }

        private void CreateOptions(IEnumerable<string> data)
        {
            if (IsTemplatedNodes)
            {
                var d1 = data.Where(d => !SelectOptionItems.Any(o => o.Value == d));
                CreateOptionsByTreeNode(d1);
                return;
            }

            data.ForEach(menuId =>
            {
                var d = _tree._allNodes.FirstOrDefault(m => m.Key == menuId);
                if (d != null)
                {
                    var o = CreateOption(d, true);
                }
            });
        }

        private void CreateOptionsByTreeNode(IEnumerable<string> data)
        {
            data.ForEach(menuId =>
            {
                var d = _tree.FindFirstOrDefaultNode(n => n.Key == menuId);
                if (d != null)
                {
                    var o = CreateOption(d, true);
                }
            });
        }

        private SelectOptionItem<string, TItem> CreateOption(TreeNode<TItem> data, bool append = false)
        {
            var o = new SelectOptionItem<string, TItem>()
            {
                Label = data.Title,
                LabelTemplate = data.TitleTemplate,
                Value = data.Key,
                Item = data.DataItem,
                IsAddedTag = SelectMode != SelectMode.Default,
            };
            if (append && !SelectOptionItems.Any(m => m.Value == o.Value))
                SelectOptionItems.Add(o);
            return o;
        }

        protected override Task OnFirstAfterRenderAsync()
        {
            if (Value != null)
            {
                UpdateValueAndSelection();
            }

            if (Values != null)
            {
                UpdateValuesSelection();
            }

            return base.OnFirstAfterRenderAsync();
        }

        private void OnKeyDownAsync(KeyboardEventArgs args)
        {
        }

        protected async void OnInputAsync(ChangeEventArgs e)
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
            _searchValue = e.Value?.ToString();
            StateHasChanged();
        }

        protected async Task OnKeyUpAsync(KeyboardEventArgs e)
        {
        }

        protected async Task OnInputFocusAsync(FocusEventArgs _)
        {
            await SetInputFocusAsync();
        }

        protected async Task OnInputBlurAsync(FocusEventArgs _)
        {
            await SetInputBlurAsync();
        }

        protected async Task SetInputBlurAsync()
        {
            if (Focused)
            {
                Focused = false;

                SetClassMap();

                await JsInvokeAsync(JSInteropConstants.Blur, _inputRef);

                OnBlur?.Invoke();
            }
        }

        private async Task OnOverlayVisibleChangeAsync(bool visible)
        {
            if (visible)
            {
                await SetDropdownStyleAsync();

                await SetInputFocusAsync();
            }
            else
            {
                OnOverlayHide();
            }
        }

        protected async Task OnRemoveSelectedAsync(SelectOptionItem<string, TItem> selectOption)
        {
            if (selectOption == null) throw new ArgumentNullException(nameof(selectOption));
            await SetValueAsync(selectOption);

            foreach (var item in _tree.SelectedNodesDictionary.Select(x => x.Value).ToList())
            {
                if (item.Key == selectOption.Value)
                    item.SetSelected(false);
            }
        }

        private async Task OnTreeNodeClick(TreeEventArgs<TItem> args)
        {
            var node = args.Node;

            if (!TreeCheckable && !node.Selected)
                return;

            var key = node.Key;
            if (Value != null && Value.Equals(key))
                return;
            if (Values != null && Values.Contains(key))
                return;

            var option = CreateOption(node, true);

            await SetValueAsync(option);

            if (!Multiple)
            {
                var unselectedNodes = _tree._allNodes.Where(x => x.Key != node.Key);
                unselectedNodes.ForEach(x => x.SetSelected(false));
            }

            if (SelectMode == SelectMode.Default)
            {
                await CloseAsync();
            }
        }

        protected async Task OnTreeNodeUnSelect(TreeEventArgs<TItem> args)
        {
            // Prevent deselect in sigle selection mode
            if (!Multiple && args.Node.Key == Value)
            {
                args.Node.SetSelected(true);
                return;
            }

            // Deselect in Multiple mode
            var node = SelectOptionItems.Where(o => o.Value == args.Node.Key).FirstOrDefault();
            if (node != null)
            {
                await SetValueAsync(node);
            }
        }

        private async Task OnTreeCheck(TreeEventArgs<TItem> args)
        {
            var option = CreateOption(args.Node, true);
            await SetValueAsync(option);
        }

        protected async Task SetDropdownStyleAsync()
        {
            string maxWidth = "", minWidth = "", definedWidth = "";
            var domRect = await JsInvokeAsync<DomRect>(JSInteropConstants.GetBoundingClientRect, Ref);
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

            if (Multiple)
            {
                if (_selectedValues == null)
                    return;
                _tree._allNodes.ForEach(n => n.SetSelected(_selectedValues.Contains(n.Key)));
            }
            else
            {
                _tree?.FindFirstOrDefaultNode(node => node.Key == Value)?.SetSelected(true);
            }
        }

        protected override void SetClassMap()
        {
            ClassMapper
                .Add("ant-tree-select")
                .Add($"{ClassPrefix}")
                .If($"{ClassPrefix}-open", () => _dropDown?.IsOverlayShow() ?? false)
                .If($"{ClassPrefix}-focused", () => Focused)
                .If($"{ClassPrefix}-single", () => SelectMode == SelectMode.Default)
                .If($"{ClassPrefix}-multiple", () => SelectMode != SelectMode.Default)
                .If($"{ClassPrefix}-sm", () => Size == AntSizeLDSType.Small)
                .If($"{ClassPrefix}-lg", () => Size == AntSizeLDSType.Large)
                .If($"{ClassPrefix}-show-arrow", () => ShowArrowIcon)
                .If($"{ClassPrefix}-show-search", () => IsSearchEnabled)
                .If($"{ClassPrefix}-loading", () => Loading)
                .If($"{ClassPrefix}-disabled", () => Disabled)
                .If($"{ClassPrefix}-rtl", () => RTL)
                .If($"{ClassPrefix}-status-error", () => ValidationMessages.Length > 0)
                .If($"{ClassPrefix}-status-warning", () => Status == "warning")
                .If($"{ClassPrefix}-allow-clear", () => AllowClear)
                ;
        }

        private void UpdateValueAndSelection()
        {
            if (SelectOptionItems.Any(o => o.Value == Value))
            {
                _ = SetValueAsync(SelectOptionItems.First(o => o.Value == Value));
            }
            else
            {
                var data = _tree?._allNodes.FirstOrDefault(x => x.Key == Value);
                if (data != null)
                {
                    var o = CreateOption(data, true);
                    _ = SetValueAsync(o);
                }
            }
        }

        private void UpdateValuesSelection()
        {
            if (_tree == null)
                return;

            if (_selectedValues?.Any() != true)
            {
                ClearOptions();
                return;
            }

            CreateOptions(_selectedValues);
            _ = OnValuesChangeAsync(_selectedValues);
        }
    }
}
