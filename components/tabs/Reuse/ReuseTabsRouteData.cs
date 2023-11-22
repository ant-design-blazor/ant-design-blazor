// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ReuseTabsRouteData
    {
        public IReadOnlyDictionary<string, object> RouteValues { get; set; }

        public string PageType { get; set; }

        public string PageAssembly { get; set; }

        public RenderFragment RenderBody()
        {
            return builder =>
            {
                var pageType = Assembly.Load(PageAssembly).GetType(PageType);
                if (pageType == null)
                {
                    return;
                }

                builder.OpenComponent(0, pageType);
                foreach (var routeValue in RouteValues)
                {
                    builder.AddAttribute(1, routeValue.Key, routeValue.Value);
                }
                builder.CloseComponent();
            };
        }

        public ReuseTabsRouteData() { }

        public ReuseTabsRouteData(RouteData routeData)
        {
            RouteValues = routeData.RouteValues;
            PageType = routeData.PageType.FullName;
            PageAssembly = routeData.PageType.Assembly.FullName;
        }
    }
}
