// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// Display mode. See <see cref="CalendarMode"/> for valid options
        /// </summary>
        /// <default value="CalendarMode.Month"/>
        [Parameter]
        public CalendarMode Mode { get; set; } = CalendarMode.Month;

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

        protected DatePickerType _picker;
        protected readonly DateTime[] PickerValues = new DateTime[] { DateTime.Now, DateTime.Now };
        protected Stack<DatePickerType> _prePickerStack = new Stack<DatePickerType>();

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

            _picker = Mode.Name switch
            {
                CalendarMode.MONTH => DatePickerType.Date,
                CalendarMode.YEAR => DatePickerType.Month,
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

        protected void OnSelectValue(DateTime date)
        {
            Value = date;

            OnSelect.InvokeAsync(date);
            OnChange.InvokeAsync(date);
            StateHasChanged();
        }

        internal void ChangeValue(DateTime date)
        {
            OnSelectValue(date);
        }

        internal void OnSelectValue(DateTime date, int index)
        {
            OnSelectValue(date);
        }

        internal void ChangeMode(CalendarMode mode)
        {
            Mode = mode;

            DatePickerType picker = mode.Name switch
            {
                DatePickerType.YEAR => DatePickerType.Month,
                DatePickerType.MONTH => DatePickerType.Date,
                _ => DatePickerType.Date,
            };

            _prePickerStack.Push(_picker);
            _picker = picker;

            OnPanelChange?.Invoke(PickerValues[0], _picker.Name);

            StateHasChanged();
        }

        public void ChangePickerType(DatePickerType type, int index)
        {
        }

        public void ChangePickerType(DatePickerType type)
        {
        }

        string IDatePicker.GetFormatValue(DateTime value, int index)
        {
            return value.ToString(CultureInfo);
        }

        void IDatePicker.ChangePlaceholder(string placeholder, int index)
        {
        }

        int IDatePicker.GetOnFocusPickerIndex()
        {
            return 0;
        }

        void IDatePicker.ResetPlaceholder(int index)
        {

        }

        void IDatePicker.Close()
        {

        }

        internal DatePickerType Picker { get { return _picker; } }
    }
}
