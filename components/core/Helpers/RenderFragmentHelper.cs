using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public static class RenderFragmentHelper
    {
        public static RenderFragment ToRenderFragment(this string value) => builder => builder.AddContent(1, value);
    }
}
