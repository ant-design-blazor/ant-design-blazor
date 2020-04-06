using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntCheckboxGroupBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] public IList<AntCheckbox> CheckboxItems { get; set; } = new List<AntCheckbox>();

        [Parameter] public EventCallback<object> OnChange { get; set; }

        public Action onTouched;

        [Parameter]
        public CheckBoxOption[] options { get; set; } = Array.Empty<CheckBoxOption>();

        [Parameter]
        public IList<string> Value { get; set; } = Array.Empty<string>();

        protected override async Task OnParametersSetAsync()
        {
            foreach (var item in Value)
            {
                options.Where(o => o.value == item).ForEach(o => o.@checked = true);
            }
            await base.OnParametersSetAsync();
        }

        [Parameter]
        public bool disabled { get; set; }

        public AntCheckboxGroupBase()
        {
            ClassMapper.Add("ant-checkbox-group");
        }

        public async void onOptionChange(bool change)
        {
            await this.OnChange.InvokeAsync(this.options);
            StateHasChanged();
        }

        internal async Task OnCheckboxChange(AntCheckboxBase checkboxBase)
        {
            if (checkboxBase is AntCheckbox checkbox)
            {
                int index = CheckboxItems.IndexOf(checkbox);
                if (options[index] != null)
                {
                    options[index].@checked = checkbox.@checked;
                }
            }
            
            StateHasChanged();
        }
    }
}