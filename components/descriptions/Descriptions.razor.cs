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

        [Parameter]
        public string Size { get; set; }

        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter]
        public bool Colon { get; set; }

        #endregion

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public IList<IDescriptionsItem> Items { get; } = new List<IDescriptionsItem>();

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
            SetClassMap();
            base.OnInitialized();
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

            for (int i = 0; i < this.Items.Count; i++)
            {
                var item = this.Items[i];
                width += item.Span;

                if (width >= column)
                {
                    if (width > column)
                    {
                        Console.WriteLine(@$"""Column"" is {column} but we have row length ${width}");
                    }
                    item.Span = column - (width - item.Span);
                    currentRow.Add(item);
                    FlushRow();
                }
                else if (i == this.Items.Count - 1)
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
