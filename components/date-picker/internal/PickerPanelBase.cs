using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntBlazor.Internal
{
    public class PickerPanelBase : AntDomComponentBase
    {
        [Parameter]
        public Action<DateTime, int> OnSelect { get; set; }

        [Parameter]
        public int PickerIndex { get; set; } = 0;

    }
}
