using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AntBlazor.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntBlazor
{
    using GutterType = OneOf<int, Dictionary<string, int>, (int, int)>;

    public class AntRowBase : AntDomComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter] public string type { get; set; }

        /// <summary>
        /// 'top' | 'middle' | 'bottom'
        /// </summary>
        [Parameter] public string align { get; set; }

        /// <summary>
        /// 'start' | 'end' | 'center' | 'space-around' | 'space-between'
        /// </summary>
        [Parameter] public string justify { get; set; }

        [Parameter] public GutterType gutter { get; set; }

        [Inject] public DomEventService domEventService { get; set; }

        protected string gutterStyle { get; set; }

        public IList<AntCol> Cols { get; set; } = new List<AntCol>();

        public static Hashtable gridResponsiveMap = new Hashtable()
        {
            [nameof(BreakpointEnum.xs)] = "(max-width: 575px)",
            [nameof(BreakpointEnum.sm)] = "(max-width: 576px)",
            [nameof(BreakpointEnum.md)] = "(max-width: 768px)",
            [nameof(BreakpointEnum.lg)] = "(max-width: 992px)",
            [nameof(BreakpointEnum.xl)] = "(max-width: 1200px)",
            [nameof(BreakpointEnum.xxl)] = "(max-width: 1600px)",
        };

        protected override async Task OnInitializedAsync()
        {
            var prefixCls = "ant-row";
            ClassMapper.Add(prefixCls)
                .If($"{prefixCls}-top", () => align == "top")
                .If($"{prefixCls}-middle", () => align == "middle")
                .If($"{prefixCls}-bottom", () => align == "bottom")
                .If($"{prefixCls}-start", () => justify == "start")
                .If($"{prefixCls}-end", () => justify == "end")
                .If($"{prefixCls}-center", () => justify == "center")
                .If($"{prefixCls}-space-around", () => justify == "space-around")
                .If($"{prefixCls}-space-between", () => justify == "space-between")
                ;

            await this.setGutterStyle();
            domEventService.AddEventListener<object>("window", "resize", async _ => await this.setGutterStyle());

            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            //await this.setGutterStyle();
            await base.OnParametersSetAsync();
        }

        private async Task setGutterStyle()
        {
            string breakPoint = null;

            await typeof(BreakpointEnum).GetEnumNames().ForEachAsync(async bp =>
            {
                if (await JsInvokeAsync<bool>(JSInteropConstants.matchMedia, gridResponsiveMap[bp]))
                {
                    breakPoint = bp;
                }
            });
            var gutter = this.getGutter(breakPoint);
            Cols.ForEach(x => x.RowGutterChanged(gutter));

            gutterStyle = "";
            if (gutter.horizontalGutter > 0)
            {
                gutterStyle = $"margin-left: -{gutter.horizontalGutter / 2}px;margin-right: -{gutter.horizontalGutter / 2}px;";
            }
            if (gutter.verticalGutter > 0)
            {
                gutterStyle += $"margin-top: -{gutter.verticalGutter / 2}px;margin-bottom: -{gutter.verticalGutter / 2}px;";
            }

            InvokeStateHasChanged();
        }

        private (int horizontalGutter, int verticalGutter) getGutter(string breakPoint)
        {
            GutterType gutter = 0;
            if (this.gutter.Value != null)
                gutter = this.gutter;

            return gutter.Match(
                num => (num, 0),
                dic => breakPoint != null && dic.ContainsKey(breakPoint) ? (dic[breakPoint], 0) : (0, 0),
                tuple => tuple
            );
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