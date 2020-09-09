using System;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class ResizeHandle : AntDomComponentBase
    {
        [Parameter]
        public int Index { get; set; }

        [Parameter]
        public bool VerticalOrHorizontal { get; set; } = false;

        [Parameter]
        public bool IsDiagonal { get; set; } = false;

        [Parameter]
        public double Width { get; set; } = 5;

        [Parameter]
        public double Height { get; set; } = 30;

        [Parameter]
        public string BgColor { get; set; } = "silver";

        [Parameter]
        public Action<bool, int, int> OnPositionChange { get; set; }

        [Parameter]
        public Action<int, int, int> OnDiagonalPositionChange { get; set; }

        [Parameter]
        public Action<int, int, int> OnDragStart { get; set; }

        [Parameter]
        public Action<int, int, int> OnDragEnd { get; set; }

        [Parameter]
        public string Direction { get; set; } = "bottomRight";

        [CascadingParameter]
        public Resizable Parent { get; set; }

        private readonly Splitter _splitter = new Splitter();

        private bool _dragMode = false;

        [Parameter]
        public bool EnableRender { get; set; } = true;

        private bool _boxHover = false;

        protected override void OnInitialized()
        {
            _dragMode = false;
            SetClass();
            Parent?.AddHandle(this);
            base.OnInitialized();
        }

        protected override bool ShouldRender()
        {
            return EnableRender;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (EnableRender && builder != null)
            {
                base.BuildRenderTree(builder);

                int i = 0;
                builder.OpenElement(i++, "div");
                builder.AddAttribute(i++, "id", Id);
                builder.AddAttribute(i++, "style", GetStyle());
                builder.AddAttribute(i++, "css", ClassMapper.Class);
                builder.AddAttribute(i++, "onpointerdown", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerDown));
                builder.AddAttribute(i++, "onpointermove", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerMove));
                builder.AddAttribute(i++, "onpointerup", EventCallback.Factory.Create<PointerEventArgs>(this, OnPointerUp));
                builder.AddAttribute(i++, "onmouseenter", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseEnter));
                builder.AddAttribute(i++, "onmouseleave", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseLeave));
                builder.AddAttribute(i++, "onmousedown", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseDown));
                builder.AddAttribute(i++, "ontouchstart", EventCallback.Factory.Create<TouchEventArgs>(this, OnMouseDown));

                builder.AddEventPreventDefaultAttribute(i++, "onmousemove", true);

                builder.AddElementReferenceCapture(i++, (value) => { Ref = value; });
                builder.CloseElement();

                EnableRender = false;
            }
        }

        private void SetClass()
        {
            var prefixCls = "ant-resizable-handle";

            ClassMapper
                .Add($"{prefixCls}")
                .If($"{prefixCls}-top", () => Direction == ResizeHandleDirection.Top)
                .If($"{prefixCls}-right", () => Direction == ResizeHandleDirection.Right)
                .If($"{prefixCls}-bottom", () => Direction == ResizeHandleDirection.Bottom)
                .If($"{prefixCls}-topRight", () => Direction == ResizeHandleDirection.TopRight)
                .If($"{prefixCls}-bottomRight", () => Direction == ResizeHandleDirection.BottomRight)
                .If($"{prefixCls}-bottomLeft", () => Direction == ResizeHandleDirection.BottomLeft)
                .If($"{prefixCls}-topLeft", () => Direction == ResizeHandleDirection.TopLeft)
                .If($"{prefixCls}-box-hover", () => _boxHover)
                ;
        }

        internal string GetStyle()
        {
            StringBuilder sb1 = new StringBuilder();

            sb1.Append("width:" + Width + "px;height:" + Height + "px;");

            if (!VerticalOrHorizontal)
            {
                sb1.Append("display:inline-block;");
            }

            //sb1.Append("background-color:red;");
            sb1.Append("background-color:" + BgColor + ";");

            if (IsDiagonal)
            {
                sb1.Append("cursor:nwse-resize;");
            }
            else
            {
                if (VerticalOrHorizontal)
                {
                    sb1.Append("cursor:s-resize;");
                    //sb1.Append("cursor:col-resize;");
                }
                else
                {
                    sb1.Append("cursor:w-resize;");
                    //sb1.Append("cursor:col-resize;");
                }
            }

            return sb1.ToString();
        }

        private void OnMouseEnter()
        {
            this._boxHover = true;
        }

        private void OnMouseLeave()
        {
            this._boxHover = false;
        }

        private void OnMouseDown()
        {
        }

        private void OnPointerDown(PointerEventArgs e)
        {
            JsInvokeAsync<bool>(JSInteropConstants.SetPointerCapture, Ref, e.PointerId);
            _dragMode = true;

            if (IsDiagonal)
            {
                _splitter.PreviousPositionX = (int)e.ClientX;
                _splitter.PreviousPositionY = (int)e.ClientY;
            }
            else
            {
                if (VerticalOrHorizontal)
                {
                    _splitter.PreviousPositionX = (int)e.ClientY;
                    _splitter.PreviousPositionY = (int)e.ClientX;
                }
                else
                {
                    _splitter.PreviousPositionX = (int)e.ClientX;
                    _splitter.PreviousPositionY = (int)e.ClientY;
                }
            }

            OnDragStart?.Invoke(Index, (int)e.ClientX, (int)e.ClientY);
        }

        private void OnPointerMove(PointerEventArgs e)
        {
            if (_dragMode)
            {
                if (e.Buttons == 1)
                {
                    int positionX = 0;
                    int positionY = 0;

                    if (IsDiagonal)
                    {
                        positionX = (int)e.ClientX;
                        positionY = (int)e.ClientY;

                        if (_splitter.PreviousPositionX != positionX || _splitter.PreviousPositionY != positionY)
                        {
                            OnDiagonalPositionChange?.Invoke(Index, positionX - _splitter.PreviousPositionX, positionY - _splitter.PreviousPositionY);

                            _splitter.PreviousPositionX = positionX;
                            _splitter.PreviousPositionY = positionY;
                        }
                    }
                    else
                    {
                        if (VerticalOrHorizontal)
                        {
                            positionX = (int)e.ClientY;
                            positionY = (int)e.ClientX;
                        }
                        else
                        {
                            positionX = (int)e.ClientX;
                            positionY = (int)e.ClientY;
                        }

                        if (Math.Abs(_splitter.PreviousPositionY - positionY) < 100)
                        {
                            if (_splitter.PreviousPositionX != positionX)
                            {
                                OnPositionChange?.Invoke(VerticalOrHorizontal, Index, positionX - _splitter.PreviousPositionX);

                                _splitter.PreviousPositionX = positionX;
                            }
                        }
                        //else
                        //{
                        //    //BsJsInterop.StopDrag(bSplitter.bsbSettings.ID);
                        //}
                    }
                }
            }
        }

        private void OnPointerUp(PointerEventArgs e)
        {
            JsInvokeAsync<bool>(JSInteropConstants.ReleasePointerCapture, Ref, e.PointerId);
            _dragMode = false;

            OnDragEnd?.Invoke(Index, (int)e.ClientX, (int)e.ClientY);
        }

        protected override void Dispose(bool disposing)
        {
            Parent?.RemoveHandle(this);
            base.Dispose(disposing);
        }

        public void SetColor(string c)
        {
            BgColor = c;
            EnableRender = true;
            StateHasChanged();
        }
    }
}
