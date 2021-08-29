using System;
using System.Collections.Generic;
using System.Text;
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
        }

        [CascadingParameter]
        public IAutoCompleteRef Component { get; set; }

        [CascadingParameter(Name = "OverlayTriggerContext")]
        public ForwardRef OverlayTriggerContext
        {
            get => RefBack;
            set { RefBack = value; }
        }

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

        protected override async Task OnkeyDownAsync(KeyboardEventArgs args)
        {
            await base.OnkeyDownAsync(args);

            if (Component != null) await Component?.InputKeyDown(args);
        }


        protected override async void OnInputAsync(ChangeEventArgs args)
        {
            base.OnInputAsync(args);

            if (Component != null) await Component?.InputInput(args);
        }


        #region IAutoCompleteInput

        public void SetValue(object value)
        {
            this.CurrentValue = (TValue)value;
        }

        #endregion
    }
}
