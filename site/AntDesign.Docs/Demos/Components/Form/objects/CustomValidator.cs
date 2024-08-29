// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Docs
{
    public class CustomValidator : ComponentBase
    {
        [CascadingParameter]
        internal EditContext EditContext { get; set; }

        protected override void OnInitialized()
        {
            var messages = new ValidationMessageStore(EditContext);
            EditContext.OnFieldChanged += (sender, args) =>
            {
                messages.Clear();
                messages.Add(args.FieldIdentifier, "Message from custom validator");
                EditContext.NotifyValidationStateChanged();
            };
        }
    }
}
