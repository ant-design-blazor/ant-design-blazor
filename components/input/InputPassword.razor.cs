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
                {
                    Type = AntDesign.InputType.Text;
                    _eyeIcon = IconType.Outline.Eye;
                }
                else
                {
                    Type = AntDesign.InputType.Password;
                    _eyeIcon = IconType.Outline.EyeInvisible;
                }
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
            Type = _visible ? AntDesign.InputType.Text : AntDesign.InputType.Password;
            _eyeIcon = _visible ? IconType.Outline.Eye : IconType.Outline.EyeInvisible;
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
                        builder.OpenElement(2, "span");
                        builder.AddAttribute(3, "role", "button");
                        builder.AddAttribute(4, "tabindex", Disabled ? -1 : 0);
                        builder.AddAttribute(5, "class", $"{PrefixCls}-password-icon");
                        builder.AddAttribute(6, "aria-disabled", Disabled ? "true" : "false");
                        builder.AddAttribute(7, "aria-pressed", _visible ? "true" : "false");
                        builder.AddAttribute(8, "aria-label", _visible ? "Hide" : "Show");
                        builder.AddAttribute(9, "onmousedown", CallbackFactory.Create<MouseEventArgs>(this, PreventDefault));
                        builder.AddEventPreventDefaultAttribute(10, "onmousedown", true);
                        builder.AddAttribute(11, "onmouseup", CallbackFactory.Create<MouseEventArgs>(this, PreventDefault));
                        builder.AddEventPreventDefaultAttribute(12, "onmouseup", true);
                        builder.AddAttribute(13, "onclick", CallbackFactory.Create<MouseEventArgs>(this, ToggleVisibilityAndFocusAsync));
                        builder.AddAttribute(14, "onkeydown", CallbackFactory.Create<KeyboardEventArgs>(this, HandleToggleKeyDownAsync));
                        builder.OpenComponent<Icon>(16);
                        builder.AddAttribute(17, "type", _eyeIcon);
                        builder.CloseComponent();
                        builder.CloseElement();
                    }
                    else
                    {
                        builder.OpenElement(2, "span");
                        builder.AddAttribute(3, "role", "button");
                        builder.AddAttribute(4, "tabindex", Disabled ? -1 : 0);
                        builder.AddAttribute(5, "class", $"{PrefixCls}-password-icon");
                        builder.AddAttribute(6, "aria-disabled", Disabled ? "true" : "false");
                        builder.AddAttribute(7, "aria-pressed", _visible ? "true" : "false");
                        builder.AddAttribute(8, "aria-label", _visible ? "Hide" : "Show");
                        builder.AddAttribute(9, "onmousedown", CallbackFactory.Create<MouseEventArgs>(this, PreventDefault));
                        builder.AddEventPreventDefaultAttribute(10, "onmousedown", true);
                        builder.AddAttribute(11, "onmouseup", CallbackFactory.Create<MouseEventArgs>(this, PreventDefault));
                        builder.AddEventPreventDefaultAttribute(12, "onmouseup", true);
                        builder.AddAttribute(13, "onclick", CallbackFactory.Create<MouseEventArgs>(this, ToggleVisibilityAndFocusAsync));
                        builder.AddAttribute(14, "onkeydown", CallbackFactory.Create<KeyboardEventArgs>(this, HandleToggleKeyDownAsync));
                        builder.AddContent(16, IconRender);
                        builder.CloseElement();
                    }
                });
            }
        }

        protected override bool AppliesStatusToInputElement => false;

        protected override string GetAffixWrapperStatusClass(string statusClass) => $"ant-input-status-{statusClass}";

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
            if (!VisibilityToggle || Disabled)
            {
                return;
            }

            _visible = !_visible;
            _eyeIcon = _visible ? IconType.Outline.Eye : IconType.Outline.EyeInvisible;
            Type = _visible ? AntDesign.InputType.Text : AntDesign.InputType.Password;
        }

        private static void PreventDefault(MouseEventArgs args)
        {
        }

        private async Task ToggleVisibilityAndFocusAsync(MouseEventArgs args)
        {
            if (Disabled) return;
            ToggleVisibility(args);
            await Focus(FocusBehavior.FocusAtLast);
        }

        private Task HandleToggleKeyDownAsync(KeyboardEventArgs args)
        {
            if (Disabled || args.Repeat || (args.Key != "Enter" && args.Key != " ")) return Task.CompletedTask;
            ToggleVisibility(new MouseEventArgs());
            return Task.CompletedTask;
        }
    }
}
