using System;
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
        public IList<Checkbox> CheckboxItems { get; set; } = new List<Checkbox>();

        [Parameter]
        public OneOf<CheckboxOption[], string[]> Options { get; set; }

        [Parameter]
        public EventCallback<string[]> OnChange { get; set; }

        private string[] _selectedValues;

        [Parameter]
        public bool Disabled { get; set; }

        public CheckboxGroup()
        {
            ClassMapper.Add("ant-checkbox-group");
        }

        public async void OnCheckedChange()
        {
            //StateHasChanged();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Value != null)
            {
                _selectedValues = Value;
            }

            _selectedValues ??= Array.Empty<string>();
        }

        internal void OnCheckboxChange(Checkbox checkbox)
        {
            var index = CheckboxItems.IndexOf(checkbox);

            Options.Switch(opts =>
            {
                if (opts[index] != null)
                {
                    opts[index].Checked = checkbox.Checked;
                }

                _selectedValues = Options.AsT0.Where(x => x.Checked).Select(x => x.Value).ToArray();
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
            });

            CurrentValue = _selectedValues;

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(CurrentValue);
            }

            StateHasChanged();
        }
    }
}
