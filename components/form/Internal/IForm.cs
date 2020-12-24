using System;
using AntDesign.Forms;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Internal
{
    public interface IForm
    {
        internal ColLayoutParam WrapperCol { get; }

        internal ColLayoutParam LabelCol { get; }

        internal EditContext EditContext { get; }

        internal string Size { get; }

        internal void AddFormItem(IFormItem formItem);

        internal void AddControl(IControlValueAccessor valueAccessor);

        internal bool ValidateOnChange { get; }

        event Action<IForm> OnFinishEvent;

        bool IsModified { get; }

        string Name { get; }
        object Model { get; }

        void Reset();

        void Submit();

        bool Validate();
    }
}
