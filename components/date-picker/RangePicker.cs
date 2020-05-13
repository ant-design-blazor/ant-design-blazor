using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class RangePicker : DatePicker
    {
        public RangePicker()
        {
            IsRange = true;
        }
    }
}
