// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public abstract class AntInputBoolComponentBase : AntInputComponentBase<bool>
    {
        /// <summary>
        /// Whether to autofocus on the input or not
        /// </summary>
        /// <default value="false" />
        [Parameter]
        public bool AutoFocus { get; set; }

        private bool _checked;

        /// <summary>
        /// If the input is checked or not
        /// </summary>
        /// <default value="false"/>
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
        /// Callback executed when the input changes
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnChange { get; set; }

        /// <summary>
        /// Gets or sets a callback that updates the bound checked value.
        /// </summary>
        [Parameter]
        public virtual EventCallback<bool> CheckedChanged { get; set; }

        /// <summary>
        /// Disable the input
        /// </summary>
        /// <default value="false"/>
        [Parameter]
        public bool Disabled { get; set; }

        internal virtual string PrefixCls { get; }

        protected override void OnValueChange(bool value)
        {
            base.OnValueChange(value);

            if (value == Checked)
                return;

            Checked = value;

            if (CheckedChanged.HasDelegate)
            {
                CheckedChanged.InvokeAsync(value);
            }
        }

        protected virtual async Task ChangeValue(bool value)
        {
            if (value == CurrentValue)
                return;

            CurrentValue = value;
            if (this.OnChange.HasDelegate)
                await this.OnChange.InvokeAsync(value);
        }
    }
}
