using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntRangePicker : AntDatePicker
    {
        public AntRangePicker()
        {
            IsRange = true;
        }
        //[Parameter]
        //public new string PrefixCls { get; set; } = "ant-picker-range";

        //protected override void OnInitialized()
        //{
        //    this.SetClass();

        //    base.OnInitialized();
        //}

        //protected override void OnParametersSet()
        //{
        //    this.SetClass();

        //    base.OnParametersSet();
        //}

        //protected new void SetClass()
        //{
        //    this.ClassMapper.Clear()
        //        .Add(PrefixCls)
        //        .If($"{PrefixCls}-borderless", () => Bordered == false)
        //        .If($"{PrefixCls}-disabled", () => Disabled == true)
        //        .If($"{ClassName}", () => !string.IsNullOrEmpty(ClassName))
        //       //.If($"{PrefixCls}-focused", () => AutoFocus == true)
        //       //.If($"{PrefixCls}-normal", () => Image.IsT1 && Image.AsT1 == AntEmpty.PRESENTED_IMAGE_SIMPLE)
        //       //.If($"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
        //       ;
        //}

    }
}
