using System;
using AntDesign.Form.Locale;
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

        internal EditContext EditContext { get; }

        internal FormValidateMode ValidateMode { get; }

        internal string Size { get; }

        internal bool UseLocaleValidateMessage { get; }

        internal void AddFormItem(IFormItem formItem);

        internal void RemoveFormItem(IFormItem formItem);

        internal void AddControl(IControlValueAccessor valueAccessor);

        internal void RemoveControl(IControlValueAccessor valueAccessor);

        internal bool ValidateOnChange { get; }

        internal FormLocale Locale { get; }

        event Action<IForm> OnFinishEvent;

        bool IsModified { get; }

        string Name { get; }
        object Model { get; }
        FormRequiredMark RequiredMark { get; set; }

        void Reset();

        void Submit();

        bool Validate();
    }
}
