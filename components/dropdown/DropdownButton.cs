using Microsoft.AspNetCore.Components;

namespace AntBlazor
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

        private string _size = AntSizeLDSType.Default;
        [Parameter]
        public string Size
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

        private string _type = AntButtonType.Default;
        [Parameter]
        public string Type
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
            Trigger = new TriggerType[] { TriggerType.Click };
        }
    }
}
