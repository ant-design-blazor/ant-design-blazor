using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class RangePicker : DatePicker
    {
        public RangePicker()
        {
            IsRange = true;
        }
    }
}
