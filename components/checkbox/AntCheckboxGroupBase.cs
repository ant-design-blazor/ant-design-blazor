using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntCheckboxGroupBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] public List<AntCheckbox> CheckboxItems { get; set; } = new List<AntCheckbox>();

        [Parameter] public EventCallback<object> OnChange { get; set; }

        public Action onTouched;

        [Parameter]
        public CheckBoxOption[] options { get; set; } = Array.Empty<CheckBoxOption>();

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