using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using AntDesign.Core.Helpers.MemberPath;
using AntDesign.JsInterop;
using OneOf;
using System.Linq;

namespace AntDesign
{
    public partial class TreeSelect<TItem> : SelectBase<string, TItem> where TItem : class
    {
        [Parameter] public bool ShowExpand { get; set; } = true;

        protected Tree<TItem> _tree;

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

        [Parameter] public bool TreeCheckable { get; set; }

        [Parameter] public string PopupContainerSelector { get; set; } = "body";

        [Parameter] public Action OnMouseEnter { get; set; }

        [Parameter] public Action OnMouseLeave { get; set; }

        [Parameter] public RenderFragment<TItem> LabelTemplate { get; set; }

        [Parameter] public bool ShowSearchIcon { get; set; } = true;

        [Parameter] public bool ShowArrowIcon { get; set; } = true;

        [Parameter] public TreeNode<TItem>[] Nodes { get; set; }

        [Parameter] public IEnumerable<TItem> DataSource { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public bool TreeDefaultExpandAll { get; set; }

        [Parameter]
        public Func<IEnumerable<TItem>, string, TItem> DataItemExpression { get; set; }

        [Parameter]
        public Func<IList<TItem>, IEnumerable<string>, IEnumerable<TItem>> DataItemsExpression { get; set; }

        [Parameter]

        public string RootValue { get; set; } = "0";

        protected Func<TreeNode<TItem>, string> TreeNodeTitleExpression
        {
            get
            {
                return node => TitleExpression(node.DataItem);
            }
        }

        [Parameter]
        public Func<TItem, string> TitleExpression { get; set; }


        protected virtual Dictionary<string, object> TreeAttributes
        {
            get
            {
                return new()
                {
                    { "DataSource", DataSource },
                    { "TitleExpression", DataSource == null ? null : TreeNodeTitleExpression },
                    { "DefaultExpandAll", TreeDefaultExpandAll },
                    { "KeyExpression", DataSource == null ? null : TreeNodeKeyExpression },
                    { "ChildrenExpression", DataSource == null ? null : TreeNodeChildrenExpression },
                    { "DisabledExpression", DataSource == null ? null : TreeNodeDisabledExpression },
                    { "IsLeafExpression", DataSource == null ? null : TreeNodeIsLeafExpression }
                };
            }
        }


        protected Func<TreeNode<TItem>, string> TreeNodeKeyExpression
        {
            get
            {
                return node => KeyExpression(node.DataItem);
            }
        }

        [Parameter]
        public Func<TItem, string> KeyExpression { get; set; }


        protected Func<TreeNode<TItem>, string> TreeNodeIconExpression
        {
            get
            {
                return node => IconExpression(node.DataItem);
            }
        }

        /// <summary>
        /// Specifies a method to return the node icon.
        /// </summary>
        [Parameter]
        public Func<TItem, string> IconExpression { get; set; }

        protected Func<TreeNode<TItem>, bool> TreeNodeIsLeafExpression
        {
            get
            {
                return node => IsLeafExpression(node.DataItem);
            }
        }


        private bool IsInnerModel => ChildContent != null;

        /// <summary>
        /// Specifies a method that returns whether the expression is a leaf node.
        /// </summary>
        [Parameter]
        public Func<TItem, bool> IsLeafExpression { get; set; }


        protected virtual Func<TreeNode<TItem>, IList<TItem>> TreeNodeChildrenExpression
        {
            get
            {
                return node => ChildrenExpression == null ? null : ChildrenExpression(node.DataItem);
            }
        }

        [Parameter]
        public virtual Func<TItem, IList<TItem>> ChildrenExpression { get; set; }

        protected Func<TreeNode<TItem>, bool> TreeNodeDisabledExpression
        {
            get
            {
                return node => DisabledExpression != null && DisabledExpression(node.DataItem);
            }
        }


        /// <summary>
        /// Specifies a method to return a disabled node
        /// </summary>
        [Parameter]
        public Func<TItem, bool> DisabledExpression { get; set; }

        [Parameter] public OneOf<bool, string> DropdownMatchSelectWidth { get; set; } = true;
        [Parameter] public string DropdownMaxWidth { get; set; } = "auto";

        [Parameter] public string PopupContainerMaxHeight { get; set; } = "256px";



        private bool IsMultiple => Multiple || TreeCheckable;

        internal override SelectMode SelectMode => IsMultiple ? SelectMode.Multiple : base.SelectMode;

        private string[] SelectedKeys => Values?.ToArray();
        //private readonly IList<TreeNode<TItem>> _selectedNodes = new List<TreeNode<TItem>>();

        private string _dropdownStyle = string.Empty;
        private bool _multiple;
        private readonly string _dir = "ltr";




        public override string Value
        {
            get => base.Value;
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;
                if (base.Value == value)
                    return;
                base.Value = value;

                if (SelectOptionItems.Any(o => o.Value == value))
                {
                    _ = SetValueAsync(SelectOptionItems.First(o => o.Value == value));
                }
                else
                {
                    var data = DataItemExpression?.Invoke(DataSource, value);
                    if (data != null)
                    {
                        var o = CreateOption(data, true);
                        _ = SetValueAsync(o);
                    }
                }


            }
        }

