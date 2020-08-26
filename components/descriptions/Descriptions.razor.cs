using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    using ColumnType = OneOf<int, Dictionary<string, int>>;

    public partial class Descriptions : AntDomComponentBase
    {
        #region Parameters

        [Parameter]
        public bool Bordered { get; set; } = false;

        [Parameter]
        public string Layout { get; set; } = DescriptionsLayout.Horizontal;

        [Parameter]
        public ColumnType Column { get; set; }

        [Parameter]
        public string Size { get; set; }

        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter]
        public bool Colon { get; set; }

        #endregion Parameters

        private ElementReference _divRef;

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public IList<IDescriptionsItem> Items { get; } = new List<IDescriptionsItem>();

        private List<List<(IDescriptionsItem item, int realSpan)>> _itemMatrix = new List<List<(IDescriptionsItem item, int realSpan)>>();

        [Inject]
        public DomEventService DomEventService { get; set; }

        private int _realColumn;

        private readonly Dictionary<string, int> _defaultColumnMap = new Dictionary<string, int>
        {
            { "xxl", 3 },
            { "xl", 3},
            { "lg", 3},
            { "md", 3},
            { "sm", 2},
            { "xs", 1}
        };

        private static readonly List<(int PixelWidth, BreakpointEnum Breakpoint)> _descriptionsResponsiveMap = new List<(int, BreakpointEnum)>()
        {
            (575,BreakpointEnum.xs),
            (576,BreakpointEnum.sm),
            (768,BreakpointEnum.md),
            (992,BreakpointEnum.lg),
            (1200,BreakpointEnum.xl),
            (1600,BreakpointEnum.xxl)
        };

        private void SetClassMap()
        {
            ClassMapper.Clear().Add("ant-descriptions")
                .If("ant-descriptions-bordered", () => this.Bordered)
                .If("ant-descriptions-middle", () => this.Size == DescriptionsSize.Middle)
                .If("ant-descriptions-small", () => this.Size == DescriptionsSize.Small);
        }

        protected override async Task OnInitializedAsync()
        {
            SetClassMap();


            if (Column.IsT1)
            {
                DomEventService.AddEventListener<object>("window", "resize", OnResize, false);
            }
            await base.OnInitializedAsync();
        }

        private async void OnResize(object o)
        {
            await SetRealColumn();
            PrepareMatrix();
            await InvokeAsync(StateHasChanged);
        }

        protected override async Task OnFirstAfterRenderAsync()
        {
            await SetRealColumn();
            PrepareMatrix();
            await InvokeAsync(StateHasChanged);
            await base.OnFirstAfterRenderAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            SetClassMap();
            PrepareMatrix();
            await InvokeAsync(StateHasChanged);
            await base.OnParametersSetAsync();
        }

        private void PrepareMatrix()
        {
            List<List<(IDescriptionsItem item, int realSpan)>> itemMatrix = new List<List<(IDescriptionsItem item, int realSpan)>>();

            List<(IDescriptionsItem item, int realSpan)> currentRow = new List<(IDescriptionsItem item, int realSpan)>();
            var width = 0;

            for (int i = 0; i < this.Items.Count; i++)
            {
                var item = this.Items[i];
                width += item.Span;

                if (width >= _realColumn)
                {
                    if (width > _realColumn)
                    {
                        Console.WriteLine(@$"""Column"" is {_realColumn} but we have row length ${width}");
                    }
                    currentRow.Add((item, _realColumn - (width - item.Span)));
                    FlushRow();
                }
                else if (i == this.Items.Count - 1)
                {
                    currentRow.Add((item, _realColumn - (width - item.Span)));
                    FlushRow();
                }
                else
                {
                    currentRow.Add((item, item.Span));
                }
            }
            this._itemMatrix = itemMatrix;

            void FlushRow()
            {
                itemMatrix.Add(currentRow);
                currentRow = new List<(IDescriptionsItem item, int realSpan)>();
                width = 0;
            }
        }

        private async Task SetRealColumn()
        {

            if (Column.IsT0)
            {
                _realColumn = Column.AsT0 == 0 ? 3 : Column.AsT0;
            }
            else
            {
                Element element = await JsInvokeAsync<Element>(JSInteropConstants.getDomInfo, _divRef);
                var breakpointTuple = _descriptionsResponsiveMap.FirstOrDefault(x => x.PixelWidth > element.clientWidth);
                var bp = breakpointTuple == default ? BreakpointEnum.xxl : breakpointTuple.Breakpoint;
                _realColumn = Column.AsT1.ContainsKey(bp.ToString()) ? Column.AsT1[bp.ToString()] : _defaultColumnMap[bp.ToString()];
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            DomEventService.RemoveEventListerner<object>("window", "resize", OnResize);
        }
    }
}
