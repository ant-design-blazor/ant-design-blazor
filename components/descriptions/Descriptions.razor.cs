﻿using System;
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

        public IList<IDescriptionsItem> Items { get; } = new List<IDescriptionsItem>();

        private List<List<(IDescriptionsItem item, int realSpan)>> _itemMatrix = new List<List<(IDescriptionsItem item, int realSpan)>>();

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

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
            DomEventListener.Dispose();
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
            PrepareMatrix();
            await InvokeAsync(StateHasChanged);
            await base.OnFirstAfterRenderAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
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
                HtmlElement element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Ref);
                var breakpointTuple = _descriptionsResponsiveMap.FirstOrDefault(x => x.PixelWidth > element.ClientWidth);
                var bp = breakpointTuple == default ? BreakpointEnum.xxl : breakpointTuple.Breakpoint;
                _realColumn = Column.AsT1.ContainsKey(bp.ToString()) ? Column.AsT1[bp.ToString()] : _defaultColumnMap[bp.ToString()];
            }
        }
    }
}
