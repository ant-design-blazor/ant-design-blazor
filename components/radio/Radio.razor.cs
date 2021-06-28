using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

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
        public bool DefaultChecked
        {
            get => _defaultChecked;
            set
            {
                _defaultChecked = value;
                _hasDefaultChecked = true;
            }
        }

        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public EventCallback<bool> CheckedChange { get; set; }

        [CascadingParameter]
        public RadioGroup<TValue> RadioGroup { get; set; }

        [CascadingParameter(Name = "InGroup")]
        public bool InGroup { get; set; }

        private ClassMapper _radioClassMapper = new ClassMapper();
        private ClassMapper _inputClassMapper = new ClassMapper();
        private ClassMapper _innerClassMapper = new ClassMapper();

        private ElementReference _inputRef;

        private bool IsChecked => _checked ?? this.Checked;

        private bool? _checked;

        private bool _hasDefaultChecked;
        private bool _defaultCheckedSetted;

        private string _name;
        private bool _defaultChecked;

        protected void SetClass()
        {
            string prefixCls = "ant-radio";
            ClassMapper
                .If($"{prefixCls}-wrapper", () => !RadioButton)
                .If($"{prefixCls}-button-wrapper", () => RadioButton)
                .If($"{prefixCls}-wrapper-checked", () => IsChecked && !RadioButton)
                .If($"{prefixCls}-button-wrapper-checked", () => IsChecked && RadioButton)
                .If($"{prefixCls}-wrapper-disabled", () => Disabled && !RadioButton)
                .If($"{prefixCls}-button-wrapper-disabled", () => Disabled && RadioButton)
                .If($"{prefixCls}-button-wrapper-rtl", () => RTL);

            _radioClassMapper
                .If(prefixCls, () => !RadioButton)
                .If($"{prefixCls}-checked", () => IsChecked && !RadioButton)
                .If($"{prefixCls}-disabled", () => Disabled && !RadioButton)
                .If($"{prefixCls}-button", () => RadioButton)
                .If($"{prefixCls}-button-checked", () => IsChecked && RadioButton)
                .If($"{prefixCls}-button-disabled", () => Disabled && RadioButton)
                .If($"{prefixCls}-rtl", () => RTL);

            _inputClassMapper
                .If($"{prefixCls}-input", () => !RadioButton)
                .If($"{prefixCls}-button-input", () => RadioButton);

            _innerClassMapper
                .If($"{prefixCls}-inner", () => !RadioButton)
                .If($"{prefixCls}-button-inner", () => RadioButton);
        }

        internal void SetName(string name)
        {
            _name = name;
        }

        protected override void OnInitialized()
        {
            SetClass();

            if (InGroup && RadioGroup == null)
            {
                throw new InvalidOperationException($"Please make sure that both RadioGroup and Radio have the same generic type: {typeof(TValue)} .");
            }

            RadioGroup?.AddRadio(this);

            if (RadioGroup?.Disabled == true)
            {
                Disabled = true;
            }

            if (_hasDefaultChecked && !_defaultCheckedSetted)
            {
                _checked = _defaultChecked;
                _defaultCheckedSetted = true;
            }

            base.OnInitialized();
        }

        protected override void Dispose(bool disposing)
        {
            RadioGroup?.RemoveRadio(this);
            base.Dispose(disposing);
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            if (this.AutoFocus)
            {
                await FocusAsync(this._inputRef);
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
        }

        internal async Task UnSelect()
        {
            if (!Disabled && this.IsChecked)
            {
                this._checked = false;
                await CheckedChange.InvokeAsync(false);
                await CheckedChanged.InvokeAsync(false);
            }
        }

        public async Task OnClick()
        {
            if (Disabled)
            {
                return;
            }

            if (RadioGroup != null)
            {
                await RadioGroup.OnRadioChange(this.Value);
            }
            else
            {
                await Select();
            }
        }

        protected async Task Blur()
        {
            await JsInvokeAsync(JSInteropConstants.Blur, this._inputRef);
        }
    }
}
