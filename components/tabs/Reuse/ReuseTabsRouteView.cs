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
    public class ReuseTabsRouteView : RouteView
    {
        private readonly Dictionary<string, ReuseTabsPageItem> _pageMap = new();

        public ReuseTabsRouteView() : base()
        {
            this.ScanReuseTabsPageAttribute();
        }

        [Inject]
        public NavigationManager Navmgr { get; set; }

        [Parameter]
        public RenderFragment<RenderFragment> ChildContent { get; set; }

        public string CurrentUrl => this.GetNewKeyByUrl(Navmgr.ToBaseRelativePath(Navmgr.Uri));

        internal ReuseTabsPageItem[] Pages => _pageMap.Values.Where(x => !x.Ignore).OrderBy(x => x.CreatedAt).ToArray();

        public void RemovePage(string key)
        {
            _pageMap.Remove(key);
        }

        public void RemovePageWithRegex(string pattern)
        {
            foreach (var key in _pageMap.Keys)
            {
                if (Regex.IsMatch(key, pattern))
                {
                    _pageMap.Remove(key);
                }
            }
        }

        public void ReplaceBody(string key, RenderFragment body)
        {
            _pageMap[key].Body = body;
        }

        protected override void Render(RenderTreeBuilder builder)
        {
            var layoutType = RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType ?? DefaultLayout;

            var body = CreateBody(RouteData, CurrentUrl);

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

            var reuseTabsPageItem = _pageMap.ContainsKey(CurrentUrl) ? _pageMap[CurrentUrl] : null;
            if (reuseTabsPageItem == null)
            {
                _pageMap[CurrentUrl] = new ReuseTabsPageItem
                {
                    Body = body,
                    Url = CurrentUrl,
                    CreatedAt = DateTime.Now,
                    Ignore = false
                };

                return;
            }

            if (reuseTabsPageItem.Body is null) reuseTabsPageItem.Body = body;
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
                    GetPageInfo(_pageMap[url], routeData.PageType, url, @ref);
                });

                builder.CloseComponent();
            };
        }

        private void GetPageInfo(ReuseTabsPageItem pageItem, Type pageType, string url, object page)
        {
            if (page is IReuseTabsPage resuse)
            {
                pageItem.Title ??= resuse.GetPageTitle();
            }

            var attributes = pageType.GetCustomAttributes(true);

            if (attributes.FirstOrDefault(x => x is ReuseTabsPageTitleAttribute) is ReuseTabsPageTitleAttribute titleAttr && titleAttr != null)
            {
                pageItem.Title ??= titleAttr.Title?.ToRenderFragment();
            }

            if (attributes.FirstOrDefault(x => x is ReuseTabsPageAttribute) is ReuseTabsPageAttribute attr && attr != null)
            {
                pageItem.Title ??= attr.Title?.ToRenderFragment();
                pageItem.Ignore = attr.Ignore;
                pageItem.Closable = attr.Closable;
                pageItem.Pin = attr.Pin;
            }

            pageItem.Title ??= new Uri(url).PathAndQuery.ToRenderFragment();
        }

        public void AddReuseTabsPageItem(string url, Type pageType)
        {
            url = this.GetNewKeyByUrl(url);

            if (_pageMap.ContainsKey(url)) return;

            var reuseTabsPageItem = new ReuseTabsPageItem();
            this.GetPageInfo(reuseTabsPageItem, pageType, url, null);
            reuseTabsPageItem.CreatedAt = DateTime.Now;
            reuseTabsPageItem.Url = url;
            _pageMap[url] = reuseTabsPageItem;
        }

        /// <summary>
        /// 获取所有程序集
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<Assembly> GetAllAssembly()
        {
            IEnumerable<Assembly> assemblies = new List<Assembly>();
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null) return assemblies;
            var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
            assemblies = new List<Assembly> { entryAssembly }.Union(referencedAssemblies);

            var paths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
                .Where(w => w.EndsWith(".dll") && !w.Contains(nameof(Microsoft)))
                .Select(w => w)
             ;

            return assemblies;
        }

        /// <summary>
        /// 扫描 ReuseTabsPageAttribute 特性
        /// </summary>
        private void ScanReuseTabsPageAttribute()
        {
            var list = GetAllAssembly();

            foreach (var item in list)
            {
                var allClass = item.ExportedTypes
                    .Where(w => w.GetCustomAttribute<ReuseTabsPageAttribute>()?.Pin == true);
                foreach (var pageType in allClass)
                {
                    var routeAttribute = pageType.GetCustomAttribute<RouteAttribute>();
                    var reuseTabsPageAttribute = pageType.GetCustomAttribute<ReuseTabsPageAttribute>();

                    this.AddReuseTabsPageItem(routeAttribute.Template, pageType);
                }
            }
        }

        public string GetNewKeyByUrl(string url)
        {
            if (url.StartsWith("/"))
            {
                return url;
            }

            return "/" + url;
        }
    }
}
