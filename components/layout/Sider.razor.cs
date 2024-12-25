// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    /// <summary>
    /// The sidebar of <see cref="Layout" />
    /// </summary>
    public partial class Sider : AntDomComponentBase
    {
        private static readonly string _prefixCls = "ant-layout-sider";

        /// <summary>
        /// If sider is collapsible or not
        /// </summary>
        [Parameter]
        public bool Collapsible { get; set; }

        /// <summary>
        /// Content of the sider
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Collapse/expand UI element to allow manually changing.
        /// </summary>
        [Parameter]
        public RenderFragment Trigger { get; set; }

        /// <summary>
        /// Remove UI trigger for collapse/expanding manually
        /// </summary>
        [Parameter]
        public bool NoTrigger { get; set; }

        [CascadingParameter] 
        public Layout Parent { get; set; }

        /// <summary>
        /// Breakpoint at which sider will collapse by default
        /// </summary>
        [Parameter]
        public BreakpointType? Breakpoint { get; set; }

        /// <summary>
        /// Color theme
        /// </summary>
        /// <default value="SiderTheme.Dark"/>
        [Parameter]
        public SiderTheme Theme { get; set; } = SiderTheme.Dark;

        /// <summary>
        /// Width of sider when expanded, in pixels
        /// </summary>
        /// <default value="200" />
        [Parameter]
        public int Width { get; set; } = 200;

        /// <summary>
        /// Width of sider when collapsed, in pixels
        /// </summary>
        /// <default value="80" />
        [Parameter]
        public int CollapsedWidth { get; set; } = 80;

        /// <summary>
        /// If sider is collapsed or not
        /// </summary>
        [Parameter]
        public bool Collapsed
        {
            get => _isCollapsed;
            set
            {
                if (_isCollapsed == value)
                    return;

                _isCollapsed = value;
                _menu?.CollapseUpdated(value);
            }
        }

        /// <summary>
        /// Callback executed when sider is changes from open to collapsed, regardless of what caused it
        /// </summary>
        [Parameter]
        public EventCallback<bool> CollapsedChanged { get; set; }

        /// <summary>
        /// Callback executed when sider is changes from open to collapsed, regardless of what caused it
        /// </summary>
        [Obsolete("Use CollapsedChanged instead")]
        [Parameter]
        public EventCallback<bool> OnCollapse { get; set; }

        /// <summary>
        /// Callback executed when window size changes the breakpoint
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnBreakpoint { get; set; }

        /// <summary>
        /// To set the initial status
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool DefaultCollapsed { get; set; }

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        private int ComputedWidth => _isCollapsed ? CollapsedWidth : Width;

        private bool _isCollapsed;

        private bool _showTrigger;

        private bool _brokenPoint;

        private Menu _menu;

        private string SiderStyles =>
            $"flex: 0 0 {ComputedWidth}px;" +
            $"max-width: {ComputedWidth}px;" +
            $"min-width: {ComputedWidth}px;" +
            $"width: {ComputedWidth}px;" +
            (_isCollapsed ? "overflow:initial;" : "");

        private RenderFragment DefaultTrigger => builder =>
        {
            builder.OpenComponent<Icon>(1);
            builder.AddAttribute(2, "Type", _isCollapsed ? IconType.Outline.Right : IconType.Outline.Left);
            builder.AddAttribute(3, "Theme", IconThemeType.Outline);
            builder.CloseComponent();
        };

        private void SetClass()
        {
            var hashId = UseStyle("ant-layout", LayoutStyle.UseComponentStyle);
            ClassMapper.Clear()
                .Add(_prefixCls)
                .Add(hashId)
                .If($"{_prefixCls}-dark", () => Theme == SiderTheme.Dark)
                .If($"{_prefixCls}-light", () => Theme == SiderTheme.Light)
                .If($"{_prefixCls}-has-trigger", () => Collapsible)
                .If($"{_prefixCls}-collapsed", () => _isCollapsed)
                .If($"{_prefixCls}-zero-width", () => CollapsedWidth == 0 && _isCollapsed);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent?.HasSider();

            if (Trigger == null && !NoTrigger)
            {
                Trigger = DefaultTrigger;
            }

            if (DefaultCollapsed)
            {
                Collapsed = true;
            }

            SetClass();
        }

        internal void AddMenu(Menu menu)
        {
            _menu = menu;
            if (_isCollapsed)
            {
                _menu.CollapseUpdated(true);
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender && Breakpoint != null)
            {
                var dimensions = await JsInvokeAsync<Window>(JSInteropConstants.GetWindow);
                DomEventListener.AddShared<Window>("window", "resize", OnResize);
                OptimizeSize(dimensions.InnerWidth);
            }
        }

        internal void OnResize(Window window)
        {
            OptimizeSize(window.InnerWidth);
        }

        private void ToggleCollapsed()
        {
            _isCollapsed = !_isCollapsed;
            _menu?.CollapseUpdated(_isCollapsed);

            if (OnCollapse.HasDelegate)
            {
                OnCollapse.InvokeAsync(_isCollapsed);
            }

            if (CollapsedChanged.HasDelegate)
            {
                CollapsedChanged.InvokeAsync(_isCollapsed);
            }
        }

        private void OptimizeSize(decimal windowWidth)
        {
            var originalCollapsed = _isCollapsed;
            var originlBrokenPoint = _brokenPoint;

            if (windowWidth < (int)Breakpoint)
            {
                _brokenPoint = true;
                _isCollapsed = true;
                _showTrigger = true;
            }
            else
            {
                _brokenPoint = false;
                _isCollapsed = false;
                _showTrigger = false;
            }

            if (originlBrokenPoint != _brokenPoint && OnBreakpoint.HasDelegate)
            {
                OnBreakpoint.InvokeAsync(_brokenPoint);
            }

            if (originalCollapsed != _isCollapsed)
            {
                _menu?.CollapseUpdated(_isCollapsed);

                if (OnCollapse.HasDelegate)
                {
                    OnCollapse.InvokeAsync(_isCollapsed);
                }

                if (CollapsedChanged.HasDelegate)
                {
                    CollapsedChanged.InvokeAsync(_isCollapsed);
                }
            }

            InvokeAsync(StateHasChanged);
        }

        protected override void Dispose(bool disposing)
        {
            _menu = null;
            DomEventListener?.Dispose();
            base.Dispose(disposing);
        }
    }
}
