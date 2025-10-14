// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class AutoCompleteSearch : Search, IAutoCompleteInput
    {
        public AutoCompleteSearch()
        {
            AutoComplete = false;
            BindOnInput = true;
        }

        [CascadingParameter]
        internal IAutoCompleteRef Component { get; set; }

        [CascadingParameter(Name = "OverlayTriggerContext")]
        public ForwardRef OverlayTriggerContext
        {
            get => RefBack;
            set
            {
                WrapperRefBack = value;
                RefBack = value;
            }
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

        protected override async Task OnKeyDownAsync(KeyboardEventArgs args)
        {
            await base.OnKeyDownAsync(args);

            if (Component != null) await Component?.InputKeyDown(args);
        }

        protected override async Task OnInputAsync(ChangeEventArgs args)
        {
            await base.OnInputAsync(args);

            await Component?.InputInput(args);
        }

        protected override void OnValueChange(string value)
        {
            base.OnValueChange(value);

            Component?.InputValueChange(value);
        }

        void IAutoCompleteInput.SetValue(object value)
        {
            this.CurrentValue = value?.ToString();
        }
    }
}
