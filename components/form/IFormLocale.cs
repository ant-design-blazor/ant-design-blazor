using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.Form.Locale
{
    public interface IFormLocale
    {
        public IDefaultValidateMessages DefaultValidateMessages { get; }
    }

    public interface IDefaultValidateMessages
    {
        public string Default { get; }
        public string Required { get; }
        public string Enum { get; }
        public string Whitespace { get; }
        public IDateLocale Date { get; }
        public ITypesLocale Types { get; }
    }

    public interface IDateLocale
    {
        public string Format { get; }
        public string Parse { get; }
        public string Invalid { get; }
    }

    public interface ITypesLocale
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
