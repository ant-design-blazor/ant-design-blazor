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

        internal FormValidateMode ValidateMode { get; }

        internal string Size { get; }

        internal bool UseLocaleValidateMessage { get; }

        internal void AddFormItem(IFormItem formItem);

        internal void RemoveFormItem(IFormItem formItem);

        internal void AddControl(IControlValueAccessor valueAccessor);

        internal void RemoveControl(IControlValueAccessor valueAccessor);

        internal bool ValidateOnChange { get; }

        internal FormLocale Locale { get; }

        internal event Action<IForm> OnFinishEvent;


        internal FormRequiredMark RequiredMark { get; set; }

        /// <summary>
        /// The data object that the form is bound to.
        /// </summary>
        public object Model { get; }

        /// <summary>
        /// The name of the form.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Get the current EditContext from the Form.
        /// </summary>
        EditContext EditContext { get; }

        /// <summary>
        /// Whether the form has been modified.
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// Reset the values and validation messages of all fields.
        /// </summary>
        void Reset();

        /// <summary>
        /// Trigger `OnFinish` while all fields are valid, otherwise, trigger `OnFinishFailed`.
        /// </summary>
        void Submit();

        /// <summary>
        /// Validate all fields.
        /// </summary>
        /// <returns> true if all fields are valid, otherwise false. </returns>
        bool Validate();

        /// <summary>
        /// Set validation messages for a specific field.
        /// <code>
        /// <![CDATA[
        /// <Form @ref="form">
        ///     <FormItem>
        ///         <Input @bind-value="model.Name" />
        ///     </FormItem>
        /// </Form>
        ///
        /// @code {
        ///     private IForm _form;
        ///     private void SetError()
        ///     {
        ///         _form.SetValidationMessages("name", new[] { "error message" });
        ///     }
        /// }
        /// ]]>
        /// </code>
        /// </summary>
        /// <param name="field"></param>
        /// <param name="errorMessages"></param>
        void SetValidationMessages(string field, string[] errorMessages);
    }
}
