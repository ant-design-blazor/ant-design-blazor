using System;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Sider : AntDomComponentBase
    {
        private static readonly string _prefixCls = "ant-layout-sider";

        [Parameter] public bool Collapsible { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// When Trigger is null, `OnCollapse` won't be invoked after `Collapsed` was changed.
        /// </summary>
        [Parameter] public RenderFragment Trigger { get; set; }

        [Parameter] public bool NoTrigger { get; set; }

        [CascadingParameter] public Layout Parent { get; set; }

        [Parameter] public BreakpointType? Breakpoint { get; set; }

        [Parameter] public SiderTheme Theme { get; set; } = SiderTheme.Dark;

        [Parameter] public int Width { get; set; } = 200;

        [Parameter] public int CollapsedWidth { get; set; } = 80;

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

        [Parameter]
        public EventCallback<bool> CollapsedChanged { get; set; }

        [Obsolete("Use CollapsedChanged instead")]
        [Parameter]
        public EventCallback<bool> OnCollapse { get; set; }

        [Parameter]
        public EventCallback<bool> OnBreakpoint { get; set; }

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
            builder.AddAttribute(2, "Type", _isCollapsed ? "right" : "left");
            builder.AddAttribute(3, "Theme", "outline");
            builder.CloseComponent();
        };

        private void SetClass()
        {
            ClassMapper.Clear()
                .Add(_prefixCls)
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

        public void ToggleCollapsed()
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
