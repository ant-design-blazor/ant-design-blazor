using System;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Dropdown : OverlayTrigger
    {
        [Parameter]
        public bool Arrow { get; set; }

        [Parameter]
        public bool ArrowPointAtCenter { get; set; }

        internal Func<RenderFragment, RenderFragment, RenderFragment> ButtonsRender { get; set; }
        internal bool Block { get; set; }

        private string _rightButtonIcon = "ellipsis";
        private string _buttonSize = AntSizeLDSType.Default;
        private bool _danger;
        private bool _ghost;
        private bool _isLoading;

        private string _buttonTypeRight = ButtonType.Default;
        private string _buttonTypeLeft = ButtonType.Default;
        protected string _buttonClassRight;
        protected string _buttonClassLeft;
        protected string _buttonStyleRight;
        protected string _buttonStyleLeft;

        internal string RightButtonIcon => _rightButtonIcon;
        internal string ButtonSize => _buttonSize;
        internal string ButtonTypeRight => _buttonTypeRight;
        internal string ButtonTypeLeft => _buttonTypeLeft;
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

        protected void ChangeButtonSize(string size)
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

        protected void ChangeButtonType(string leftButton, string rightButton)
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
