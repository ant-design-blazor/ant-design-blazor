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

        private string[] selectedValues;

        protected override void OnParametersSet()
        {
            //if (Value != null && Options.IsT0)
            //{
            //    foreach (var item in Value)
            //    {
            //        Options.AsT0.Where(o => o.Value == item).ForEach(o => o.Checked = true);
            //    }
            //}
        }

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
                selectedValues = Value;
            }

            selectedValues ??= Array.Empty<string>();
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

                selectedValues = Options.AsT0.Where(x => x.Checked).Select(x => x.Value).ToArray();
            }, opts =>
            {
                if (checkbox.Checked && !opts[index].IsIn(selectedValues))
                {
                    selectedValues = selectedValues.Append(opts[index]);
                }
                else
                {
                    selectedValues = selectedValues.Except(new[] { opts[index] }).ToArray();
                }
            });

            CurrentValue = selectedValues;

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(CurrentValue);
            }

            StateHasChanged();
        }
    }
}
