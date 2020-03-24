using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public partial class AntInputPassword : AntInput
    {
        private bool _visible = false;
        private string _eyeIcon;

        [Parameter]
        public bool visibilityToggle { get; set; } = true;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            type = "password";
            ToggleVisibility(new MouseEventArgs());
        }

        protected override void SetClasses()
        {
            base.SetClasses();
            //ant-input-password-large ant-input-affix-wrapper ant-input-affix-wrapper-lg
            ClassMapper
                .If($"{PrefixCls}-password-large", () => size == AntInputSize.Large)
                .If($"{PrefixCls}-password-small", () => size == AntInputSize.Small);

            _affixWrapperClass = string.Join(" ", _affixWrapperClass, $"{PrefixCls}-password");

            if (visibilityToggle)
            {
                suffix = new RenderFragment((builder) =>
                {
                    int i = 0;
                    builder.OpenElement(i++, "span");
                    builder.AddAttribute(i++, "class", $"{PrefixCls}-suffix");
                    builder.OpenComponent<AntIcon>(i++);
                    builder.AddAttribute(i++, "class", $"{PrefixCls}-password-icon");
                    builder.AddAttribute(i++, "type", _eyeIcon);
                    builder.AddAttribute(i++, "onclick", _callbackFactory.Create(this, ToggleVisibility));
                    builder.CloseComponent();
                    builder.CloseElement();
                });
            }
        }

        private void ToggleVisibility(MouseEventArgs args)
        {
            if (visibilityToggle)
            {
                if (_visible)
                {
                    _eyeIcon = "eye";
                    type = "text";
                }
                else
                {
                    _eyeIcon = "eye-invisible";
                    type = "password";
                }

                _visible = !_visible;
            }
        }
    }
}