using System;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntCheckboxGroup : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public Action<object> _onChange;

        public Action _onTouched;

        [Parameter]
        public CheckBoxOption[] Options { get; set; } = Array.Empty<CheckBoxOption>();

        [Parameter]
        public bool Disabled { get; set; }

        public AntCheckboxGroup()
        {
            ClassMapper.Add("ant-checkbox-group");
        }

        public void OnOptionChange()
        {
            this._onChange?.Invoke(this.Options);
        }
    }
}
