﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Internal
{
    public interface IFormItem
    {
        public FormValidateStatus ValidateStatus { get; }

        public bool HasFeedback { get; }

        public RenderFragment FeedbackIcon { get; }

        internal bool IsRequiredByValidation { get; }

        internal void AddControl<TValue>(AntInputComponentBase<TValue> control);

        internal ValidationResult[] ValidateField();

        internal FieldIdentifier GetFieldIdentifier();
    }
}
