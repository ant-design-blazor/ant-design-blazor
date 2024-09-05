﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Internal
{
    public interface IFormItem
    {
        public FormValidateStatus ValidateStatus { get; }

        public bool HasFeedback { get; }

        public RenderFragment FeedbackIcon { get; }

        public string Name { get; }

        internal IForm Form { get; }

        internal bool IsRequired { get; }

        internal void AddControl<TValue>(AntInputComponentBase<TValue> control);

        internal ValidationResult[] ValidateFieldWithRules();

        internal FieldIdentifier GetFieldIdentifier();

        internal void SetValidationMessage(string[] errorMessages);
    }
}
