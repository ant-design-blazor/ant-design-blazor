using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class AutoCompleteOptGroup : AntDomComponentBase
    {
        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public RenderFragment LabelFragment { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
