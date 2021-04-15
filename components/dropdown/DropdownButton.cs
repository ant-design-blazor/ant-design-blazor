using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class DropdownButton : Dropdown
    {
        private string _icon = "ellipsis";
        [Parameter]
        public string Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                ChangeRightButtonIcon(value);
            }
        }

        private ButtonSize _size = ButtonSize.Middle;
        [Parameter]
        public ButtonSize Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                ChangeButtonSize(value);
            }
        }

        private ButtonType _type = ButtonType.Default;
        [Parameter]
        public ButtonType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                ChangeButtonType(value);
            }
        }

        public DropdownButton()
        {
            IsButton = true;
        }
    }
}
