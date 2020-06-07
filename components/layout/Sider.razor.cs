using System;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Sider
    {
        private static string _prefixCls = "ant-layout-sider";

        private string SiderStyles =>
            $"flex: 0 0 {width}px;" +
            $"max-width: {width}px;" +
            $"min-width: {width}px;" +
            $"width: {width}px;" +
            (isCollapsed ? "overflow:initial;" : "");

        [Parameter] public bool Collapsible { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public RenderFragment Trigger { get; set; }

        [Parameter] public bool NoTrigger { get; set; }

        [CascadingParameter] public Layout Parent { get; set; }

        [Parameter] public BreakpointType Breakpoint { get; set; }

        [Parameter] public SiderTheme Theme { get; set; } = SiderTheme.Dark;

        [Parameter] public int Width { get; set; } = 200;

        [Parameter] public int CollapsedWidth { get; set; } = 80;

        public event Action<bool> OnCollapsed;

        private bool isCollapsed;

        [Parameter]
        public bool Collapsed
        {
            get => isCollapsed;
            set
            {
                if (isCollapsed == value)
                    return;

                isCollapsed = value;
                OnCollapsed?.Invoke(isCollapsed);
            }
        }

        [Parameter]
        public EventCallback<bool> OnCollapse { get; set; }

        [Parameter]
        public EventCallback<bool> OnBreakpoint { get; set; }

        [Inject] public DomEventService DomEventService { get; set; }

        private int width => isCollapsed ? CollapsedWidth : Width;

        private RenderFragment defaultTrigger => builder =>
        {
            builder.OpenComponent<Icon>(1);
            builder.AddAttribute(2, "Type", isCollapsed ? "right" : "left");
            builder.AddAttribute(3, "Theme", "outline");
            builder.CloseComponent();
        };

        private void SetClass()
        {
            ClassMapper.Clear()
                .Add(_prefixCls)
                .Add($"{_prefixCls}-{Theme}")
                .If($"{_prefixCls}-has-trigger", () => Collapsible)
                .If($"{_prefixCls}-collapsed", () => isCollapsed)
                .If($"{_prefixCls}-zero-width", () => CollapsedWidth == 0 && isCollapsed);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent?.HasSider();
            if (Trigger == null && !NoTrigger)
            {
                Trigger = defaultTrigger;
            }
            isCollapsed = Collapsed;
            SetClass();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                var dimensions = await JsInvokeAsync<Window>(JSInteropConstants.getWindow);
                DomEventService.AddEventListener<Window>("window", "resize", args => OptimizeSize(args.innerWidth));
                OptimizeSize(dimensions.innerWidth);
            }
        }

        private void ToggleCollapsed()
        {
            isCollapsed = !isCollapsed;
            OnCollapsed?.Invoke(isCollapsed);
        }

        private void OptimizeSize(decimal windowWidth)
        {
            if (windowWidth < Breakpoint?.Width)
            {
                isCollapsed = true;
                OnBreakpoint.InvokeAsync(true);
            }
            else
                isCollapsed = false;

            OnCollapsed?.Invoke(isCollapsed);
            StateHasChanged();
        }
    }
}