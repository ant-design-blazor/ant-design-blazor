// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public FormFieldType? Type { get; set; }
        public ValidationAttribute ValidationAttribute { get; set; }
    }
}
