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
                PrePicker = _picker;

                _picker = value;

                // note first picker type
                if(InitPicker == null)
                {
                    InitPicker = value;
                }
            } 
        }

        [Parameter] 
        public bool Disabled { get; set; } = false;

        [Parameter] 
        public bool Bordered { get; set; } = true;

        public bool IsClose { get; set; } = true;

        public DateTime CurrentDate { get; private set; } = DateTime.Now;
        public DateTime CurrentShowDate { get; private set; } = DateTime.Now;
        public DateTime CurrentSelectDate { get; private set; } = DateTime.Now;

        private string InitPicker { get; set; } = null;
        private string PrePicker { get; set; } = null;

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
               //.If($"{PrefixCls}-normal", () => Image.IsT1 && Image.AsT1 == AntEmpty.PRESENTED_IMAGE_SIMPLE)
               //.If($"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
               ;
        }

        protected void OnSelect(DateTime date)
        {
            // InitPicker is the finally value
            if (Picker == InitPicker)
            {
                CurrentSelectDate = date;
                IsClose = true;
            }
            else
            {
                Picker = PrePicker;
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
