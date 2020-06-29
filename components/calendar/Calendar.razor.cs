using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Calendar : AntDomComponentBase, IDatePicker
    {
        DateTime IDatePicker.CurrentDate { get; set; } = DateTime.Now;

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

        [Parameter]
        public Action<DateTime, string> OnPanelChange { get; set; }

        [Parameter]
        public Func<DateTime, bool> DisabledDate { get; set; } = null;

        protected string _picker;
        protected readonly DateTime[] PickerValues = new DateTime[] { DateTime.Now, DateTime.Now };
        protected Stack<string> _prePickerStack = new Stack<string>();

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

        public void ChangePickerType(string type, int index)
        {
            Mode = type;

            string mode = type switch
            {
                DatePickerType.Year => DatePickerType.Month,
                DatePickerType.Month => DatePickerType.Date,
                _ => DatePickerType.Date,
            };

            _prePickerStack.Push(_picker);
            _picker = mode;

            OnPanelChange?.Invoke(PickerValues[index], _picker);

            StateHasChanged();
        }

        public void ChangePickerType(string type)
        {
            ChangePickerType(type, 0);
        }

        public void Close()
        {
        }

        public string Picker { get { return _picker; } }

        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;
    }
}
