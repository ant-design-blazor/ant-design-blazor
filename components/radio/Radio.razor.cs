using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class Radio<TValue> : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public TValue Value { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; } = false;

        [Parameter]
        public bool RadioButton { get; set; }

        [Parameter]
        public bool Checked { get => _checked ?? false; set { _checked = value; } }

        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public EventCallback<bool> CheckedChange { get; set; }

        [CascadingParameter] public RadioGroup<TValue> RadioGroup { get; set; }

        protected ClassMapper RadioClassMapper { get; set; } = new ClassMapper();

        protected ClassMapper InputClassMapper { get; set; } = new ClassMapper();

        protected ClassMapper InnerClassMapper { get; set; } = new ClassMapper();

        protected ElementReference InputRef { get; set; }

        protected bool IsChecked => _checked ?? this.Checked;

        private bool? _checked;

        internal string _name;

        protected void SetClass()
        {
            string prefixCls = "ant-radio";
            ClassMapper.Clear()
                .If($"{prefixCls}-wrapper", () => !RadioButton)
                .If($"{prefixCls}-button-wrapper", () => RadioButton)
                .If($"{prefixCls}-wrapper-checked", () => IsChecked && !RadioButton)
                .If($"{prefixCls}-button-wrapper-checked", () => IsChecked && RadioButton)
                .If($"{prefixCls}-wrapper-disabled", () => Disabled && !RadioButton)
                .If($"{prefixCls}-button-wrapper-disabled", () => Disabled && RadioButton);

            RadioClassMapper.Clear()
                .If(prefixCls, () => !RadioButton)
                .If($"{prefixCls}-checked", () => IsChecked && !RadioButton)
                .If($"{prefixCls}-disabled", () => Disabled && !RadioButton)
                .If($"{prefixCls}-button", () => RadioButton)
                .If($"{prefixCls}-button-checked", () => IsChecked && RadioButton)
                .If($"{prefixCls}-button-disabled", () => Disabled && RadioButton);

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
            if (this is Radio<TValue> radio)
            {
                RadioGroup?.AddRadio(radio);
            }
            await base.OnFirstAfterRenderAsync();
        }

        internal async Task Select()
        {
            if (!Disabled && !IsChecked)
            {
                this._checked = true;
                await CheckedChange.InvokeAsync(true);
                await CheckedChanged.InvokeAsync(true);
            }

            if (RadioGroup != null)
            {
                await RadioGroup.OnRadioChange(this.Value);
            }
        }

        internal async Task UnSelect()
        {
            if (this.IsChecked)
            {
                this._checked = false;
                await CheckedChange.InvokeAsync(false);
                await CheckedChanged.InvokeAsync(false);
            }
            await Task.CompletedTask;
        }

        public async Task OnClick()
        {
            await Select();
        }

        protected async Task Focus()
        {
            await JsInvokeAsync(JSInteropConstants.focus, this.InputRef);
        }

        protected async Task Blur()
        {
            await JsInvokeAsync(JSInteropConstants.blur, this.InputRef);
        }
    }
}
