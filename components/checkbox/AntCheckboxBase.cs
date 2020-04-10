using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntBlazor
{
    public class AntCheckboxBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected Action<bool> onChange;

        protected Func<object> onTouched;

        protected ElementReference inputElement;

        protected ElementReference contentElement;

        [Parameter]
        public EventCallback<bool> checkedChange { get; set; }

        [Parameter]
        public string value { get; set; }

        [Parameter]
        public bool autoFocus { get; set; }

        [Parameter]
        public bool disabled { get; set; }

        [Parameter]
        public bool indeterminate { get; set; }

        [Parameter]
        public bool @checked { get; set; }

        [CascadingParameter]
        public AntCheckboxGroup CheckboxGroup { get; set; }

        protected Dictionary<string, object> inputAttributes { get; set; }

        protected string contentStyles = "";

        protected override void OnParametersSet()
        {
            setClass();
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            if (this is AntCheckbox checkbox)
            {
                CheckboxGroup?.CheckboxItems.Add(checkbox);
            }
            await base.OnInitializedAsync();
        }

        protected void setClass()
        {
            var prefixName = "ant-checkbox";
            ClassMapper.Clear()
                .Add(prefixName)
                .If($"{prefixName}-checked", () => @checked && !indeterminate)
                .If($"{prefixName}-disabled", () => disabled)
                .If($"{prefixName}-indeterminate", () => indeterminate);
        }

        protected async Task inputCheckedChange(ChangeEventArgs args)
        {
            await innerCheckedChange(Convert.ToBoolean(args.Value));
        }

        protected async Task innerCheckedChange(bool @checked)
        {
            if (!this.disabled)
            {
                this.@checked = @checked;
                onChange?.Invoke(this.@checked);
                await this.checkedChange.InvokeAsync(this.@checked);
                CheckboxGroup?.OnCheckboxChange(this);
            }
        }

        protected void updateAutoFocus()
        {
            if (this.autoFocus)
            {
                inputAttributes.Add("autofocus", "autofocus");
            }
            else
            {
                inputAttributes.Remove("autofocus");
            }
        }

        protected void writeValue(bool value)
        {
            this.@checked = value;
        }
    }
}