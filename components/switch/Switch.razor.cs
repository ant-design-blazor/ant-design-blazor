using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class Switch : AntInputComponentBase<bool>
    {
        protected string _prefixCls = "ant-switch";

        [Parameter] public bool Checked { get; set; }

        [Parameter] public bool Disabled { get; set; }

        [Parameter] public bool Loading { get; set; }

        [Parameter] public bool Control { get; set; }

        [Parameter] public EventCallback<bool> OnChange { get; set; }

        [Parameter] public OneOf<string, RenderFragment> CheckedChildren { get; set; }

        [Parameter] public OneOf<string, RenderFragment> UnCheckedChildren { get; set; }

        private bool _clickAnimating = false;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            this.CurrentValue = this.CurrentValue ? this.CurrentValue : this.Checked;

            ClassMapper.Clear()
                .Add(_prefixCls)
                .If($"{_prefixCls}-checked", () => CurrentValue)
                .If($"{_prefixCls}-disabled", () => Disabled || Loading)
                .If($"{_prefixCls}-loading", () => Loading)
                .If($"{_prefixCls}-small", () => Size == "small")
                ;
        }

        private void HandleClick(MouseEventArgs e)
        {
            if (!Disabled && !Loading && !Control)
            {
                this.CurrentValue = !CurrentValue;

                this.OnChange.InvokeAsync(CurrentValue);
            }
        }

        private void HandleMouseOver(MouseEventArgs e)
        {
            _clickAnimating = true;
        }

        private void HandleMouseOut(MouseEventArgs e)
        {
            _clickAnimating = false;
        }
    }
}
