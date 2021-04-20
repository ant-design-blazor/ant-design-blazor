using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AntDesign.core.JsInterop.EventArg;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Checkbox : AntInputComponentBase<bool>
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public EventCallback<bool> CheckedChange { get; set; }

        /// <summary>
        /// Gets or sets a callback that updates the bound checked value.
        /// </summary>
        [Parameter] public EventCallback<bool> CheckedChanged { get; set; }

        [Parameter] public Expression<Func<bool>> CheckedExpression { get; set; }

        [Parameter] public bool AutoFocus { get; set; }

        [Parameter] public bool Disabled { get; set; }

        [Parameter] public bool Indeterminate { get; set; }

        [Parameter]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                if (_checked != Value)
                {
                    Value = _checked;
                }
            }
        }

        [Parameter]
        public string Label { get; set; }

        [CascadingParameter]
        public CheckboxGroup CheckboxGroup { get; set; }

        protected Dictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

        private bool _checked;

        protected override void OnParametersSet()
        {
            SetClass();
            base.OnParametersSet();
        }

        protected override void OnInitialized()
        {
            UpdateAutoFocus();
            CheckboxGroup?.AddItem(this);
        }

        protected override void Dispose(bool disposing)
        {
            CheckboxGroup?.RemoveItem(this);
            base.Dispose(disposing);
        }

        protected ClassMapper ClassMapperSpan { get; } = new ClassMapper();

        protected void SetClass()
        {
            string prefixName = "ant-checkbox";
            ClassMapper.Clear()
                .Add(prefixName)
                .Add($"{prefixName}-wrapper")
                .If($"{prefixName}-wrapper-checked", () => Checked);

            ClassMapperSpan.Clear()
                .Add(prefixName)
                .If($"{prefixName}-checked", () => Checked && !Indeterminate)
                .If($"{prefixName}-disabled", () => Disabled)
                .If($"{prefixName}-indeterminate", () => Indeterminate)
                .If($"{prefixName}-rtl", () => RTL);
        }

        protected override void OnValueChange(bool value)
        {
            base.OnValueChange(value);
            this.CurrentValue = value;
            Checked = value;
            CheckedChanged.InvokeAsync(value);
        }

        protected async Task InputCheckedChange(ChangeEventArgs args)
        {
            if (args != null && args.Value is bool value)
            {
                CurrentValue = value;
                await InnerCheckedChange(value);
            }
        }

        protected async Task InnerCheckedChange(bool @checked)
        {
            if (!this.Disabled)
            {
                await CheckedChanged.InvokeAsync(@checked);
                if (this.CheckedChange.HasDelegate)
                {
                    await this.CheckedChange.InvokeAsync(@checked);
                }
                CheckboxGroup?.OnCheckboxChange(this);
            }
        }

        protected void UpdateAutoFocus()
        {
            if (this.AutoFocus)
            {
                if (InputAttributes.ContainsKey("autofocus") == false)
                    InputAttributes.Add("autofocus", "autofocus");
            }
            else
            {
                if (InputAttributes.ContainsKey("autofocus") == true)
                    InputAttributes.Remove("autofocus");
            }
        }
    }
}
