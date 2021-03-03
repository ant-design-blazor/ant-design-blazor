﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class CheckboxGroup : AntInputComponentBase<string[]>
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
                    _optionsAsArrayOfStrings = Enum.GetNames(Options.AsT2.GetType());
                }
            }
        }

        [Parameter]
        public EventCallback<string[]> OnChange { get; set; }

        private string[] _selectedValues;
        private string[] _optionsAsArrayOfStrings;


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

            if (Value != null)
            {
                _selectedValues = Value;
                if (Options.IsT0)
                {
                    Options.AsT0.ForEach(opt => opt.Checked = opt.Value.IsIn(_selectedValues));
                }
            }

            _selectedValues ??= Array.Empty<string>();
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
                if (checkbox.Checked && !opts[index].IsIn(_selectedValues))
                {
                    _selectedValues = _selectedValues.Append(opts[index]);
                }
                else
                {
                    _selectedValues = _selectedValues.Except(new[] { opts[index] }).ToArray();
                }

                CurrentValue = _selectedValues;
            }, opts =>
            {
                if (checkbox.Checked && !_optionsAsArrayOfStrings[index].IsIn(_selectedValues))
                {
                    _selectedValues = _selectedValues.Append(_optionsAsArrayOfStrings[index]);
                }
                else
                {
                    _selectedValues = _selectedValues.Except(new[] { _optionsAsArrayOfStrings[index] }).ToArray();
                }

                CurrentValue = _selectedValues;
            });

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(CurrentValue);
            }

            StateHasChanged();
        }
    }
}
