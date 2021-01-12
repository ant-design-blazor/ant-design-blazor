using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AntDesign.Internal;
using AntDesign.Select.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class TreeSelect<TItemValue, TItem> : SelectBase<TItemValue, TItem>
    {
        [Parameter] public bool ShowExpand { get; set; } = true;

        [Parameter] public bool Multiple { get; set; }

        [Parameter] public bool Checkable { get; set; }

        [Parameter] public bool ShowPlaceholder { get; set; }

        [Parameter] public string PopupContainerSelector { get; set; } = "body";

        [Parameter] public Action OnMouseEnter { get; set; }

        [Parameter] public Action OnMouseLeave { get; set; }

        [Parameter] public RenderFragment<TItem> LabelTemplate { get; set; }

        [Parameter] public bool ShowSearchIcon { get; set; } = true;

        [Parameter] public bool ShowArrowIcon { get; set; } = true;

        [Parameter] public TreeNode<TItem>[] Nodes { get; set; }

        [Parameter] public IList<TItem> DataSource { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        private bool IsMultiple => Multiple || Checkable;

        private string _dir = "ltr";

        private bool _isComposing;
        private bool _isDestroy = true;
        private bool _isNotFound;
        private bool _focused;
        private string _inputValue = "";

        protected OverlayTrigger _dropDown;

        private string _searchValue = string.Empty;

        /// <summary>
        ///  'top' | 'center' | 'bottom'
        /// </summary>
        private string _dropDownPosition = "bottom";

        protected override void SetClassMap()
        {
            ClassMapper
                .Add("ant-select")
                .Add("ant-tree-select")
                .If("ant-select-lg", () => Size == "large")
                .If("ant-select-rtl", () => _dir == "rtl")
                .If("ant-select-sm", () => Size == "rtl")
                .If("ant-select-disabled", () => Disabled)
                .If("ant-select-single", () => !IsMultiple)
                .If("ant-select-show-arrow", () => !IsMultiple)
                .If("ant-select-show-search", () => !IsMultiple)
                .If("ant-select-multiple", () => IsMultiple)
                .If("ant-select-allow-clear", () => AllowClear)
                .If("ant-select-open", () => Open)
                .If("ant-select-focused", () => Open || Focused)
                ;
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
        }

        private async Task OnOverlayVisibleChangeAsync(bool visible)
        {
        }
    }
}
