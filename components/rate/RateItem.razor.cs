using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class RateItem
    {

        [CascadingParameter(Name = "Character")]
        [Parameter] public RenderFragment<RateItemRenderContext> Character { get; set; }

        [Parameter] public RenderFragment DefaultCharacter { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public bool AllowHalf { get; set; } = false;

        [Parameter] public EventCallback<bool> OnItemHover { get; set; }

        [Parameter] public EventCallback<bool> OnItemClick { get; set; }


        [Parameter] public string ToolTip { get; set; }
        [Parameter] public int IndexOfGroup { get; set; }
        [Parameter] public ClassMapper StarClassMapper { get; set; } = new ClassMapper();

        [Parameter] public int HoverValue { get; set; }
        [Parameter] public bool HasHalf { get; set; }
        [Parameter] public bool IsFocused { get; set; }

        private async Task OnHover(bool isHalf)
        {
            await OnItemHover.InvokeAsync(isHalf && this.AllowHalf);
        }
        private async Task OnClick(bool isHalf)
        {
            await OnItemClick.InvokeAsync(isHalf && this.AllowHalf);
        }

        protected override void OnParametersSet()
        {
            UpdateClass(HoverValue, HasHalf, IsFocused);
            base.OnParametersSet();
        }

        public void UpdateClass(int hoverValue, bool hasHalf = true, bool isFocused = true)
        {
            decimal val = IndexOfGroup + 1;
            string prefixName = "ant-rate-star";

            StarClassMapper.Clear();
            StarClassMapper
              .Add(prefixName)
          .If($"{prefixName}-full", () => val < hoverValue || (!hasHalf && val == hoverValue))
          .If($"{prefixName}-half", () => hasHalf && val == hoverValue)
          .If($"{prefixName}-active", () => hasHalf && val == hoverValue)
          .If($"{prefixName}-zero", () => val > hoverValue)
          .If($"{prefixName}-focused", () => hasHalf && val == hoverValue && isFocused)
          ;

        }
    }
}
