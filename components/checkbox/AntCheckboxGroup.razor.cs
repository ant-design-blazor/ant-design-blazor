using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntCheckboxGroup : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public IList<AntCheckbox> CheckboxItems { get; set; } = new List<AntCheckbox>();

        [Parameter]
        public EventCallback<string[]> ValueChanged { get; set; }

        [Parameter]
#pragma warning disable CA1819 // 属性不应返回数组
        public CheckBoxOption[] Options { get; set; }

#pragma warning restore CA1819 // 属性不应返回数组

        [Parameter]
        public IList<string> Value { get; set; } = Array.Empty<string>();

        protected override void OnParametersSet()
        {
            foreach (string item in Value)
            {
                Options.Where(o => o.Value == item).ForEach(o => o.Checked = true);
            }
        }

        [Parameter]
        public bool Disabled { get; set; }

        public AntCheckboxGroup()
        {
            ClassMapper.Add("ant-checkbox-group");
        }

        public async void OnOptionChange()
        {
            await this.ValueChanged.InvokeAsync(this.Options.Where(x => x.Checked).Select(x => x.Value).ToArray());
            StateHasChanged();
        }

        internal void OnCheckboxChange(AntCheckbox checkboxBase)
        {
            if (checkboxBase is AntCheckbox checkbox)
            {
                int index = CheckboxItems.IndexOf(checkbox);
                if (Options[index] != null)
                {
                    Options[index].Checked = checkbox.Checked;
                }
            }

            StateHasChanged();
        }
    }
}
