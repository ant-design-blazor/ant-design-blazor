using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class SelectPlaceholder
    {
        private static string Prefix => "ant-select";

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
