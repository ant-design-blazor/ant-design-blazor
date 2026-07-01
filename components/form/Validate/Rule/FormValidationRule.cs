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
        /// <summary>
        /// Validate the length. Valid <see cref="Type"/>s: String, Number, or Array.
        /// </summary>
        public decimal? Len { get; set; }

        /// <summary>
        /// Validate length is greater than or equal to this number. Valid <see cref="Type"/>s: String, Number or Array.
        /// </summary>
        public decimal? Min { get; set; }

        /// <summary>
        /// Validate length is less than or equal to this number. Valid <see cref="Type"/>s: String, Number, or Array.
        /// </summary>
        public decimal? Max { get; set; }

        /// <summary>
        /// Make a field required
        /// </summary>
        public bool? Required { get; set; }

        /// <summary>
        /// Validate the value passes a regular expression pattern
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// Custom error message. Will be auto generated if not provided.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Validate the value is in the specified range
        /// </summary>
        public (double Min, double Max)? Range { get; set; }

        /// <summary>
        /// Validate rule for all array elements. Valid <see cref="Type"/>s: Array. (<c>FormItem</c> not supported now)
        /// </summary>
        public FormValidationRule DefaultField { get; set; }

        /// <summary>
        /// Whether the value is in specified values
        /// </summary>
        public object[] OneOf { get; set; }

        /// <summary>
        /// Validate the value is in specified enum type.
        /// </summary>
        public Type Enum { get; set; }

        /// <summary>
        /// Validate rule for child elements. Valid <see cref="Type"/>s: Array, Object
        /// </summary>
        public Dictionary<object, FormValidationRule> Fields { get; set; }

        /// <summary>
        /// Custom validation function
        /// </summary>
        public Func<FormValidationContext, ValidationResult> Validator { get; set; }

        /// <summary>
        /// Transformation function called before validation executes. The return value will have the validation ran against it.
        /// </summary>
        public Func<object, object> Transform { get; set; } = (value) => value;

        /// <summary>
        /// Type of form field. See <see cref="FormFieldType"/> for all possible values.
        /// </summary>
        public FormFieldType? Type { get; set; }

        /// <summary>
        /// Validate the value using the specified attribute.
        /// </summary>
        public ValidationAttribute ValidationAttribute { get; set; }
    }
}
