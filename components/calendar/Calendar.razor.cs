﻿using System;
using System.Threading.Tasks;
using AntBlazor.Internal;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class Calendar : DatePickerBase
    {
        [Parameter]
        public DateTime Value { get; set; } = DateTime.Now;

        private DateTime _defaultValue;
        [Parameter]
        public DateTime DefaultValue
        {
            get
            {
                return _defaultValue;
            }
            set
            {
                _defaultValue = value;
                Value = _defaultValue;
            }
        }

        [Parameter]
        public DateTime[] ValidRange { get; set; }

        [Parameter]
        public string Mode { get; set; } = DatePickerType.Month;

        [Parameter]
        public bool FullScreen { get; set; } = true;

        [Parameter]
        public EventCallback<DateTime> OnSelect { get; set; }

        [Parameter]
        public EventCallback<DateTime> OnChange { get; set; }

        [Parameter]
        public Func<CalendarHeaderRenderArgs, RenderFragment> HeaderRender { get; set; }

        [Parameter]
        public Func<DateTime, RenderFragment> DateCellRender { get; set; }

        [Parameter]
        public Func<DateTime, RenderFragment> DateFullCellRender { get; set; }

        [Parameter]
        public Func<DateTime, RenderFragment> MonthCellRender { get; set; }

        [Parameter]
        public Func<DateTime, RenderFragment> MonthFullCellRender { get; set; }

        public readonly string PrefixCls = "ant-picker-calendar";

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _picker = Mode switch
            {
                DatePickerType.Month => DatePickerType.Date,
                DatePickerType.Year => DatePickerType.Month,
                _ => DatePickerType.Date,
            };

            if (ValidRange != null)
            {
                if (Value < ValidRange[0])
                {
                    Value = ValidRange[0];
                }
                else if (Value > ValidRange[1])
                {
                    Value = ValidRange[1];
                }
            }
        }

        protected override void OnParametersSet()
        {
            this.SetClass();

            base.OnParametersSet();
        }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-full", () => FullScreen)
               ;
        }

        protected async Task OnSelectValue(DateTime date)
        {
            Value = date;

            await OnSelect.InvokeAsync(date);
            await OnChange.InvokeAsync(date);
            StateHasChanged();
        }

        public async Task ChangeValue(DateTime date)
        {
            await OnSelectValue(date);
            StateHasChanged();
        }

        public override void ChangePickerType(string type, int index)
        {
            Mode = type;

            string mode = type switch
            {
                DatePickerType.Year => DatePickerType.Month,
                DatePickerType.Month => DatePickerType.Date,
                _ => DatePickerType.Date,
            };

            base.ChangePickerType(mode, index);
        }

        public string Picker { get { return _picker; } }
    }
}
