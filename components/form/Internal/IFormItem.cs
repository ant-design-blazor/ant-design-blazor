using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Internal
{
    public interface IFormItem
    {
        internal void AddControl<TValue>(AntInputComponentBase<TValue> control);
        internal ValidationResult[] ValidateField();
        internal FieldIdentifier GetFieldIdentifier();
    }
}
