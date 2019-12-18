using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public class AntCheckboxBase : AntInputComponentBase<bool>
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

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

        protected Action<bool> onChange;

        protected Func<object> onTouched;

        protected ElementReference inputElement;

        protected ElementReference contentElement;

        protected Dictionary<string, object> inputAttributes { get; set; }

        protected async Task innerCheckedChange(bool @checked)
        {
            if (!this.disabled)
            {
                this.@checked = @checked;
                this.onChange(this.@checked);
                await this.checkedChange.InvokeAsync(this.@checked);
                //if (this.checkboxWrapperComponent)
                //{
                //    this.nzCheckboxWrapperComponent.onChange();
                //}
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

        protected void registerOnChange(Action<bool> fn)
        {
            if (fn != null)
            {
                this.onChange = fn;
            }
        }

        protected void registerOnTouched(Func<object> fn)
        {
            if (fn != null)
            {
                this.onTouched = fn;
            }
        }

        protected void setDisabledState(bool isDisabled)
        {
            this.disabled = isDisabled;
        }
    }
}