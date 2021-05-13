namespace AntDesign
{
    public class RuleValidationContext
    {
        public ValidateMessages ValidateMessages { get; set; }
        public Rule Rule { get; set; }
        public object Value { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }

    }
}
