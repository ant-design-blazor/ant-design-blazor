using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    internal abstract class AntTypographyBase:AntComponentBase
    {
        [Parameter]
        public bool copyable { get; set; } = false;


    }
}
