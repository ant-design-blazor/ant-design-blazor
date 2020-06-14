using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Descriptions : AntDomComponentBase
    {
        #region Parameters

        [Parameter]
        public bool Bordered { get; set; } = false;

        [Parameter]
        public string Layout { get; set; } = DescriptionsLayout.Horizontal;

        [Parameter]
        public int Column { get; set; } = 3;//TODO:缺少响应式布局支持
        //  @Input() @WithConfig(NZ_CONFIG_COMPONENT_NAME) nzColumn: number | { [key in NzBreakpointEnum]: number} = defaultColumnMap;

        [Parameter]
        public string Size { get; set; }

        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter]
        public bool Colon { get; set; }

        #endregion

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<IDescriptionsItem> _items = new List<IDescriptionsItem>();

        private List<List<IDescriptionsItem>> _itemMatrix = new List<List<IDescriptionsItem>>();

        private void SetClassMap()
        {
            ClassMapper.Clear().Add("ant-descriptions")
                .If("ant-descriptions-bordered", () => this.Bordered)
                .If("ant-descriptions-middle", () => this.Size == DescriptionsSize.Middle)
                .If("ant-descriptions-small", () => this.Size == DescriptionsSize.Small);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetClassMap();
        }

        public void AddDescriptionsItem(IDescriptionsItem item)
        {
            _items.Add(item);
        }

        protected override Task OnFirstAfterRenderAsync()
        {
            PrepareMatrix();
            StateHasChanged();
            return base.OnFirstAfterRenderAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            SetClassMap();
            StateHasChanged();
            return base.OnParametersSetAsync();
        }

        private void PrepareMatrix()
        {
            List<List<IDescriptionsItem>> itemMatrix = new List<List<IDescriptionsItem>>();

            List<IDescriptionsItem> currentRow = new List<IDescriptionsItem>();
            var width = 0;
            var column = this.Column;

            for (int i = 0; i < this._items.Count; i++)
            {
                var item = this._items[i];
                width += item.Span;

                if (width >= column)
                {
                    if (width > column)
                    {
                        Console.WriteLine(@$"""nzColumn"" is {column} but we have row length ${width}");
                    }
                    item.Span = column - (width - item.Span);
                    currentRow.Add(item);
                    FlushRow();
                }
                else if (i == this._items.Count - 1)
                {
                    item.Span = column - (width - item.Span);
                    currentRow.Add(item);
                    FlushRow();
                }
                else
                {
                    currentRow.Add(item);
                }
            }

            this._itemMatrix = itemMatrix;

            Console.WriteLine(this._itemMatrix.Count);

            void FlushRow()
            {
                itemMatrix.Add(currentRow);
                currentRow = new List<IDescriptionsItem>();
                width = 0;
            }

        }
    }
}
