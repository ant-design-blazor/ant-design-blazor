using System;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Sider
    {
        private static string _prefixCls = "ant-layout-sider";

        [Parameter] public bool Collapsible { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// When Trigger is null, `OnCollapse` won't be invoked after `Collapsed` was changed.
        /// </summary>
        [Parameter] public RenderFragment Trigger { get; set; }

        [Parameter] public bool NoTrigger { get; set; }

        [CascadingParameter] public Layout Parent { get; set; }

        [Parameter] public BreakpointType Breakpoint { get; set; }

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
                OnCollapsed?.Invoke(_isCollapsed);
            }
        }

        [Parameter]
        public EventCallback<bool> OnCollapse { get; set; }

        [Parameter]
        public EventCallback<bool> OnBreakpoint { get; set; }

        [Inject] public DomEventService DomEventService { get; set; }

        private int ComputedWidth => _isCollapsed ? CollapsedWidth : Width;

        /// <summary>
        /// param1: collapsed or not
        /// param2: call inside sider or not
        /// </summary>
        public event Action<bool> OnCollapsed;

        private bool _isCollapsed;

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

            SetClass();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender && Breakpoint != null)
            {
                var dimensions = await JsInvokeAsync<Window>(JSInteropConstants.GetWindow);
                DomEventService.AddEventListener<Window>("window", "resize", OnResize, false);
                OptimizeSize(dimensions.innerWidth);
            }
        }

        private void OnResize(Window window)
        {
            OptimizeSize(window.innerWidth);
        }

        public void ToggleCollapsed()
        {
            _isCollapsed = !_isCollapsed;
            OnCollapsed?.Invoke(_isCollapsed);

            if (OnCollapse.HasDelegate)
            {
                OnCollapse.InvokeAsync(_isCollapsed);
            }
        }

        private void OptimizeSize(decimal windowWidth)
        {
            if (windowWidth < Breakpoint?.Width)
            {
                _isCollapsed = true;
                OnBreakpoint.InvokeAsync(true);
            }
            else
                _isCollapsed = false;

            OnCollapsed?.Invoke(_isCollapsed);
            if (OnCollapse.HasDelegate)
            {
                OnCollapse.InvokeAsync(_isCollapsed);
            }

            StateHasChanged();
        }

        private void OnFocusLost()
        {
            if (_isCollapsed ||
                CollapsedWidth != 0)
            {
                return;
            }
            
            ToggleCollapsed();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            DomEventService.RemoveEventListerner<Window>("window", "resize", OnResize);
        }
    }
}
