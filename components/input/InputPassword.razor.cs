// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using AntDesign.Core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class InputPassword : Input<string>
    {
        private bool _visible = false;
        private string _eyeIcon;

        /// <summary>
        /// Custom icon render
        /// </summary>
        [Parameter]
        public RenderFragment IconRender { get; set; }

        /// <summary>
        /// Whether to show password or not
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool ShowPassword
        {
            get => _visible;
            set
            {
                _visible = value;
                if (_visible)
                    Type = AntDesign.InputType.Text;
                else
                    Type = AntDesign.InputType.Password;
            }
        }

        /// <summary>
        /// Whether to show password visibility toggle button or not
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool VisibilityToggle { get; set; } = true;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Type = AntDesign.InputType.Password;
            ToggleVisibility(new MouseEventArgs());
        }

        protected override void SetClasses()
        {
            base.SetClasses();
            //ant-input-password-large ant-input-affix-wrapper ant-input-affix-wrapper-lg
            ClassMapper
                .If($"{PrefixCls}-password-large", () => Size == InputSize.Large)
                .If($"{PrefixCls}-password-small", () => Size == InputSize.Small)
                .If($"{PrefixCls}-password-rtl", () => RTL);

            AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-password");
            if (VisibilityToggle)
            {
                Suffix = new RenderFragment((builder) =>
                {
                    if (IconRender is null)
                    {
                        builder.OpenComponent<Icon>(2);
                        builder.AddAttribute(3, "class", $"{PrefixCls}-password-icon");
                        builder.AddAttribute(4, "type", _eyeIcon);
                        builder.AddAttribute(5, "onclick", CallbackFactory.Create<MouseEventArgs>(this, async args =>
                        {
                            ToggleVisibility(args);

                            await Focus(FocusBehavior.FocusAtLast);
                        }));
                        builder.CloseComponent();
                    }
                    else
                    {
                        builder.AddContent(6, IconRender);
                    }
                });
            }
        }

        /// <summary>
        /// Focus behavior for InputPassword component with optional behaviors.
        /// Special behavior required for wasm.
        /// </summary>
        /// <param name="behavior">enum: AntDesign.FocusBehavior</param>
        /// <param name="preventScroll">When true, element receiving focus will not be scrolled to.</param>
        public override async Task Focus(FocusBehavior behavior = FocusBehavior.FocusAtLast, bool preventScroll = false)
        {
            await base.Focus(behavior, preventScroll);
            //delay enforces focus - it counters the js blur that is called on button pressed
            await Task.Delay(5);
            //An ugly solution for InputPassword in wasm to receive focus and keep
            //cursor at the last character. Any improvements are very welcome.
            if (Js.IsBrowser())
            {
                await base.Focus(behavior, preventScroll);
            }
        }

        private void ToggleVisibility(MouseEventArgs args)
        {
            if (VisibilityToggle)
            {
                if (_visible)
                {
                    _eyeIcon = "eye";
                    Type = AntDesign.InputType.Text;
                }
                else
                {
                    _eyeIcon = "eye-invisible";
                    Type = AntDesign.InputType.Password;
                }

                _visible = !_visible;
            }
        }
    }
}
