using System;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public partial class DatePickerTemplate<TValue> : DatePickerPanelBase<TValue>
    {
        [Parameter]
        public RenderFragment RenderPickerHeader { get; set; }

        [Parameter]
        public RenderFragment RenderTableHeader { get; set; }

        [Parameter]
        public RenderFragment<DateTime> RenderFisrtCol { get; set; }

        [Parameter]
        public RenderFragment<DateTime> RenderColValue { get; set; }

        [Parameter]
        public RenderFragment<DateTime> RenderLastCol { get; set; }

        [Parameter]
        public DateTime ViewStartDate { get; set; }

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
        public int MaxRow { get; set; } = 0;

        [Parameter]
        public int MaxCol { get; set; } = 0;

        private void DateOnMouseEnter(DateTime hoverDateTime)
        {
            if (IsRange)
            {
                DatePicker.HoverDateTime = hoverDateTime;

                DatePicker.InvokeStateHasChanged();
            }
        }

        private void DateOnMouseLeave()
        {
            if (IsRange)
            {
                DatePicker.HoverDateTime = null;
            }
        }

        private bool IsDateInRange(DateTime currentColDate)
        {
            if (!IsRange || Picker.IsIn(DatePickerType.Date, DatePickerType.Year, DatePickerType.Month) == false)
            {
                return false;
            }

            DateTime? startValue = FormatDateByPicker(GetIndexValue(0));
            DateTime? endValue = FormatDateByPicker(GetIndexValue(1));

            if (startValue == null || endValue == null)
            {
                return false;
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

            string disabledCls = "";
            if (DisabledDate != null && DisabledDate(currentColDate))
            {
                disabledCls = $"{PrefixCls}-cell-disabled";
            }

            string rangeStartCls = GetRangeStartCls(currentColDate);
            string rangeEndCls = GetRangeEndCls(currentColDate);
            string rangeHoverCls = GetRangeHoverCls(currentColDate);
            string rangeEdgeCls = GetRangeEdgeCls(currentColDate);

            return $"{PrefixCls}-cell {inViewCls} {todayCls} {selectedCls} {disabledCls} {inRangeCls} {rangeStartCls} {rangeEndCls} {rangeHoverCls} {rangeEdgeCls}";
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
                || Picker.IsIn(DatePickerType.Date, DatePickerType.Year, DatePickerType.Month) == false)
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

            int onfocusPickerIndex = DatePicker.GetOnFocusPickerIndex();

            StringBuilder cls = new StringBuilder();

            if (onfocusPickerIndex == 1 && startValue != null)
            {
                if (currentDate.Date == ((DateTime)startValue).Date)
                {
                    cls.Append($" {PrefixCls}-cell-range-hover-start");
                }
                else if (currentDate.Date < hoverDateTime.Date)
                {
                    cls.Append($"{PrefixCls}-cell-range-hover");

                    // when pre day is not inview, then current day is the start.
                    if (IsInView(currentDate.AddDays(-1)) == false)
                    {
                        cls.Append($" {PrefixCls}-cell-range-hover-start");
                    }
                    // when next day is not inview, then current day is the end.
                    else if (IsInView(currentDate.AddDays(1)) == false)
                    {
                        cls.Append($" {PrefixCls}-cell-range-hover-end");
                    }
                }
                else if (currentDate.Date == hoverDateTime.Date)
                {
                    cls.Append($" {PrefixCls}-cell-range-hover-end");
                }
            }
            else if (onfocusPickerIndex == 0 && endValue != null)
            {
                if (currentDate.Date == ((DateTime)endValue).Date)
                {
                    cls.Append($" {PrefixCls}-cell-range-hover-end");
                }
                else if (currentDate.Date > hoverDateTime.Date)
                {
                    cls.Append($"{PrefixCls}-cell-range-hover");

                    // when pre day is not inview, then current day is the start.
                    if (IsInView(currentDate.AddDays(-1)) == false)
                    {
                        cls.Append($" {PrefixCls}-cell-range-hover-start");
                    }
                    // when next day is not inview, then current day is the end.
                    else if (IsInView(currentDate.AddDays(1)) == false)
                    {
                        cls.Append($" {PrefixCls}-cell-range-hover-end");
                    }
                }
                else if (currentDate.Date == hoverDateTime.Date)
                {
                    cls.Append($" {PrefixCls}-cell-range-hover-start");
                }
            }

            return cls.ToString();
        }

        private string GetRangeStartCls(DateTime currentColDate)
        {
            if (!IsRange || Picker.IsIn(DatePickerType.Date, DatePickerType.Year, DatePickerType.Month) == false)
            {
                return "";
            }

            string cls = "";

            DateTime? startDate = FormatDateByPicker(GetIndexValue(0));

            if (startDate == null)
            {
                return cls;
            }

            DateTime? endDate = FormatDateByPicker(GetIndexValue(1));
            DateTime currentDate = FormatDateByPicker(currentColDate);
            DateTime? hoverDateTime = FormatDateByPicker(DatePicker.HoverDateTime);

            if (currentDate.Date == ((DateTime)startDate).Date)
            {
                if (endDate == null)
                {
                    cls += $"{PrefixCls}-cell-range-start-single";
                }

                cls += $"{PrefixCls}-cell-range-start";

                if (hoverDateTime != null && currentDate.Date > ((DateTime)hoverDateTime).Date)
                {
                    cls += $" {PrefixCls}-cell-range-start-near-hover";
                }
            }

            return cls;
        }

        private string GetRangeEndCls(DateTime currentColDate)
        {
            if (!IsRange || Picker.IsIn(DatePickerType.Date, DatePickerType.Year, DatePickerType.Month) == false)
            {
                return "";
            }

            string cls = "";

            DateTime? endDate = FormatDateByPicker(GetIndexValue(1));

            if (endDate == null)
            {
                return cls;
            }

            DateTime? startDate = FormatDateByPicker(GetIndexValue(0));
            DateTime currentDate = FormatDateByPicker(currentColDate);
            DateTime? hoverDateTime = FormatDateByPicker(DatePicker.HoverDateTime);

            if (currentDate.Date == ((DateTime)endDate).Date)
            {
                if (startDate == null)
                {
                    cls += $"{PrefixCls}-cell-range-end-single";
                }

                cls += $"{PrefixCls}-cell-range-end";

                if (hoverDateTime != null && currentDate.Date < ((DateTime)hoverDateTime).Date)
                {
                    cls += $" {PrefixCls}-cell-range-end-near-hover";
                }
            }

            return cls;
        }

        private string GetRangeEdgeCls(DateTime currentColDate)
        {
            if (!IsRange || Picker.IsIn(DatePickerType.Date, DatePickerType.Year, DatePickerType.Month) == false)
            {
                return "";
            }

            string cls = "";

            DateTime currentDate = FormatDateByPicker(currentColDate);
            DateTime? hoverDateTime = FormatDateByPicker(DatePicker.HoverDateTime);

            DateTime preDate = Picker switch
            {
                DatePickerType.Date => currentDate.AddDays(-1),
                DatePickerType.Year => currentDate.AddYears(-1),
                DatePickerType.Month => currentDate.AddMonths(-1),
                _ => currentDate,
            };

            DateTime nextDate = Picker switch
            {
                DatePickerType.Date => currentDate.AddDays(1),
                DatePickerType.Year => currentDate.AddYears(1),
                DatePickerType.Month => currentDate.AddMonths(1),
                _ => currentDate,
            };

            bool isCurrentDateInView = IsInView(currentColDate);

            if (isCurrentDateInView && !IsInView(preDate))
            {
                cls += $"{PrefixCls}-cell-range-hover-edge-start";
            }
            else if (isCurrentDateInView && !IsInView(nextDate))
            {
                cls += $"{PrefixCls}-cell-range-hover-edge-end";
            }

            DateTime? startDate = FormatDateByPicker(GetIndexValue(0));
            DateTime? endDate = FormatDateByPicker(GetIndexValue(1));
            if (startDate != null && endDate != null)
            {
                if (preDate == endDate)
                {
                    cls += $" {PrefixCls}-cell-range-hover-edge-start";
                    cls += $" {PrefixCls}-cell-range-hover-edge-start-near-range";
                }
                else if (nextDate == startDate)
                {
                    cls += $" {PrefixCls}-cell-range-hover-edge-end";
                    cls += $" {PrefixCls}-cell-range-hover-edge-end-near-range";
                }
            }

            return cls;
        }

        private DateTime? FormatDateByPicker(DateTime? dateTime)
        {
            return DateHelper.FormatDateByPicker(dateTime, Picker);
        }

        private DateTime FormatDateByPicker(DateTime value)
        {
            return DateHelper.FormatDateByPicker(value, Picker);
        }
    }
}
