using System;
using AntDesign.Core.Extensions;
using AntDesign.JsInterop;
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
        ///  Whether to show password
        /// </summary>
        [Parameter]
        public bool ShowPassword
        {
            get => _visible;
            set
            {
                _visible = value;
                if (_visible)
                    Type = "text";
                else
                    Type = "password";
            }
        }

        /// <summary>
        /// Whether show toggle button
        /// </summary>
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
                .If($"{PrefixCls}-password-small", () => Size == InputSize.Small)
                .If($"{PrefixCls}-password-rtl", () => RTL);

            AffixWrapperClass = string.Join(" ", AffixWrapperClass, $"{PrefixCls}-password");

            if (VisibilityToggle)
            {
                Suffix = new RenderFragment((builder) =>
                {
                    builder.OpenElement(0, "span");
                    builder.AddAttribute(1, "class", $"{PrefixCls}-suffix");
                    if (IconRender is null)
                    {
                        builder.OpenComponent<Icon>(2);
                        builder.AddAttribute(3, "class", $"{PrefixCls}-password-icon");
                        builder.AddAttribute(4, "type", _eyeIcon);
                        builder.AddAttribute(5, "onclick", CallbackFactory.Create<MouseEventArgs>(this, async args =>
                        {
                            var element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Ref);

                            IsFocused = true;
                            await this.FocusAsync(Ref);

                            ToggleVisibility(args);

                            if (element.SelectionStart != 0)
                                await Js.SetSelectionStartAsync(Ref, element.SelectionStart);
                        }));
                        builder.CloseComponent();
                    }
                    else
                    {
                        builder.AddContent(6, IconRender);
                    }
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
