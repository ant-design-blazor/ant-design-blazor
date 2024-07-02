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
        public string Title { get; set; }

        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        [Parameter]
        public bool Colon { get; set; }

        #endregion Parameters

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private IList<IDescriptionsItem> Items { get; } = new List<IDescriptionsItem>();

        private List<List<(IDescriptionsItem item, int realSpan)>> _itemMatrix = new List<List<(IDescriptionsItem item, int realSpan)>>();

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        private int _realColumn;

        private static Dictionary<string, int> _defaultColumnMap = new Dictionary<string, int>
        {
            { "Xxl", 3 },
            { "Xl", 3},
            { "Lg", 3},
            { "Md", 3},
            { "Sm", 2},
            { "Xs", 1}
        };

        private static readonly List<(int PixelWidth, BreakpointType Breakpoint)> _descriptionsResponsiveMap = new List<(int, BreakpointType)>()
        {
            (575,BreakpointType.Xs),
            (576,BreakpointType.Sm),
            (768,BreakpointType.Md),
            (992,BreakpointType.Lg),
            (1200,BreakpointType.Xl),
            (1600,BreakpointType.Xxl)
        };

        private void SetClassMap()
        {
            ClassMapper
                .Add("ant-descriptions")
                .If("ant-descriptions", () => RTL)
                .If("ant-descriptions-bordered", () => this.Bordered)
                .If("ant-descriptions-middle", () => this.Size == DescriptionsSize.Middle)
                .If("ant-descriptions-small", () => this.Size == DescriptionsSize.Small);
        }

        protected override async Task OnInitializedAsync()
        {
            SetClassMap();
            await base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender && Column.IsT1)
            {
                DomEventListener.AddShared<object>("window", "resize", OnResize);
            }

            base.OnAfterRender(firstRender);
        }

        protected override void Dispose(bool disposing)
        {
            DomEventListener?.Dispose();
            base.Dispose(disposing);
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
            await base.OnFirstAfterRenderAsync();
        }

        protected void ReRender()
        {
            PrepareMatrix();
            this.StateHasChanged();
        }

        public void AddItem(IDescriptionsItem item)
        {
            this.Items.Add(item);
            ReRender();
        }

        public void RemoveItem(IDescriptionsItem item)
        {
            this.Items.Remove(item);
            ReRender();
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
                HtmlElement element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Ref);
                var breakpointTuple = _descriptionsResponsiveMap.FirstOrDefault(x => x.PixelWidth > element.ClientWidth);
                var bp = breakpointTuple == default ? BreakpointType.Xxl : breakpointTuple.Breakpoint;
                _realColumn = Column.AsT1.ContainsKey(bp.ToString()) ? Column.AsT1[bp.ToString()] : _defaultColumnMap[bp.ToString()];
            }
        }
    }
}
