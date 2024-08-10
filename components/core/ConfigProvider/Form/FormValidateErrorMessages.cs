namespace AntDesign
{
    public class FormValidateErrorMessages
    {
        private static string _typeTemplate = "'{0}' is not a valid {1}";

        public string Default { get; set; } = "Validation error on field '{0}'";
        public string Required { get; set; } = "'{0}' is required";
        public string Enum { get; set; } = "'{0}' must be one of [{1}]";
        public string Whitespace { get; set; } = "'{0}' cannot be empty";

        public DateMessage Date { get; set; } = new()
        {
            Format = "'{0}' date format is invalid",
            Parse = "'{0}' cannot be converted to a date",
            Invalid = "'{0}' is an invalid date",
        };

        public TypesMessage Types { get; set; } = new()
        {
            String = _typeTemplate,
            Array = _typeTemplate,
            Object = _typeTemplate,
            Enum = _typeTemplate,
            Number = _typeTemplate,
            Date = _typeTemplate,
            Boolean = _typeTemplate,
            Integer = _typeTemplate,
            Float = _typeTemplate,
            Regexp = _typeTemplate,
            Email = _typeTemplate,
            Url = _typeTemplate,
        };

        public CompareMessage String { get; set; } = new()
        {
            Len = "'{0}' must be exactly {1} characters",
            Min = "'{0}' must be at least {1} characters",
            Max = "'{0}' cannot be longer than {1} characters",
            Range = "'{0}' must be between {1} and {2} characters",
        };

        public CompareMessage Number { get; set; } = new()
        {
            Len = "'{0}' must equal {1}",
            Min = "'{0}' cannot be less than {1}",
            Max = "'{0}' cannot be greater than {1}",
            Range = "'{0}' must be between {1} and {2}",
        };

        public CompareMessage Array { get; set; } = new()
        {
            Len = "'{0}' must be exactly {1} in length",
            Min = "'{0}' cannot be less than {1} in length",
            Max = "'{0}' cannot be greater than {1} in length",
            Range = "'{0}' must be between {1} and {2} in length",
        };

        public PatternMessage Pattern { get; set; } = new()
        {
            Mismatch = "'{0}' does not match pattern {1}",
        };

        internal string GetTypeMessage(FormFieldType type)
        {
            return type switch
            {
                FormFieldType.String => Types.String,
                FormFieldType.Number => Types.Number,
                FormFieldType.Boolean => Types.Boolean,
                FormFieldType.Regexp => Types.Regexp,
                FormFieldType.Integer => Types.Integer,
                FormFieldType.Float => Types.Float,
                FormFieldType.Array => Types.Array,
                FormFieldType.Object => Types.Object,
                FormFieldType.Enum => Types.Enum,
                FormFieldType.Date => Types.Date,
                FormFieldType.Url => Types.Url,
                FormFieldType.Email => Types.Email,
                _ => "",
            };
        }

        #region classes
        public class TypesMessage
        {
            public string String { get; set; }
            public string Array { get; set; }
            public string Object { get; set; }
            public string Enum { get; set; }
            public string Number { get; set; }
            public string Date { get; set; }
            public string Boolean { get; set; }
            public string Integer { get; set; }
            public string Float { get; set; }
            public string Regexp { get; set; }
            public string Email { get; set; }
            public string Url { get; set; }
        }

        public class CompareMessage
        {
            public string Len { get; set; }
            public string Min { get; set; }
            public string Max { get; set; }
            public string Range { get; set; }
        }

        public class PatternMessage
        {
            public string Mismatch { get; set; }
        }

        public class DateMessage
        {
            public string Format { get; set; }
            public string Parse { get; set; }
            public string Invalid { get; set; }
        };

        #endregion classes
    }
}
