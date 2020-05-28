using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class Empty : AntDomComponentBase
    {
        [Parameter]
        public string PrefixCls { get; set; } = "ant-empty";

        /// <summary>
        /// "ltr"|"rtl"
        /// </summary>
        [Parameter]
        public string Direction { get; set; } = "ltr";

        [Parameter]
        public string ImageStyle { get; set; }

        [Parameter]
        public bool Small { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public OneOf<string, bool, RenderFragment> Description { get; set; } = "暂无数据";

        [Parameter]
        public OneOf<string, RenderFragment> Image { get; set; } = Empty.PRESENTED_IMAGE_DEFAULT;

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-normal", () => Image.IsT1 && Image.AsT1 == Empty.PRESENTED_IMAGE_SIMPLE)
                .If($"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
                .If($"{PrefixCls}-small", () => Small)
                ;
        }

        protected override void OnInitialized()
        {
            this.SetClass();
        }

        protected override void OnParametersSet()
        {
            this.SetClass();
        }
    }
}
