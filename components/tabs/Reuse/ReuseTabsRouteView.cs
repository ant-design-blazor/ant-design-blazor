using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AntDesign
{
    [Obsolete("Pleaes use <CascadingValue Value=\"RouteData\"> to warp the RouteView.")]
    public class ReuseTabsRouteView : RouteView
    {
        [Parameter]
        public RenderFragment<RenderFragment> ChildContent { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            builder.OpenComponent<CascadingValue<RouteData>>(100);
            builder.AddAttribute(101, "Value", RouteData);

            base.Render(builder);

            builder.CloseComponent();
        }
    }
}
