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
    public partial class TreeSelect<TItemValue, TItem> : SelectBase<TItemValue, TItem>
    {
        [Parameter] public bool ShowExpand { get; set; } = true;

        private bool _multiple;

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

        [Parameter] public IList<TItem> DataSource { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public bool TreeDefaultExpandAll { get; set; }

        /// <summary>
        /// Specifies a method that returns the text of the node.
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, string> TitleExpression { get; set; }

        /// <summary>
        /// Specifies a method that returns the key of the node.
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, string> KeyExpression { get; set; }

        /// <summary>
        /// Specifies a method to return the node icon.
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, string> IconExpression { get; set; }

        /// <summary>
        /// Specifies a method that returns whether the expression is a leaf node.
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, bool> IsLeafExpression { get; set; }

        /// <summary>
        /// Specifies a method  to return a child node
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, IList<TItem>> ChildrenExpression { get; set; }

        /// <summary>
        /// Specifies a method to return a disabled node
        /// </summary>
        [Parameter]
        public Func<TreeNode<TItem>, bool> DisabledExpression { get; set; }

        [Parameter] public OneOf<bool, string> DropdownMatchSelectWidth { get; set; } = true;
        [Parameter] public string DropdownMaxWidth { get; set; } = "auto";

        [Parameter] public string PopupContainerMaxHeight { get; set; } = "256px";



        private bool IsMultiple => Multiple || TreeCheckable;

        internal override SelectMode SelectMode => IsMultiple ? SelectMode.Multiple : base.SelectMode;

        private string[] SelectedKeys => _selectedNodes.Select(x => KeyExpression(x)).ToArray();
        private readonly IList<TreeNode<TItem>> _selectedNodes = new List<TreeNode<TItem>>();

        private string _dropdownStyle = string.Empty;
        private bool _multiple;
        private readonly string _dir = "ltr";

        private readonly bool _isComposing;
        private readonly bool _isDestroy = true;
        private readonly bool _isNotFound;
        private readonly bool _focused;
        private readonly string _inputValue = "";


        /// <summary>
        ///  'top' | 'center' | 'bottom'
        /// </summary>
        private readonly string _dropDownPosition = "bottom";

        protected override void SetClassMap()
        {
            var classPrefix = "ant-select";
            ClassMapper
                .Add("ant-select")
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

        protected override void OnInitialized()
        {
            SelectOptions = "".ToRenderFragment();
            base.OnInitialized();
        }

        private void OnClick()
        {
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

        protected async Task OnInputClearClickAsync(MouseEventArgs _)
        {
        }

        protected async Task OnRemoveSelectedAsync(SelectOptionItem<TItemValue, TItem> selectOption)
        {
            if (selectOption == null) throw new ArgumentNullException(nameof(selectOption));
            await SetValueAsync(selectOption);

            foreach (var item in _tree.SelectedNodesDictionary.Select(x => x.Value).ToList())
            {
                if (item.Key == selectOption.Value.ToString())
                    item.SetSelected(false);
            }
        }

        private async Task OnOverlayVisibleChangeAsync(bool visible)
        {
            if (visible)
            {
                await SetDropdownStyleAsync();
            }
        }

        private async Task OnTreeNodeClick(TreeEventArgs<TItem> args)
        {
            if (!args.Node.Selected)
                return;
            var data = args.Node;
            var item = new SelectOptionItem<TItemValue, TItem>()
            {
                Label = TitleExpression(data),
                Value = (TItemValue)Convert.ChangeType(KeyExpression(data), typeof(TItemValue)),
                Item = data.DataItem,
                IsAddedTag = SelectMode != SelectMode.Default
                //IsSelected = true
            };


            if (SelectMode == SelectMode.Default)
            {
                SelectOptionItems.Clear();
            }

            _selectedNodes.Add(data);
            SelectOptionItems.Add(item);
            //SelectedOptionItems.Add(item);

            await SetValueAsync(item);

            if (SelectMode == SelectMode.Default)
                await CloseAsync();
        }


        protected void OnTreeNodeUnSelect(TreeEventArgs<TItem> args)
        {
            if (args == null) throw new ArgumentNullException(nameof(args));
            var key = args.Node.Key;
            SelectOptionItems.Where(o => o.Value.ToString() == key).ForEach(o =>
            {
                _ = SetValueAsync(o);
            });
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
        }

    }
}
