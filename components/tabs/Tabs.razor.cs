using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Tabs : AntDomComponentBase
    {
        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }


        /// <summary>
        /// Current <see cref="TabPane"/>'s <see cref="TabPane.Key"/>
        /// </summary>
        [Parameter]
        public string ActiveKey
        {
            get
            {
                return _activeKey;
            }
            set
            {
                if (_activeKey != value)
                {
                    _activeKey = value;

                    ActivatePane(_activeKey);
                }
            }
        }

        [Parameter]
        public EventCallback<string> ActiveKeyChanged { get; set; }

        /// <summary>
        /// Whether to change tabs with animation. Only works while <see cref="TabPosition"/> = <see cref="TabPosition.Top"/> or <see cref="TabPosition.Bottom"/>
        /// </summary>
        [Parameter]
        public bool Animated { get; set; }

        [Parameter]
        public bool InkBarAnimated { get; set; } = true;

        /// <summary>
        /// Replace the TabBar
        /// </summary>
        [Parameter]
        public object RenderTabBar { get; set; }

        /// <summary>
        /// Initial active <see cref="TabPane"/>'s <see cref="TabPane.Key"/>, if <see cref="ActiveKey"/> is not set
        /// </summary>
        [Parameter]
        public string DefaultActiveKey { get; set; }

        /// <summary>
        /// Hide plus icon or not. Only works while <see cref="Type"/> = <see cref="TabType.EditableCard"/>
        /// </summary>
        [Parameter]
        public bool HideAdd { get; set; } = false;

        /// <summary>
        /// Preset tab bar size
        /// </summary>
        [Parameter]
        public TabSize Size { get; set; } = TabSize.Default;

        /// <summary>
        /// Extra content in tab bar
        /// </summary>
        [Parameter]
        public RenderFragment TabBarExtraContent { get; set; }

        [Parameter]
        public RenderFragment TabBarExtraContentLeft { get; set; }

        [Parameter]
        public RenderFragment TabBarExtraContentRight { get; set; }

        /// <summary>
        /// The gap between tabs
        /// </summary>
        [Parameter]
        public int TabBarGutter { get; set; }

        /// <summary>
        /// Tab bar style object
        /// </summary>
        [Parameter]
        public string TabBarStyle { get; set; }

        /// <summary>
        /// Position of tabs
        /// </summary>
        [Parameter]
        public TabPosition TabPosition { get; set; } = TabPosition.Top;

        /// <summary>
        /// Basic style of tabs
        /// </summary>
        [Parameter]
        public TabType Type { get; set; } = TabType.Line;

        /// <summary>
        /// Callback executed when active tab is changed
        /// </summary>
        [Parameter]
        public EventCallback<string> OnChange { get; set; }

        /// <summary>
        /// Callback executed when tab is added or removed. Only works while <see cref="Type"/> = <see cref="TabType.EditableCard"/>
        /// </summary>
        [Parameter]
        public Func<string, string, Task<bool>> OnEdit { get; set; } = (key, action) => Task.FromResult(true);

        /// <summary>
        /// Callback when tab is closed
        /// </summary>
        [Parameter]
        public EventCallback<string> OnClose { get; set; }


        [Parameter]
        public EventCallback OnAddClick { get; set; }

        [Parameter]
        public EventCallback<string> AfterTabCreated { get; set; }

        /// <summary>
        /// Callback executed when next button is clicked
        /// </summary>
        [Parameter]
        public EventCallback OnNextClick { get; set; }

        /// <summary>
        /// Callback executed when prev button is clicked
        /// </summary>
        [Parameter]
        public EventCallback OnPrevClick { get; set; }

        /// <summary>
        /// Callback executed when tab is clicked
        /// </summary>
        [Parameter]
        public EventCallback<string> OnTabClick { get; set; }

        /// <summary>
        /// Whether to turn on keyboard navigation
        /// </summary>
        [Parameter]
        public bool Keyboard { get; set; } = true;

        [Parameter]
        public bool Draggable { get; set; }

        [Parameter]
        public bool Centered { get; set; }

        [CascadingParameter]
        public Card Card { get; set; }

        #endregion Parameters


        private const string PrefixCls = "ant-tabs";
        private bool IsHorizontal => TabPosition.IsIn(TabPosition.Top, TabPosition.Bottom);

        //private ClassMapper _barClassMapper = new ClassMapper();
        //private ClassMapper _prevClassMapper = new ClassMapper();
        //private ClassMapper _nextClassMapper = new ClassMapper();
        //private ClassMapper _navClassMapper = new ClassMapper();
        private TabPane _activePane;
        private TabPane _activeTab;

        private string _activeKey;
        private TabPane _renderedActivePane;

        private ElementReference _scrollTabBar;
        private ElementReference _tabBars;

        private string _inkStyle;

        private string _navStyle;

        private bool _wheelDisabled;

        //private string _contentStyle;
        //private bool? _prevIconEnabled;
        //private bool? _nextIconEnabled;
        private string _operationClass;

        private string _tabsNavWarpPingClass;
        private string _operationStyle;

        private int _scrollOffset;
        private int _listWidth;
        private int _listHeight;
        private int _navWidth;
        private int _navHeight;
        private bool _needRefresh;
        private bool _afterFirstRender;

        private string _contentAnimatedStyle;

        private readonly ClassMapper _inkClassMapper = new ClassMapper();
        private readonly ClassMapper _contentClassMapper = new ClassMapper();

        private List<TabPane> _panes = new List<TabPane>();
        private List<TabPane> _tabs = new List<TabPane>();


        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Type == TabType.Card)
            {
                Animated = false;
            }

            ClassMapper
                .Add(PrefixCls)
                .Get(() => $"{PrefixCls}-{TabPosition.ToString().ToLowerInvariant()}")
                .If($"{PrefixCls}-line", () => Type == TabType.Line)
                .If($"{PrefixCls}-editable-card", () => Type == TabType.EditableCard)
                .If($"{PrefixCls}-card", () => Type.IsIn(TabType.EditableCard, TabType.Card))
                .If($"{PrefixCls}-large", () => Size == TabSize.Large || Card != null)
                .If($"{PrefixCls}-head-tabs", () => Card != null)
                .If($"{PrefixCls}-small", () => Size == TabSize.Small)
                .If($"{PrefixCls}-no-animation", () => !Animated)
                .If($"{PrefixCls}-centered", () => Centered)
                .If($"{PrefixCls}-rtl", () => RTL);

            _inkClassMapper
                .Add($"{PrefixCls}-ink-bar")
                .If($"{PrefixCls}-ink-bar-animated", () => InkBarAnimated);

            _contentClassMapper
                .Add($"{PrefixCls}-content")
                .Get(() => $"{PrefixCls}-content-{TabPosition.ToString().ToLowerInvariant()}")
                .If($"{PrefixCls}-content-animated", () => Animated);

        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            _needRefresh = true;
            _renderedActivePane = null;
        }

        /// <summary>
        /// Add <see cref="TabPane"/> to <see cref="Tabs"/>
        /// </summary>
        /// <param name="tabPane">The AntTabPane to be added</param>
        /// <exception cref="ArgumentNullException">Key is null</exception>
        /// <exception cref="ArgumentException">An AntTabPane with the same key already exists</exception>
        internal void AddTabPane(TabPane tabPane)
        {
            if (tabPane.IsTab)
            {
                _tabs.Add(tabPane);
            }
            else
            {
                _panes.Add(tabPane);
            }

            if (string.IsNullOrWhiteSpace(tabPane.Key))
            {
                if (tabPane.IsTab)
                {
                    tabPane.SetKey($"ant-tabs-{_tabs.Count}");
                }
                else
                {
                    var key = _tabs[_panes.Count].Key;
                    tabPane.SetKey(key);
                }
            }

            if (_tabs.Count == _panes.Count)
            {
                this.Complete();
            }
        }

        internal void Complete()
        {
            if (!string.IsNullOrWhiteSpace(ActiveKey))
            {
                var activedPane = _panes.Find(x => x.Key == ActiveKey);
                if (activedPane?.IsActive == false)
                {
                    ActivatePane(activedPane.Key);
                }
            }
            else if (!string.IsNullOrWhiteSpace(DefaultActiveKey))
            {
                var defaultPane = _panes.FirstOrDefault(x => x.Key == DefaultActiveKey);
                if (defaultPane != null)
                {
                    ActivatePane(defaultPane.Key);
                }
            }

            if (_activePane == null || _panes.All(x => !x.IsActive))
            {
                ActivatePane(_panes.FirstOrDefault()?.Key);
            }

            if (AfterTabCreated.HasDelegate)
            {
                AfterTabCreated.InvokeAsync(_activeKey);
            }
        }

        public async Task RemoveTab(TabPane tab)
        {
            if (await OnEdit.Invoke(tab.Key, "remove"))
            {
                var tabKey = tab.Key;
                var index = _tabs.IndexOf(tab);
                var pane = _panes.Find(x => x.Key == tab.Key);

                tab.Close();
                pane.Close();

                if (pane != null && pane.IsActive && _panes.Count > 0)
                {
                    var p = index > 1 ? _panes[index - 1] : _panes[0];
                    ActivatePane(p.Key);
                }

                if (OnClose.HasDelegate)
                {
                    await OnClose.InvokeAsync(tabKey);
                }

                _needRefresh = true;
                StateHasChanged();
            }
        }

        internal void RemovePane(TabPane pane)
        {
            if (pane.IsTab)
            {
                _tabs.Remove(pane);
            }
            else
            {
                _panes.Remove(pane);
            }
        }

        internal void HandleTabClick(TabPane tabPane)
        {
            if (tabPane.IsActive)
                return;

            if (OnTabClick.HasDelegate)
            {
                OnTabClick.InvokeAsync(tabPane.Key);
            }

            ActivatePane(tabPane.Key);
        }

        private void ActivatePane(string key)
        {
            if (_panes.Count == 0)
                return;

            var tabIndex = _tabs.FindIndex(p => p.Key == key);
            var tab = _tabs.Find(p => p.Key == key);
            var tabPane = _panes.Find(p => p.Key == key);

            if (tab == null || tabPane == null)
                return;

            if (tabPane.Disabled)
            {
                return;
            }

            _activePane?.SetActive(false);
            _activeTab?.SetActive(false);

            tab.SetActive(true);
            tabPane.SetActive(true);

            _activeTab = tab;
            _activePane = tabPane;

            if (_activeKey != _activePane.Key)
            {
                if (ActiveKeyChanged.HasDelegate)
                {
                    ActiveKeyChanged.InvokeAsync(_activePane.Key);
                }

                if (OnChange.HasDelegate)
                {
                    OnChange.InvokeAsync(_activePane.Key);
                }

                _activeKey = _activePane.Key;
            }

            _contentAnimatedStyle = Animated && tabIndex > 0 ? $"margin-left: -{tabIndex * 100}%" : "";

            Card?.SetBody(_activePane.ChildContent);

            _needRefresh = true;
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                _afterFirstRender = true;
            }

            var element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _scrollTabBar);
            _listWidth = element.ClientWidth;
            _listHeight = element.ClientHeight;
            var navSection = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _tabBars);
            _navWidth = navSection.ClientWidth;
            _navHeight = navSection.ClientHeight;

            if (IsHorizontal && !_wheelDisabled)
            {
                DomEventListener.AddExclusive<string>(_scrollTabBar, "wheel", OnWheel, true);
                _wheelDisabled = true;
            }

            if (!IsHorizontal && _wheelDisabled)
            {
                DomEventListener.RemoveExclusive(_scrollTabBar, "wheel");
                _wheelDisabled = false;
            }

            if (_afterFirstRender && _activePane != null)
            {
                await TryRenderInk();
                await TryRenderNavOperation();
            }
            _needRefresh = false;
        }

        private void OnWheel(string json)
        {
            int maxOffset;
            if (IsHorizontal)
            {
                maxOffset = _listWidth - _navWidth;
            }
            else
            {
                maxOffset = _listHeight - _navHeight;
            }

            int delta = JsonDocument.Parse(json).RootElement.GetProperty("wheelDelta").GetInt32();
            if (delta >= 0)
            {
                _scrollOffset -= 100;
            }
            else
            {
                _scrollOffset += 100;
            }

            _scrollOffset = Math.Max(0, _scrollOffset);
            _scrollOffset = Math.Min(maxOffset, _scrollOffset);

            _renderedActivePane = null;

            if (IsHorizontal)
            {
                _navStyle = $"transform: translate(-{_scrollOffset}px, 0px);";
            }
            else
            {
                _navStyle = $"transform: translate(0px, -{_scrollOffset}px);";
            }
            StateHasChanged();

            _renderedActivePane = _activePane;
        }

        private async Task TryRenderNavOperation()
        {
            int navWidth = (await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _tabBars)).ClientWidth;
            int navTotalWidth = (await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _scrollTabBar)).ClientWidth;
            if (navTotalWidth < navWidth)
            {
                _operationClass = "ant-tabs-nav-operations ant-tabs-nav-operations-hidden";
                _operationStyle = "visibility: hidden; order: 1;";
                _tabsNavWarpPingClass = string.Empty;
            }
            else
            {
                _operationClass = "ant-tabs-nav-operations";
                _tabsNavWarpPingClass = "ant-tabs-nav-wrap-ping-right";
                _operationStyle = string.Empty;
            }

            StateHasChanged();
        }

        private async Task TryRenderInk()
        {
            if (_renderedActivePane == _activePane)
            {
                return;
            }

            // TODO: slide to activated tab
            // animate Active Ink
            // ink bar
            var element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _activeTab.TabRef);
            var navSection = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _tabBars);

            if (IsHorizontal)
            {
                //_inkStyle = "left: 0px; width: 0px;";
                _inkStyle = $"left: {element.OffsetLeft}px; width: {element.ClientWidth}px";
                if (element.OffsetLeft > _scrollOffset + navSection.ClientWidth
                    || element.OffsetLeft < _scrollOffset)
                {
                    // need to scroll tab bars
                    _scrollOffset = element.OffsetLeft;
                    _navStyle = $"transform: translate(-{_scrollOffset}px, 0px);";
                }
            }
            else
            {
                _inkStyle = $"top: {element.OffsetTop}px; height: {element.ClientHeight}px;";
                if (element.OffsetTop > _scrollOffset + navSection.ClientHeight
                    || element.OffsetTop < _scrollOffset)
                {
                    // need to scroll tab bars
                    _scrollOffset = element.OffsetTop;
                    _navStyle = $"transform: translate(0px, -{_scrollOffset}px);";
                }
            }
            StateHasChanged();
            _renderedActivePane = _activePane;
        }

        protected override bool ShouldRender()
        {
            return _needRefresh || _renderedActivePane != _activePane;
        }

        private IEnumerable<TabPane> GetInvisibleTabs()
        {
            double average;
            int invisibleHeadCount, visibleCount;

            if (IsHorizontal)
            {
                average = 1.0 * _listWidth / _panes.Count;
                visibleCount = (int)(_navWidth / average);
            }
            else
            {
                average = 1.0 * _listHeight / _panes.Count;
                visibleCount = (int)(_navHeight / average);
            }

            invisibleHeadCount = (int)Math.Ceiling(_scrollOffset / average);
            IEnumerable<TabPane> invisibleTabs = _panes.Take(invisibleHeadCount).Concat(_panes.Skip(invisibleHeadCount + visibleCount));

            return invisibleTabs;
        }

        #region DRAG & DROP

        private TabPane _draggingTab;

        internal void HandleDragStart(DragEventArgs args, TabPane tab)
        {
            if (Draggable)
            {
                args.DataTransfer.DropEffect = "move";
                args.DataTransfer.EffectAllowed = "move";
                _draggingTab = tab;
            }
        }

        internal void HandleDrop(TabPane tab)
        {
            if (Draggable && _draggingTab != null)
            {
                var oldIndex = _tabs.IndexOf(_draggingTab);
                var newIndex = _tabs.IndexOf(tab);

                if (oldIndex == newIndex)
                {
                    return;
                }

                tab.ExchangeWith(_draggingTab);

                var diffTabs = newIndex < oldIndex ? _tabs.GetRange(newIndex, oldIndex - newIndex) : _tabs.GetRange(oldIndex, newIndex - oldIndex);

                for (var i = diffTabs.Count - 2; i >= 0; i--)
                {
                    diffTabs[i].ExchangeWith(diffTabs[i + 1]);
                }

                _draggingTab = null;
                _needRefresh = true;
                _renderedActivePane = null;

                ActivatePane(_activeKey);
            }
        }

        #endregion DRAG & DROP

        protected override void Dispose(bool disposing)
        {
            DomEventListener.DisposeExclusive();
            base.Dispose(disposing);
        }
    }
}
