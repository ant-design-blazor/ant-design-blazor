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
        public Func<RenderFragment, RenderFragment, RenderFragment> ButtonsRender { get; set; }

        private string _rightButtonIcon = "ellipsis";
        private ButtonSize _buttonSize = ButtonSize.Middle;
        private ButtonType _buttonType = ButtonType.Default;

        private RenderFragment _leftButton;
        private RenderFragment _rightButton;

        protected void ChangeRightButtonIcon(string icon)
        {
            _rightButtonIcon = icon;

            StateHasChanged();
        }

        protected void ChangeButtonSize(ButtonSize size)
        {
            _buttonSize = size;

            StateHasChanged();
        }

        protected void ChangeButtonType(ButtonType type)
        {
            _buttonType = type;

            StateHasChanged();
        }
    }
}
