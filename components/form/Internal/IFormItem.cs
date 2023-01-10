using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Internal
{
    public interface IFormItem
    {
        internal FormValidateStatus ValidateStatus { get; }

        internal bool HasFeedback { get; }

        internal RenderFragment FeedbackIcon { get; }

        internal void AddControl<TValue>(AntInputComponentBase<TValue> control);

        internal ValidationResult[] ValidateField();

        internal FieldIdentifier GetFieldIdentifier();
    }
}
