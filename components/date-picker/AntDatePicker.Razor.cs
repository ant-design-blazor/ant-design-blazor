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

        [Parameter] 
        public string Picker { get; set; } = AntDatePickerType.Date;

        public DateTime CurrentDate { get; set; } = DateTime.Now;
        public DateTime CurrentShowDate { get; set; } = DateTime.Now;
        public DateTime CurrentSelectDate { get; set; } = DateTime.Now;

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
               //.If($"{PrefixCls}-normal", () => Image.IsT1 && Image.AsT1 == AntEmpty.PRESENTED_IMAGE_SIMPLE)
               //.If($"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
               ;
        }

        protected void OnSelect(DateTime date)
        {
            CurrentSelectDate = date;
            CurrentShowDate = date;

            StateHasChanged();
        }

        public void ChangeShowDate(DateTime date)
        {
            CurrentShowDate = date;

            StateHasChanged();
        }
    }
}
