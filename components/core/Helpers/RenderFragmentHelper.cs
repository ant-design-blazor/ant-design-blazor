using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public static class RenderFragmentHelper
    {
        public static RenderFragment ToRenderFragment(this string value) => builder => builder.AddContent(1, value);
    }
}
