namespace AntDesign.Form.Locale
{
    public class FormLocale
    {
        public FormValidateErrorMessages DefaultValidateMessages { get; set; } = new();

        public string Optional { get; set; } = "(optional)";
    }

    public class DefaultValidateMessages
    {
        public string Default { get; set; } = "Field validation error ${label}";
        public string Required { get; set; } = "Please enter ${label}";
        public string Enum { get; set; } = "${label} must be one of [${enum}]";
        public string Whitespace { get; set; } = "${label} cannot be a blank character";
        public DateLocale Date { get; set; } = new();
        public TypesLocale Types { get; set; } = new();
    }

    public class DateLocale
    {
        public string Format { get; set; } = "${label} date format is invalid";
        public string Parse { get; set; } = "${label} cannot be converted to a date";
        public string Invalid { get; set; } = "${label} is an invalid date";
    }

    public class TypesLocale
    {
        public string String { get; set; } = "${label} is not a valid ${type}";
        public string Method { get; set; } = "${label} is not a valid ${type}";
        public string Array { get; set; } = "${label} is not a valid ${type}";
        public string Object { get; set; } = "${label} is not a valid ${type}";
        public string Number { get; set; } = "${label} is not a valid ${type}";
        public string Date { get; set; } = "${label} is not a valid ${type}";
        public string Boolean { get; set; } = "${label} is not a valid ${type}";
        public string Integer { get; set; } = "${label} is not a valid ${type}";
        public string Float { get; set; } = "${label} is not a valid ${type}";
        public string Regexp { get; set; } = "${label} is not a valid ${type}";
        public string Email { get; set; } = "${label} is not a valid ${type}";
        public string Url { get; set; } = "${label} is not a valid ${type}";
        public string Hex { get; set; } = "${label} is not a valid ${type}";
    }
}
