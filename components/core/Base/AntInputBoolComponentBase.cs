using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class AntInputBoolComponentBase : AntInputComponentBase<bool>
    {
        [Parameter] public bool AutoFocus { get; set; }

        private bool _checked;
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

        [Parameter]
        public EventCallback<bool> OnChange { get; set; }

        /// <summary>
        /// Gets or sets a callback that updates the bound checked value.
        /// </summary>
        [Parameter]
        public virtual EventCallback<bool> CheckedChanged { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        internal virtual string PrefixCls { get; }

        protected override void OnValueChange(bool value)
        {
            base.OnValueChange(value);
            Checked = value;
            CheckedChanged.InvokeAsync(value);
        }

        protected virtual async Task ChangeValue(bool value)
        {
            CurrentValue = value;
            if (this.OnChange.HasDelegate)
                await this.OnChange.InvokeAsync(value);
        }
    }
}
