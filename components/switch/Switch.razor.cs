using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public partial class Switch : AntInputBoolComponentBase
    {
        [Parameter]
        public bool Loading { get; set; }

        [Parameter]
        public string CheckedChildren { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment CheckedChildrenTemplate { get; set; }

        /// <summary>
        /// The status of Switch is completely up to the user and no longer 
        /// automatically changes the data based on the click event.
        /// </summary>
        [Parameter]
        public bool Control { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        [Parameter]
        public string UnCheckedChildren { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment UnCheckedChildrenTemplate { get; set; }

        private bool _clickAnimating = false;

        internal override string PrefixCls => "ant-switch";

        protected override void SetClass()
        {
            base.SetClass();
            ClassMapper
                .If($"{PrefixCls}-checked", () => CurrentValue)
                .If($"{PrefixCls}-disabled", () => Disabled || Loading)
                .If($"{PrefixCls}-loading", () => Loading)
                .If($"{PrefixCls}-small", () => Size == "small")
                ;
        }

        private async Task HandleClick(MouseEventArgs e)
        {
            if (!Control)
                await base.ChangeValue(!CurrentValue);
            if (OnClick.HasDelegate)
                await OnClick.InvokeAsync(null);
        }

        private void HandleMouseOver(MouseEventArgs e) => _clickAnimating = true;

        private void HandleMouseOut(MouseEventArgs e) => _clickAnimating = false;

    }
}
