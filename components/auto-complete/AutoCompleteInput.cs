// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class AutoCompleteInput<TValue> : Input<TValue>, IAutoCompleteInput
    {
        public AutoCompleteInput()
        {
            AutoComplete = false;
            Type = AntDesign.InputType.Search;
            BindOnInput = true;
        }

        [CascadingParameter]
        private IAutoCompleteRef Component { get; set; }

        [CascadingParameter(Name = "OverlayTriggerContext")]
        public ForwardRef OverlayTriggerContext
        {
            get => RefBack;
            set { RefBack = value; }
        }

        IAutoCompleteRef IAutoCompleteInput.Component { get => Component; set => Component = value; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Component != null) Component?.SetInputComponent(this);
        }

        internal override async Task OnFocusAsync(FocusEventArgs e)
        {
            if (Component != null) await Component?.InputFocus(e);

            await base.OnFocusAsync(e);
        }

        internal async override Task OnBlurAsync(FocusEventArgs e)
        {
            if (Component != null) await Component?.InputBlur(e);

            await base.OnBlurAsync(e);
        }

        protected override async Task OnKeyDownAsync(KeyboardEventArgs args)
        {
            await base.OnKeyDownAsync(args);

            if (Component != null) await Component?.InputKeyDown(args);
        }

        protected override async Task OnInputAsync(ChangeEventArgs args)
        {
            await base.OnInputAsync(args);

            if (Component != null) await Component?.InputInput(args);
        }

        protected override void OnValueChange(TValue value)
        {
            base.OnValueChange(value);

            Component?.InputValueChange(value?.ToString());
        }

        #region IAutoCompleteInput

        void IAutoCompleteInput.SetValue(object value)
        {
            this.CurrentValue = (TValue)value;
        }

        #endregion IAutoCompleteInput
    }
}
