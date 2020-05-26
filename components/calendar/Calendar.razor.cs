using System;
using System.Threading.Tasks;
using AntBlazor.Internal;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class Calendar : DatePickerBase
    {
        [Parameter]
        public DateTime Value { get; set; } = DateTime.Now;

        [Parameter]
        public string Mode { get; set; } = DatePickerType.Date;

        [Parameter]
        public bool FullScreen { get; set; } = true;

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

        protected async Task OnSelect(DateTime date)
        {
            Value = date;

            StateHasChanged();
        }

        public void ChangeValue(DateTime date)
        {
            Value = date;

            StateHasChanged();
        }

        public string Picker { get { return _picker; } }
    }
}
