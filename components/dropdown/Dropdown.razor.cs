// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    /**
    <summary>
    <para>A dropdown list.</para>

    <h2>When To Use</h2>

    <para>
        When there are more than a few options to choose from, you can wrap them in a <c>Dropdown</c>. 
        By hovering or clicking on the trigger, a dropdown menu will appear, which allows you to choose an option and execute the relevant action.
    </para>

    <h2>Two types</h2>

    <para>There are 2 rendering approaches for <c>Dropdown</c>:</para>
    <list type="number">
        <item>Wraps child element (content of the <c>Dropdown</c>) with a <c>div</c> (default approach).</item>
        <item>
            Child element is not wrapped with anything. This approach requires usage of <c>Unbound</c> tag inside <c>Dropdown</c> and depending on the child element type (please refer to the first example):
            <list type="bullet">
                <item>html tag: has to have its <c>@ref</c> set to <c>@context.Current</c> </item>
                <item><c>Ant Design Blazor</c> component: has to have its <c>RefBack</c> attribute set to <c>@context</c>.</item>
            </list>
        </item>
    </list>
    </summary>
    <seealso cref="DropdownButton"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.Navigation, "https://gw.alipayobjects.com/zos/alicdn/eedWN59yJ/Dropdown.svg", Title = "Dropdown", SubTitle = "下拉菜单")]
    public partial class Dropdown : OverlayTrigger
    {
        internal void RefreshPopupContent() => _overlay?.RefreshOverlayContent();

        /// <summary>
        /// Data-driven menu configuration, matching React's <c>menu</c> prop.
        /// Explicit <see cref="OverlayTrigger.Overlay"/> content remains available
        /// for custom or legacy menu markup.
        /// </summary>
        [Parameter]
        public DropdownMenuOptions Menu { get; set; }

        /// <summary>
        /// Optional semantic classes keyed by Dropdown Semantic DOM name.
        /// </summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> ClassNames { get; set; }

        /// <summary>
        /// Optional semantic inline styles keyed by Dropdown Semantic DOM name.
        /// </summary>
        [Parameter]
        public IReadOnlyDictionary<string, string> Styles { get; set; }

        /// <summary>
        /// Custom popup content. When set, this takes precedence over <see cref="Overlay"/>.
        /// </summary>
        [Parameter]
        public RenderFragment<RenderFragment> PopupRender { get; set; }

        /// <summary>
        /// Custom popup content. This React-compatible alias is kept for callers
        /// using the older <c>dropdownRender</c> name; prefer <see cref="PopupRender"/>.
        /// </summary>
        [Parameter]
        public RenderFragment<RenderFragment> DropdownRender { get; set; }

        /// <summary>
        /// Whether to adjust the popup placement when it would overflow the viewport.
        /// This aliases <see cref="OverlayTrigger.BoundaryAdjustMode"/> for React API parity.
        /// </summary>
        [Parameter]
        public bool? AutoAdjustOverflow { get; set; }

        /// <summary>
        /// Whether the popup is removed from the DOM after it closes. This
        /// React-compatible legacy name forwards to <see cref="DestroyOnHidden"/>.
        /// </summary>
        [Obsolete("Use DestroyOnHidden instead.")]
        [Parameter]
        public bool? DestroyPopupOnHide { get; set; }

        /// <summary>Additional class applied to the dropdown popup root.</summary>
        [Parameter]
        public string RootClassName { get; set; }

        /// <summary>Keep popup content rendered while it is closed.</summary>
        [Parameter]
        public bool ForceRender { get; set; }

        /// <summary>Additional class applied to the trigger while the popup is open.</summary>
        [Parameter]
        public string OpenClassName { get; set; }

        /// <summary>
        /// Whether the dropdown arrow should be visible.
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Arrow { get; set; }

        /// <summary>
        /// Whether the dropdown arrow should point at center
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool ArrowPointAtCenter { get; set; }

        internal Func<RenderFragment, RenderFragment, RenderFragment> ButtonsRender { get; set; }
        internal bool Block { get; set; }

        private string _rightButtonIcon = "ellipsis";
        private ButtonSize _buttonSize = AntDesign.ButtonSize.Default;
        private bool _danger;
        private bool _ghost;
        private bool _isLoading;
        private MouseEventArgs _menuDomEvent;

        private ButtonType _buttonTypeRight = ButtonType.Default;
        private ButtonType _buttonTypeLeft = ButtonType.Default;
        protected string _buttonClassRight;
        protected string _buttonClassLeft;
        protected string _buttonStyleRight;
        protected string _buttonStyleLeft;

        internal string RightButtonIcon => _rightButtonIcon;
        internal ButtonSize ButtonSize => _buttonSize;
        internal ButtonType ButtonTypeRight => _buttonTypeRight;
        internal ButtonType ButtonTypeLeft => _buttonTypeLeft;
        internal string ButtonClassRight => _buttonClassRight;
        internal string ButtonClassLeft => _buttonClassLeft;
        internal string ButtonStyleRight => _buttonStyleRight;
        internal string ButtonStyleLeft => _buttonStyleLeft;
        internal bool ButtonDanger => _danger;
        internal bool ButtonGhost => _ghost;
        internal bool IsLoading => _isLoading;
        internal string ButtonHtmlType { get; set; } = "button";
        internal string ButtonHref { get; set; }
        internal string ButtonTitle { get; set; }

        private static readonly EventCallbackFactory _callbackFactory = new EventCallbackFactory();

        public Dropdown()
        {
            TriggerCls = "ant-dropdown-trigger";
            MouseEnterDelay = 0.15;
            MouseLeaveDelay = 0.1;
            // React Dropdown sizes its popup to its menu content while keeping
            // the trigger width as a minimum; matching every trigger's width
            // makes ordinary dropdown menus wrap unexpectedly.
            PopupWidthMode = "content";
        }

        protected override void OnParametersSet()
        {
            if (AutoAdjustOverflow.HasValue)
            {
                BoundaryAdjustMode = AutoAdjustOverflow.Value
                    ? TriggerBoundaryAdjustMode.InView
                    : TriggerBoundaryAdjustMode.None;
            }

#pragma warning disable CS0618
            if (DestroyPopupOnHide.HasValue)
            {
                DestroyOnHidden = DestroyPopupOnHide.Value;
            }
#pragma warning restore CS0618
            if (ForceRender)
            {
                HiddenMode = true;
            }

            base.OnParametersSet();
        }

        internal string GetSemanticClass(string slot, string baseClass = null)
        {
            if (ClassNames != null && ClassNames.TryGetValue(slot, out var customClass)
                && !string.IsNullOrWhiteSpace(customClass))
            {
                return string.IsNullOrWhiteSpace(baseClass) ? customClass : $"{baseClass} {customClass}";
            }

            return baseClass;
        }

        internal string GetSemanticStyle(string slot, string baseStyle = null)
        {
            if (Styles == null || !Styles.TryGetValue(slot, out var semanticStyle)
                || string.IsNullOrWhiteSpace(semanticStyle))
            {
                return baseStyle;
            }

            return string.IsNullOrWhiteSpace(baseStyle) ? semanticStyle : $"{baseStyle.TrimEnd(';')};{semanticStyle}";
        }

        internal string GetDropdownRootClass()
        {
            var semanticClass = ClassNames != null && ClassNames.TryGetValue("root", out var value)
                ? value
                : null;
            return string.Join(" ", new[] { OverlayClassName, RootClassName, semanticClass }
                .Where(name => !string.IsNullOrWhiteSpace(name)));
        }

        internal string GetDropdownRootStyle() => GetSemanticStyle("root", OverlayStyle);

        internal string GetTriggerWrapperClass()
        {
            return ClassMapper.Class;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            await SyncTriggerOpenClassAsync();
        }

        protected override async Task OnTriggerMouseEnter()
        {
            await base.OnTriggerMouseEnter();
            await SyncTriggerOpenClassAsync();
        }

        protected override async Task OnTriggerMouseLeave()
        {
            await base.OnTriggerMouseLeave();
            await SyncTriggerOpenClassAsync();
        }

        protected override async Task OnTriggerFocusIn()
        {
            await base.OnTriggerFocusIn();
            await SyncTriggerOpenClassAsync();
        }

        protected override async Task OnTriggerFocusOut()
        {
            await base.OnTriggerFocusOut();
            await SyncTriggerOpenClassAsync();
        }

        protected override async Task OnTriggerContextmenu(MouseEventArgs args)
        {
            await base.OnTriggerContextmenu(args);
            await SyncTriggerOpenClassAsync();
        }

        private async Task SyncTriggerOpenClassAsync()
        {
            if (ChildContent == null)
            {
                return;
            }

            var classOperation = IsOverlayShow()
                ? JSInteropConstants.StyleHelper.AddClsToFirstChild
                : JSInteropConstants.StyleHelper.RemoveClsFromFirstChild;

            await JsInvokeAsync(classOperation, Ref, "ant-dropdown-open");
            if (!string.IsNullOrWhiteSpace(OpenClassName))
            {
                await JsInvokeAsync(classOperation, Ref, OpenClassName);
            }
        }

        internal override string GetOverlayAdditionalClass() => GetDropdownRootClass();

        internal override string GetOverlayAdditionalStyle() =>
            GetSemanticStyle("root", base.GetOverlayAdditionalStyle());

        internal RenderFragment GetPopupContent() => Menu == null ? Overlay : RenderMenu;

        private void RenderMenu(RenderTreeBuilder builder)
        {
            if (Menu == null) return;

            var sequence = 0;
            builder.OpenComponent<Menu>(sequence++);
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.Theme), Menu.Theme);
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.Mode), Menu.Mode);
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.ActiveKey), Menu.ActiveKey);
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.Selectable), Menu.Selectable);
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.Multiple), Menu.Multiple);
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.InlineIndent), Menu.InlineIndent);
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.DefaultSelectedKeys), Menu.DefaultSelectedKeys);
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.DefaultOpenKeys), Menu.DefaultOpenKeys);
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.OnOpenChange), Menu.OnOpenChange);
            if (Menu.OpenKeys != null)
            {
                builder.AddAttribute(sequence++, nameof(AntDesign.Menu.OpenKeys), Menu.OpenKeys);
                builder.AddAttribute(sequence++, nameof(AntDesign.Menu.OpenKeysChanged), Menu.OpenKeysChanged);
            }
            if (Menu.SelectedKeys != null)
            {
                builder.AddAttribute(sequence++, nameof(AntDesign.Menu.SelectedKeys), Menu.SelectedKeys);
                builder.AddAttribute(sequence++, nameof(AntDesign.Menu.SelectedKeysChanged), Menu.SelectedKeysChanged);
            }
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.AutoCloseDropdown), false);
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.OnMenuItemClicked), EventCallback.Factory.Create<MenuItem>(this, HandleMenuItemClicked));
            builder.AddAttribute(sequence++, nameof(AntDesign.Menu.ChildContent), RenderMenuItems(Menu.Items));
            builder.CloseComponent();
        }

        private RenderFragment RenderMenuItems(IReadOnlyList<DropdownMenuItem> items) => builder =>
        {
            if (items == null) return;

            var sequence = 0;
            foreach (var item in items)
            {
                if (item == null) continue;

                switch (item.Type)
                {
                    case DropdownMenuItemKind.Divider:
                        builder.OpenComponent<MenuDivider>(sequence++);
                        builder.AddAttribute(sequence++, nameof(MenuDivider.Class), item.Class);
                        builder.AddAttribute(sequence++, nameof(MenuDivider.Style), item.Style);
                        builder.AddAttribute(sequence++, nameof(MenuDivider.Dashed), item.Dashed);
                        builder.CloseComponent();
                        break;
                    case DropdownMenuItemKind.Group:
                        builder.OpenComponent<MenuItemGroup>(sequence++);
                        builder.AddAttribute(sequence++, nameof(MenuItemGroup.Key), item.Key);
                        builder.AddAttribute(sequence++, nameof(MenuItemGroup.Title), item.Label);
                        builder.AddAttribute(sequence++, nameof(MenuItemGroup.TitleTemplate), item.LabelTemplate);
                        builder.AddAttribute(sequence++, nameof(MenuItemGroup.Class), item.Class);
                        builder.AddAttribute(sequence++, nameof(MenuItemGroup.Style), item.Style);
                        builder.AddAttribute(sequence++, nameof(MenuItemGroup.ChildContent), RenderMenuItems(item.Children));
                        builder.CloseComponent();
                        break;
                    case DropdownMenuItemKind.SubMenu:
                        builder.OpenComponent<SubMenu>(sequence++);
                        builder.AddAttribute(sequence++, nameof(SubMenu.Key), item.Key);
                        builder.AddAttribute(sequence++, nameof(SubMenu.Title), item.Label);
                        builder.AddAttribute(sequence++, nameof(SubMenu.TitleTemplate), item.LabelTemplate);
                        builder.AddAttribute(sequence++, nameof(SubMenu.Icon), item.Icon);
                        builder.AddAttribute(sequence++, nameof(SubMenu.IconTemplate), item.IconTemplate);
                        builder.AddAttribute(sequence++, nameof(SubMenu.Disabled), item.Disabled);
                        builder.AddAttribute(sequence++, nameof(SubMenu.Class), item.Class);
                        builder.AddAttribute(sequence++, nameof(SubMenu.Style), item.Style);
                        builder.AddAttribute(sequence++, nameof(SubMenu.ChildContent), RenderMenuItems(item.Children));
                        builder.CloseComponent();
                        break;
                    default:
                        builder.OpenComponent<MenuItem>(sequence++);
                        builder.AddAttribute(sequence++, nameof(MenuItem.Key), item.Key);
                        builder.AddAttribute(sequence++, nameof(MenuItem.Title), item.Title);
                        builder.AddAttribute(sequence++, nameof(MenuItem.Disabled), item.Disabled);
                        builder.AddAttribute(sequence++, nameof(MenuItem.Danger), item.Danger);
                        builder.AddAttribute(sequence++, nameof(MenuItem.Class), item.Class);
                        builder.AddAttribute(sequence++, nameof(MenuItem.Style), item.Style);
                        builder.AddAttribute(sequence++, nameof(MenuItem.Icon), item.Icon);
                        builder.AddAttribute(sequence++, nameof(MenuItem.IconTemplate), item.IconTemplate);
                        builder.AddAttribute(sequence++, nameof(MenuItem.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, args => _menuDomEvent = args));
                        builder.AddAttribute(sequence++, nameof(MenuItem.ChildContent), item.LabelTemplate ?? (item.Label == null ? null : (RenderFragment)(contentBuilder => contentBuilder.AddContent(0, item.Label))));
                        builder.CloseComponent();
                        break;
                }
            }
        };

        private async Task HandleMenuItemClicked(MenuItem menuItem)
        {
            if (Menu == null || menuItem == null) return;

            var keyPath = new List<string>();
            FindMenuItemPath(Menu.Items, menuItem.Key, keyPath);
            await Menu.OnClick.InvokeAsync(new DropdownMenuClickInfo(menuItem.Key, keyPath, _menuDomEvent ?? new MouseEventArgs()));
            _menuDomEvent = null;

            if (!Menu.Selectable || !Menu.Multiple)
            {
                await Close();
            }
        }

        private static bool FindMenuItemPath(IReadOnlyList<DropdownMenuItem> items, string targetKey, List<string> path)
        {
            if (items == null) return false;

            foreach (var item in items)
            {
                if (item == null || item.Type == DropdownMenuItemKind.Divider) continue;
                if (item.Type == DropdownMenuItemKind.Group)
                {
                    if (FindMenuItemPath(item.Children, targetKey, path)) return true;
                    continue;
                }
                path.Add(item.Key);
                if (string.Equals(item.Key, targetKey, StringComparison.Ordinal) || FindMenuItemPath(item.Children, targetKey, path))
                {
                    return true;
                }
                path.RemoveAt(path.Count - 1);
            }

            return false;
        }

        protected void ChangeRightButtonIcon(string icon)
        {
            _rightButtonIcon = icon;

            StateHasChanged();
        }

        protected void ChangeButtonClass(string leftButton, string rightButton)
        {
            _buttonClassLeft = leftButton;
            _buttonClassRight = rightButton;
            StateHasChanged();
        }

        protected void ChangeButtonStyles(string leftButton, string rightButton)
        {
            _buttonStyleLeft = leftButton;
            _buttonStyleRight = rightButton;
            StateHasChanged();
        }

        protected void ChangeButtonSize(ButtonSize size)
        {
            _buttonSize = size;

            StateHasChanged();
        }

        protected void ChangeButtonDanger(bool danger)
        {
            _danger = danger;

            StateHasChanged();
        }

        protected void ChangeButtonGhost(bool ghost)
        {
            _ghost = ghost;

            StateHasChanged();
        }

        protected void ChangeButtonLoading(bool isLoading)
        {
            _isLoading = isLoading;

            StateHasChanged();
        }

        protected void ChangeButtonType(ButtonType leftButton, ButtonType rightButton)
        {
            _buttonTypeLeft = leftButton;
            _buttonTypeRight = rightButton;
            StateHasChanged();
        }

        /// <summary>
        /// Handle the trigger click.
        /// </summary>
        /// <param name="args">MouseEventArgs</param>
        /// <returns></returns>
        protected override async Task OnClickDiv(MouseEventArgs args)
        {
            if (!IsButton)
            {
                await OnTriggerClick();
                await OnClick.InvokeAsync(args);
            }

            await SyncTriggerOpenClassAsync();
        }

        internal override string GetArrowClass()
        {
            if (Arrow || ArrowPointAtCenter)
                return $"{PrefixCls}-show-arrow";

            return "";
        }
    }
}
