using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public class AntDatePickerBase : AntDomComponentBase
    {
        [Parameter] public string PrefixCls { get; set; } = "ant-picker";
        [Parameter] public string Picker { get; set; } = AntDatePickerType.Date;

        protected DateTime CurrentSelectDate { get; set; } = DateTime.Now;

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

        protected void OnSelectDay(DateTime selectedDate)
        {
            CurrentSelectDate = selectedDate;

            StateHasChanged();
        }
    }
}
