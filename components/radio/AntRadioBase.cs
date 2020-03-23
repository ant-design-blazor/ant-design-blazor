using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public class AntRadioBase : AntDomComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string Value { get; set; }

        //[Parameter] public bool Disabled { get; set; } = false;

        [Parameter] public bool AutoFocus { get; set; } = false;

        [Parameter] public bool RadioButton { get; set; }

        protected ClassMapper RadioClassMapper { get; set; } = new ClassMapper();

        protected ClassMapper InputClassMapper { get; set; } = new ClassMapper();

        protected ClassMapper InnerClassMapper { get; set; } = new ClassMapper();

        protected ElementReference inputRef { get; set; }

        private Action onChange;

        private Action onTouched;

        protected bool isChecked => _checked ?? (this.Attributes.TryGetValue("checked", out var val)
            ? bool.TryParse(val.ToString(), out bool _disabled) ? _disabled : true : false);

        protected bool? _checked { get; set; }

        protected string name { get; set; } = null;

        protected bool disabled => this.Attributes.TryGetValue("disabled", out var val)
            ? bool.TryParse(val.ToString(), out bool _disabled) ? _disabled : true : false;

        protected void SetClass()
        {
            string prefixCls = "ant-radio";
            ClassMapper.Clear()
                .If($"{prefixCls}-wrapper", () => !RadioButton)
                .If($"{prefixCls}-button-wrapper", () => RadioButton)
                .If($"{prefixCls}-wrapper-checked", () => isChecked && !RadioButton)
                .If($"{prefixCls}-button-wrapper-checked", () => isChecked && RadioButton)
                .If($"{prefixCls}-wrapper-disabled", () => disabled && !RadioButton)
                .If($"{prefixCls}-button-wrapper-disabled", () => disabled && RadioButton);

            RadioClassMapper.Clear()
                .If(prefixCls, () => !RadioButton)
                .If($"{prefixCls}-checked", () => isChecked && !RadioButton)
                .If($"{prefixCls}-disabled", () => disabled && !RadioButton)
                .If($"{prefixCls}-button", () => RadioButton)
                .If($"{prefixCls}-button-checked", () => isChecked && RadioButton)
                .If($"{prefixCls}-button-disabled", () => disabled && RadioButton);

            InputClassMapper.Clear()
                .If($"{prefixCls}-input", () => !RadioButton)
                .If($"{prefixCls}-button-input", () => RadioButton);

            InnerClassMapper.Clear()
                .If($"{prefixCls}-inner", () => !RadioButton)
                .If($"{prefixCls}-button-inner", () => RadioButton);
        }

        protected override void OnInitialized()
        {
            SetClass();
            base.OnInitialized();
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            if (this.AutoFocus)
            {
                await this.Focus();
            }

            await base.OnFirstAfterRenderAsync();
        }

        public async Task OnClick(MouseEventArgs e)
        {
            await this.Focus();

            if (!disabled && !isChecked)
            {
                this._checked = true;
            }
        }

        protected async Task Focus()
        {
            await JsInvokeAsync(JSInteropConstants.focus, this.inputRef);
        }

        protected async Task Blur()
        {
            await JsInvokeAsync(JSInteropConstants.blur, this.inputRef);
        }
    }
}