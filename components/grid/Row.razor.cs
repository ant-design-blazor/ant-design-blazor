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
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string Type { get; set; }

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

        [Parameter] public GutterType Gutter { get; set; }

        [Inject] public DomEventService DomEventService { get; set; }

        private string GutterStyle { get; set; }

        public IList<Col> Cols { get; } = new List<Col>();

        private static Hashtable _gridResponsiveMap = new Hashtable()
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
                .If($"{prefixCls}-top", () => Align == "top")
                .If($"{prefixCls}-middle", () => Align == "middle")
                .If($"{prefixCls}-bottom", () => Align == "bottom")
                .If($"{prefixCls}-start", () => Justify == "start")
                .If($"{prefixCls}-end", () => Justify == "end")
                .If($"{prefixCls}-center", () => Justify == "center")
                .If($"{prefixCls}-space-around", () => Justify == "space-around")
                .If($"{prefixCls}-space-between", () => Justify == "space-between")
                ;

            await this.SetGutterStyle();
            DomEventService.AddEventListener<object>("window", "resize", async _ => await this.SetGutterStyle());

            await base.OnInitializedAsync();
        }

        private async Task SetGutterStyle()
        {
            string breakPoint = null;

            await typeof(BreakpointEnum).GetEnumNames().ForEachAsync(async bp =>
            {
                if (await JsInvokeAsync<bool>(JSInteropConstants.matchMedia, _gridResponsiveMap[bp]))
                {
                    breakPoint = bp;
                }
            });

            var gutter = this.GetGutter(breakPoint);
            Cols.ForEach(x => x.RowGutterChanged(gutter));

            GutterStyle = "";
            if (gutter.horizontalGutter > 0)
            {
                GutterStyle =
                    $"margin-left: -{gutter.horizontalGutter / 2}px;margin-right: -{gutter.horizontalGutter / 2}px;";
            }

            if (gutter.verticalGutter > 0)
            {
                GutterStyle +=
                    $"margin-top: -{gutter.verticalGutter / 2}px;margin-bottom: -{gutter.verticalGutter / 2}px;";
            }

            InvokeStateHasChanged();
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
