using System;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntCheckboxGroupBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public Action<object> onChange;

        public Action onTouched;

        [Parameter]
        public CheckBoxOption[] options { get; set; } = Array.Empty<CheckBoxOption>();

        [Parameter]
        public bool disabled { get; set; }

        public AntCheckboxGroupBase()
        {
            ClassMapper.Add("ant-checkbox-group");
        }

        public void onOptionChange()
        {
            this.onChange(this.options);
        }
    }
}