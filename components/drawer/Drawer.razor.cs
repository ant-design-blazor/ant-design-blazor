// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using AntDesign.Core.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
        <para>A panel which slides in from the edge of the screen.</para>

        <h2>When To Use</h2>
        <para>
            A Drawer is a panel that is typically overlaid on top of a page and slides in from the side.
            It contains a set of information or actions.
            Since the user can interact with the Drawer without leaving the current page, tasks can be achieved more efficiently within the same context.
        </para>
        <list type="bullet">
            <item>Use a Form to create or edit a set of information.</item>
            <item>Processing subtasks. When subtasks are too heavy for a Popover and we still want to keep the subtasks in the context of the main task, Drawer comes very handy.</item>
            <item>When the same Form is needed in multiple places.</item>
        </list>
    </summary>
    <seealso cref="DrawerService" />
    <seealso cref="DrawerOptions" />
    <seealso cref="DrawerRef" />
     */
    [Documentation(DocumentationCategory.Components, DocumentationType.Feedback, "https://gw.alipayobjects.com/zos/alicdn/7z8NJQhFb/Drawer.svg", Title = "Drawer", SubTitle = "抽屉")]
    public partial class Drawer : AntDomComponentBase
    {
        #region Parameters

        [CascadingParameter(Name = "FromContainer")]
        private DrawerRef DrawerRef { get; set; }

        /// <summary>
        /// The content of Drawer.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// The content of Drawer. <para>If <see cref="Content"/> is a string, it will be rendered as HTML.</para>
        /// </summary>
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
        /// Whether a close (x) button is visible on top right of the Drawer dialog or not.
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Closable { get; set; } = true;

        /// <summary>
        /// Clicking on the mask (area outside the Drawer) to close the Drawer or not.
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool MaskClosable { get; set; } = true;

        /// <summary>
        /// Whether to show mask or not.
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Mask { get; set; } = true;

        /// <summary>
        /// Style for Drawer's mask element.
        /// </summary>
        [Parameter]
        public string MaskStyle { get; set; }

        /// <summary>
        /// Whether to support keyboard esc off
        /// </summary>
        /// <default value="true" />
        [Parameter]
        public bool Keyboard { get; set; } = true;

        private RenderFragment TitleTemplate { get; set; }

        private string TitleString { get; set; }

        private OneOf<RenderFragment, string> _title;

        /// <summary>
        /// The title for Drawer.
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
        /// The placement of the Drawer, option could be left, top, right, bottom
        /// </summary>
        /// <default value="right" />
        [Parameter]
        public DrawerPlacement Placement { get; set; } = DrawerPlacement.Right;

        /// <summary>
        /// Body style for modal body element. Such as height, padding etc.
        /// </summary>
        [Parameter]
        public string BodyStyle { get; set; }

        /// <summary>
        /// Header style for modal header element. Such as height, padding etc.
        /// </summary>
        [Parameter]
        public string HeaderStyle { get; set; }

        /// <summary>
        /// The class name of the container of the Drawer dialog. 
        /// </summary>
        [Parameter]
        public string WrapClassName { get; set; }

        /// <summary>
        /// Width of the Drawer dialog, only when placement is 'left' or 'right'.
        /// </summary>
        /// <default value="256" />
        [Parameter]
        public string Width { get; set; } = "256";

        /// <summary>
        /// Height of the Drawer dialog, only when placement is 'top' or 'bottom'.
        /// </summary>
        /// <default value="256" />
        [Parameter]
        public string Height { get; set; } = "256";

        /// <summary>
        /// The z-index of the Drawer.
        /// </summary>
        /// <default value="1000" />
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
        /// The the X coordinate offset(px), only when placement is 'left' or 'right'.
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int OffsetX { get; set; } = 0;

        /// <summary>
        /// The the Y coordinate offset(px), only when placement is 'top' or 'bottom'.
        /// </summary>
        /// <default value="0" />
        [Parameter]
        public int OffsetY { get; set; } = 0;

        /// <summary>
        /// Whether the Drawer dialog is visible or not.
        /// </summary>
        /// <default value="false" />
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
        /// Specify a callback that will be called before drawer displayed
        /// </summary>
        [Parameter]
        public EventCallback OnOpen { get; set; }

        /// <summary>
        /// Specify a callback that will be called when a user clicks mask, close button or Cancel button.
        /// </summary>
        [Parameter]
        public EventCallback OnClose { get; set; }

        [Parameter]
        public RenderFragment Handler { get; set; }

        [Inject] private ClientDimensionService ClientDimensionService { get; set; }

        private OneOf<RenderFragment, string> _content;

        private string ContentString { get; set; }

        private RenderFragment ContentTemplate { get; set; }

        #endregion Parameters

        private ComponentStatus _status;
        private bool _hasInvokeClosed;
        private bool _isOpen = default;

        private DrawerPlacement _originalPlacement;

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
                    DrawerPlacement.Left => $"translateX({OffsetX}px);",
                    DrawerPlacement.Right => $"translateX(-{OffsetX}px);",
                    DrawerPlacement.Top => $"translateY({OffsetY}px);",
                    DrawerPlacement.Bottom => $"translateY(-{OffsetY}px);",
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
                    DrawerPlacement.Left => "translateX(-100%)",
                    DrawerPlacement.Right => "translateX(100%)",
                    DrawerPlacement.Top => "translateY(-100%)",
                    DrawerPlacement.Bottom => "translateY(100%)",
                    _ => null
                };
            }
        }

        private bool IsLeftOrRight => Placement == DrawerPlacement.Left || Placement == DrawerPlacement.Right;

        private string WidthPx => IsLeftOrRight ? StyleHelper.ToCssPixel(Width) : null;

        private string HeightPx => !IsLeftOrRight ? StyleHelper.ToCssPixel(Height) : null;

        private ClassMapper TitleClassMapper { get; set; } = new ClassMapper();

        private string WrapperStyle => $@"
            {(WidthPx != null ? $"width:{WidthPx};" : "")}
            {(HeightPx != null ? $"height:{HeightPx};" : "")}
            {(Transform != null ? $"transform:{Transform};" : "")}
            {(PlacementChanging ? "transition:none;" : "")}";

        // private static Regex _renderInCurrentContainerRegex = new Regex("position:[\\s]*absolute");

        private string _drawerStyle = "";

        // private bool _isPlacementFirstChange = true;

        private void SetClass()
        {
            var prefixCls = "ant-drawer";
            ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-open", () => _isOpen)
                .Add($"{prefixCls}-{Placement.ToString().ToLowerInvariant()}")
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
            if (isFirst)
            {
                await ClientDimensionService.GetScrollBarSizeAsync();
            }
            switch (_status)
            {
                case ComponentStatus.Opening:
                    {
                        _status = ComponentStatus.Opened;

                        if (OnOpen.HasDelegate)
                        {
                            await OnOpen.InvokeAsync(this);
                        }

                        if (Visible == false && VisibleChanged.HasDelegate)
                        {
                            await VisibleChanged.InvokeAsync(true);
                        }

                        _hasInvokeClosed = true;// avoid closing again

                        _drawerStyle = !string.IsNullOrWhiteSpace(OffsetTransform)
                            ? $"transform: {OffsetTransform};"
                            : string.Empty;
                        StateHasChanged();
                        break;
                    }
                case ComponentStatus.Closing:
                    {
                        if (!_hasInvokeClosed)
                        {
                            await HandleClose();
                        }

                        _status = ComponentStatus.Closed;
                        StateHasChanged();
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
        private void MaskClick(MouseEventArgs _)
        {
            if (MaskClosable && Mask)
            {
                CloseClick();
            }
        }

        /// <summary>
        /// trigger when Closer is clicked
        /// </summary>
        /// <returns></returns>
        private void CloseClick()
        {
            _hasInvokeClosed = false;
            _status = ComponentStatus.Closing;
        }

        /// <summary>
        /// clean-up after close
        /// </summary>
        /// <returns></returns>
        private async Task HandleClose()
        {
            _hasInvokeClosed = true;
            if (OnClose.HasDelegate)
            {
                await OnClose.InvokeAsync(this);
            }
            if (VisibleChanged.HasDelegate)
            {
                await VisibleChanged.InvokeAsync(false);
            }
            _isOpen = false;

            if (DrawerRef != null)
            {
                await DrawerRef.CloseAsync();
            }
        }

        private void CalcDrawerStyle()
        {
            string style = null;
            if (_status == ComponentStatus.Opened)
            {
                var widthHeightTransition = IsLeftOrRight
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
            base.Dispose(disposing);
        }
    }
}
