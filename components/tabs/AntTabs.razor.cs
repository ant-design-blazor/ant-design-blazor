using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AntBlazor
{
    public partial class AntTabs : AntDomComponentBase
    {
        private const string PrefixCls = "ant-tabs";
        private ClassMapper _barClassMapper = new ClassMapper();
        private List<AntTabPane> _panes = new List<AntTabPane>();
        private AntTabPane _activePane;
        private AntTabPane _renderedActivePane;
        private ElementReference _activeTabBar;
        private string _inkStyle;
        private string _contentStyle;

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
                _activeKey = value;
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
        public EventCallback<object> OnNextClick { get; set; }

        /// <summary>
        /// Callback executed when prev button is clicked
        /// </summary>
        [Parameter]
        public EventCallback<object> OnPrevClick { get; set; }

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

        #endregion Parameters

        public override Task SetParametersAsync(ParameterView parameters)
        {
            string type = parameters.GetValueOrDefault<string>(nameof(Type));

            if (type == AntTabType.Card)
            {
                // according to ant design documents,
                // Animated default to false when type="card"
                Animated = false;
            }

            return base.SetParametersAsync(parameters);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            ClassMapper.Clear()
                .Add(PrefixCls)
                .Add($"{PrefixCls}-{TabPosition}")
                .Add($"{PrefixCls}-{Type}")
                .If($"{PrefixCls}-{AntTabType.Card}", () => Type == AntTabType.EditableCard)
                .If($"{PrefixCls}-no-animation", () => !Animated);

            _barClassMapper.Clear()
                .Add($"{PrefixCls}-bar")
                .Add($"{PrefixCls}-{TabPosition}-bar");
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

            if (_panes.Select(p => p.Key).Contains(tabPane.Key))
            {
                throw new ArgumentException("An AntTabPane with the same key already exists");
            }

            _panes.Add(tabPane);
            if (_activePane == null)
            {
                ActivatePane(tabPane);
            }
            StateHasChanged();
        }

        private async void ActivatePane(AntTabPane tabPane)
        {
            if (!tabPane.disabled)
            {
                if (_activePane != null)
                {
                    _activePane.IsActive = false;
                }
                tabPane.IsActive = true;
                _activePane = tabPane;
                StateHasChanged();
            }
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (_renderedActivePane != _activePane)
            {
                _renderedActivePane = _activePane;
                // ink bar
                Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _activeTabBar);
                _inkStyle = $"width: {element.clientWidth}px; display: block; transform: translate3d({element.offsetLeft}px, 0px, 0px);";
                _contentStyle = $"margin-left: -{_panes.IndexOf(_activePane)}00%;";
                StateHasChanged();
            }
        }
    }
}