// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
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

        private static readonly EventCallbackFactory _callbackFactory = new EventCallbackFactory();

        public Dropdown()
        {
            TriggerCls = "ant-dropdown-trigger";
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            UseStyle(PrefixCls, DropdownStyle.UseComponentStyle);
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
        }

        internal override string GetArrowClass()
        {
            if (Arrow || ArrowPointAtCenter)
                return $"{PrefixCls}-show-arrow";

            return "";
        }
    }
}
