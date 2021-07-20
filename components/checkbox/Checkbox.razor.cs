﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

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

        [CascadingParameter] public CheckboxGroup CheckboxGroup { get; set; }

        internal bool IsFromOptions { get; set; }
        private bool _isInitalized;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClass();
            CheckboxGroup?.AddItem(this);
            _isInitalized = true;
        }

        protected override void Dispose(bool disposing)
        {
            CheckboxGroup?.RemoveItem(this);
            base.Dispose(disposing);
        }

        protected ClassMapper ClassMapperLabel { get; } = new ClassMapper();

        private string _prefixCls = "ant-checkbox";
        protected void SetClass()
        {
            ClassMapperLabel.Clear()
                .Add($"{_prefixCls}-wrapper")
                .If($"{_prefixCls}-wrapper-checked", () => Checked);

            ClassMapper.Clear()
                .Add(_prefixCls)
                .If($"{_prefixCls}-checked", () => Checked && !Indeterminate)
                .If($"{_prefixCls}-disabled", () => Disabled)
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

        internal void SetValue(bool value) => Checked = value;

    }
}
