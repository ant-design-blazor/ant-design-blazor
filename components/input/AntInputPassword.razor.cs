using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace AntBlazor
{
    public partial class AntInputPassword : AntInputBase
    {
        private bool _visible;
        private string _eyeIcon;

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
                .If($"{PrefixCls}-password-large", () => size == AntInputSize.Large)
                .If($"{PrefixCls}-password-small", () => size == AntInputSize.Small);

            ToggleVisibility();
        }

        private void ToggleVisibility()
        {
            if (visibilityToggle)
            {
                if (!_visible)
                {
                    _eyeIcon = "eye";
                    _type = "text";
                }
                else
                {
                    _eyeIcon = "eye-invisible";
                    _type = "password";
                }

                _visible = !_visible;
            }
        }
    }
}