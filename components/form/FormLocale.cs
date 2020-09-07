using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.Form.Locale
{
    public class FormLocale
    {
        public DefaultValidateMessages DefaultValidateMessages { get; set; }
    }

    public class DefaultValidateMessages
    {
        public string Default { get; set; }
        public string Required { get; set; }
        public string Enum { get; set; }
        public string Whitespace { get; set; }
        public DateLocale Date { get; set; }
        public TypesLocale Types { get; set; }
    }

    public class DateLocale
    {
        public string Format { get; set; }
        public string Parse { get; set; }
        public string Invalid { get; set; }
    }

    public class TypesLocale
    {
        public string String { get; set; }
        public string Method { get; set; }
        public string Array { get; set; }
        public string Object { get; set; }
        public string Number { get; set; }
        public string Date { get; set; }
        public string Boolean { get; set; }
        public string Integer { get; set; }
        public string Float { get; set; }
        public string Regexp { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string Hex { get; set; }
    }
}
