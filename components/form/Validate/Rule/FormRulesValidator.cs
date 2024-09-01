// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Internal
{
    public class FormRulesValidator : ComponentBase
    {
        private ValidationMessageStore _validationMessageStore;

        [CascadingParameter]
        internal EditContext EditContext { get; set; }

        protected override void OnInitialized()
        {
            _validationMessageStore = new ValidationMessageStore(EditContext);
        }

        public void DisplayErrors(Dictionary<FieldIdentifier, List<string>> errors)
        {
            foreach (var err in errors)
            {
                _validationMessageStore.Add(err.Key, err.Value);
            }

            EditContext.NotifyValidationStateChanged();
        }

        public void ClearError(FieldIdentifier fieldIdentifier)
        {
            _validationMessageStore.Clear(fieldIdentifier);
            EditContext.NotifyValidationStateChanged();
        }

        public void ClearErrors()
        {
            _validationMessageStore.Clear();
            EditContext.NotifyValidationStateChanged();
        }
    }
}
