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
        public bool Simple { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public OneOf<string, bool?> Description { get; set; } = LocaleProvider.CurrentLocale.Empty.Description;

        [Parameter]
        public RenderFragment DescriptionTemplate { get; set; }

        [Parameter]
        public string Image { get; set; }

        [Parameter]
        public RenderFragment ImageTemplate { get; set; }

        protected void SetClass()
        {
            this.ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-normal", () => Simple)
                .GetIf(() => $"{PrefixCls}-{Direction}", () => Direction.IsIn("ltr", "rlt"))
                .If($"{PrefixCls}-small", () => Small)
                ;
        }

        protected override void OnInitialized()
        {
            this.SetClass();
        }
    }
}
