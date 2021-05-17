namespace AntDesign
{
    public class RuleValidationContext
    {
        public ValidateMessages ValidateMessages { get; set; }
        public FormValidationRule Rule { get; set; }
        public object Value { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }

    }
}
