// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Docs
{
    /// <summary>
    /// Source sode from Microsoft.AspNetCore.Components.Forms.ObjectGraphDataAnnotationsValidator
    /// </summary>
    public class CustomValidator : ComponentBase
    {
        private static readonly object _validationContextValidatorKey = new object();

        private static readonly object _validatedObjectsKey = new object();

        private ValidationMessageStore _validationMessageStore;

        [CascadingParameter]
        internal EditContext EditContext { get; set; }

        protected override void OnInitialized()
        {
            _validationMessageStore = new ValidationMessageStore(EditContext);

            // Perform object-level validation (starting from the root model) on request
            EditContext.OnValidationRequested += (sender, eventArgs) =>
            {
                _validationMessageStore.Clear();
                ValidateObject(EditContext.Model, new HashSet<object>());
                EditContext.NotifyValidationStateChanged();
            };

            // Perform per-field validation on each field edit
            EditContext.OnFieldChanged += (sender, eventArgs) =>
                ValidateField(EditContext, _validationMessageStore, eventArgs.FieldIdentifier);
        }

        internal void ValidateObject(object value, HashSet<object> visited)
        {
            if (value is null)
            {
                return;
            }

            if (!visited.Add(value))
            {
                // Already visited this object.
                return;
            }

            if (value is IEnumerable<object> enumerable)
            {
                var index = 0;
                foreach (var item in enumerable)
                {
                    ValidateObject(item, visited);
                    index++;
                }

                return;
            }

            var validationResults = new List<ValidationResult>();
            ValidateObject(value, visited, validationResults);

            // Transfer results to the ValidationMessageStore
            foreach (var validationResult in validationResults)
            {
                if (!validationResult.MemberNames.Any())
                {
                    _validationMessageStore.Add(new FieldIdentifier(value, string.Empty), validationResult.ErrorMessage);
                    continue;
                }

                foreach (var memberName in validationResult.MemberNames)
                {
                    var fieldIdentifier = new FieldIdentifier(value, memberName);
                    _validationMessageStore.Add(fieldIdentifier, validationResult.ErrorMessage);
                }
            }
        }

        private void ValidateObject(object value, HashSet<object> visited, List<ValidationResult> validationResults)
        {
            var validationContext = new ValidationContext(value);
            validationContext.Items.Add(_validationContextValidatorKey, this);
            validationContext.Items.Add(_validatedObjectsKey, visited);
            Validator.TryValidateObject(value, validationContext, validationResults, validateAllProperties: true);
            foreach (var validationResult in validationResults)
            {
                validationResult.ErrorMessage += " -- by CustomValidator";
            }
        }

        internal static bool TryValidateRecursive(object value, ValidationContext validationContext)
        {
            if (validationContext.Items.TryGetValue(_validationContextValidatorKey, out var result) && result is CustomValidator validator)
            {
                var visited = (HashSet<object>)validationContext.Items[_validatedObjectsKey];
                validator.ValidateObject(value, visited);

                return true;
            }

            return false;
        }

        private static void ValidateField(EditContext editContext, ValidationMessageStore messages, in FieldIdentifier fieldIdentifier)
        {
            // DataAnnotations only validates public properties, so that's all we'll look for
            var propertyInfo = fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName);
            if (propertyInfo != null)
            {
                var propertyValue = propertyInfo.GetValue(fieldIdentifier.Model);
                var validationContext = new ValidationContext(fieldIdentifier.Model) {MemberName = propertyInfo.Name};
                var results = new List<ValidationResult>();

                Validator.TryValidateProperty(propertyValue, validationContext, results);
                messages.Clear(fieldIdentifier);
                messages.Add(fieldIdentifier, results.Select(result => result.ErrorMessage + " -- by CustomValidator"));

                // We have to notify even if there were no messages before and are still no messages now,
                // because the "state" that changed might be the completion of some async validation task
                editContext.NotifyValidationStateChanged();
            }
        }
    }
}
