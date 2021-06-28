using System;
using System.Threading.Tasks;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Dropdown : OverlayTrigger
    {
        internal Func<RenderFragment, RenderFragment, RenderFragment> ButtonsRender { get; set; }
        internal bool Block { get; set; }

        private string _rightButtonIcon = "ellipsis";
        private string _buttonSize = AntSizeLDSType.Default;
        private bool _danger;
        private bool _ghost;
        private bool _isLoading;

        private string _buttonTypeRight = ButtonType.Default;
        private string _buttonTypeLeft = ButtonType.Default;
        protected void ChangeRightButtonIcon(string icon)
        {
            _rightButtonIcon = icon;

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

        protected void ChangeButtonType((string LeftButton, string RightButton) type)
        {
            (_buttonTypeLeft, _buttonTypeRight) = type;
            StateHasChanged();
        }

        /// <summary>
        /// Handle the trigger click.
        /// </summary>
        /// <param name="args">MouseEventArgs</param>
        /// <returns></returns>
        public override async Task OnClickDiv(MouseEventArgs args)
        {
            if (!IsButton)
            {
                await OnTriggerClick();
                await OnClick.InvokeAsync(args);
            }
        }

    }
}
