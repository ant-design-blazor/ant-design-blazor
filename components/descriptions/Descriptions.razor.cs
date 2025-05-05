// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntDesign.JsInterop;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /**
    <summary>
    <para>Display multiple read-only fields in groups.</para>

    <h2>When To Use</h2>

    <para>Commonly displayed on the details page.</para>
    </summary>
    <seealso cref="DescriptionsItem"/>
    */
    [Documentation(DocumentationCategory.Components, DocumentationType.DataDisplay, "https://gw.alipayobjects.com/zos/alicdn/MjtG9_FOI/Descriptions.svg", Columns = 1, Title = "Descriptions", SubTitle = "描述列表")]
    public partial class Descriptions : AntDomComponentBase
    {
        #region Parameters

        /// <summary>
        /// Whether to display the border
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Bordered { get; set; } = false;

        /// <summary>
        /// Layout - Horizontal or Vertical
        /// </summary>
        /// <default value="DescriptionsLayout.Horizontal"/>
        [Parameter]
        public DescriptionsLayout Layout { get; set; } = DescriptionsLayout.Horizontal;

        /// <summary>
        /// The number of <see cref="DescriptionsItem"/> elements in a row. Could be a number or a object like { xs: 8, sm: 16, md: 24}
        /// </summary>
        [Parameter]
        public OneOf<int, Dictionary<string, int>> Column { get; set; }

        /// <summary>
        /// Size of the list
        /// </summary>
        [Parameter]
        public DescriptionsSize Size { get; set; }

        /// <summary>
        /// Title shown at the top of the element
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Title content shown at the top of the element. Takes priority over <see cref="Title"/>
        /// </summary>
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        /// <summary>
        /// Change default props <c>Colon</c> value of <see cref="DescriptionsItem"/>.
        /// </summary>
        [Parameter]
        public bool Colon { get; set; }

        #endregion Parameters

        /// <summary>
        /// Content for the element. Typically contains <see cref="DescriptionsItem"/> elements
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal IList<IDescriptionsItem> Items { get; } = [];

        private List<List<(IDescriptionsItem item, int realSpan)>> _itemMatrix = [];

        [Inject]
        private IDomEventListener DomEventListener { get; set; }

        private int _realColumn;

        private static readonly Dictionary<string, int> _defaultColumnMap = new()
        {
            { "Xxl", 3 },
            { "Xl", 3 },
            { "Lg", 3 },
            { "Md", 3 },
            { "Sm", 2 },
            { "Xs", 1 }
        };

        private static readonly List<(int PixelWidth, BreakpointType Breakpoint)> _descriptionsResponsiveMap =
        [
            (575,BreakpointType.Xs),
            (576,BreakpointType.Sm),
            (768,BreakpointType.Md),
            (992,BreakpointType.Lg),
            (1200,BreakpointType.Xl),
            (1600,BreakpointType.Xxl)
        ];

        private void SetClassMap()
        {
            ClassMapper
                .Add("ant-descriptions")
                .If("ant-descriptions", () => RTL)
                .If("ant-descriptions-bordered", () => Bordered)
                .If("ant-descriptions-middle", () => Size == DescriptionsSize.Middle)
                .If("ant-descriptions-small", () => Size == DescriptionsSize.Small);
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

        private async Task OnResize(object o)
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
            List<List<(IDescriptionsItem item, int realSpan)>> itemMatrix = [];

            List<(IDescriptionsItem item, int realSpan)> currentRow = [];
            var width = 0;

            var items = Items;
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                width += item.Span;

                if (width >= _realColumn)
                {
                    currentRow.Add((item, _realColumn - (width - item.Span)));
                    FlushRow();
                }
                else if (i == items.Count - 1)
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
                currentRow = [];
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
                var element = await JsInvokeAsync<HtmlElement>(JSInteropConstants.GetDomInfo, Ref);
                var breakpointTuple = _descriptionsResponsiveMap.FirstOrDefault(x => x.PixelWidth > element.ClientWidth);
                var bp = breakpointTuple == default ? BreakpointType.Xxl : breakpointTuple.Breakpoint;
                var bpKey = bp.ToString();
                _realColumn = Column.AsT1.TryGetValue(bpKey, out var col) ? col : _defaultColumnMap[bpKey];
            }
        }
    }
}
