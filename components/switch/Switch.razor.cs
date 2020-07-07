using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class Switch : AntInputComponentBase<bool>
    {
        protected string _prefixCls = "ant-switch";

        protected override Task OnParametersSetAsync()
        {
            ClassMapper.Clear()
                .Add(_prefixCls)
                .If($"{_prefixCls}-checked", () => _isChecked)
                .If($"{_prefixCls}-disabled", () => Disabled || Loading)
                .If($"{_prefixCls}-loading", () => Loading)
                .If($"{_prefixCls}-small", () => Size.Equals("small"))
                ;

            return base.OnParametersSetAsync();
        }

        private bool _isChecked = false;

        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Loading { get; set; }

        [Parameter]
        public bool Control { get; set; }

        [Parameter]
        public EventCallback<bool> OnChange { get; set; }

        [Parameter]
        public OneOf<string, RenderFragment> CheckedChildren { get; set; }

        [Parameter]
        public OneOf<string, RenderFragment> UnCheckedChildren { get; set; }

        private bool _clickAnimating = false;

        protected override void OnInitialized()
        {
            this._isChecked = Value ? Value : Checked;
            base.OnInitialized();
        }

        private void UpdateValue(bool value)
        {
            if (this._isChecked != value)
            {
                this._isChecked = value;
                CurrentValue = value;
                this.OnChange.InvokeAsync(this._isChecked);
            }
        }

        private void HandleClick(MouseEventArgs e)
        {
            if (!Disabled && !Loading && !Control)
            {
                CurrentValue = !this._isChecked;
                this.UpdateValue(!this._isChecked);
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
