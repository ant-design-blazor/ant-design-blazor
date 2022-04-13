﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class RadioGroup<TValue> : AntInputComponentBase<TValue>
    {

        [Inject]
        private IComponentIdGenerator ComponentIdGenerator { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Disabled
        {
            get => _disabled;
            set
            {
                if (_disabled != value)
                {
                    _disabled = value;
                    OnDisabledValueChanged?.Invoke(_disabled);
                }
            }
        }

        [Parameter]
        public RadioButtonStyle ButtonStyle { get; set; } = RadioButtonStyle.Outline;

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public TValue DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                _hasDefaultValue = true;
            }
        }

        [Parameter]
        public EventCallback<TValue> OnChange { get; set; }

        [Parameter]
        public OneOf<string[], RadioOption<TValue>[]> Options { get; set; }

        private bool _disabled;
        private Action<bool> OnDisabledValueChanged { get; set; }

        private TValue _defaultValue;

        private bool _hasDefaultValue;
        private bool _defaultValueSetted;

        private readonly List<Radio<TValue>> _radioItems = new();

        private static readonly Dictionary<RadioButtonStyle, string> _buttonStyleDics = new()
        {
            [RadioButtonStyle.Outline] = "outline",
            [RadioButtonStyle.Solid] = "solid",
        };

        protected override void OnInitialized()
        {
            string prefixCls = "ant-radio-group";
            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-large", () => Size == "large")
                .If($"{prefixCls}-small", () => Size == "small")
                .GetIf(() => $"{prefixCls}-{_buttonStyleDics[ButtonStyle]}", () => ButtonStyle.IsIn(RadioButtonStyle.Outline, RadioButtonStyle.Solid))
                .If($"{prefixCls}-rtl", () => RTL)
                ;

            base.OnInitialized();

            if (_hasDefaultValue && !_defaultValueSetted)
            {
                CurrentValue = _defaultValue;
                _defaultValueSetted = true;
            }

            if (string.IsNullOrEmpty(Name))
            {
                Name = PropertyName ?? ComponentIdGenerator.Generate(this);
            }
        }

        internal async Task AddRadio(Radio<TValue> radio)
        {
            if (this.Name != null)
            {
                radio.SetName(Name);
            }

            _radioItems.Add(radio);
            // If the current radio has been already disabled, this radio group won't sync the value of `Disabled`.
            if (!radio.Disabled)
            {
                radio.SetDisabledValue(_disabled);
                OnDisabledValueChanged += radio.SetDisabledValue;
            }
            if (EqualsValue(this.CurrentValue, radio.Value))
            {
                await radio.Select();
                StateHasChanged();
            }
        }

        internal void RemoveRadio(Radio<TValue> radio)
        {
            _radioItems.Remove(radio);
            OnDisabledValueChanged -= radio.SetDisabledValue;
        }

        protected override async Task OnParametersSetAsync()
        {
            foreach (var radio in _radioItems)
            {
                if (EqualsValue(this.CurrentValue, radio.Value))
                {
                    await radio.Select();
                }
                else
                {
                    await radio.UnSelect();
                }
            }
            await base.OnParametersSetAsync();
        }

        internal async Task OnRadioChange(TValue value)
        {
            var oldValue = CurrentValue;
            // If the current value changes, it will invoke `ValueChanged` among property-set method.
            CurrentValue = value;
            // Have to check equal again in order to decide whether invoking `OnChange` or not.
            if (!EqualsValue(oldValue, CurrentValue))
            {
                if (OnChange.HasDelegate)
                {
                    await OnChange.InvokeAsync(value);
                }
            }
        }

        private static bool EqualsValue(TValue left, TValue right)
            => EqualityComparer<TValue>.Default.Equals(left, right);
    }
}
