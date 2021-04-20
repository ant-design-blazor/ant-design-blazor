using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OneOf;

namespace AntDesign
{
    public partial class Switch : AntInputComponentBase<bool>
    {
        protected string _prefixCls = "ant-switch";

        [Parameter]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                if (_checked != Value)
                {
                    Value = _checked;
                }
            }
        }

        /// <summary>
        /// Gets or sets a callback that updates the bound checked value.
        /// </summary>
        [Parameter]
        public virtual EventCallback<bool> CheckedChanged { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool Loading { get; set; }

        [Parameter]
        public bool Control { get; set; }

        [Parameter]
        public EventCallback<bool> OnChange { get; set; }

        [Parameter]
        public string CheckedChildren { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment CheckedChildrenTemplate { get; set; }

        [Parameter]
        public string UnCheckedChildren { get; set; } = string.Empty;

        [Parameter]
        public RenderFragment UnCheckedChildrenTemplate { get; set; }

        private bool _clickAnimating = false;
        private bool _checked;

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
                .If($"{_prefixCls}-rtl", () => RTL)
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

        protected override void OnValueChange(bool value)
        {
            base.OnValueChange(value);
            Checked = value;
            CheckedChanged.InvokeAsync(value);
        }

    }
}
