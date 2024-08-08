using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AntDesign
{
    public class FormValidationRule
    {
        public decimal? Len { get; set; }
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
        public bool? Required { get; set; }
        public string Pattern { get; set; }
        public string Message { get; set; }
        public (double Min, double Max)? Range { get; set; }
        public FormValidationRule DefaultField { get; set; }
        public object[] OneOf { get; set; }
        public Type Enum { get; set; }
        public Dictionary<object, FormValidationRule> Fields { get; set; }
        public Func<FormValidationContext, ValidationResult> Validator { get; set; }
        public Func<object, object> Transform { get; set; } = (value) => value;
        public FormFieldType Type { get; set; } = FormFieldType.String;
        public ValidationAttribute ValidationAttribute { get; set; }
    }
}
