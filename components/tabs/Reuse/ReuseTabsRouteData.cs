// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
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
                if (RouteData.PageType == null)
                    return;

                builder.OpenComponent(0, RouteData.PageType);
                foreach (var routeValue in RouteData.RouteValues)
                {
                    builder.AddAttribute(1, routeValue.Key, routeValue.Value);
                }
                builder.CloseComponent();
            };
        }

        public ReuseTabsRouteData(RouteData routeData)
        {
            _routeData = routeData;
            RouteValues = routeData.RouteValues;
            PageType = routeData.PageType.FullName;
            PageAssembly = routeData.PageType.Assembly.FullName;
        }

        [JsonConstructor]
        public ReuseTabsRouteData(IReadOnlyDictionary<string, object> routeValues, string pageType, string pageAssembly)
        {
            RouteValues = routeValues;
            PageType = pageType;
            PageAssembly = pageAssembly;
            var type = Assembly.Load(pageAssembly).GetType(pageType);
            _routeData = new RouteData(type, routeValues);
        }

        [JsonIgnore]
        private RouteData _routeData;

        [JsonIgnore]
        public RouteData RouteData => _routeData;

    }
}
