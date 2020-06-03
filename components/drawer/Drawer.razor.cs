using System.Text.RegularExpressions;
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

        private bool _isRenderAnimation = false;
        private const string _duration = "0.3s";
        private const string _ease = "cubic-bezier(0.78, 0.14, 0.15, 0.86)";
        private string _widthTransition = "";
        private readonly string _transformTransition = $"transform {_duration} {_ease} 0s";
        private string _heightTransition = "";

        private string Transform
        {
            get
            {
                if (this._isOpen && this._isRenderAnimation)
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

        private Regex _renderInCurrentContainerRegex = new Regex("position:[\\s]*absolute");

        private string DrawerStyle;

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

            DrawerStyle = "";

            base.OnParametersSet();
        }

        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
            if (_isOpen && !NoAnimation)
            {
                if (!_isRenderAnimation)
                {
                    _isRenderAnimation = true;
                    CalcDrawerStyle();
                    await Task.Delay(10);
                    StateHasChanged();

                    if (string.IsNullOrWhiteSpace(Style))
                    {
                        _ = JsInvokeAsync(JSInteropConstants.disableBodyScroll);
                    }
                    else if (!string.IsNullOrWhiteSpace(Style))
                    {
                        var m = _renderInCurrentContainerRegex.IsMatch(Style);
                        if (!m)
                        {
                            await JsInvokeAsync(JSInteropConstants.disableBodyScroll);
                        }
                    }
                }
                else
                {
                    DrawerStyle = "";
                    StateHasChanged();
                }
            }
            await base.OnAfterRenderAsync(isFirst);
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
                await HandleClose();
            }
        }

        private async Task CloseClick()
        {
            if (OnClose.HasDelegate)
            {
                _timer?.Dispose();

                await HandleClose();
            }
        }

        private async Task HandleClose()
        {
            _isRenderAnimation = false;
            await OnClose.InvokeAsync(this);
            await Task.Delay(10);
            await JsInvokeAsync(JSInteropConstants.enableDrawerBodyScroll);
        }

        private void CalcAnimation()
        {
            switch (this.Placement)
            {
                case "left":
                    _widthTransition = $"width 0s {_ease} {_duration}";
                    break;
                case "right":
                    _widthTransition = $"width 0s {_ease} {_duration}";
                    break;
                case "top":
                case "bottom":
                    _heightTransition = $"height 0s {_ease} {_duration}";
                    break;
                default:
                    break;
            }
        }

        private void CalcDrawerStyle()
        {
            string style = null;
            if (_isOpen && _isRenderAnimation)
            {
                CalcAnimation();
                if (string.IsNullOrWhiteSpace(_heightTransition))
                {
                    _heightTransition += ",";
                }

                style = $"transition:{_transformTransition} {_heightTransition} {_widthTransition};";
            }
            DrawerStyle = style;
        }
    }
}
