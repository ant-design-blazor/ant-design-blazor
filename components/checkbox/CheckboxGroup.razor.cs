using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class CheckboxGroup : AntInputComponentBase<OneOf<string[], Enum>>
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public OneOf<CheckboxOption[], string[], Enum> Options
        {
            get { return _options; }
            set
            {
                _options = value;
                if (_options.IsT2)
                {
                    _enumType = Options.AsT2.GetType();
                    _optionsAsArrayOfStrings = Enum.GetNames(_enumType);
                    _enumIsFlag = Attribute.IsDefined(_enumType, typeof(FlagsAttribute));
                }
            }
        }

        [Parameter]
        public EventCallback OnChange { get; set; }

        private OneOf<string[], Enum> _value;

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter]
        public override OneOf<string[], Enum> Value
        {
            get { return _value; }
            set
            {
                bool hasChanged = false;
                if (_value.IsT0 && value.IsT1)
                    _value = value;
                else
                {
                    value.Switch(
                        arrayType =>
                        {
                            hasChanged = !EqualityComparer<string[]>.Default.Equals(_value.AsT0, arrayType);
                        }, enumType =>
                        {
                            hasChanged = !EqualityComparer<Enum>.Default.Equals(_value.AsT1, enumType);
                        });
                }
                if (hasChanged)
                {
                    _value = value;
                    OnValueChange(value);
                }
            }
        }

        [Parameter] public override EventCallback<OneOf<string[], Enum>> ValueChanged { get; set; }

        private OneOf<string[], Enum> _selectedValues;
        private string[] _optionsAsArrayOfStrings;
        private bool _enumIsFlag;
        private Type _enumType;

        private IList<Checkbox> _checkboxItems;
        private OneOf<CheckboxOption[], string[], Enum> _options;

        [Parameter]
        public bool Disabled { get; set; }

        public CheckboxGroup()
        {
            ClassMapper.Add("ant-checkbox-group");
        }

        internal void AddItem(Checkbox checkbox)
        {
            this._checkboxItems ??= new List<Checkbox>();
            this._checkboxItems?.Add(checkbox);
        }

        internal void RemoveItem(Checkbox checkbox)
        {
            this._checkboxItems?.Remove(checkbox);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (Options.IsT0 && Options.Value is null && Value.IsT1 && Value.Value != null)
                Options = Value.AsT1;

            if (Value.Value == null)
            {
                if (Options.IsT2)
                    _value = (Enum)Activator.CreateInstance(Options.AsT2.GetType());
                else
                    _value = Array.Empty<string>();
            }
            else
            {
                _selectedValues = Value;
                if (Options.IsT0)
                {
                    Options.AsT0.ForEach(opt => opt.Checked = opt.Value.IsIn(_selectedValues.AsT0));
                }
            }
            if (_selectedValues.Value is null)
            {
                if (Options.IsT2)
                    _selectedValues = Value.AsT1; 
                else
                    _selectedValues = Array.Empty<string>();
            }
        }

        internal void OnCheckboxChange(Checkbox checkbox)
        {
            var index = _checkboxItems.IndexOf(checkbox);

            Options.Switch(opts =>
            {
                if (opts[index] != null)
                {
                    opts[index].Checked = checkbox.Checked;
                }

                CurrentValue = Options.AsT0.Where(x => x.Checked).Select(x => x.Value).ToArray();
            }, opts =>
            {
                if (checkbox.Checked && !opts[index].IsIn(_selectedValues.AsT0))
                {
                    _selectedValues = _selectedValues.AsT0.Append(opts[index]);
                }
                else
                {
                    _selectedValues = _selectedValues.AsT0.Except(new[] { opts[index] }).ToArray();
                }

                CurrentValue = _selectedValues;
            }, opts =>
            {
                int newEnumOptionAsInt = Convert.ToInt32(Enum.GetValues(_enumType).GetValue(index), CultureInfo.InvariantCulture);
                int selectedValuesAsInt = Convert.ToInt32(_selectedValues.AsT1, CultureInfo.InvariantCulture);
                if (checkbox.Checked)
                {
                    _selectedValues = (Enum)Enum.ToObject(_enumType, newEnumOptionAsInt + selectedValuesAsInt);
                }
                else
                {
                    _selectedValues = (Enum)Enum.ToObject(_enumType, selectedValuesAsInt - newEnumOptionAsInt);
                }
                CurrentValue = _selectedValues;
            });

            if (OnChange.HasDelegate)
            {
                CurrentValue.Switch(
                    arrayType => OnChange.InvokeAsync(arrayType),
                    enumType => OnChange.InvokeAsync(enumType)
                );
            }

            StateHasChanged();
        }

        protected override void OnValueChange(OneOf<string[], Enum> value)
        {
            if (ValueChanged.HasDelegate)
            {
                value.Switch(
                    arrayType => ValueChanged.InvokeAsync(arrayType),
                    enumType => ValueChanged.InvokeAsync(enumType)
                );
            }
        }
    }
}
