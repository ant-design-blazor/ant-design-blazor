using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class MentionsOption
    {
        [CascadingParameter] public Mentions Mentions { get; set; }

        [Parameter] public string Value { get; set; }

        [Parameter] public bool Disable { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        public bool Selected { get; set; }

        public bool Active { get; set; }

        protected override void OnInitialized()
        {
            Mentions?.AddOption(this);

            SetClassMap();

            base.OnInitialized();
        }

        private void SetClassMap()
        {
            var prefixCls = "ant-mentions-dropdown-menu-item";
            this.ClassMapper.Clear()
                .Add(prefixCls)
                .If($"{prefixCls}-disable", () => this.Disable)
                .If($"{prefixCls}-selected", () => this.Selected)
                .If($"{prefixCls}-active", () => this.Active)
                ;

            InvokeStateHasChanged();
        }

        internal void OnMouseEnter()
        {
            this.Selected = true;
        }

        internal void OnMouseLeave()
        {
            this.Selected = false;
        }

        internal async Task OnClick(MouseEventArgs args)
        {
            if (args.Button == 0)   //left click
            {
                await Mentions.OnOptionClick(this);
            }

            InvokeStateHasChanged();
        }
    }
}
