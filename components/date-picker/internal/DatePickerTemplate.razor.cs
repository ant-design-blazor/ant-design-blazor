using System;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public partial class DatePickerTemplate<TValue> : DatePickerPanelBase<TValue>
    {
        private int _maxCol;
        private int _maxRow;
        private DateTime _viewStartDate;

        [Parameter]
        public RenderFragment RenderPickerHeader { get; set; }

        [Parameter]
        public RenderFragment RenderTableHeader { get; set; }

        [Parameter]
        public RenderFragment<DateTime> RenderFirstCol { get; set; }

        [Parameter]
        public RenderFragment<DateTime> RenderColValue { get; set; }

        [Parameter]
        public RenderFragment<DateTime> RenderLastCol { get; set; }

        [Parameter]
        public DateTime ViewStartDate
        {
            get { return _viewStartDate; }
            set
            {
                if (_viewStartDate != value)
                    _viewStartDate = value;
            }
        }

        [Parameter]
        public Func<DateTime, string> GetColTitle { get; set; }

        [Parameter]
        public Func<DateTime, string> GetRowClass { get; set; }

        [Parameter]
        public Func<DateTime, DateTime> GetNextColValue { get; set; }

        [Parameter]
        public Func<DateTime, bool> IsInView { get; set; }

        [Parameter]
        public Func<DateTime, bool> IsToday { get; set; }

        [Parameter]
        public Func<DateTime, bool> IsSelected { get; set; }

        [Parameter]
        public Action<DateTime> OnRowSelect { get; set; }

        [Parameter]
        public Action<DateTime> OnValueSelect { get; set; }

        [Parameter]
        public bool ShowFooter { get; set; } = false;

        [Parameter]
        public int MaxRow
        {
            get { return _maxRow; }
            set
            {
                if (_maxRow != value)
                    _maxRow = value;
            }
        }

        [Parameter]
        public int MaxCol
        {
            get { return _maxCol; }
            set
            {
                if (_maxCol != value)
                    _maxCol = value;
            }
        }

        [Parameter]
        public int SkipDays { get; set; }

        private void DateOnMouseEnter(DateTime hoverDateTime)
        {
            if (Value is not null)
            {
                return;
            }

            if (IsRange)
            {
                DatePicker.HoverDateTime = hoverDateTime;
            }

            int focusIndex = DatePicker.GetOnFocusPickerIndex();
            string placeholder = DatePicker.GetFormatValue(hoverDateTime, focusIndex);

            DatePicker.ChangePlaceholder(placeholder, focusIndex);
        }

        private void DateOnMouseLeave()
        {
            if (IsRange)
            {
                DatePicker.HoverDateTime = null;
            }

            DatePicker.ResetPlaceholder();
        }

        private bool IsDateInRange(DateTime currentColDate)
        {
            if (!IsRange ||
                !Picker.IsIn(DatePickerType.Date, DatePickerType.Year, DatePickerType.Month, DatePickerType.Quarter, DatePickerType.Week))
            {
                return false;
            }

            DateTime? startValue = FormatDateByPicker(GetIndexValue(0));
            DateTime? endValue = FormatDateByPicker(GetIndexValue(1));

            if (startValue == null || endValue == null)
            {
                return false;
            }

            if (Picker.IsIn(DatePickerType.Week))
            {
                startValue = startValue.Value.AddDays(6 - (int)startValue.Value.DayOfWeek);
            }

            DateTime currentDate = FormatDateByPicker(currentColDate);

            return ((DateTime)startValue).Date < currentDate.Date && currentDate.Date < ((DateTime)endValue).Date;
        }

        private string GetCellCls(DateTime currentColDate)
        {
            bool isInView = IsInView(currentColDate);
            bool isToday = IsToday(currentColDate);
            bool isSelected = IsSelected(currentColDate);
            bool isInRange = IsDateInRange(currentColDate);

            if (isInRange && Picker == DatePickerType.Year)
            {
                DateTime? endValue = FormatDateByPicker(GetIndexValue(1));
                if (endValue != null && ((DateTime)endValue).Year == currentColDate.Year)
                {
                    isInRange = false;
                }
            }

            string inViewCls = isInView ? $"{PrefixCls}-cell-in-view" : "";
            string todayCls = isToday ? $"{PrefixCls}-cell-today" : "";
            string selectedCls = isSelected ? $"{PrefixCls}-cell-selected" : "";
            string inRangeCls = isInRange ? $"{PrefixCls}-cell-in-range" : "";

            string disabledCls = GetDisabledCls(currentColDate);
            string rangeStartCls = GetRangeStartCls(currentColDate);
            string rangeEndCls = GetRangeEndCls(currentColDate);
            string rangeHoverCls = GetRangeHoverCls(currentColDate);
            string rangeEdgeCls = GetRangeEdgeCls(currentColDate);

            string rangeCls = $"{rangeStartCls} {rangeEndCls} {rangeHoverCls} {rangeEdgeCls}";

            return $"{PrefixCls}-cell {inViewCls} {todayCls} {selectedCls} {disabledCls} {inRangeCls} {rangeCls}";
        }

        private string GetInnerCellCls(DateTime currentColDate)
        {
            StringBuilder cls = new StringBuilder($"{PrefixCls}-cell-inner");

            if (IsCalendar)
            {
                cls.Append($" {PrefixCls}-calendar-date");

                if (IsToday(currentColDate))
                {
                    cls.Append($" {PrefixCls}-calendar-date-today");
                }
            }

            return cls.ToString();
        }

        private string GetRangeHoverCls(DateTime currentColDate)
        {
            if (!IsRange || DatePicker.HoverDateTime == null
                || Picker.IsIn(DatePickerType.Date, DatePickerType.Year, DatePickerType.Month, DatePickerType.Quarter) == false)
            {
                return "";
            }

            var startValue = FormatDateByPicker(GetIndexValue(0));
            var endValue = FormatDateByPicker(GetIndexValue(1));

            DateTime hoverDateTime = FormatDateByPicker((DateTime)DatePicker.HoverDateTime);

            if ((startValue != null && ((DateTime)startValue).Date == hoverDateTime.Date)
                || (endValue != null && ((DateTime)endValue).Date == hoverDateTime.Date))
            {
                return "";
            }

            DateTime currentDate = FormatDateByPicker(currentColDate);
            DateTime preDate = GetPreDate(currentDate);
            DateTime nextDate = GetNextDate(currentDate);

            int onfocusPickerIndex = DatePicker.GetOnFocusPickerIndex();

            StringBuilder cls = new StringBuilder();

            if (onfocusPickerIndex == 1 && startValue != null)
            {
                if (startValue != endValue && currentDate == startValue)
                {
                    cls.Append($" {PrefixCls}-cell-range-hover-start");
                }
                else if (currentDate < hoverDateTime)
                {
                    cls.Append($"{PrefixCls}-cell-range-hover");

                    // when pre day is not inview, then current day is the start.
                    if (IsInView(preDate) == false)
                    {
                        cls.Append($" {PrefixCls}-cell-range-hover-start");
                    }
                    // when next day is not inview, then current day is the end.
                    else if (IsInView(nextDate) == false)
                    {
                        cls.Append($" {PrefixCls}-cell-range-hover-end");
                    }
                }
                else if (currentDate == hoverDateTime)
                {
                    cls.Append($" {PrefixCls}-cell-range-hover-end");
                }
            }
            else if (onfocusPickerIndex == 0 && endValue != null)
            {
                if (startValue != endValue && currentDate == endValue)
                {
                    cls.Append($" {PrefixCls}-cell-range-hover-end");
                }
                else if (currentDate > hoverDateTime)
                {
                    cls.Append($"{PrefixCls}-cell-range-hover");

                    // when pre day is not inview, then current day is the start.
                    if (IsInView(preDate) == false)
                    {
                        cls.Append($" {PrefixCls}-cell-range-hover-start");
                    }
                    // when next day is not inview, then current day is the end.
                    else if (IsInView(nextDate) == false)
                    {
                        cls.Append($" {PrefixCls}-cell-range-hover-end");
                    }
                }
                else if (currentDate == hoverDateTime)
                {
                    cls.Append($" {PrefixCls}-cell-range-hover-start");
                }
            }

            return cls.ToString();
        }

        private string GetRangeStartCls(DateTime currentColDate)
        {
            if (!IsRange || Picker.IsIn(DatePickerType.Date, DatePickerType.Year, DatePickerType.Month, DatePickerType.Quarter) == false)
            {
                return "";
            }

            DateTime? startDate = FormatDateByPicker(GetIndexValue(0));

            if (startDate == null)
            {
                return "";
            }

            StringBuilder cls = new StringBuilder();

            DateTime? endDate = FormatDateByPicker(GetIndexValue(1));
            DateTime currentDate = FormatDateByPicker(currentColDate);
            DateTime? hoverDateTime = FormatDateByPicker(DatePicker.HoverDateTime);

            if (currentDate == startDate)
            {
                if (endDate == null)
                {
                    cls.Append($" {PrefixCls}-cell-range-start-single");
                }

                if (startDate != endDate || currentDate > hoverDateTime)
                {
                    cls.Append($" {PrefixCls}-cell-range-start");
                }

                if (currentDate > hoverDateTime)
                {
                    cls.Append($" {PrefixCls}-cell-range-start-near-hover");
                }
            }

            return cls.ToString();
        }

        private string GetRangeEndCls(DateTime currentColDate)
        {
            if (!IsRange || Picker.IsIn(DatePickerType.Date, DatePickerType.Year, DatePickerType.Month, DatePickerType.Quarter) == false)
            {
                return "";
            }

            DateTime? endDate = FormatDateByPicker(GetIndexValue(1));

            if (endDate == null)
            {
                return "";
            }

            StringBuilder cls = new StringBuilder();

            DateTime? startDate = FormatDateByPicker(GetIndexValue(0));
            DateTime currentDate = FormatDateByPicker(currentColDate);
            DateTime? hoverDateTime = FormatDateByPicker(DatePicker.HoverDateTime);

            if (currentDate == endDate)
            {
                if (startDate == null)
                {
                    cls.Append($" {PrefixCls}-cell-range-end-single");
                }

                if (startDate != endDate || currentDate < hoverDateTime)
                {
                    cls.Append($" {PrefixCls}-cell-range-end");
                }

                if (currentDate < hoverDateTime)
                {
                    cls.Append($" {PrefixCls}-cell-range-end");
                    cls.Append($" {PrefixCls}-cell-range-end-near-hover");
                }
            }

            return cls.ToString();
        }

        private string GetRangeEdgeCls(DateTime currentColDate)
        {
            if (!IsRange || Picker.IsIn(DatePickerType.Date, DatePickerType.Year, DatePickerType.Month, DatePickerType.Quarter) == false)
            {
                return "";
            }

            StringBuilder cls = new StringBuilder();

            DateTime currentDate = FormatDateByPicker(currentColDate);
            DateTime? hoverDateTime = FormatDateByPicker(DatePicker.HoverDateTime);

            DateTime preDate = GetPreDate(currentDate);
            DateTime nextDate = GetNextDate(currentDate);

            bool isCurrentDateInView = IsInView(currentColDate);

            if (isCurrentDateInView && !IsInView(preDate))
            {
                cls.Append($" {PrefixCls}-cell-range-hover-edge-start");
            }
            else if (isCurrentDateInView && !IsInView(nextDate))
            {
                cls.Append($" {PrefixCls}-cell-range-hover-edge-end");
            }

            DateTime? startDate = FormatDateByPicker(GetIndexValue(0));
            DateTime? endDate = FormatDateByPicker(GetIndexValue(1));
            if (startDate != null && endDate != null)
            {
                if (preDate == endDate)
                {
                    cls.Append($" {PrefixCls}-cell-range-hover-edge-start");
                    cls.Append($" {PrefixCls}-cell-range-hover-edge-start-near-range");
                }
                else if (nextDate == startDate)
                {
                    cls.Append($" {PrefixCls}-cell-range-hover-edge-end");
                    cls.Append($" {PrefixCls}-cell-range-hover-edge-end-near-range");
                }
            }

            return cls.ToString();
        }

        private string GetDisabledCls(DateTime currentColDate)
        {
            string disabledCls = "";

            if (DisabledDate?.Invoke(currentColDate) == true)
            {
                disabledCls = $"{PrefixCls}-cell-disabled";
            }

            return disabledCls;
        }

        private DateTime? FormatDateByPicker(DateTime? dateTime)
        {
            return DateHelper.FormatDateByPicker(dateTime, Picker);
        }

        private DateTime FormatDateByPicker(DateTime value)
        {
            return DateHelper.FormatDateByPicker(value, Picker);
        }

        private DateTime GetPreDate(DateTime dateTime)
        {
            return DateHelper.GetPreviousStartDateOfPeriod(dateTime, Picker);
        }

        private DateTime GetNextDate(DateTime dateTime)
        {
            return DateHelper.GetNextStartDateOfPeriod(dateTime, Picker);
        }

        private bool ShouldStopRenderDate(DateTime preDate, DateTime nextDate)
        {
            return Picker switch
            {
                DatePickerType.Date => DateHelper.IsSameDay(preDate, nextDate),
                DatePickerType.Year => DateHelper.IsSameYear(preDate, nextDate),
                DatePickerType.Month => DateHelper.IsSameMonth(preDate, nextDate),
                DatePickerType.Quarter => DateHelper.IsSameQuarter(preDate, nextDate),
                DatePickerType.Decade => DateHelper.IsSameYear(preDate, nextDate) || nextDate.Year == DateTime.MaxValue.Year,
                _ => false,
            };
        }
    }
}
