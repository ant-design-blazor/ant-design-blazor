using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Drawer : AntDomComponentBase
    {
        #region Parameters

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

        [Parameter]
        public int ZIndex
        {
            get { return _zIndex; }
            set
            {
                _zIndex = value;
                if (_zIndex == 1000)
                    _zIndexStyle = "";
                else
                    _zIndexStyle = $"z-index: {_zIndex};";
            }
        }

        private string InnerZIndexStyle => (_status.IsOpen() || _status == ComponentStatus.Closing) ? _zIndexStyle : "z-index:-9999;";

        [Parameter] public int OffsetX { get; set; } = 0;

        [Parameter] public int OffsetY { get; set; } = 0;

        [Parameter]
        public bool Visible
        {
            get => this._isOpen;
            set
            {
                if (this._isOpen && !value)
                {
                    _status = ComponentStatus.Closing;
                }
                else if (!this._isOpen && value)
                {
                    _status = ComponentStatus.Opening;
                }
                this._isOpen = value;
            }
        }

        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public RenderFragment Handler { get; set; }

        #endregion

        private OneOf<RenderFragment, string> _content;

        private string ContentString { get; set; }

        private RenderFragment ContentTemplate { get; set; }

        private ComponentStatus _status;
        private bool _hasInvokeClosed;
        private bool _isOpen = default;

        private string _originalPlacement;

        private bool PlacementChanging { get; set; } = false;

        /// <summary>
        /// 设置 Drawer 是否显示，以及显示时候的位置 Offset
        /// </summary>
        private string OffsetTransform
        {
            get
            {
                if (_status.IsNotOpen() || (OffsetX == 0 && OffsetY == 0))
                {
                    return null;
                }

                return Placement switch
                {
                    "left" => $"translateX({this.OffsetX}px);",
                    "right" => $"translateX(-{this.OffsetX}px);",
                    "top" => $"translateY({this.OffsetY}px);",
                    "bottom" => $"translateY(-{this.OffsetY}px);",
                    _ => null
                };
            }
        }

        private const string Duration = "0.3s";
        private const string Ease = "cubic-bezier(0.78, 0.14, 0.15, 0.86)";
        private string _widthTransition = "";
        private readonly string _transformTransition = $"transform {Duration} {Ease} 0s";
        private string _heightTransition = "";

        /// <summary>
        /// 设置 Drawer 是否隐藏，以及隐藏时候的位置 Offset
        /// </summary>
        private string Transform
        {
            get
            {
                if (_status.IsOpen())
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

        private static Regex _renderInCurrentContainerRegex = new Regex("position:[\\s]*absolute");

        private string _drawerStyle = "";

        private bool _isPlacementFirstChange = true;

        private void SetClass()
        {
            var prefixCls = "ant-drawer";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-open", () => _isOpen)
                .If($"{prefixCls}-{Placement}", () => Placement.IsIn("top", "bottom", "right", "left"))
                .If($"{prefixCls}-rtl", () => RTL)
                ;

            this.TitleClassMapper.Clear()
                .If("ant-drawer-header", () => _title.Value != null)
                .If("ant-drawer-header-no-title", () => _title.Value == null)
                ;
        }

        protected override void OnInitialized()
        {
            this._originalPlacement = Placement;

            // TODO: remove
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

            _drawerStyle = "";

            base.OnParametersSet();
        }

        protected override async Task OnAfterRenderAsync(bool isFirst)
        {
            switch (_status)
            {
                case ComponentStatus.Opening:
                    {
                        _status = ComponentStatus.Opened;
                        _hasInvokeClosed = false;
                        if (string.IsNullOrWhiteSpace(Style))
                        {
                            _ = JsInvokeAsync(JSInteropConstants.DisableBodyScroll);
                        }
                        else if (!_renderInCurrentContainerRegex.IsMatch(Style))
                        {
                            await JsInvokeAsync(JSInteropConstants.DisableBodyScroll);
                        }

                        CalcDrawerStyle();
                        StateHasChanged();
                        await Task.Delay(3000);
                        _drawerStyle = !string.IsNullOrWhiteSpace(OffsetTransform) ? $"transform: {OffsetTransform};" : "";
                        StateHasChanged();
                        break;
                    }
                case ComponentStatus.Closing:
                    {
                        _status = ComponentStatus.Closed;
                        if (!_hasInvokeClosed)
                        {
                            await HandleClose(true);
                        }
                        await JsInvokeAsync(JSInteropConstants.EnableBodyScroll);
                        break;
                    }
            }
            await base.OnAfterRenderAsync(isFirst);
        }

        private Timer _timer;
        private int _zIndex = 1000;
        private string _zIndexStyle = "";

        private void TriggerPlacementChangeCycleOnce()
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

        /// <summary>
        /// trigger when mask is clicked
        /// </summary>
        /// <returns></returns>
        private async Task MaskClick()
        {
            if (this.MaskClosable && this.Mask && this.OnClose.HasDelegate)
            {
                await HandleClose();
            }
        }

        /// <summary>
        /// trigger when Closer is clicked
        /// </summary>
        /// <returns></returns>
        private async Task CloseClick()
        {
            if (OnClose.HasDelegate)
            {
                _timer?.Dispose();

                await HandleClose();
            }
        }

        /// <summary>
        /// clean-up after close
        /// </summary>
        /// <param name="isChangeByParamater"></param>
        /// <returns></returns>
        private async Task HandleClose(bool isChangeByParamater = false)
        {
            _hasInvokeClosed = true;
            if (!isChangeByParamater)
            {
                await OnClose.InvokeAsync(this);
            }
        }

        private void CalcAnimation()
        {
            switch (this.Placement)
            {
                case "left":
                case "right":
                    _widthTransition = $"width 0s {Ease} {Duration}";
                    break;

                case "top":
                case "bottom":
                    _heightTransition = $"height 0s {Ease} {Duration}";
                    break;

                default:
                    break;
            }
        }

        private void CalcDrawerStyle()
        {
            string style = null;
            if (_status == ComponentStatus.Opened)
            {
                CalcAnimation();
                if (string.IsNullOrWhiteSpace(_heightTransition))
                {
                    _heightTransition += ",";
                }

                style = $"transition:{_transformTransition} {_heightTransition} {_widthTransition};";
            }

            if (!string.IsNullOrWhiteSpace(OffsetTransform))
            {
                style += $"transform: {OffsetTransform};";
            }

            _drawerStyle = style;
        }

        protected override void Dispose(bool disposing)
        {
            _timer?.Dispose();
            base.Dispose(disposing);
        }
    }
}
