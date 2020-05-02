using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntCheckbox : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private ElementReference _inputElement;
        private ElementReference _contentElement;

        [Parameter]
        public EventCallback<bool> CheckedChange { get; set; }

        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public bool AutoFocus { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Indeterminate { get; set; }

        [Parameter]
        public bool Checked { get; set; }

        [CascadingParameter]
        public AntCheckboxGroup CheckboxGroup { get; set; }

        protected Dictionary<string, object> InputAttributes { get; set; }

        protected override void OnParametersSet()
        {
            SetClass();
            base.OnParametersSet();
        }

        protected override void OnInitialized()
        {
            if (this is AntCheckbox checkbox)
            {
                CheckboxGroup?.CheckboxItems.Add(checkbox);
            }
        }

        protected void SetClass()
        {
            string prefixName = "ant-checkbox";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-checked", () => Checked && !Indeterminate)
                .If($"{prefixName}-disabled", () => Disabled)
                .If($"{prefixName}-indeterminate", () => Indeterminate);
        }

        protected async Task InputCheckedChange(ChangeEventArgs args)
        {
            if (args != null && args.Value is bool value)
            {
                await InnerCheckedChange(value);
            }
        }

        protected async Task InnerCheckedChange(bool @checked)
        {
            if (!this.Disabled)
            {
                this.Checked = @checked;
                await this.CheckedChange.InvokeAsync(this.Checked);
                CheckboxGroup?.OnCheckboxChange(this);
            }
        }

        protected void UpdateAutoFocus()
        {
            if (this.AutoFocus)
            {
                InputAttributes.Add("autofocus", "autofocus");
            }
            else
            {
                InputAttributes.Remove("autofocus");
            }
        }

        protected void WriteValue(bool value)
        {
            this.Checked = value;
        }
    }
}
