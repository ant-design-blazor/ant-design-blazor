using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.Form.Locale
{
    public class FormLocale
    {
        public DefaultValidateMessages DefaultValidateMessages { get; }
    }

    public class DefaultValidateMessages
    {
        public string Default { get; }
        public string Required { get; }
        public string Enum { get; }
        public string Whitespace { get; }
        public DateLocale Date { get; }
        public TypesLocale Types { get; }
    }

    public class DateLocale
    {
        public string Format { get; }
        public string Parse { get; }
        public string Invalid { get; }
    }

    public class TypesLocale
    {
        public string String { get; }
        public string Method { get; }
        public string Array { get; }
        public string Object { get; }
        public string Number { get; }
        public string Date { get; }
        public string Boolean { get; }
        public string Integer { get; }
        public string Float { get; }
        public string Regexp { get; }
        public string Email { get; }
        public string Url { get; }
        public string Hex { get; }
    }
}
