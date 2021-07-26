﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AntDesign
{
    public class ReuseTabsRouteView : RouteView
    {
        private readonly Dictionary<string, ReuseTabsPageItem> _pageMap = new();

        [Inject]
        public NavigationManager Navmgr { get; set; }

        [Parameter]
        public RenderFragment<RenderFragment> ChildContent { get; set; }

        private string CurrentUrl => Navmgr.Uri;

        internal ReuseTabsPageItem[] Pages => _pageMap.Values.ToArray();

        public void RemovePage(string key)
        {
            _pageMap.Remove(key);
        }

        public void ReplaceBody(string key, RenderFragment body)
        {
            _pageMap[key].Body = body;
        }

        protected override void Render(RenderTreeBuilder builder)
        {
            var layoutType = RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType ?? DefaultLayout;

            var body = CreateBody(RouteData, Navmgr.Uri);

            builder.OpenComponent<CascadingValue<ReuseTabsRouteView>>(0);
            builder.AddAttribute(1, "Name", "RouteView");
            builder.AddAttribute(2, "Value", this);

            if (ChildContent != null)
            {
                builder.AddAttribute(3, "ChildContent", ChildContent(body));
            }
            else
            {
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b =>
                {
                    b.OpenComponent(20, layoutType);
                    b.AddAttribute(21, "Body", body);
                    b.CloseComponent();
                }));
            }

            builder.CloseComponent();

            if (!_pageMap.ContainsKey(CurrentUrl))
            {
                _pageMap[CurrentUrl] = new ReuseTabsPageItem
                {
                    Body = body,
                    Url = CurrentUrl,
                };
            }
        }

        private RenderFragment CreateBody(RouteData routeData, string url)
        {
            return builder =>
            {
                builder.OpenComponent(0, routeData.PageType);
                foreach (var routeValue in routeData.RouteValues)
                {
                    builder.AddAttribute(1, routeValue.Key, routeValue.Value);
                }

                builder.AddComponentReferenceCapture(2, @ref =>
                {
                    _pageMap[url].Title ??= GetPageTitle(routeData.PageType, url, @ref);
                });

                builder.CloseComponent();
            };
        }

        private static RenderFragment GetPageTitle(Type pageType, string url, object page)
        {
            if (page is IReuseTabsPage resuse)
            {
                return resuse.GetPageTitle();
            }
            else if (pageType.GetCustomAttributes(true).FirstOrDefault(o => o is ReuseTabsPageTitleAttribute) is ReuseTabsPageTitleAttribute attr && attr is { })
            {
                return attr.Title?.ToRenderFragment();
            }

            return new Uri(url).PathAndQuery.ToRenderFragment();
        }
    }
}
