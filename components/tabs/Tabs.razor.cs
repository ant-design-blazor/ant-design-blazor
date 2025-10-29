// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign.Core.Documentation;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace AntDesign
{
    /**
    <summary>
    <para>Tabs make it easy to switch between different views.</para>

    <h2>When To Use</h2>

    <para>Ant Design has 3 types of Tabs for different situations.</para>

    <list type="bullet">
        <item>Card Tabs: for managing too many closeable views.</item>
        <item>Normal Tabs: for functional aspects of a page.</item>
        <item><see cref="RadioGroup{TValue}"/>: for secondary tabs.</item>
    </list>
    </summary>
    <seealso cref="TabPane" />
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/antfincdn/lkI2hNEDr2V/Tabs.svg", Columns = 1, Title = "Tabs", SubTitle = "标签页")]
    public partial class Tabs : AntDomComponentBase
    {
        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        #region Parameters

        /// <summary>
        /// Content for tabs. Should include <c>TabPane</c> elements
        /// </summary>
        [Parameter]
        public virtual RenderFragment ChildContent { get; set; }

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

        /// <summary>
        /// Callback executed when the active tab changes
        /// </summary>
        [Parameter]
        public EventCallback<string> ActiveKeyChanged { get; set; }

        /// <summary>
        /// Whether to change tabs with animation. Only works while <see cref="TabPosition"/> = <see cref="TabPosition.Top"/> or <see cref="TabPosition.Bottom"/>
        /// </summary>
        [Parameter]
        public bool Animated { get; set; }

        /// <summary>
        /// Whether the ink bar is animated
        /// </summary>
        /// <default value="true" />
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
        /// <default value="false" />
        [Parameter]
        public bool HideAdd { get; set; } = false;

        /// <summary>
        /// Preset tab bar size
        /// </summary>
        /// <default value="TabSize.Default" />
        [Parameter]
        public TabSize Size { get; set; } = TabSize.Default;

        /// <summary>
        /// Extra content in tab bar
        /// </summary>
        [Parameter]
        public RenderFragment TabBarExtraContent { get; set; }

        /// <summary>
        /// Extra content to the left of the tab bar
        /// </summary>
        [Parameter]
        public RenderFragment TabBarExtraContentLeft { get; set; }

        /// <summary>
        /// Extra content to the right of the tab bar
        /// </summary>
        [Parameter]
        public RenderFragment TabBarExtraContentRight { get; set; }

        /// <summary>
        /// The gap between tabs
        /// </summary>
        /// <default value="0" />
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
        /// <default value="TabPosition.Top" />
        [Parameter]
        public TabPosition TabPosition { get; set; } = TabPosition.Top;

        /// <summary>
        /// Basic style of tabs
        /// </summary>
        /// <default value="TabType.Line" />
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

        /// <summary>
        /// Callback executed when add button clicked
        /// </summary>
        [Parameter]
        public EventCallback OnAddClick { get; set; }

        /// <summary>
        /// Callback executed after a tab is created
        /// </summary>
        [Parameter]
        public EventCallback<string> AfterTabCreated { get; set; }

        /// <summary>
        /// Callback executed when tab is clicked
        /// </summary>
        [Parameter]
        public EventCallback<string> OnTabClick { get; set; }

        /// <summary>
        /// Make tabs draggable
        /// </summary>
        [Parameter]
        public bool Draggable { get; set; }

        /// <summary>
        /// If tabs are centered or not
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool Centered { get; set; }

        /// <summary>
        /// Whether to render the tabs standalone in a card, otherwise it will be rendered as a part of a TabbedCard by default.
        /// </summary>
        [PublicApi("1.4.1")]
        [Parameter]
        public bool StandaloneInCard { get; set; }

        /// <summary>
        /// Enable swipe to switch tabs
        /// </summary>
        /// <default value="false" />
        [Parameter]
        [PublicApi("1.5.0")]
        public bool EnableSwipe { get; set; }

        [CascadingParameter]
        private Card Card { get; set; }

        #endregion Parameters

        private const string PrefixCls = "ant-tabs";
        private bool IsHorizontal => TabPosition.IsIn(TabPosition.Top, TabPosition.Bottom);

        private TabPane _activeTab;
        private HtmlElement _activeTabElement;
        private Dictionary<string, HtmlElement> _itemRefs;

        private string _activeKey;
        private TabPane _renderedActivePane;

        private ElementReference _navListRef;
        private ElementReference _navWarpRef;
        private ElementReference _contentHolderRef;

        private string _inkStyle;
        private string _navListStyle;

        private string _operationClass = "ant-tabs-nav-operations ant-tabs-nav-operations-hidden";

        private const string HiddenStyle = "visibility: hidden; order: 1;";

        private string _operationStyle = "visibility: hidden; order: 1;";
        private string _firstAddButtonStyle = HiddenStyle;
        private string _secondAddButtonStyle = HiddenStyle;

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

        private readonly List<TabPane> _tabs = new List<TabPane>();
        private List<TabPane> _invisibleTabs = new List<TabPane>();

        private static RenderFragment EmptyRenderFragment => builder => { };
        private bool NavWrapPingLeft => _scrollOffset > 0;
        private bool NavWrapPingRight => _scrollListWidth - _wrapperWidth - _scrollOffset > 0;

        private bool HasAddButton => Type == TabType.EditableCard && !HideAdd;

        private bool IsOverflowed => _scrollListWidth <= _wrapperWidth;

        private bool IsTabbedCard => Card != null && !StandaloneInCard;

        private readonly int _dropDownBtnWidth = 46;
        private readonly int _addBtnWidth = 40;
        private bool _shownDropdown;
        private bool _needUpdateScrollListPosition;
        private bool _retryingGetSize;
        private bool _hasNewTab = false;
        private string _waittingActiveKey;

        private DotNetObjectReference<Tabs> _dotNetHelper;

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
                .If($"{PrefixCls}-large", () => Size == TabSize.Large || (IsTabbedCard && Card.Size != CardSize.Small))
                .If($"{PrefixCls}-head-tabs", () => IsTabbedCard)
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

            if (IsTabbedCard)
            {
                Card.SetTabs(RenderTabs);
                Card.SetTabPanels(RenderTabPanels);
            }
        }

        /// <summary>
        /// Add <see cref="TabPane"/> to <see cref="Tabs"/>
        /// </summary>
        /// <param name="tabPane">The AntTabPane to be added</param>
        /// <exception cref="ArgumentNullException">Key is null</exception>
        /// <exception cref="ArgumentException">An AntTabPane with the same key already exists</exception>
        internal void AddTabPane(TabPane tabPane)
        {
            tabPane.SetIndex(_tabs.Count);
            _tabs.Add(tabPane);
            _needUpdateScrollListPosition = true;
            StateHasChanged();
            if (_hasNewTab)
            {
                ActivatePane(tabPane.Key);
                _hasNewTab = false;
            }

            // when adding tab and active it at the same time outside,
            // the tab may not be rendered before the ActivatePane() was called,
            // so we need to call it again.
            if (_waittingActiveKey == tabPane.Key)
            {
                ActivatePane(_waittingActiveKey);
            }
        }
        private async Task OnAddTab()
        {
            if (OnAddClick.HasDelegate)
            {
                _hasNewTab = true;
                await OnAddClick.InvokeAsync(null);
            }
        }
        internal void Complete()
        {
            if (_afterFirstRender)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(ActiveKey))
            {
                var activedPane = _tabs.Find(x => x.Key == ActiveKey);
                if (activedPane?.IsActive == false)
                {
                    ActivatePane(activedPane.Key);
                }
            }
            else if (!string.IsNullOrWhiteSpace(DefaultActiveKey))
            {
                var defaultPane = _tabs.FirstOrDefault(x => x.Key == DefaultActiveKey);
                if (defaultPane != null)
                {
                    ActivatePane(defaultPane.Key);
                }
            }

            if (_activeTab == null || _tabs.All(x => !x.IsActive))
            {
                ActivatePane(_tabs.FirstOrDefault()?.Key);
            }

            if (AfterTabCreated.HasDelegate)
            {
                AfterTabCreated.InvokeAsync(_activeKey);
            }

            // Only update scroll list position once the tabs have not been rendered
            _needUpdateScrollListPosition = !_afterFirstRender;
        }

        internal async Task RemoveTab(TabPane tab)
        {
            if (await OnEdit.Invoke(tab.Key, "remove"))
            {
                var tabKey = tab.Key;

                tab.Close();

                OnRemoveTab(tab);

                if (OnClose.HasDelegate)
                {
                    await OnClose.InvokeAsync(tabKey);
                }
            }
        }

        protected virtual void OnRemoveTab(TabPane tab) { }

        protected virtual void OnActiveTabChanged(TabPane tab) { }

        internal void RemovePane(TabPane tab)
        {
            // fix https://github.com/ant-design-blazor/ant-design-blazor/issues/2180
            if (IsDisposed)
                return;

            // if it is active, need to activiate the previous tab, or the next one if no previous
            if (_activeKey == tab.Key)
            {
                if (_tabs.IndexOf(tab) > 0)
                {
                    Previous();
                }
                else
                {
                    Next();
                }
            }

            _shouldRender = true;
            _needUpdateScrollListPosition = true;

            _tabs.Remove(tab);
            StateHasChanged();

        }

        internal async Task HandleTabClick(TabPane tabPane)
        {
            if (OnTabClick.HasDelegate)
            {
                await OnTabClick.InvokeAsync(tabPane.Key);
            }

            if (tabPane.IsActive)
                return;

            ActivatePane(tabPane.Key);
        }

        internal void HandleKeydown(KeyboardEventArgs e, TabPane tabPane)
        {
            switch (e.Code)
            {
                case "Enter" or "NumpadEnter": GoTo(tabPane.TabIndex); break;
                case "ArrowLeft": Previous(); break;
                case "ArrowRight": Next(); break;
                case "ArrowUp": GoTo(0); break;
                case "ArrowDown": GoTo(_tabs.Count - 1); break;
                default: return;
            }
        }

        /// <summary>
        /// Activate the tab with the specified index
        /// </summary>
        /// <param name="tabIndex"></param>
        [PublicApi("1.0.0")]
        public void GoTo(int tabIndex)
        {
            var activeIndex = Math.Min(_tabs.Count - 1, Math.Max(0, tabIndex));
            var tab = _tabs.Find(x => x.TabIndex == activeIndex);
            if (tab == null)
            {
                return;
            }
            ActivatePane(tab.Key);
        }

        /// <summary>
        /// Move to next tab
        /// </summary>
        [PublicApi("1.0.0")]
        public void Next()
        {
            GoTo(_activeTab.TabIndex + 1);
        }

        /// <summary>
        /// Move to previous tab
        /// </summary>
        [PublicApi("1.0.0")]
        public void Previous()
        {
            GoTo(_activeTab.TabIndex - 1);
        }

        /// <summary>
        /// Activate the specified tab
        /// </summary>
        /// <param name="key">The key of the tab to activate</param>
        [PublicApi("1.0.0")]
        public void ActivatePane(string key)
        {
            _waittingActiveKey = key;

            if (_tabs.Count == 0)
                return;

            var tab = _tabs.Find(p => p.Key == key);

            if (tab == null)
                return;

            // Even if a TabPane is disabled, it is allowed to be activated at initial load time (at initial load time, _activeTab is null)
            // fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/2732
            if (_activeTab != null && tab.Disabled)
            {
                return;
            }

            if (_activeTab?.Key == tab.Key)
            {
                return;
            }

            _activeTab?.SetActive(false);

            tab.SetActive(true);

            _activeTab = tab;
            _activeKey = _activeTab.Key;
            _waittingActiveKey = null;

            if (ActiveKeyChanged.HasDelegate)
            {
                ActiveKeyChanged.InvokeAsync(_activeKey);
            }

            if (OnChange.HasDelegate)
            {
                OnChange.InvokeAsync(tab.Key);
            }

            OnActiveTabChanged(_activeTab);

            _retryingGetSize = false;// each activation only can try once
            TryRenderInk();

            if (IsTabbedCard)
            {
                if (Card.Body == null)
                {
                    Card.SetBody(EmptyRenderFragment);
                }
                else
                {
                    Card.InvokeStateHasChagned();
                }
            }

            // render the classname of the actived tab
            // Needs to be optimized to render only one tab instead all the tabs
            StateHasChanged();

            //_ = FocusAsync(_activeTab.TabBtnRef);
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (parameters.IsParameterChanged(nameof(TabPosition), TabPosition))
            {
                _needUpdateScrollListPosition = true;
            }
            return base.SetParametersAsync(parameters);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                if (EnableSwipe)
                {
                    _dotNetHelper = DotNetObjectReference.Create(this);
                    await Js.InvokeVoidAsync("AntDesign.interop.touchHelper.initializeTouch", new { element = _contentHolderRef, dotNetHelper = _dotNetHelper, minSwipeDistance = 50, vertical = !IsHorizontal });
                }
                _afterFirstRender = true;
            }

            if (_afterFirstRender && _needUpdateScrollListPosition)
            {
                await ResetSizes();
                UpdateScrollListPosition();
                TryRenderInk();
                _needUpdateScrollListPosition = false;
            }

            _shouldRender = false;
        }

        [JSInvokable]
        public void HandleSwipe(string direction)
        {
            if (!EnableSwipe) return;

            switch (direction)
            {
                case "left":
                    if (IsHorizontal) Next();
                    break;
                case "right":
                    if (IsHorizontal) Previous();
                    break;
                case "up":
                    if (!IsHorizontal) Next();
                    break;
                case "down":
                    if (!IsHorizontal) Previous();
                    break;
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (EnableSwipe)
                {
                    await Js.InvokeVoidAsync("AntDesign.interop.touchHelper.dispose", _contentHolderRef);
                    _dotNetHelper?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disposing tabs: {ex.Message}");
            }
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener.DisposeExclusive();
            _tabs.Clear();
            _invisibleTabs.Clear();
            base.Dispose(disposing);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            _shouldRender = true;
        }

        protected override bool ShouldRender()
        {
            return _shouldRender || _renderedActivePane != _activeTab;
        }

        internal void SetShowRender()
        {
            _shouldRender = true;
        }

        private async Task ResetSizes()
        {
            if (IsDisposed)
                return;

            ElementReference[] refs = [_navListRef, _navWarpRef, .. _tabs.Select(x => x.TabRef).ToArray()];
            _itemRefs = await JsInvokeAsync<Dictionary<string, HtmlElement>>(JSInteropConstants.GetElementsDomInfo, refs);

            if (_itemRefs == null)
            {
                return;
            }

            var navListKey = Id + "-nav-list";
            var navWrapperKey = Id + "-nav-wrapper";

            // Use TryGetValue to safely access dictionary keys
            if (_itemRefs.TryGetValue(navListKey, out var navList) && _itemRefs.TryGetValue(navWrapperKey, out var navWarp))
            {
                _scrollListWidth = navList.ClientWidth;
                _scrollListHeight = navList.ClientHeight;
                _wrapperWidth = navWarp.ClientWidth;
                _wrapperHeight = navWarp.ClientHeight;

                _itemRefs.Remove(navListKey);
                _itemRefs.Remove(navWrapperKey);
            }
        }

        private void UpdateScrollListPosition()
        {
            // 46 is the size of dropdown button
            if (IsOverflowed)
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
            _firstAddButtonStyle = _secondAddButtonStyle = HiddenStyle;
            if (HasAddButton)
            {
                if (IsOverflowed)
                {
                    _firstAddButtonStyle = string.Empty;
                }
                else
                {
                    _secondAddButtonStyle = string.Empty;
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
            _renderedActivePane = _activeTab;
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
            if (_itemRefs is not { Count: > 0 })
            {
                return;
            }
            if (_activeTab == null)
            {
                return;
            }

            if (!_itemRefs.TryGetValue(_activeTab.TabId, out _activeTabElement))
            {
                return;
            }

            // the tabs maybe inside other components like modal, it can't get the element info at the first time
            // so here is retrying to get the element info again.
            // Fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/4061
            if (!_retryingGetSize && (_activeTabElement.ClientWidth <= 0 || _activeTabElement.ClientHeight <= 0))
            {
                _retryingGetSize = true;
                _needUpdateScrollListPosition = true;
                StateHasChanged();
                return;
            }

            if (IsHorizontal)
            {
                _inkStyle = $"left: {_activeTabElement.OffsetLeft}px; width: {_activeTabElement.ClientWidth}px";

                var additionalWidth = HasAddButton && (IsOverflowed) ? _addBtnWidth : 0;

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

            if (IsTabbedCard)
            {
                Card.InvokeStateHasChagned();
            }
            else
            {
                _shouldRender = true;
                StateHasChanged();
            }
            _renderedActivePane = _activeTab;
        }

        internal void UpdateTabsPosition()
        {
            _needUpdateScrollListPosition = true;
            _shouldRender = true;
        }

        private void OnVisibleChange(bool visible)
        {
            if (!visible)
            {
                _invisibleTabs.Clear();
                return;
            }

            int invisibleHeadCount;
            int visibleCount;

            if (IsHorizontal)
            {
                var tabWidths = _itemRefs.Values.Select(x => x.OffsetWidth + x.MarginLeft + x.MarginRight).ToArray();
                invisibleHeadCount = GetOverflowCount(_scrollOffset, tabWidths);
                visibleCount = GetOverflowCount(_scrollOffset + _wrapperWidth, tabWidths, true) - invisibleHeadCount;
            }
            else
            {
                var tabHeights = _itemRefs.Values.Select(x => x.ClientHeight + x.MarginTop + x.MarginBottom).ToArray();
                invisibleHeadCount = GetOverflowCount(_scrollOffset, tabHeights);
                visibleCount = GetOverflowCount(_scrollOffset + _wrapperHeight, tabHeights, true) - invisibleHeadCount;
            }

            _invisibleTabs = _tabs.ToList();
            if (invisibleHeadCount + visibleCount <= _invisibleTabs.Count)
            {
                _invisibleTabs.RemoveRange(invisibleHeadCount, visibleCount);
            }
        }

        private static int GetOverflowCount(decimal maxLength, decimal[] lengths, bool isRight = false)
        {
            var sum = 0m;
            for (var i = 0; i < lengths.Length; i++)
            {
                sum += lengths[i];
                if (sum - maxLength >= lengths[i] * (isRight ? 0.2m : 0.6m))
                {
                    return i;
                }
            }
            return lengths.Length;
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
                var oldIndex = _draggingTab.TabIndex;
                var newIndex = tab.TabIndex;

                if (oldIndex == newIndex)
                {
                    return;
                }

                tab.ExchangeWith(_draggingTab);

                /* Exchange example:
                 * 1,2,3,4,5,6 -> 1,5,3,4,2,6
                 *   ^     ^
                 */
                var diffTabs = newIndex < oldIndex ? _tabs.Where(x => x.TabIndex == newIndex && x.TabIndex < oldIndex).ToList()
                    : _tabs.Where(x => x.TabIndex > oldIndex && x.TabIndex == newIndex).ToList();

                for (var i = diffTabs.Count - 2; i >= 0; i--)
                {
                    diffTabs[i].ExchangeWith(diffTabs[i + 1]);
                }

                _draggingTab = null;
                _shouldRender = true;
                _renderedActivePane = null;
                _needUpdateScrollListPosition = true;
                StateHasChanged();
            }
        }

        #endregion DRAG & DROP
    }
}
