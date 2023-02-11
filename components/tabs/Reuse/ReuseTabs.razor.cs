using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class ReuseTabs : AntDomComponentBase
    {
        [Parameter]
        public string TabPaneClass { get; set; }

        [Parameter]
        public bool Draggable { get; set; }

        [Parameter]
        public TabSize Size { get; set; }

        [Parameter]
        public RenderFragment<ReuseTabsPageItem> Body { get; set; } = context => context.Body;

        [Parameter]
        public ReuseTabsLocale Locale { get; set; } = LocaleProvider.CurrentLocale.ReuseTabs;

        [CascadingParameter]
        private RouteData RouteData { get; set; }

        [Inject]
        private NavigationManager Navmgr { get; set; }

        [Inject]
        private ReuseTabsService ReuseTabsService { get; set; }

        private readonly Dictionary<string, ReuseTabsPageItem> _pageMap = new();

        private string CurrentUrl
        {
            get => GetNewKeyByUrl(Navmgr.ToBaseRelativePath(Navmgr.Uri));
            set => Navmgr.NavigateTo(value);
        }

        private ReuseTabsPageItem[] Pages => _pageMap.Values.Where(x => !x.Ignore).OrderBy(x => x.CreatedAt).ToArray();

        public ReuseTabs()
        {
            this.ScanReuseTabsPageAttribute();
        }

        protected override void OnInitialized()
        {
            ReuseTabsService.GetNewKeyByUrl += GetNewKeyByUrl;

            ReuseTabsService.OnClosePage += RemovePage;
            ReuseTabsService.OnCloseOther += RemoveOther;
            ReuseTabsService.OnCloseAll += RemoveAll;
            ReuseTabsService.OnCloseCurrent += RemoveCurrent;
            ReuseTabsService.OnUpdate += UpdateState;
        }

        protected override void Dispose(bool disposing)
        {
            ReuseTabsService.GetNewKeyByUrl -= GetNewKeyByUrl;

            ReuseTabsService.OnClosePage -= RemovePage;
            ReuseTabsService.OnCloseOther -= RemoveOther;
            ReuseTabsService.OnCloseAll -= RemoveAll;
            ReuseTabsService.OnCloseCurrent -= RemoveCurrent;
            ReuseTabsService.OnUpdate -= UpdateState;

            base.Dispose(disposing);
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (parameters.TryGetValue(nameof(RouteData), out RouteData routeData))
            {
                var reuseTabsPageItem = _pageMap.ContainsKey(CurrentUrl) ? _pageMap[CurrentUrl] : null;
                if (reuseTabsPageItem == null)
                {
                    reuseTabsPageItem = new ReuseTabsPageItem
                    {
                        Url = CurrentUrl,
                        CreatedAt = DateTime.Now,
                        Ignore = false
                    };

                    _pageMap[CurrentUrl] = reuseTabsPageItem;
                }

                reuseTabsPageItem.Body ??= CreateBody(routeData, reuseTabsPageItem);
            }

            return base.SetParametersAsync(parameters);
        }

        private static RenderFragment CreateBody(RouteData routeData, ReuseTabsPageItem item)
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
                    GetPageInfo(item, routeData.PageType, item.Url, @ref);
                });

                builder.CloseComponent();
            };
        }

        private static void GetPageInfo(ReuseTabsPageItem pageItem, Type pageType, string url, object page)
        {
            if (page is IReuseTabsPage resuse)
            {
                pageItem.Title = resuse.GetPageTitle();
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

            pageItem.Title ??= url.ToRenderFragment();
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

        public void AddReuseTabsPageItem(string url, Type pageType)
        {
            url = this.GetNewKeyByUrl(url);

            if (_pageMap.ContainsKey(url)) return;

            var reuseTabsPageItem = new ReuseTabsPageItem();
            GetPageInfo(reuseTabsPageItem, pageType, url, Activator.CreateInstance(pageType));
            reuseTabsPageItem.CreatedAt = DateTime.Now;
            reuseTabsPageItem.Url = url;
            _pageMap[url] = reuseTabsPageItem;
        }

        private void RemovePage(string key)
        {
            var reuseTabsPageItem = Pages.FirstOrDefault(w => w.Url == key);
            if (reuseTabsPageItem?.Pin == true)
            {
                return;
            }

            RemovePageBase(key);
            StateHasChanged();
        }

        private void RemoveOther(string key)
        {
            foreach (var item in Pages.Where(x => x.Closable && x.Url != key && !x.Pin))
            {
                RemovePageBase(item.Url);
            }
            StateHasChanged();
        }

        private void RemoveAll()
        {
            foreach (var item in Pages.Where(x => x.Closable && !x.Pin))
            {
                RemovePageBase(item.Url);
            }
            StateHasChanged();
        }

        private void RemoveCurrent()
        {
            RemovePage(this.CurrentUrl);
        }

        private void UpdateState()
        {
            StateHasChanged();
        }

        private string GetNewKeyByUrl(string url)
        {
            return GetNewKeyByUrlBase(url);
        }

        public void RemovePageBase(string key)
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

        public string GetNewKeyByUrlBase(string url)
        {
            if (url.StartsWith("/"))
            {
                return url;
            }

            return "/" + url;
        }
    }
}
