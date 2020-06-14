using System;
using System.Collections.Generic;
using System.Text;
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
        public AntDirectionVHType Layout { get; set; } = AntDirectionVHType.Horizontal;

        [Parameter]
        public int Column { get; set; } = 3;//TODO:缺少响应式布局支持
        //  @Input() @WithConfig(NZ_CONFIG_COMPONENT_NAME) nzColumn: number | { [key in NzBreakpointEnum]: number} = defaultColumnMap;

        [Parameter]
        public string Size { get; set; }

        [Parameter]
        public OneOf<string, RenderFragment> Title { get; set; }

        [Parameter]
        public bool Colon { get; set; } = false;

        #endregion

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal List<IDescriptionsItem> _items = new List<IDescriptionsItem>();

        private void SetClassMap()
        {
            ClassMapper.Clear().Add("ant-descriptions")
                .If("descriptions-bordered", () => this.Bordered)
                .If("descriptions-middle", () => this.Size == DescriptionsSize.Middle)
                .If("descriptions-small", () => this.Size == DescriptionsSize.Small);
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
    }
}
