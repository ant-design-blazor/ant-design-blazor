using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using AntDesign.Internal;
using System.Collections.Generic;
using System.Linq;

namespace AntDesign
{
    public partial class Checkbox : AntInputBoolComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        //[Obsolete] attribute does not work with [Parameter] for now. Tracking issue: https://github.com/dotnet/aspnetcore/issues/30967
        [Obsolete("Instead use @bing-Checked or EventCallback<bool> CheckedChanged .")]
        [Parameter]
        public EventCallback<bool> CheckedChange { get; set; }

        [Parameter] public Expression<Func<bool>> CheckedExpression { get; set; }

        [Parameter] public bool Indeterminate { get; set; }

        [Parameter] public string Label { get; set; }

        [CascadingParameter] private ICheckboxGroup CheckboxGroup { get; set; }

        [CascadingParameter(Name = "ItemValue")]
        internal object ItemValue { get; set; }

        internal bool IsFromOptions { get; set; }

        private bool IsDisabled => Disabled || (CheckboxGroup?.Disabled ?? false);

        private Dictionary<string, object> _attributes;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();
            CheckboxGroup?.AddItem(this);
        }

        protected override void Dispose(bool disposing)
        {
            CheckboxGroup?.RemoveItem(this);
            base.Dispose(disposing);
        }

        protected override void OnParametersSet()
        {
            _attributes = AdditionalAttributes?.ToDictionary(x => x.Key, x => x.Value) ?? [];

            var name = CheckboxGroup?.NameAttributeValue ?? NameAttributeValue;
            if (!string.IsNullOrWhiteSpace(name))
            {
                _attributes.TryAdd("name", name);
            }
            base.OnParametersSet();
        }

        protected ClassMapper ClassMapperLabel { get; } = new ClassMapper();

        private string _prefixCls = "ant-checkbox";

        protected void SetClass()
        {
            ClassMapperLabel
                .Add($"{_prefixCls}-wrapper")
                .If($"{_prefixCls}-wrapper-checked", () => Checked)
                .If($"{_prefixCls}-wrapper-disabled", () => IsDisabled)
                .If($"{_prefixCls}-group-item", () => CheckboxGroup != null);

            ClassMapper
                .Add(_prefixCls)
                .If($"{_prefixCls}-checked", () => Checked && !Indeterminate)
                .If($"{_prefixCls}-disabled", () => IsDisabled)
                .If($"{_prefixCls}-indeterminate", () => Indeterminate)
                .If($"{_prefixCls}-rtl", () => RTL);
        }

        protected async Task InputCheckedChange(ChangeEventArgs args)
        {
            if (args != null && args.Value is bool value)
            {
                await base.ChangeValue(value);

                if (CheckedChange.HasDelegate) //kept for compatibility reasons with previous versions
                    await CheckedChange.InvokeAsync(value);
                CheckboxGroup?.OnCheckboxChange(this);
            }
        }

        internal void SetValue(bool value)
        {
            if (value == Checked)
                return;

            Checked = value;
            StateHasChanged();
        }

        internal void SetItemValue(object itemValue) => ItemValue = itemValue;
    }
}
