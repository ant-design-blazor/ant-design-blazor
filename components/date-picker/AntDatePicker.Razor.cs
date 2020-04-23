using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public partial class AntDatePicker : AntDomComponentBase
    {
        [Parameter] 
        public string PrefixCls { get; set; } = "ant-picker";

        private string _picker = AntDatePickerType.Date;
        [Parameter] 
        public string Picker { 
            get {
                return _picker;
            } 
            
            set {
                _prePicker = _picker;

                _picker = value;

                if(_initPicker == null)
                {
                    // note first picker type
                    _initPicker = value;

                    // set default placeholder
                    Placeholder = _initPicker switch
                    {
                        AntDatePickerType.Date => AntDatePickerPlaceholder.Date,
                        AntDatePickerType.Week => AntDatePickerPlaceholder.Week,
                        AntDatePickerType.Month => AntDatePickerPlaceholder.Month,
                        AntDatePickerType.Quarter => AntDatePickerPlaceholder.Quarter,
                        AntDatePickerType.Year => AntDatePickerPlaceholder.Year,
                        _ => "",
                    };
                }
            } 
        }

        [Parameter] 
        public bool Disabled { get; set; } = false;
        [Parameter]
        public bool Bordered { get; set; } = true;
        [Parameter]
        public bool AutoFocus { get; set; } = false;
        [Parameter]
        public bool Open { get; set; } = false;
        [Parameter]
        public bool InputReadOnly { get; set; } = false;
        [Parameter]
        public bool AllowClear { get; set; } = true; // TODO
        [Parameter]
        public string Placeholder { get; set; }
        [Parameter]
        public string PopupStyle { get; set; }
        [Parameter]
        public string ClassName { get; set; }
        [Parameter]
        public string DropdownClassName { get; set; }
        [Parameter]
        public string Size { get; set; } = AntDatePickerSize.Middle;
        [Parameter]
        public string Format { get; set; }
        [Parameter]
        public DateTime? DefaultValue { get; set; } = null;
        [Parameter]
        public RenderFragment SuffixIcon { get; set; }
        [Parameter]
        public Action<bool> OnOpenChange { get; set; }
        [Parameter]
        public Action<DateTime, string> OnPanelChange { get; set; }
        [Parameter]
        public Action<DateTime, string> OnChange { get; set; }
        [Parameter]
        public Action<DateTime, string> DisabledDate { get; set; } = null;
        [Parameter]
        public Func<DateTime, DateTime, RenderFragment> DateRender { get; set; }
        [Parameter]
        public DateTime Value { get; set; }

        public DateTime CurrentDate { get; private set; } = DateTime.Now;
        public DateTime CurrentShowDate { get; private set; } = DateTime.Now;

        private string _initPicker = null;
        private string _prePicker = null;
        private bool _hadSelectValue = false;
        private bool _isClose = true;

        protected override void OnInitialized()
        {
            this.SetClass();

            base.OnInitialized();
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
                .If($"{PrefixCls}-borderless", () => Bordered == false)
                .If($"{PrefixCls}-disabled", () => Disabled == true)
                .If($"{ClassName}", () => !string.IsNullOrEmpty(ClassName))
               //.If($"{PrefixCls}-focused", () => AutoFocus == true)
               //.If($"{PrefixCls}-normal", () => Image.IsT1 && Image.AsT1 == AntEmpty.PRESENTED_IMAGE_SIMPLE)
               //.If($"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
               ;
        }

        protected void OnSelect(DateTime date)
        {
            // InitPicker is the finally value
            if (Picker == _initPicker)
            {
                Value = date;

                OnChange?.Invoke(date, date.ToString(Format));

                _hadSelectValue = true;

                _isClose = true;

                StateHasChanged();
            }
            else
            {
                Picker = _prePicker;

                ChangeShowDate(date);
            }
        }

        protected string GetInputValue()
        {
            DateTime value;

            if (_hadSelectValue)
            {
                value = Value;
            }
            else if (DefaultValue != null)
            {
                value = (DateTime)DefaultValue;
            }
            else
            {
                return "";
            }


            if (!string.IsNullOrEmpty(Format))
            {
                // TODO：Locale
                return value.ToString(Format);
            }

            // TODO：Locale
            string formatValue = Picker switch
            {
                AntDatePickerType.Date => value.ToString("yyyy-MM-dd"),
                AntDatePickerType.Week => $"{value.Year}-{DateHelper.GetWeekOfYear(value)}周",
                AntDatePickerType.Month => value.ToString("yyyy-MM"),
                AntDatePickerType.Quarter => $"{value.Year}-{DateHelper.GetDayOfQuarter(value)}",
                AntDatePickerType.Year => value.ToString("yyyy"),
                _ => value.ToString("yyyy-MM-dd"),
            };

            return formatValue;
        }

        protected void OpenOrClose()
        {
            _isClose = !_isClose;

            OnOpenChange?.Invoke(!_isClose);
        }

        public void ChangeShowDate(DateTime date)
        {
            CurrentShowDate = date;

            OnPanelChange?.Invoke(CurrentShowDate, Picker);

            StateHasChanged();
        }

        public void ChangePickerType(string type)
        {
            Picker = type;

            OnPanelChange?.Invoke(CurrentShowDate, Picker);

            StateHasChanged();
        }
    }
}
