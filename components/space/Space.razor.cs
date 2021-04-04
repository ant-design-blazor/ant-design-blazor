using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Space : AntDomComponentBase
    {
        [Parameter]
        public string Align { get; set; }

        [Parameter]
        public string Direction { get; set; } = "horizontal";

        [Parameter]
        public string Size { get; set; } = "small";

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        private bool HasAlign => Align.IsIn("start", "end", "center", "baseline");

        private const string PrefixCls = "ant-space";

        public void SetClass()
        {
            ClassMapper
                .Add(PrefixCls)
                .GetIf(() => $"{PrefixCls}-{Direction}", () => Direction.IsIn("horizontal", "vertical"))
                .GetIf(() => $"{PrefixCls}-align-{Align}", () => HasAlign)
                .If($"{PrefixCls}-align-center", () => !HasAlign && Direction == "horizontal")
                .If($"{PrefixCls}-rtl", () => RTL);
        }

        protected override void OnInitialized()
        {
            SetClass();

            base.OnInitialized();
        }
    }
}
