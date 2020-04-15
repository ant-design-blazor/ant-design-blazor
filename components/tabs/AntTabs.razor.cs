using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntTabs : AntDomComponentBase
    {
        private const string PrefixCls = "ant-tabs";
        private bool IsHorizontal { get => TabPosition == AntTabPosition.Top || TabPosition == AntTabPosition.Bottom; }
        private ClassMapper _barClassMapper = new ClassMapper();
        private ClassMapper _prevClassMapper = new ClassMapper();
        private ClassMapper _nextClassMapper = new ClassMapper();
        private ClassMapper _navClassMapper = new ClassMapper();
        private AntTabPane _activePane;
        private AntTabPane _renderedActivePane;
        private ElementReference _activeTabBar;
        private ElementReference _scrollTabBar;
        private ElementReference _tabBars;
        private string _inkStyle;
        private string _navStyle;
        private string _contentStyle;
        private bool? _prevIconEnabled;
        private bool? _nextIconEnabled;
        private int _navIndex;
        private int _navTotal;
        private int _navSection;
        private bool _needRefresh;
        internal List<AntTabPane> Panes = new List<AntTabPane>();

        #region Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private string _activeKey;

        /// <summary>
        /// Current <see cref="AntTabPane"/>'s <see cref="AntTabPane.Key"/>
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
                    ActivatePane(Panes.Single(p => p.Key == _activeKey));
                }
            }
        }

        /// <summary>
        /// Whether to change tabs with animation. Only works while <see cref="TabPosition"/> = <see cref="AntTabPosition.Top"/> or <see cref="AntTabPosition.Bottom"/>
        /// </summary>
        [Parameter]
        public bool Animated { get; set; } = true;

        /// <summary>
        /// Replace the TabBar
        /// </summary>
        [Parameter]
        public object RenderTabBar { get; set; }

        /// <summary>
        /// Initial active <see cref="AntTabPane"/>'s <see cref="AntTabPane.Key"/>, if <see cref="ActiveKey"/> is not set
        /// </summary>
        [Parameter]
        public string DefaultActiveKey { get; set; }

        /// <summary>
        /// Hide plus icon or not. Only works while <see cref="Type"/> = <see cref="AntTabType.EditableCard"/>
        /// </summary>
        [Parameter]
        public bool HideAdd { get; set; } = false;

        /// <summary>
        /// Preset tab bar size
        /// </summary>
        [Parameter]
        public string Size { get; set; } = AntTabSize.Default;

        /// <summary>
        /// Extra content in tab bar
        /// </summary>
        [Parameter]
        public RenderFragment TabBarExtraContent { get; set; }

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
        public string TabPosition { get; set; } = AntTabPosition.Top;

        /// <summary>
        /// Basic style of tabs
        /// </summary>
        [Parameter]
        public string Type { get; set; } = AntTabType.Line;

        /// <summary>
        /// Callback executed when active tab is changed
        /// </summary>
        [Parameter]
        public EventCallback<object> OnChange { get; set; }

        /// <summary>
        /// Callback executed when tab is added or removed. Only works while <see cref="Type"/> = <see cref="AntTabType.EditableCard"/>
        /// </summary>
        [Parameter]
        public EventCallback<object> OnEdit { get; set; }

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
        public EventCallback<object> OnTabClick { get; set; }

        /// <summary>
        /// Whether to turn on keyboard navigation
        /// </summary>
        [Parameter]
        public bool Keyboard { get; set; } = true;

        [Parameter]
        public Func<AntTabPane> CreateTabPane { get; set; }

        #endregion Parameters

        public override Task SetParametersAsync(ParameterView parameters)
        {
            _needRefresh = true;
            _renderedActivePane = null;
            string type = parameters.GetValueOrDefault<string>(nameof(Type));
            if (type == AntTabType.Card)
            {
                // according to ant design documents,
                // Animated default to false when type="card"
                Animated = false;
            }

            string position = parameters.GetValueOrDefault<string>(nameof(TabPosition));
            if (!string.IsNullOrEmpty(position))
            {
                _navIndex = 0;
            }

            return base.SetParametersAsync(parameters);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (Type == AntTabType.EditableCard && !HideAdd)
            {
                TabBarExtraContent = (b) =>
                {
                    b.OpenComponent<AntIcon>(0);
                    b.AddAttribute(1, "Type", "plus");
                    b.AddAttribute(2, "class", $"{PrefixCls}-new-tab");
                    b.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, AddTabPane));
                    b.CloseComponent();
                };
            }

            ClassMapper.Clear()
                .Add(PrefixCls)
                .Add($"{PrefixCls}-{TabPosition}")
                .Add($"{PrefixCls}-{Type}")
                .If($"{PrefixCls}-large", () => Size == AntTabSize.Large)
                .If($"{PrefixCls}-small", () => Size == AntTabSize.Small)
                .If($"{PrefixCls}-{AntTabType.Card}", () => Type == AntTabType.EditableCard)
                .If($"{PrefixCls}-no-animation", () => !Animated);

            _barClassMapper.Clear()
                .Add($"{PrefixCls}-bar")
                .Add($"{PrefixCls}-{TabPosition}-bar")
                .Add($"{PrefixCls}-{Type}-bar")
                .If($"{PrefixCls}-{AntTabType.Card}-bar", () => Type == AntTabType.EditableCard)
                .If($"{PrefixCls}-large-bar", () => Size == AntTabSize.Large)
                .If($"{PrefixCls}-small-bar", () => Size == AntTabSize.Small);

            _prevClassMapper.Clear()
                .Add($"{PrefixCls}-tab-prev")
                .If($"{PrefixCls}-tab-btn-disabled", () => !_prevIconEnabled.HasValue || !_prevIconEnabled.Value)
                .If($"{PrefixCls}-tab-arrow-show", () => _prevIconEnabled.HasValue);

            _nextClassMapper.Clear()
                .Add($"{PrefixCls}-tab-next")
                .If($"{PrefixCls}-tab-btn-disabled", () => !_nextIconEnabled.HasValue || !_nextIconEnabled.Value)
                .If($"{PrefixCls}-tab-arrow-show", () => _nextIconEnabled.HasValue);

            _navClassMapper.Clear()
                .Add($"{PrefixCls}-nav-container")
                .If($"{PrefixCls}-nav-container-scrolling", () => _prevIconEnabled.HasValue || _nextIconEnabled.HasValue);

            _navStyle = "transform: translate3d(0px, 0px, 0px);";
            _inkStyle = "width: 0px; display: block; transform: translate3d(0px, 0px, 0px);";
            _contentStyle = "margin-" + (IsHorizontal ? "left" : "top") + ": 0;";
        }

        /// <summary>
        /// Add <see cref="AntTabPane"/> to <see cref="AntTabs"/>
        /// </summary>
        /// <param name="tabPane">The AntTabPane to be added</param>
        /// <exception cref="ArgumentNullException">Key is null</exception>
        /// <exception cref="ArgumentException">An AntTabPane with the same key already exists</exception>
        internal void AddTabPane(AntTabPane tabPane)
        {
            if (string.IsNullOrEmpty(tabPane.Key))
            {
                throw new ArgumentNullException("Key is null");
            }

            if (Panes.Select(p => p.Key).Contains(tabPane.Key))
            {
                throw new ArgumentException("An AntTabPane with the same key already exists");
            }

            Panes.Add(tabPane);
            if (tabPane.Key == DefaultActiveKey)
            {
                ActivatePane(tabPane);
            }
        }

        public void AddTabPane(MouseEventArgs args)
        {
            if (CreateTabPane != null)
            {
                AntTabPane pane = CreateTabPane();
                Dictionary<string, object> properties = new Dictionary<string, object>();
                properties[nameof(AntTabPane.Parent)] = this;
                properties[nameof(AntTabPane.ForceRender)] = pane.ForceRender;
                properties[nameof(AntTabPane.Key)] = pane.Key;
                properties[nameof(AntTabPane.Tab)] = pane.Tab;
                properties[nameof(AntTabPane.ChildContent)] = pane.ChildContent;
                properties[nameof(AntTabPane.disabled)] = pane.disabled;
                pane.SetParametersAsync(ParameterView.FromDictionary(properties));
                pane.Parent = this;
                ActivatePane(pane);
            }
        }

        public void RemoveTabPane(AntTabPane pane)
        {
            _needRefresh = true;
            Panes.Remove(pane);
            if (pane.IsActive && Panes.Count > 0)
            {
                ActivatePane(Panes[0]);
            }
        }

        private async void ActivatePane(AntTabPane tabPane)
        {
            if (!tabPane.disabled && Panes.Contains(tabPane))
            {
                if (_activePane != null)
                {
                    _activePane.IsActive = false;
                }
                tabPane.IsActive = true;
                _activePane = tabPane;
                ActiveKey = _activePane.Key;
                StateHasChanged();
            }
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_activePane == null)
            {
                throw new ArgumentNullException($"One of {nameof(ActiveKey)} and {nameof(DefaultActiveKey)} should be set");
            }

            await TryRenderInk();

            await TryRenderNavIcon();
            _needRefresh = false;
        }

        private async Task TryRenderNavIcon()
        {
            if (_needRefresh)
            {
                if (IsHorizontal)
                {
                    // Prev/Next icon, show icon if scroll div's width less than tab bars' total width
                    _navSection = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _scrollTabBar)).clientWidth;
                    _navTotal = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _tabBars)).clientWidth;
                }
                else
                {
                    _navSection = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _scrollTabBar)).clientHeight;
                    _navTotal = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _tabBars)).clientHeight;
                }
                RefreshNavIcon();
            }
        }

        private async Task TryRenderInk()
        {
            if (_renderedActivePane != _activePane)
            {
                // TODO: slide to activated tab
                // animate Active Ink
                // ink bar
                Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _activeTabBar);
                if (IsHorizontal)
                {
                    _inkStyle = $"width: {element.clientWidth}px; display: block; transform: translate3d({element.offsetLeft}px, 0px, 0px);";
                    _contentStyle = $"margin-left: -{Panes.IndexOf(_activePane)}00%;";
                }
                else
                {
                    _inkStyle = $"height: {element.clientHeight}px; display: block; transform: translate3d(0px, {element.offsetTop}px, 0px);";
                    _contentStyle = $"margin-top: -{Panes.IndexOf(_activePane)}00%;";
                }
                StateHasChanged();
                _renderedActivePane = _activePane;
            }
        }

        private async void OnPrevClicked()
        {
            _needRefresh = true;
            if (OnPrevClick.HasDelegate)
            {
                await OnPrevClick.InvokeAsync(null);
            }

            // get the old offset to the left, and _navIndex != 0 because prev will be disabled
            int left = _navIndex * _navSection;
            if (IsHorizontal)
            {
                _navSection = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _scrollTabBar)).clientWidth;
                _navTotal = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _tabBars)).clientWidth;
            }
            else
            {
                _navSection = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _scrollTabBar)).clientHeight;
                _navTotal = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _tabBars)).clientHeight;
            }
            // calculate the current _navIndex after users resize the browser, and _navIndex > 0 guaranteed since left > 0
            _navIndex = (int)Math.Ceiling(1.0 * left / _navSection);
            int offset = --_navIndex * _navSection;
            if (IsHorizontal)
            {
                _navStyle = $"transform: translate3d(-{offset}px, 0px, 0px);";
            }
            else
            {
                _navStyle = $"transform: translate3d(0px, -{offset}px, 0px);";
            }
            RefreshNavIcon();
            _needRefresh = false;
        }

        private async void OnNextClicked()
        {
            // BUG: when vertical
            _needRefresh = true;
            if (OnNextClick.HasDelegate)
            {
                await OnNextClick.InvokeAsync(null);
            }

            // get the old offset to the left
            int left = _navIndex * _navSection;
            if (IsHorizontal)
            {
                _navSection = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _scrollTabBar)).clientWidth;
                _navTotal = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _tabBars)).clientWidth;
            }
            else
            {
                _navSection = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _scrollTabBar)).clientHeight;
                _navTotal = (await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _tabBars)).clientHeight;
            }
            // calculate the current _navIndex after users resize the browser
            _navIndex = left / _navSection;
            int offset = Math.Min(++_navIndex * _navSection, _navTotal / _navSection * _navSection);
            if (IsHorizontal)
            {
                _navStyle = $"transform: translate3d(-{offset}px, 0px, 0px);";
            }
            else
            {
                _navStyle = $"transform: translate3d(0px, -{offset}px, 0px);";
            }
            RefreshNavIcon();
            _needRefresh = false;
        }

        private void RefreshNavIcon()
        {
            if (_navTotal > _navSection)
            {
                if (_navIndex == 0)
                {
                    // reach the first section
                    _prevIconEnabled = false;
                }
                else
                {
                    _prevIconEnabled = true;
                }

                if ((_navIndex + 1) * _navSection > _navTotal)
                {
                    // reach the last section
                    _nextIconEnabled = false;
                }
                else
                {
                    _nextIconEnabled = true;
                }
            }
            else
            {
                // hide icon
                _prevIconEnabled = null;
                _nextIconEnabled = null;
            }

            StateHasChanged();
        }

        protected override bool ShouldRender()
        {
            return _needRefresh || _renderedActivePane != _activePane;
        }
    }
}