        public override IEnumerable<string> Values
        {
            get => base.Values;
            set
            {
                if (!_isInitialized)
                    return;

                if (!Multiple)
                    throw new NotImplementedException("not Multiple select, no die");

                if (value != null && _selectedValues != null)
                {
                    var hasChanged = !value.SequenceEqual(_selectedValues);

                    if (!hasChanged)
                        return;

                    ClearOptions();

                    _selectedValues = value;
                    CreateOptions(value);
                    _ = OnValuesChangeAsync(value);
                }
                else if (value != null && _selectedValues == null)
                {
                    _selectedValues = value;
                    CreateOptions(value);
                    _ = OnValuesChangeAsync(value);
                }
                else if (value == null && _selectedValues != null)
                {
                    _selectedValues = default;

                    ClearOptions();

                    _ = OnValuesChangeAsync(default);
                }
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
        }

        private void CreateOptions(IEnumerable<string> data)
        {
            if (IsInnerModel)
            {
                var d1 = data.Where(d => !SelectOptionItems.Any(o => o.Value == d));
                CreateOptionsByTreeNode(d1);
                return;
            }

            data.ForEach(menuId =>
            {
                var d = DataItemExpression?.Invoke(DataSource, menuId);
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
                Value = data.Key,
                Item = data.DataItem,
                IsAddedTag = SelectMode != SelectMode.Default
            };
            if (append)
                SelectOptionItems.Add(o);
            return o;
        }

        private SelectOptionItem<string, TItem> CreateOption(TItem data, bool append = false)
        {
            var o = new SelectOptionItem<string, TItem>()
            {
                Label = TitleExpression(data),
                Value = KeyExpression(data),
                Item = data,
                IsAddedTag = SelectMode != SelectMode.Default
            };
            if (append)
                SelectOptionItems.Add(o);
            return o;
        }

        protected override void OnInitialized()
        {
            SelectOptions = "".ToRenderFragment();
            //_inputValue = Value;
            base.OnInitialized();
        }


        private void OnKeyDownAsync(KeyboardEventArgs args)
        {
        }

        protected async void OnInputAsync(ChangeEventArgs e)
        {
        }

        protected async Task OnKeyUpAsync(KeyboardEventArgs e)
        {
        }

        protected async Task OnInputFocusAsync(FocusEventArgs _)
        {
        }

        protected async Task OnInputBlurAsync(FocusEventArgs _)
        {
        }

        private async Task OnOverlayVisibleChangeAsync(bool visible)
        {
            if (visible)
            {
                await SetDropdownStyleAsync();
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
            if (!args.Node.Selected)
                return;

            var key = args.Node.Key;
            if (Value != null && Value.Equals(key))
                return;
            if (Values != null && Values.Contains(key))
                return;

            var data = args.Node;
            SelectOptionItem<string, TItem> item;
            if (IsInnerModel)
                item = CreateOption(data, true);
            else
                item = CreateOption(data.DataItem, true);

            //_selectedNodes.Add(data);

            await SetValueAsync(item);

            if (SelectMode == SelectMode.Default)
                await CloseAsync();
        }


        protected void OnTreeNodeUnSelect(TreeEventArgs<TItem> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var key = args.Node.Key;
            var nodes = SelectOptionItems.Where(o => o.Value == key).ToArray();
            foreach (var item in nodes)
            {
                _ = SetValueAsync(item);
            }
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
            _dropdownStyle = minWidth + definedWidth + maxWidth;

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
            var classPrefix = "ant-select";
            ClassMapper
                .Add(classPrefix)
                .Add("ant-tree-select")
                .If("ant-select-lg", () => Size == "large")
                .If("ant-select-rtl", () => _dir == "rtl")
                .If("ant-select-sm", () => Size == "rtl")
                .If("ant-select-disabled", () => Disabled)
                .If("ant-select-single", () => SelectMode == SelectMode.Default)
                .If($"ant-select-multiple", () => SelectMode != SelectMode.Default)
                .If("ant-select-show-arrow", () => !IsMultiple)
                .If("ant-select-show-search", () => !IsMultiple)
                .If("ant-select-allow-clear", () => AllowClear)
                .If("ant-select-open", () => Open)
                .If("ant-select-focused", () => Open || Focused)
                ;
        }
    }
}
