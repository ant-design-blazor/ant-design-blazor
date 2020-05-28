using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Drawer : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

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

        [Parameter]
        public bool Closable { get; set; } = true;

        [Parameter]
        public bool MaskClosable { get; set; } = true;

        [Parameter]
        public bool Mask { get; set; } = true;

        [Parameter]
        public bool NoAnimation { get; set; } = false;

        [Parameter]
        public bool Keyboard { get; set; } = true;

        private RenderFragment TitleTemplate { get; set; }

        private string TitleString { get; set; }

        private OneOf<RenderFragment, string> _title;

        [Parameter]
        public OneOf<RenderFragment, string> Title
        {
            get => _title;
            set
            {
                _title = value;
                value.Switch(rf =>
                {
                    TitleTemplate = rf;
                    TitleString = null;
                }, str =>
                {
                    TitleString = str;
                    TitleTemplate = null;
                });
            }
        }

        /// <summary>
        /// "left" | "right" | "top" | "bottom"
        /// </summary>
        [Parameter] public string Placement { get; set; } = "right";

        [Parameter] public string MaskStyle { get; set; }

        [Parameter] public string BodyStyle { get; set; }

        [Parameter] public string WrapClassName { get; set; }

        [Parameter] public int Width { get; set; } = 256;

        [Parameter] public int Height { get; set; } = 256;

        [Parameter] public int ZIndex { get; set; } = 1000;

        [Parameter] public int OffsetX { get; set; } = 0;

        [Parameter] public int OffsetY { get; set; } = 0;

        [Parameter]
        public bool Visible
        {
            get => this._isOpen;
            set => this._isOpen = value;
        }

        [Parameter] public EventCallback OnViewInit { get; set; }

        [Parameter] public EventCallback OnClose { get; set; }

        private OneOf<RenderFragment, string> _content;

        private string ContentString { get; set; }

        private RenderFragment ContentTemplate { get; set; }

        private bool _isOpen = default;

        private string _originalPlacement;

        private bool PlacementChanging { get; set; } = false;

        private string OffsetTransform
        {
            get
            {
                if (!this._isOpen || this.OffsetX + this.OffsetY == 0)
                {
                    return null;
                }

                return Placement switch
                {
                    "left" => $"translateX({this.OffsetX}px)",
                    "right" => $"translateX(-{this.OffsetX}px)",
                    "top" => $"translateY({this.OffsetY}px)",
                    "bottom" => $"translateY(-{this.OffsetY}px)",
                    _ => null
                };
            }
        }

        private string Transform
        {
            get
            {
                if (this._isOpen)
                {
                    return null;
                }

                return this.Placement switch
                {
                    "left" => "translateX(-100%)",
                    "right" => "translateX(100%)",
                    "top" => "translateY(-100%)",
                    "bottom" => "translateY(100%)",
                    _ => null
                };
            }
        }

        private bool IsLeftOrRight => Placement == "left" || this.Placement == "right";

        private string WidthPx => this.IsLeftOrRight ? StyleHelper.ToCssPixel(this.Width) : null;

        private string HeightPx => !this.IsLeftOrRight ? StyleHelper.ToCssPixel(this.Height) : null;

        private ClassMapper TitleClassMapper { get; set; } = new ClassMapper();

        private string WrapperStyle => $@"
            {(WidthPx != null ? $"width:{WidthPx};" : "")}
            {(HeightPx != null ? $"height:{HeightPx};" : "")}
            {(Transform != null ? $"transform:{Transform};" : "")}
            {(PlacementChanging ? "transition:none;" : "")}";

        private string DrawerStyle => $@"
            {(Transform != null ? $"transform:{Transform};" : "")}
            {(PlacementChanging ? "transition:none;" : "")}
            z-index:{ZIndex};";

        private bool _isPlacementFirstChange = true;

        private void SetClass()
        {
            var prefixCls = "ant-drawer";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-open", () => _isOpen)
                .If($"{prefixCls}-{Placement}", () => Placement.IsIn("top", "bottom", "right", "left"))
                ;

            this.TitleClassMapper.Clear()
                .If("ant-drawer-header", () => _title.Value != null)
                .If("ant-drawer-header-no-title", () => _title.Value != null)
                ;
        }

        protected override void OnInitialized()
        {
            this._originalPlacement = Placement;

            this.SetClass();

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            this.SetClass();
            if (string.IsNullOrEmpty(Placement) && Placement != _originalPlacement)
            {
                this._originalPlacement = Placement;
                _isPlacementFirstChange = false;
                if (!_isPlacementFirstChange)
                {
                    this.TriggerPlacementChangeCycleOnce();
                }
            }

            base.OnParametersSet();
        }

        private Timer _timer;

        private void TriggerPlacementChangeCycleOnce()
        {
            if (!NoAnimation)
            {
                this.PlacementChanging = true;
                InvokeStateHasChanged();
                _timer = new Timer()
                {
                    AutoReset = false,
                    Interval = 300,
                };
                _timer.Elapsed += (_, args) =>
                {
                    this.PlacementChanging = false;
                    InvokeStateHasChanged();
                };
                _timer.Start();
            }
        }

        private async Task MaskClick()
        {
            if (this.MaskClosable && this.Mask && this.OnClose.HasDelegate)
            {
                await this.OnClose.InvokeAsync(this);
            }
        }

        private async Task CloseClick()
        {
            if (OnClose.HasDelegate)
            {
                _timer?.Dispose();

                await OnClose.InvokeAsync(this);
            }
        }
    }
}
