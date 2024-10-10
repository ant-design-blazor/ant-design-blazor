// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <h2>When To Use</h2>

    <list type="bullet">
        <item>Used to select a single state from multiple options.</item>
        <item>The difference from Select is that Radio is visible to the user and can facilitate the comparison of choice, which means there shouldn't be too many of them.</item>
    </list>
    </summary>
    <seealso cref="RadioGroup{TValue}"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataEntry, "https://gw.alipayobjects.com/zos/alicdn/8cYb5seNB/Radio.svg", Title = "Radio", SubTitle = "单选框")]
    public partial class Radio<TValue> : AntDomComponentBase
    {
        /// <summary>
        /// Display label content for the radio
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Selected value
        /// </summary>
        [Parameter]
        public TValue Value { get; set; }

        /// <summary>
        /// Autofocus or not
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool AutoFocus { get; set; } = false;

        /// <summary>
        /// Set to <c>true</c> to style the radio as button group.
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool RadioButton { get; set; }

        /// <summary>
        /// Specifies whether the radio is selected
        /// </summary>
        [Parameter]
        public bool Checked
        {
            get => _checked.GetValueOrDefault();
            set => _checked = value;
        }

        /// <summary>
        /// Specify if the radio button is checked initially or not
        /// </summary>
        /// <default value="false"/>
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

        /// <summary>
        /// Callback executed when the checked state changes
        /// </summary>
        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }

        /// <summary>
        /// Disable the radio buton
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Callback executed when the checked state changes
        /// </summary>
        [Obsolete("Use CheckedChanged instead")]
        [Parameter]
        public EventCallback<bool> CheckedChange { get; set; }

        [CascadingParameter]
        public RadioGroup<TValue> RadioGroup { get; set; }

        [CascadingParameter(Name = "InGroup")]
        public bool InGroup { get; set; }

        private readonly ClassMapper _radioClassMapper = new();
        private readonly ClassMapper _inputClassMapper = new();
        private readonly ClassMapper _innerClassMapper = new();

        private ElementReference _inputRef;

        private bool? _checked;

        private bool _hasDefaultChecked;
        private bool _defaultCheckedSetted;

        private string _name;
        private bool _defaultChecked;

        protected void SetClass()
        {
            var prefixCls = "ant-radio";
            var hashId = UseStyle(prefixCls, RadioStyle.UseComponentStyle);
            ClassMapper
                .Add(hashId)
                .If($"{prefixCls}-wrapper", () => !RadioButton)
                .If($"{prefixCls}-button-wrapper", () => RadioButton)
                .If($"{prefixCls}-wrapper-checked", () => Checked && !RadioButton)
                .If($"{prefixCls}-button-wrapper-checked", () => Checked && RadioButton)
                .If($"{prefixCls}-wrapper-disabled", () => Disabled && !RadioButton)
                .If($"{prefixCls}-button-wrapper-disabled", () => Disabled && RadioButton)
                .If($"{prefixCls}-button-wrapper-rtl", () => RTL);

            _radioClassMapper
                .If(prefixCls, () => !RadioButton)
                .Add(hashId)
                .If($"{prefixCls}-checked", () => Checked && !RadioButton)
                .If($"{prefixCls}-disabled", () => Disabled && !RadioButton)
                .If($"{prefixCls}-button", () => RadioButton)
                .If($"{prefixCls}-button-checked", () => Checked && RadioButton)
                .If($"{prefixCls}-button-disabled", () => Disabled && RadioButton)
                .If($"{prefixCls}-rtl", () => RTL);

            _inputClassMapper
                .Add(hashId)
                .If($"{prefixCls}-input", () => !RadioButton)
                .If($"{prefixCls}-button-input", () => RadioButton);

            _innerClassMapper
                .Add(hashId)
                .If($"{prefixCls}-inner", () => !RadioButton)
                .If($"{prefixCls}-button-inner", () => RadioButton);
        }

        internal void SetName(string name) => _name = name;

        internal void SetDisabledValue(bool value) => Disabled = value;

        protected override void OnInitialized()
        {
            SetClass();

            if (InGroup && RadioGroup == null)
            {
                throw new InvalidOperationException($"Please make sure that both RadioGroup and Radio have the same generic type: {typeof(TValue)} .");
            }

            RadioGroup?.AddRadio(this);

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
            if (!Checked)
            {
                this._checked = true;
                await CheckedChange.InvokeAsync(true);
                await CheckedChanged.InvokeAsync(true);
            }
        }

        internal async Task UnSelect()
        {
            if (Checked)
            {
                this._checked = false;
                await CheckedChange.InvokeAsync(false);
                await CheckedChanged.InvokeAsync(false);
            }
        }

        internal async Task OnClick()
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
