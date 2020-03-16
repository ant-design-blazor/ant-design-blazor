using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntInputPassword : AntInputBase
    {
        private bool _visible;

        [Parameter]
        public bool visibilityToggle { get; set; } = true;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _type = "password";
        }

        protected override void SetClasses()
        {
            base.SetClasses();
            //ant-input-password-large ant-input-affix-wrapper ant-input-affix-wrapper-lg
            ClassMapper
                .If($"{prefix}-password-large", () => size == AntInputSize.Large)
                .If($"{prefix}-password-small", () => size == AntInputSize.Small);

            ToggleVisibility();
        }

        private void ToggleVisibility()
        {

            if (visibilityToggle)
            {
                if (!_visible)
                {

                    suffix = "eye";
                    _type = "text";
                }
                else
                {
                    suffix = "eye-invisible";
                    _type = "password";
                }

                _visible = !_visible;
            }
            else
            {
                suffix = string.Empty;
            }
        }
    }
}
