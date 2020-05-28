using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class InputPassword : Input
    {
        private bool _visible = false;
        private string _eyeIcon;

        [Parameter]
        public bool VisibilityToggle { get; set; } = true;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Type = "password";
            ToggleVisibility(new MouseEventArgs());
        }

        protected override void SetClasses()
        {
            base.SetClasses();
            //ant-input-password-large ant-input-affix-wrapper ant-input-affix-wrapper-lg
            ClassMapper
                .If($"{PrefixCls}-password-large", () => Size == InputSize.Large)
                .If($"{PrefixCls}-password-small", () => Size == InputSize.Small);

            AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-password");

            if (VisibilityToggle)
            {
                Suffix = new RenderFragment((builder) =>
                {
                    int i = 0;
                    builder.OpenElement(i++, "span");
                    builder.AddAttribute(i++, "class", $"{PrefixCls}-suffix");
                    builder.OpenComponent<AntIcon>(i++);
                    builder.AddAttribute(i++, "class", $"{PrefixCls}-password-icon");
                    builder.AddAttribute(i++, "type", _eyeIcon);
                    builder.AddAttribute(i++, "onclick", CallbackFactory.Create(this, ToggleVisibility));
                    builder.CloseComponent();
                    builder.CloseElement();
                });
            }
        }

        private void ToggleVisibility(MouseEventArgs args)
        {
            if (VisibilityToggle)
            {
                if (_visible)
                {
                    _eyeIcon = "eye";
                    Type = "text";
                }
                else
                {
                    _eyeIcon = "eye-invisible";
                    Type = "password";
                }

                _visible = !_visible;
            }
        }
    }
}
