using System;
using AntDesign.Forms;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    public interface IForm
    {
        internal ColLayoutParam WrapperCol { get; }

        internal ColLayoutParam LabelCol { get; }

        internal AntLabelAlignType? LabelAlign { get; }

        internal FormValidateMode ValidateMode { get; }
        internal FormValidateErrorMessages ValidateMessages { get; }

        internal string Size { get; }

        internal void AddFormItem(IFormItem formItem);

        internal void RemoveFormItem(IFormItem formItem);

        internal void AddControl(IControlValueAccessor valueAccessor);

        internal void RemoveControl(IControlValueAccessor valueAccessor);

        internal bool ValidateOnChange { get; }

        internal event Action<IForm> OnFinishEvent;

        /// <summary>
        /// Get the current EditContext from the Form.
        /// </summary>
        EditContext EditContext { get; }

        bool IsModified { get; }

        string Name { get; }
        object Model { get; }
        FormRequiredMark RequiredMark { get; set; }

        void Reset();

        void Submit();

        bool Validate();

        void AddValidationMessage(string field, string[] errorMessages);
    }
}
