using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    using GutterType = OneOf<int, Dictionary<string, int>, (int, int)>;

    public partial class Row : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string Type { get; set; }

        /// <summary>
        /// 'top' | 'middle' | 'bottom'
        /// </summary>
        [Parameter]
        public string Align { get; set; }

        /// <summary>
        /// 'start' | 'end' | 'center' | 'space-around' | 'space-between'
        /// </summary>
        [Parameter]
        public string Justify { get; set; }

        [Parameter]
        public GutterType Gutter { get; set; }

        [Parameter]
        public EventCallback<BreakpointType> OnBreakpoint { get; set; }

        [Inject]
        public DomEventService DomEventService { get; set; }

        private string GutterStyle { get; set; }

        public IList<Col> Cols { get; } = new List<Col>();

        private static Hashtable _gridResponsiveMap = new Hashtable()
        {
            [nameof(BreakpointType.Xs)] = "(max-width: 575px)",
            [nameof(BreakpointType.Sm)] = "(max-width: 576px)",
            [nameof(BreakpointType.Md)] = "(max-width: 768px)",
            [nameof(BreakpointType.Lg)] = "(max-width: 992px)",
            [nameof(BreakpointType.Xl)] = "(max-width: 1200px)",
            [nameof(BreakpointType.Xxl)] = "(max-width: 1600px)",
        };

        private static BreakpointType[] _breakpoints = new[] {
            BreakpointType.Xs,
            BreakpointType.Sm,
            BreakpointType.Md,
            BreakpointType.Lg,
            BreakpointType.Xl,
            BreakpointType.Xxl
        };

        protected override async Task OnInitializedAsync()
        {
            var prefixCls = "ant-row";
            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-top", () => Align == "top")
                .If($"{prefixCls}-middle", () => Align == "middle")
                .If($"{prefixCls}-bottom", () => Align == "bottom")
                .If($"{prefixCls}-start", () => Justify == "start")
                .If($"{prefixCls}-end", () => Justify == "end")
                .If($"{prefixCls}-center", () => Justify == "center")
                .If($"{prefixCls}-space-around", () => Justify == "space-around")
                .If($"{prefixCls}-space-between", () => Justify == "space-between")
                ;

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var dimensions = await JsInvokeAsync<Window>(JSInteropConstants.GetWindow);
                DomEventService.AddEventListener<Window>("window", "resize", OnResize, false);
                OptimizeSize(dimensions.innerWidth);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async void OnResize(Window window)
        {
            OptimizeSize(window.innerWidth);
        }

        private void OptimizeSize(decimal windowWidth)
        {
            BreakpointType actualBreakpoint = _breakpoints[_breakpoints.Length - 1];
            for (int i = 0; i < _breakpoints.Length; i++)
            {
                if (windowWidth <= _breakpoints[i].Width && (windowWidth >= (i > 0 ? _breakpoints[i - 1].Width : 0)))
                {
                    actualBreakpoint = _breakpoints[i];
                }
            }

            SetGutterStyle(actualBreakpoint.Name);

            if (OnBreakpoint.HasDelegate)
            {
                OnBreakpoint.InvokeAsync(actualBreakpoint);
            }

            StateHasChanged();
        }

        private void SetGutterStyle(string breakPoint)
        {
            var gutter = this.GetGutter(breakPoint);
            Cols.ForEach(x => x.RowGutterChanged(gutter));

            GutterStyle = "";
            if (gutter.horizontalGutter > 0)
            {
                GutterStyle = $"margin-left: -{gutter.horizontalGutter / 2}px;margin-right: -{gutter.horizontalGutter / 2}px;";
            }
            if (gutter.verticalGutter > 0)
            {
                GutterStyle += $"margin-top: -{gutter.verticalGutter / 2}px;margin-bottom: -{gutter.verticalGutter / 2}px;";
            }

            StateHasChanged();
        }

        private (int horizontalGutter, int verticalGutter) GetGutter(string breakPoint)
        {
            GutterType gutter = 0;
            if (this.Gutter.Value != null)
                gutter = this.Gutter;

            return gutter.Match(
                num => (num, 0),
                dic => breakPoint != null && dic.ContainsKey(breakPoint) ? (dic[breakPoint], 0) : (0, 0),
                tuple => tuple
            );
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            DomEventService.RemoveEventListerner<Window>("window", "resize", OnResize);
        }
    }

    public enum BreakpointEnum
    {
        xxl,
        xl,
        lg,
        md,
        sm,
        xs
    }
}
