// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

        /// <summary>
        /// <para>
        /// 是否显示右上角的关闭按钮
        /// </para>
        /// <para>
        /// Whether a close (x) button is visible on top right of the Drawer dialog or not.
        /// </para>
        /// </summary>
        [Parameter]
        public bool Closable { get; set; } = true;

        /// <summary>
        /// <para>
        /// 点击蒙层是否允许关闭
        /// </para>
        /// <para>
        /// Clicking on the mask (area outside the Drawer) to close the Drawer or not.
        /// </para>
        /// </summary>
        [Parameter]
        public bool MaskClosable { get; set; } = true;

        /// <summary>
        /// <para>
        /// 是否显示蒙层
        /// </para>
        /// <para>
        /// Whether to show mask or not.
        /// </para>
        /// </summary>
        [Parameter]
        public bool Mask { get; set; } = true;

        /// <summary>
        /// <para>
        /// 蒙层样式
        /// </para>
        /// <para>
        /// Style for Drawer's mask element.
        /// </para>
        /// </summary>
        [Parameter]
        public string MaskStyle { get; set; }

        /// <summary>
        /// <para>
        /// 是否支持键盘 esc 关闭
        /// </para>
        /// <para>
        /// Whether to support keyboard esc off
        /// </para>
        /// </summary>
        [Parameter]
        public bool Keyboard { get; set; } = true;

        private RenderFragment TitleTemplate { get; set; }

        private string TitleString { get; set; }

        private OneOf<RenderFragment, string> _title;

        /// <summary>
        /// <para>
        /// 标题
        /// </para>
        /// <para>
        /// The title for Drawer.
        /// </para>
        /// </summary>
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
        /// <para>
        /// Drawer 的位置，字符串, "left" | "right" | "top" | "bottom"
        /// </para>
        /// <para>
        /// The placement of the Drawer, option could be left, top, right, bottom
        /// </para>
        /// </summary>
        [Parameter]
        public string Placement { get; set; } = "right";

        /// <summary>
        /// <para>
        /// Drawer body 样式
        /// </para>
        /// <para>
        /// Body style for modal body element. Such as height, padding etc.
        /// </para>
        /// </summary>
        [Parameter]
        public string BodyStyle { get; set; }

        /// <summary>
        /// <para>
        /// Drawer header 抽屉头
        /// </para>
        /// <para>
        /// Header style for modal header element. Such as height, padding etc.
        /// </para>
        /// </summary>
        [Parameter]
        public string HeaderStyle { get; set; }

        /// <summary>
        /// <para>
        /// Drawer对话框外层容器的类名
        /// </para>
        /// <para>
        /// The class name of the container of the Drawer dialog.
        /// </para>
        /// </summary>
        [Parameter]
        public string WrapClassName { get; set; }

        /// <summary>
        /// <para>
        /// 宽度，仅当 <see cref="Placement"/> 为 "left" 或 "right" 时生效
        /// </para>
        /// <para>
        /// Width of the Drawer dialog, only when placement is 'left' or 'right'.
        /// </para>
        /// </summary>
        [Parameter]
        public string Width { get; set; } = "256";

        /// <summary>
        /// <para>
        /// 高度，仅当 <see cref="Placement"/> 为 "top" 或 "bottom" 时生效
        /// </para>
        /// <para>
        /// Height of the Drawer dialog, only when placement is 'top' or 'bottom'.
        /// </para>
        /// </summary>
        [Parameter]
        public string Height { get; set; } = "256";

        /// <summary>
        /// <para>
        /// 设置 Drawer 的 z-index
        /// </para>
        /// <para>
        /// The z-index of the Drawer.
        /// </para>
        /// </summary>
        [Parameter]
        public int ZIndex
        {
            get { return _zIndex; }
            set
            {
                _zIndex = value;
                if (_zIndex == DefaultZIndez)
                    _zIndexStyle = "";
                else
                    _zIndexStyle = $"z-index: {_zIndex};";
            }
        }

        private string InnerZIndexStyle => (_status.IsOpen() || _status == ComponentStatus.Closing) ? _zIndexStyle : "z-index:-9999;";

        /// <summary>
        /// <para>
        /// X 轴方向的偏移量，只在方向为 'left'或'right' 时生效.
        /// </para>
        /// <para>
        /// The the X coordinate offset(px), only when placement is 'left' or 'right'.
        /// </para>
        /// </summary>
        [Parameter]
        public int OffsetX { get; set; } = 0;

        /// <summary>
        /// <para>
        /// Y 轴方向的偏移量，只在方向为 'top'或'bottom' 时生效
        /// </para>
        /// <para>
        /// The the Y coordinate offset(px), only when placement is 'top' or 'bottom'.
        /// </para>
        /// </summary>
        [Parameter]
        public int OffsetY { get; set; } = 0;

        /// <summary>
        /// <para>
        /// Drawer 是否可见
        /// </para>
        /// <para>
        /// Whether the Drawer dialog is visible or not.
        /// </para>
        /// </summary>
        [Parameter]
        public bool Visible
        {
            get => _isOpen;
            set
            {
                if (_isOpen && !value)
                {
                    _status = ComponentStatus.Closing;
                }
                else if (!_isOpen && value)
                {
                    _status = ComponentStatus.Opening;
                }
                _isOpen = value;
            }
        }

        /// <summary>
        /// EventCallback trigger on Visible was changed.
        /// </summary>
        [Parameter]
        public EventCallback<bool> VisibleChanged { get; set; }

        /// <summary>
        /// <para>
        /// 在 Drawer 打开前的回调事件
        /// </para>
        /// <para>
        /// Specify a callback that will be called before drawer displayed
        /// </para>
        /// </summary>
        [Parameter]
        public Func<Task> OnOpen { get; set; }

        /// <summary>
        /// <para>
        /// 在 关闭 前的回调事件，应当在 OnClose 将 <see cref="Visible"/> 设置为false
        /// </para>
        /// <para>
        /// Specify a callback that will be called when a user clicks mask, close button or Cancel button.
        /// </para>
        /// </summary>
        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public RenderFragment Handler { get; set; }

        private OneOf<RenderFragment, string> _content;

        private string ContentString { get; set; }

        private RenderFragment ContentTemplate { get; set; }

        #endregion Parameters

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
                    "left" => $"translateX({OffsetX}px);",
                    "right" => $"translateX(-{OffsetX}px);",
                    "top" => $"translateY({OffsetY}px);",
                    "bottom" => $"translateY(-{OffsetY}px);",
                    _ => null
                };
            }
        }

        private const string Duration = "0.3s";
        private const string Ease = "cubic-bezier(0.78, 0.14, 0.15, 0.86)";
        private readonly string _transformTransition = $"transform {Duration} {Ease} 0s";
        private const int DefaultZIndez = 1000;

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

                return Placement switch
                {
                    "left" => "translateX(-100%)",
                    "right" => "translateX(100%)",
                    "top" => "translateY(-100%)",
                    "bottom" => "translateY(100%)",
                    _ => null
                };
            }
        }

        private bool IsLeftOrRight => Placement == "left" || Placement == "right";

        private string WidthPx => IsLeftOrRight ? StyleHelper.ToCssPixel(Width) : null;

        private string HeightPx => !IsLeftOrRight ? StyleHelper.ToCssPixel(Height) : null;

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
            ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-open", () => _isOpen)
                .If($"{prefixCls}-{Placement}", () => Placement.IsIn("top", "bottom", "right", "left"))
                .If($"{prefixCls}-rtl", () => RTL)
                ;

            TitleClassMapper.Clear()
                .If("ant-drawer-header", () => _title.Value != null)
                .If("ant-drawer-header-no-title", () => _title.Value == null)
                ;
        }

        protected override void OnInitialized()
        {
            _originalPlacement = Placement;
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            SetClass();
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

                        if (OnOpen != null)
                        {
                            await OnOpen.Invoke();
                        }

                        if (Visible == false && VisibleChanged.HasDelegate)
                        {
                            await VisibleChanged.InvokeAsync(true);
                        }

                        _hasInvokeClosed = false;
                        if (string.IsNullOrWhiteSpace(Style))
                        {
                            await JsInvokeAsync(JSInteropConstants.DisableBodyScroll);
                        }
                        else if (!_renderInCurrentContainerRegex.IsMatch(Style))
                        {
                            await JsInvokeAsync(JSInteropConstants.DisableBodyScroll);
                        }

                        CalcDrawerStyle();
                        StateHasChanged();
                        await Task.Delay(3000);
                        _drawerStyle = !string.IsNullOrWhiteSpace(OffsetTransform)
                            ? $"transform: {OffsetTransform};"
                            : string.Empty;
                        StateHasChanged();
                        break;
                    }
                case ComponentStatus.Closing:
                    {
                        await Task.Delay(3000);
                        _status = ComponentStatus.Closed;
                        StateHasChanged();
                        if (!_hasInvokeClosed)
                        {
                            await HandleClose(true);
                        }
                        break;
                    }
            }
            await base.OnAfterRenderAsync(isFirst);
        }

        private int _zIndex = DefaultZIndez;
        private string _zIndexStyle = "";

        /// <summary>
        /// trigger when mask is clicked
        /// </summary>
        /// <returns></returns>
        private async Task MaskClick(MouseEventArgs _)
        {
            if (MaskClosable && Mask)
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
            await HandleClose();
        }

        /// <summary>
        /// clean-up after close
        /// </summary>
        /// <param name="isChangeByParamater"></param>
        /// <returns></returns>
        private async Task HandleClose(bool isChangeByParamater = false)
        {
            _hasInvokeClosed = true;
            if (!isChangeByParamater && OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync(this);
            }
            if (VisibleChanged.HasDelegate)
            {
                await VisibleChanged.InvokeAsync(false);
            }
            await JsInvokeAsync(JSInteropConstants.EnableBodyScroll);
        }

        private void CalcDrawerStyle()
        {
            string style = null;
            if (_status == ComponentStatus.Opened)
            {
                var widthHeightTransition = Placement is "left" or "right"
                    ? $"width 0s {Ease} {Duration}"
                    : $"height 0s {Ease} {Duration}";

                style = $"transition:{_transformTransition} {widthHeightTransition};";
            }

            if (!string.IsNullOrWhiteSpace(OffsetTransform))
            {
                style += $"transform: {OffsetTransform};";
            }

            _drawerStyle = style;
        }

        protected override void Dispose(bool disposing)
        {
            if (_isOpen)
            {
                _ = JsInvokeAsync(JSInteropConstants.EnableBodyScroll);
            }

            base.Dispose(disposing);
        }
    }
}
