using System;
using Microsoft.AspNetCore.Components;

namespace AntDesign.Internal
{
    public class PickerPanelBase : AntDomComponentBase
    {
        [Parameter]
        public Action<DateTime, int> OnSelect { get; set; }

        [Parameter]
        public int PickerIndex { get; set; } = 0;

    }
}
