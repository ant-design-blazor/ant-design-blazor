using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Checkbox : AntInputComponentBase<bool>
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        private ElementReference _inputElement;
        private ElementReference _contentElement;
        private bool _checked;

        [Parameter] public EventCallback<bool> CheckedChange { get; set; }

        [Parameter] public Expression<Func<bool>> CheckedExpression { get; set; }

        [Parameter] public bool AutoFocus { get; set; }

        [Parameter] public bool Disabled { get; set; }

        [Parameter] public bool Indeterminate { get; set; }

        [Parameter]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    CurrentValue = value;
                }
            }
        }

        [Parameter]
        public string Label { get; set; }

        [CascadingParameter]
        public CheckboxGroup CheckboxGroup { get; set; }

        protected Dictionary<string, object> InputAttributes { get; set; }

        protected override void OnParametersSet()
        {
            SetClass();
            base.OnParametersSet();
        }

        protected override void OnInitialized()
        {
            CheckboxGroup?.AddItem(this);

            if (Checked)
            {
                Value = Checked;
            }
        }

        protected override void Dispose(bool disposing)
        {
            CheckboxGroup?.RemoveItem(this);

            base.Dispose(disposing);
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

        protected override void OnValueChange(bool value)
        {
            base.OnValueChange(value);
            this._checked = value;
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
                InputAttributes.Add("autofocus", "autofocus");
            }
            else
            {
                InputAttributes.Remove("autofocus");
            }
        }
    }
}
