// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /**
    <summary>
    <para>Container for displaying data in calendar form.</para>

    <h2>When To Use</h2>

    <para>When data is in the form of dates, such as schedules, timetables, prices calendar, lunar calendar. This component also supports Year/Month switch.</para>
    </summary>
     */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/antfincdn/dPQmLq08DI/Calendar.svg", Columns = 1)]
    public partial class Calendar : AntDomComponentBase, IDatePicker
    {
        DateTime IDatePicker.CurrentDate { get; set; } = DateTime.Now;
        DateTime? IDatePicker.HoverDateTime { get; set; }

        /// <summary>
        /// Selected value for calendar
        /// </summary>
        /// <default value="DateTime.Now"/>
        [Parameter]
        public DateTime Value { get; set; } = DateTime.Now;

        private DateTime _defaultValue;

        /// <summary>
        /// Default value for selected date. When set, will set <see cref="Value"/>
        /// </summary>
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

        /// <summary>
        /// Validate range of dates or selection
        /// </summary>
        [Parameter]
        public DateTime[] ValidRange { get; set; }

        /// <summary>
        /// Display mode. See <see cref="DatePickerType"/> for valid options
        /// </summary>
        /// <default value="DatePickerType.Month"/>
        [Parameter]
        public string Mode { get; set; } = DatePickerType.Month;

        /// <summary>
        /// Whether the calendar should take up all available space or not
        /// </summary>
        /// <default value="true"/>
        [Parameter]
        public bool FullScreen { get; set; } = true;

        /// <summary>
        /// Callback executed when a date is selected
        /// </summary>
        [Obsolete("Use OnChange instead")]
        [Parameter]
        public EventCallback<DateTime> OnSelect { get; set; }

        /// <summary>
        /// Callback executed when a date is selected
        /// </summary>
        [Parameter]
        public EventCallback<DateTime> OnChange { get; set; }

        /// <summary>
        /// Function to render a custom header
        /// </summary>
        [Parameter]
        public Func<CalendarHeaderRenderArgs, RenderFragment> HeaderRender { get; set; }

        /// <summary>
        /// Customize the display of the date cell, the returned content will be appended to the cell
        /// </summary>
        [Parameter]
        public Func<DateTime, RenderFragment> DateCellRender { get; set; }

        /// <summary>
        /// Customize the display of the date cell, the returned content will override the cell
        /// </summary>
        [Parameter]
        public Func<DateTime, RenderFragment> DateFullCellRender { get; set; }

        /// <summary>
        /// Customize the display of the month cell, the returned content will be appended to the cell
        /// </summary>
        [Parameter]
        public Func<DateTime, RenderFragment> MonthCellRender { get; set; }

        /// <summary>
        /// Customize the display of the month cell, the returned content will override the cell
        /// </summary>
        [Parameter]
        public Func<DateTime, RenderFragment> MonthFullCellRender { get; set; }

        /// <summary>
        /// Callback executed when the type of calendar being viewed changes
        /// </summary>
        [Parameter]
        public Action<DateTime, string> OnPanelChange { get; set; }

        /// <summary>
        /// Function to determine if a specific date is disabled
        /// </summary>
        [Parameter]
        public Func<DateTime, bool> DisabledDate { get; set; } = null;

        /// <summary>
        /// Locale information for UI and date formatting
        /// </summary>
        [Parameter]
        public DatePickerLocale Locale { get; set; } = LocaleProvider.CurrentLocale.DatePicker;

        /// <summary>
        /// Culture information used for formatting
        /// </summary>
        [Parameter]
        public CultureInfo CultureInfo { get; set; } = LocaleProvider.CurrentLocale.CurrentCulture;

        protected string _picker;
        protected readonly DateTime[] PickerValues = new DateTime[] { DateTime.Now, DateTime.Now };
        protected Stack<string> _prePickerStack = new Stack<string>();

        internal readonly string PrefixCls = "ant-picker-calendar";

        event EventHandler<bool> IDatePicker.OverlayVisibleChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

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
                .If($"{PrefixCls}-rtl", () => RTL)
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

        public int GetOnFocusPickerIndex()
        {
            return 0;
        }

        string IDatePicker.GetFormatValue(DateTime value, int index)
        {
            return value.ToString(CultureInfo);
        }

        void IDatePicker.ChangePlaceholder(string placeholder, int index)
        {
        }

        public void ResetPlaceholder(int rangePickerIndex = -1)
        {
        }

        public string Picker { get { return _picker; } }
    }
}
