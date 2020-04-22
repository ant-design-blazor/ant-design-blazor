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
                prePicker = _picker;

                _picker = value;

                if(initPicker == null)
                {
                    // note first picker type
                    initPicker = value;

                    // set default placeholder
                    Placeholder = initPicker switch
                    {
                        AntDatePickerType.Date => AntDatePickerPlaceholder.Date,
                        AntDatePickerType.Week => AntDatePickerPlaceholder.Date,
                        AntDatePickerType.Month => AntDatePickerPlaceholder.Date,
                        AntDatePickerType.Quarter => AntDatePickerPlaceholder.Date,
                        AntDatePickerType.Year => AntDatePickerPlaceholder.Date,
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
        public bool AllowClear { get; set; } = true; // TODO
        [Parameter]
        public string Placeholder { get; set; }
        [Parameter]
        public string PopupStyle { get; set; }

        public DateTime CurrentDate { get; private set; } = DateTime.Now;
        public DateTime CurrentShowDate { get; private set; } = DateTime.Now;
        public DateTime CurrentSelectDate { get; private set; }

        private string initPicker = null;
        private string prePicker = null;
        private bool hadSelectValue = false;
        private bool isClose = true;

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
                //.If($"{PrefixCls}-focused", () => AutoFocus == true)
               //.If($"{PrefixCls}-normal", () => Image.IsT1 && Image.AsT1 == AntEmpty.PRESENTED_IMAGE_SIMPLE)
               //.If($"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
               ;
        }

        protected void OnSelect(DateTime date)
        {
            // InitPicker is the finally value
            if (Picker == initPicker)
            {
                CurrentSelectDate = date;
                hadSelectValue = true;

                isClose = true;
                Open = false;
            }
            else
            {
                Picker = prePicker;
                CurrentShowDate = date;
            }

            StateHasChanged();
        }

        public void ChangeShowDate(DateTime date)
        {
            CurrentShowDate = date;

            StateHasChanged();
        }

        public void ChangePickerType(string type)
        {
            Picker = type;

            StateHasChanged();
        }
    }
}
