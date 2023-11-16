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
            get => _activeKey;
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
        /// Tab bar css class
        /// </summary>
        [Parameter]
        public string TabBarClass { get; set; }

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
        /// Callback executed when tab is clicked
        /// </summary>
        [Parameter]
        public EventCallback<string> OnTabClick { get; set; }

        [Parameter]
        public bool Draggable { get; set; }

        [Parameter]
        public bool Centered { get; set; }

        [CascadingParameter]
        public Card Card { get; set; }

        #endregion Parameters

        private const string PrefixCls = "ant-tabs";
        private bool IsHorizontal => TabPosition.IsIn(TabPosition.Top, TabPosition.Bottom);

        private TabPane _activePane;
        private TabPane _activeTab;
        private HtmlElement _activeTabElement;

        private string _activeKey;
        private TabPane _renderedActivePane;

        private ElementReference _navListRef;
        private ElementReference _navWarpRef;

        private string _inkStyle;
        private string _navListStyle;

        private string _operationClass = "ant-tabs-nav-operations ant-tabs-nav-operations-hidden";
        private string _operationStyle = "visibility: hidden; order: 1;";

        private decimal _scrollOffset;
        private decimal _scrollListWidth;
        private decimal _scrollListHeight;
        private decimal _wrapperWidth;
        private decimal _wrapperHeight;

        private bool _shouldRender;
        private bool _afterFirstRender;

        private readonly ClassMapper _inkClassMapper = new ClassMapper();
        private readonly ClassMapper _contentClassMapper = new ClassMapper();
        private readonly ClassMapper _tabsNavWarpPingClassMapper = new ClassMapper();
        private readonly ClassMapper _tabBarClassMapper = new ClassMapper();

        private readonly List<TabPane> _panes = new List<TabPane>();
        private readonly List<TabPane> _tabs = new List<TabPane>();
        private List<TabPane> _invisibleTabs = new List<TabPane>();

        private bool NavWrapPingLeft => _scrollOffset > 0;
        private bool NavWrapPingRight => _scrollListWidth - _wrapperWidth - _scrollOffset > 0;

        private bool HasAddButton => Type == TabType.EditableCard && !HideAdd;

        private readonly int _dropDownBtnWidth = 46;
        private readonly int _addBtnWidth = 40;
        private bool _shownDropdown;
        private bool _needUpdateScrollListPosition;

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

            _tabsNavWarpPingClassMapper
                .Add("ant-tabs-nav-wrap")
                .If("ant-tabs-nav-wrap-ping-left", () => NavWrapPingLeft)
                .If("ant-tabs-nav-wrap-ping-right", () => NavWrapPingRight);

            _tabBarClassMapper
                .Add($"{PrefixCls}-nav")
                .If(TabBarClass, () => !string.IsNullOrWhiteSpace(TabBarClass));
        }

        protected override Task OnFirstAfterRenderAsync()
        {
            if (Card is not null)
            {
                this.Complete();
            }
            return base.OnFirstAfterRenderAsync();
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
                if (Card is not null)
                {
                    _panes.Add(tabPane);
                }
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
                    tabPane.SetKey(_tabs[_panes.Count - 1].Key);
                }
            }

            if (_tabs.Count == _panes.Count && Card is null)
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

            _needUpdateScrollListPosition = true;
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

                if (OnClose.HasDelegate)
                {
                    await OnClose.InvokeAsync(tabKey);
                }
            }
        }

        internal void RemovePane(TabPane tab)
        {
            // fix https://github.com/ant-design-blazor/ant-design-blazor/issues/2180
            if (IsDisposed)
                return;

            if (tab.IsTab)
            {
                var index = _tabs.IndexOf(tab);

                _tabs.Remove(tab);

                if (tab.IsActive && _tabs.Count > 0)
                {
                    var p = index > 1 ? _tabs[index - 1] : _tabs[0];
                    ActivatePane(p.Key);
                }
            }
            else
            {
                _panes.Remove(tab);
            }

            _needUpdateScrollListPosition = true;
        }

        internal async Task HandleTabClick(TabPane tabPane)
        {
            if (tabPane.IsActive)
                return;

            if (OnTabClick.HasDelegate)
            {
                await OnTabClick.InvokeAsync(tabPane.Key);
            }

            ActivatePane(tabPane.Key);
        }

        internal async Task HandleKeydown(KeyboardEventArgs e, TabPane tabPane)
        {
            var tabIndex = _tabs.FindIndex(p => p.Key == tabPane.Key);
            switch (e.Code)
            {
                case "Enter" or "NumpadEnter": await NavigateToTab(tabPane); break;
                case "ArrowLeft": await NavigateToTab(_tabs[Math.Max(0, tabIndex - 1)]); break;
                case "ArrowRight": await NavigateToTab(_tabs[Math.Min(_tabs.Count - 1, tabIndex + 1)]); break;
                case "ArrowUp": await NavigateToTab(_tabs[0]); break;
                case "ArrowDown": await NavigateToTab(_tabs[^1]); break;
                default: return;
            }
        }

        private async Task NavigateToTab(TabPane tabPane)
        {
            await HandleTabClick(tabPane);
            await FocusAsync(_activeTab.TabBtnRef);
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

            // Even if a TabPane is disabled, it is allowed to be activated at initial load time (at initial load time, _activeTab is null)
            // fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/2732
            if (_activeTab != null && tabPane.Disabled)
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

            Card?.SetBody(_activePane.ChildContent);

            _needUpdateScrollListPosition = true;

            _shouldRender = true;
            InvokeAsync(StateHasChanged);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                _afterFirstRender = true;
            }

            if (_afterFirstRender && _needUpdateScrollListPosition)
            {
                _needUpdateScrollListPosition = false;
                await ResetSizes();
                UpdateScrollListPosition();
                TryRenderInk();
            }

            _shouldRender = false;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            _shouldRender = true;
        }

        private async Task ResetSizes()
        {
            var navList = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _navListRef);
            var navWarp = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _navWarpRef);

            _activeTabElement = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, _activeTab.TabRef);

            _scrollListWidth = navList.ClientWidth;
            _scrollListHeight = navList.ClientHeight;
            _wrapperWidth = navWarp.ClientWidth;
            _wrapperHeight = navWarp.ClientHeight;
        }

        private void UpdateScrollListPosition()
        {
            // 46 is the size of dropdown button
            if (_scrollListWidth <= _wrapperWidth)
            {
                _operationClass = "ant-tabs-nav-operations ant-tabs-nav-operations-hidden";
                _operationStyle = "visibility: hidden; order: 1;";

                DomEventListener.RemoveExclusive(_navListRef, "wheel");
            }
            else
            {
                _operationClass = "ant-tabs-nav-operations";
                _operationStyle = string.Empty;

                if (!_shownDropdown)
                {
                    _wrapperWidth -= _dropDownBtnWidth;
                    _shownDropdown = true;
                }

                if (IsHorizontal)
                {
                    DomEventListener.AddExclusive<string>(_navListRef, "wheel", OnWheel, true);
                }
            }
        }

        private void OnWheel(string json)
        {
            decimal maxOffset;
            if (IsHorizontal)
            {
                maxOffset = _scrollListWidth - _wrapperWidth;
            }
            else
            {
                maxOffset = _scrollListHeight - _wrapperHeight;
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

            SetNavListStyle();

            StateHasChanged();
            _renderedActivePane = _activePane;
        }

        private void SetNavListStyle()
        {
            if (_scrollOffset == 0)
            {
                _navListStyle = "";
            }
            else
            {
                if (IsHorizontal)
                {
                    _navListStyle = $"transform: translate(-{_scrollOffset}px, 0px);";
                }
                else
                {
                    _navListStyle = $"transform: translate(0px, -{_scrollOffset}px);";
                }
            }
        }

        private void TryRenderInk()
        {
            if (IsHorizontal)
            {
                _inkStyle = $"left: {_activeTabElement.OffsetLeft}px; width: {_activeTabElement.ClientWidth}px";

                var additionalWidth = HasAddButton ? _addBtnWidth : 0;

                // need to scroll tab bars
                if (_activeTabElement.OffsetLeft + _activeTabElement.ClientWidth + additionalWidth > _scrollOffset + _wrapperWidth
                    || _scrollListWidth - _scrollOffset < _wrapperWidth)
                {
                    // scroll　right
                    _scrollOffset = _activeTabElement.OffsetLeft + _activeTabElement.ClientWidth - _wrapperWidth + additionalWidth;
                    _scrollOffset = Math.Min(_scrollOffset, _scrollListWidth - _wrapperWidth);
                    _scrollOffset = Math.Max(_scrollOffset, 0);
                }
                else if (_activeTabElement.OffsetLeft < _scrollOffset)
                {
                    // scroll　left
                    _scrollOffset = _activeTabElement.OffsetLeft;
                    _scrollOffset = Math.Max(_scrollOffset, 0);
                }

                SetNavListStyle();
            }
            else
            {
                _inkStyle = $"top: {_activeTabElement.OffsetTop}px; height: {_activeTabElement.ClientHeight}px;";

                if (_activeTabElement.OffsetTop > _scrollOffset + _activeTabElement.ClientHeight
                  || _activeTabElement.OffsetTop < _scrollOffset)
                {
                    // need to scroll tab bars
                    _scrollOffset = _activeTabElement.OffsetTop + _activeTabElement.ClientHeight - _wrapperHeight;
                    SetNavListStyle();
                }
            }

            StateHasChanged();
            _renderedActivePane = _activePane;
        }

        protected override bool ShouldRender()
        {
            return _shouldRender || _renderedActivePane != _activePane;
        }

        private void OnVisibleChange(bool visible)
        {
            if (!visible)
            {
                _invisibleTabs.Clear();
                return;
            }

            int invisibleHeadCount;
            decimal tabSize, visibleCount;

            if (IsHorizontal)
            {
                tabSize = _scrollListWidth / _tabs.Count;
                visibleCount = Math.Ceiling(_wrapperWidth / tabSize);
            }
            else
            {
                tabSize = _scrollListHeight / _tabs.Count;
                visibleCount = Math.Ceiling(_wrapperHeight / tabSize);
            }

            invisibleHeadCount = (int)Math.Ceiling(_scrollOffset / tabSize);
            visibleCount = Math.Min(visibleCount, _tabs.Count - invisibleHeadCount);

            _invisibleTabs = _tabs.ToList();
            _invisibleTabs.RemoveRange(invisibleHeadCount, (int)visibleCount);
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
                _shouldRender = true;
                _renderedActivePane = null;

                ActivatePane(_activeKey);
            }
        }

        #endregion DRAG & DROP

        protected override void Dispose(bool disposing)
        {
            DomEventListener.DisposeExclusive();

            _panes.Clear();
            _tabs.Clear();
            _invisibleTabs.Clear();

            base.Dispose(disposing);
        }
    }
}
