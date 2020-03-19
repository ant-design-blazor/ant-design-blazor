using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntBlazor
{
    public class AntDrawerBase : AntDomComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        private OneOf<RenderFragment, string> _content;

        protected string ContentString { get; set; }

        protected RenderFragment ContentTemplate { get; set; }

        [Parameter]
        public OneOf<RenderFragment, string> Content
        {
            get => _content;
            set
            {
                _content = value;
                value.Switch(rf => ContentTemplate = rf, str => ContentString = str);
            }
        }

        [Parameter] public bool closable { get; set; } = true;

        [Parameter] public bool maskClosable { get; set; } = true;

        [Parameter] public bool mask { get; set; } = true;

        [Parameter] public bool noAnimation { get; set; } = false;

        [Parameter] public bool keyboard { get; set; } = true;

        protected RenderFragment titleTemplate { get; set; }

        protected string titleString { get; set; }

        protected OneOf<RenderFragment, string> _title;

        [Parameter]
        public OneOf<RenderFragment, string> title
        {
            set
            {
                _title = value;
                value.Switch(rf =>
                {
                    titleTemplate = rf;
                    titleString = null;
                }, str =>
                {
                    titleString = str;
                    titleTemplate = null;
                });
            }
        }

        /// <summary>
        /// "left" | "right" | "top" | "bottom"
        /// </summary>
        [Parameter] public string placement { get; set; } = "right";

        [Parameter] public string maskStyle { get; set; }

        [Parameter] public string bodyStyle { get; set; }

        [Parameter] public string wrapClassName { get; set; }

        [Parameter] public int width { get; set; } = 256;

        [Parameter] public int height { get; set; } = 256;

        [Parameter] public int zIndex { get; set; } = 1000;

        [Parameter] public int offsetX { get; set; } = 0;

        [Parameter] public int offsetY { get; set; } = 0;

        [Parameter]
        public bool visible
        {
            get => this.isOpen;
            set => this.isOpen = value;
        }

        [Parameter] public EventCallback onViewInit { get; set; }

        [Parameter] public EventCallback onClose { get; set; }

        protected bool isOpen = default;

        private string originalPlacement;

        private bool placementChanging { get; set; } = false;

        private int placementChangeTimeoutId = -1;

        protected string offsetTransform
        {
            get
            {
                if (!this.isOpen || this.offsetX + this.offsetY == 0)
                {
                    return null;
                }

                return placement switch
                {
                    "left" => $"translateX({this.offsetX}px)",
                    "right" => $"translateX(-{this.offsetX}px)",
                    "top" => $"translateY({this.offsetY}px)",
                    "bottom" => $"translateY(-{this.offsetY}px)",
                    _ => null
                };
            }
        }

        protected string transform
        {
            get
            {
                if (this.isOpen)
                {
                    return null;
                }

                return this.placement switch
                {
                    "left" => "translateX(-100%)",
                    "right" => "translateX(100%)",
                    "top" => "translateY(-100%)",
                    "bottom" => "translateY(100%)",
                    _ => null
                };
            }
        }

        protected bool isLeftOrRight => placement == "left" || this.placement == "right";

        protected string _width => this.isLeftOrRight ? StyleHelper.ToCssPixel(this.width) : null;

        protected string _height => !this.isLeftOrRight ? StyleHelper.ToCssPixel(this.height) : null;

        protected ClassMapper TitleClassMapper { get; set; } = new ClassMapper();

        protected string wrapperStyle => $@"
            {(_width != null ? $"width:{_width};" : "")}
            {(_height != null ? $"height:{_height};" : "")}
            {(transform != null ? $"transform:{transform};" : "")}
            {(placementChanging ? "transition:none;" : "")}";

        protected string drawerStyle => $@"
            {(transform != null ? $"transform:{transform};" : "")}
            {(placementChanging ? "transition:none;" : "")}
            z-index:{zIndex};";

        protected void SetClass()
        {
            var prefixCls = "ant-drawer";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-open", () => isOpen)
                .If($"{prefixCls}-{placement}", () => placement.IsIn("top", "bottom", "right", "left"))
                ;

            this.TitleClassMapper.Clear()
                .If("ant-drawer-header", () => _title.Value != null)
                .If("ant-drawer-header-no-title", () => _title.Value != null)
                ;
        }

        protected override void OnInitialized()
        {
            this.originalPlacement = placement;

            this.SetClass();

            base.OnInitialized();
        }

        private bool isPlacementFirstChange = true;

        protected override void OnParametersSet()
        {
            this.SetClass();
            if (string.IsNullOrEmpty(placement) && placement != originalPlacement)
            {
                this.originalPlacement = placement;
                isPlacementFirstChange = false;
                if (!isPlacementFirstChange)
                {
                    this.triggerPlacementChangeCycleOnce();
                }
            }

            base.OnParametersSet();
        }

        private Timer timer;

        private void triggerPlacementChangeCycleOnce()
        {
            if (!noAnimation)
            {
                this.placementChanging = true;
                InvokeStateHasChanged();
                timer = new Timer()
                {
                    AutoReset = false,
                    Interval = 300,
                };
                timer.Elapsed += (_, args) =>
                {
                    this.placementChanging = false;
                    InvokeStateHasChanged();
                };
                timer.Start();
            }
        }

        protected async Task maskClick()
        {
            if (this.maskClosable && this.mask && this.onClose.HasDelegate)
            {
                await this.onClose.InvokeAsync(this);
            }
        }

        protected async Task closeClick()
        {
            if (onClose.HasDelegate)
            {
                timer?.Dispose();

                await onClose.InvokeAsync(this);
            }
        }
    }
}