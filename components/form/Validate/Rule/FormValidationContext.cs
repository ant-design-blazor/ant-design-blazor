using System;

namespace AntDesign
{
    public class FormValidationContext
    {
        public FormValidateErrorMessages ValidateMessages { get; set; }
        public FormValidationRule Rule { get; set; }
        public object Value { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public Type FieldType { get; set; }
    }
}
