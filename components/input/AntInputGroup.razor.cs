using Microsoft.AspNetCore.Components;

namespace AntBlazor
{
    public partial class AntInputGroup : AntDomComponentBase
    {
        protected const string PrefixCls = "ant-input-group";

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public string size { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            ClassMapper.Clear()
                .Add(PrefixCls)
                .If($"{PrefixCls}-lg", () => size == AntInputSize.Large)
                .If($"{PrefixCls}-sm", () => size == AntInputSize.Small)
                .If($"{PrefixCls}-compact", () => Attributes != null && Attributes.ContainsKey("compact"));
        }
    }
}
