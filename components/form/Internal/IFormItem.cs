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

        internal bool IsRequiredByValidation { get; }

        internal void AddControl<TValue>(AntInputComponentBase<TValue> control);

        internal ValidationResult[] ValidateFieldWithRules();

        internal FieldIdentifier GetFieldIdentifier();

        internal void SetValidationMessage(string[] errorMessages);
    }
}
