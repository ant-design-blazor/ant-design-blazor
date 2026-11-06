// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using AntDesign.Internal.Form.Validate;
using Xunit;

namespace AntDesign.Tests.Form.Validation
{
    public class FormValidateHelperTest
    {
        private static readonly FormValidateErrorMessages _defValidateMsgs = new();
        private static readonly string _customSuffix = "_custom";
        private static readonly string _fieldName = "fieldName";
        private static readonly string _displayName = "displayName";

        [Theory]
        [InlineData(FormFieldType.String, "hello", true, true)]
        [InlineData(FormFieldType.String, "hello", false, true)]
        [InlineData(FormFieldType.String, null, true, false)]
        [InlineData(FormFieldType.String, null, false, true)]
        [InlineData(FormFieldType.String, "", true, false)]
        [InlineData(FormFieldType.String, "", false, true)]
        [InlineData(FormFieldType.Number, 123, true, true)]
        [InlineData(FormFieldType.Integer, 123, false, true)]
        public void RuleValidate_Required(FormFieldType type, object value, bool required, bool expectedValid)
        {
            var rule = new FormValidationRule()
            {
                Type = type,
                Required = required,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        [Theory]
        [MemberData(nameof(RuleValidate_Len_Values))]
        public void RuleValidate_Len(FormFieldType type, object value, decimal len, bool expectedValid)
        {
            var rule = new FormValidationRule()
            {
                Type = type,
                Len = len,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        public static List<object[]> RuleValidate_Len_Values => new()
        {
            new object[] { FormFieldType.String, "hello", 5m, true },
            new object[] { FormFieldType.String, "hello", 1m, false },
            new object[] { FormFieldType.Number, 123m, 123m, true },
            new object[] { FormFieldType.Number, 123m, 2m, false },
            new object[] { FormFieldType.Array, new int[] { 1, 2, 3 }, 3m, true },
            new object[] { FormFieldType.Array, new int[] { 1, 2, 3 }, 0m, false },
        };


        [Theory]
        [MemberData(nameof(RuleValidate_Min_Values))]
        public void RuleValidate_Min(FormFieldType type, object value, decimal min, bool expectedValid)
        {
            var rule = new FormValidationRule()
            {
                Type = type,
                Min = min,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        public static List<object[]> RuleValidate_Min_Values => new()
        {
            new object[] { FormFieldType.String, "hello", 5m, true },
            new object[] { FormFieldType.String, "hello", 6m, false },
            new object[] { FormFieldType.Number, 123m, 123m, true },
            new object[] { FormFieldType.Number, 123m, 124m, false },
            new object[] { FormFieldType.Number, (int)100, 100m, true }, // int value
            new object[] { FormFieldType.Number, (short)100, 100m, true }, // short value
            new object[] { FormFieldType.Number, (long)100, 100m, true }, // long value
            new object[] { FormFieldType.Number, (decimal)100, 100m, true }, // decimal value
            new object[] { FormFieldType.Number, (byte)1, 1m, true }, // byte value
            new object[] { FormFieldType.Array, new int[] { 1, 2, 3 }, 3m, true },
            new object[] { FormFieldType.Array, new int[] { 1, 2, 3 }, 10m, false },
        };


        [Theory]
        [MemberData(nameof(RuleValidate_Max_Values))]
        public void RuleValidate_Max(FormFieldType type, object value, decimal max, bool expectedValid)
        {
            var rule = new FormValidationRule()
            {
                Type = type,
                Max = max,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        public static List<object[]> RuleValidate_Max_Values => new()
        {
            new object[] { FormFieldType.String, "hello", 5m, true },
            new object[] { FormFieldType.String, "hello", 3m, false },
            new object[] { FormFieldType.Number, 123m, 123m, true },
            new object[] { FormFieldType.Number, 123m, 122m, false },
            new object[] { FormFieldType.Number, (int)100, 100m, true }, // int value
            new object[] { FormFieldType.Number, (short)100, 100m, true }, // short value
            new object[] { FormFieldType.Number, (long)100, 100m, true }, // long value
            new object[] { FormFieldType.Number, (decimal)100, 100m, true }, // decimal value
            new object[] { FormFieldType.Number, (byte)1, 1m, true }, // byte value
            new object[] { FormFieldType.Array, new int[] { 1, 2, 3 }, 3m, true },
            new object[] { FormFieldType.Array, new int[] { 1, 2, 3 }, 2m, false },
        };

        [Fact]
        public void RuleValidate_Transform()
        {
            var rule = new FormValidationRule()
            {
                Transform = value => 100,
                Max = 10,
                Type = FormFieldType.Number,
            };

            int value = 5;
            var isValid = GetValidationResult(rule, value) == null;

            Assert.False(isValid);
        }


        [Theory]
        [MemberData(nameof(RuleValidate_Pattern_Values))]
        public void RuleValidate_Pattern(object value, string pattern, bool expectedValid)
        {
            var rule = new FormValidationRule()
            {
                Pattern = pattern,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        public static List<object[]> RuleValidate_Pattern_Values => new()
        {
            new object[] { "hello", "hello", true },
            new object[] { "Number is 123, answer is 100", "Number is \\d+,", false },
        };

        [Theory]
        [MemberData(nameof(RuleValidate_Message_Values))]
        public void RuleValidate_Message(object value, string message, string expectedMessage)
        {
            var rule = new FormValidationRule()
            {
                Required = true,
                Message = message,
            };

            var resultMessage = GetValidationResult(rule, value)?.ErrorMessage;

            Assert.Equal(expectedMessage, resultMessage);
        }

        public static List<object[]> RuleValidate_Message_Values => new()
        {
            new object[] { "", "should be required", "should be required" },
            new object[] { "", "should not be null", "should not be null" },
            new object[] { "", "{0} should not be null", "displayName should not be null" },
            new object[] { "", null!, string.Format(new FormValidateErrorMessages().Required, _displayName) },
        };

        [Fact]
        public void RuleValidate_Validator()
        {
            const int MAX = 10;
            int value = 15;

            var rule = new FormValidationRule()
            {
                Type = FormFieldType.Integer,
                Validator = (validationContext) =>
                {
                    var validationAttribute = new CustomValidationAttribute(MAX);

                    if (validationAttribute.IsValid(validationContext.Value) == false)
                    {
                        string errorMessage = validationAttribute.FormatErrorMessage(validationContext.DisplayName);

                        var result = new ValidationResult(errorMessage, new string[] { validationContext.FieldName });

                        return result;
                    }

                    return null;
                },
            };

            var result = GetValidationResult(rule, value);

            Assert.NotNull(result);
            Assert.Equal($"The field displayName should not max than {MAX}.", result.ErrorMessage);
        }

        [Theory]
        [MemberData(nameof(RuleValidate_Type_Values))]
        public void RuleValidate_Type(FormFieldType type, object value, bool expectedValid)
        {
            var rule = new FormValidationRule()
            {
                Type = type,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        public static List<object[]> RuleValidate_Type_Values => new()
        {
            new object[] { FormFieldType.String, "string", true },
            new object[] { FormFieldType.String, 100, false },
            new object[] { FormFieldType.String, new string[] { }, false },

            new object[] { FormFieldType.Number, 100, true },
            new object[] { FormFieldType.Number, 100m, true },
            new object[] { FormFieldType.Number, 100.1, true },
            new object[] { FormFieldType.Number, "100", false },
            new object[] { FormFieldType.Number, new int[] { }, false },

            new object[] { FormFieldType.Array, new int[] { }, true },
            new object[] { FormFieldType.Array, new string[] { }, true },
            new object[] { FormFieldType.Array, "string", false },
            new object[] { FormFieldType.Array, 100, false },

            new object[] { FormFieldType.Boolean, true, true },
            new object[] { FormFieldType.Boolean, false, true },
            new object[] { FormFieldType.Boolean, 12312, false },
            new object[] { FormFieldType.Boolean, "true", false },

            new object[] { FormFieldType.Date, DateTime.Parse("2021-01-02"), true },
            new object[] { FormFieldType.Date, "2021-01-01", false },
            new object[] { FormFieldType.Date, 100, false },

            new object[] { FormFieldType.Email, "email@blazor.cn", true },
            new object[] { FormFieldType.Email, "email@blazor.cn.com", true },
            new object[] { FormFieldType.Email, "email#blazor.cn", false },

            new object[] { FormFieldType.Integer, 100, true },
            new object[] { FormFieldType.Integer, 100.9, false },
            new object[] { FormFieldType.Integer, "100", false },

            new object[] { FormFieldType.Float, 100.9, true },
            new object[] { FormFieldType.Float, 100, false },
            new object[] { FormFieldType.Float, "100", false },

            new object[] { FormFieldType.Regexp, "\\d", true },
            new object[] { FormFieldType.Regexp, "^????---))%%%$$3#@^^", false },

            new object[] { FormFieldType.Url, "http://blazor.cn", true },
            new object[] { FormFieldType.Url, "http://www.blazor.cn", true },
            new object[] { FormFieldType.Url, "www.blazor.cn", false },
            new object[] { FormFieldType.Url, "http:@www-blazor-cn", false },

        };

        [Theory]
        [MemberData(nameof(RuleValidate_DefaultField_Values))]
        public void RuleValidate_DefaultField(FormFieldType type, object value, FormValidationRule defaultField, bool expectedValid)
        {
            var rule = new FormValidationRule()
            {
                Type = type,
                DefaultField = defaultField,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        public static List<object[]> RuleValidate_DefaultField_Values => new()
        {
            new object[] { FormFieldType.Array, new string[] { "one", "there", "nine" }, new FormValidationRule { Type = FormFieldType.String, Max = 5 }, true },
            new object[] { FormFieldType.Array, new string[] { "one", "there", "nine" }, new FormValidationRule { Type = FormFieldType.String, Max = 3 }, false },
        };

        [Theory]
        [MemberData(nameof(RuleValidate_Fields_Values))]
        public void RuleValidate_Fields(FormFieldType type, object value, Dictionary<object, FormValidationRule> fields, bool expectedValid)
        {
            var rule = new FormValidationRule()
            {
                Type = type,
                Fields = fields,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        public static List<object[]> RuleValidate_Fields_Values => new()
        {
            new object[] {
                FormFieldType.Array,
                new string[] { "one", "there", "nine" },
                new Dictionary<object, FormValidationRule>{
                    { 0, new FormValidationRule { Type = FormFieldType.String, Max = 5 } },
                    { 1, new FormValidationRule { Type = FormFieldType.String, Len=5 } },
                    { 2, new FormValidationRule { Type = FormFieldType.String, Max = 4 } },
                },
                true
            },
            new object[] {
                FormFieldType.Array,
                new string[] { "one", "there", "nine" },
                new Dictionary<object, FormValidationRule>{
                    { 0, new FormValidationRule { Type = FormFieldType.String, Max = 2 } },
                    { 1, new FormValidationRule { Type = FormFieldType.String, Len=5 } },
                    { 2, new FormValidationRule { Type = FormFieldType.String, Max = 4 } },
                },
                false
            },
            new object[] {
                FormFieldType.Object,
                new FieldsTestObj(),
                new Dictionary<object, FormValidationRule>{
                    { "_fieldName", new FormValidationRule { Type = FormFieldType.String, Max = 3 } },
                    { "_fieldAge", new FormValidationRule { Type = FormFieldType.Integer, Len=10 } },
                    { "PropertyName", new FormValidationRule { Type = FormFieldType.String, Max = 3 } },
                    { "PropertyAge", new FormValidationRule { Type = FormFieldType.Integer, Len = 10 } },
                },
                true
            },
        };

        [Theory]
        [MemberData(nameof(RuleValidate_OneOf_Values))]
        public void RuleValidate_OneOf(FormFieldType type, object value, object[] oneOf, bool expectedValid)
        {
            var rule = new FormValidationRule()
            {
                Type = type,
                OneOf = oneOf,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        public static List<object[]> RuleValidate_OneOf_Values => new()
        {
            new object[] { FormFieldType.String, "one", new object[] { "one", "there", "nine" }, true },
            new object[] { FormFieldType.String, "there", new object[] { "one", "there", "nine" }, true },
            new object[] { FormFieldType.String, "nine", new object[] { "one", "there", "nine" }, true },
            new object[] { FormFieldType.String, "hello", new object[] { "one", "there", "nine" }, false },

            new object[] { FormFieldType.Integer, 1, new object[] { 1, 2, 3 }, true },
            new object[] { FormFieldType.Integer, 2, new object[] { 1, 2, 3 }, true },
            new object[] { FormFieldType.Integer, 3, new object[] { 1, 2, 3 }, true },
            new object[] { FormFieldType.Integer, 4, new object[] { 1, 2, 3 }, false },
        };

        [Theory]
        [MemberData(nameof(RuleValidate_ValidateMessages_Values))]
        public void RuleValidate_ValidateMessages(FormValidationRule rule, object value, string expectedMessage)
        {
            var validationContext = new FormValidationContext()
            {
                ValidateMessages = GetCustomValidateMessages(),
                Rule = rule,
                Value = value,
                FieldName = _fieldName,
                DisplayName = _displayName,
                Model = new FieldsTestObj(),
            };

            var resultMessage = FormValidateHelper.GetValidationResult(validationContext)?.ErrorMessage;

            Assert.Equal(expectedMessage, resultMessage);
        }

        public static List<object[]> RuleValidate_ValidateMessages_Values => new()
        {
            new object[] { new FormValidationRule { Required = true }, null!, string.Format($"{_defValidateMsgs.Required}{_customSuffix}", _displayName) },
            new object[] {
                new FormValidationRule { Type= FormFieldType.Integer, OneOf = new object[] { 1, 2 } },
                0,
                string.Format($"{_defValidateMsgs.Enum}{_customSuffix}", _displayName, JsonSerializer.Serialize(new object[] { 1, 2 }).Trim('[', ']'))
            },
            new object[] { new FormValidationRule { Type = FormFieldType.String }, 123, string.Format($"{_defValidateMsgs.Types.String }{_customSuffix}", _displayName, FormFieldType.String) },
            new object[] { new FormValidationRule { Type = FormFieldType.Array }, 123, string.Format($"{_defValidateMsgs.Types.Array  }{_customSuffix}", _displayName, FormFieldType.Array) },
            new object[] { new FormValidationRule { Type = FormFieldType.Number }, "str", string.Format($"{_defValidateMsgs.Types.Number }{_customSuffix}", _displayName, FormFieldType.Number) },
            new object[] { new FormValidationRule { Type = FormFieldType.Date }, 123, string.Format($"{_defValidateMsgs.Types.Date   }{_customSuffix}", _displayName, FormFieldType.Date) },
            new object[] { new FormValidationRule { Type = FormFieldType.Boolean }, "str", string.Format($"{_defValidateMsgs.Types.Boolean}{_customSuffix}", _displayName, FormFieldType.Boolean) },
            new object[] { new FormValidationRule { Type = FormFieldType.Integer }, 1.0, string.Format($"{_defValidateMsgs.Types.Integer}{_customSuffix}", _displayName, FormFieldType.Integer) },
            new object[] { new FormValidationRule { Type = FormFieldType.Float }, 123, string.Format($"{_defValidateMsgs.Types.Float  }{_customSuffix}", _displayName, FormFieldType.Float) },
            new object[] { new FormValidationRule { Type = FormFieldType.Regexp }, 123, string.Format($"{_defValidateMsgs.Types.Regexp }{_customSuffix}", _displayName, FormFieldType.Regexp) },
            new object[] { new FormValidationRule { Type = FormFieldType.Email }, "123", string.Format($"{_defValidateMsgs.Types.Email  }{_customSuffix}", _displayName, FormFieldType.Email) },
            new object[] { new FormValidationRule { Type = FormFieldType.Url }, "123", string.Format($"{_defValidateMsgs.Types.Url    }{_customSuffix}", _displayName, FormFieldType.Url) },
        };

        private FormValidateErrorMessages GetCustomValidateMessages()
        {
            var suffix = _customSuffix;

            var customValidateMessage = new FormValidateErrorMessages();
            customValidateMessage.Default = $"{customValidateMessage.Default}{suffix}";
            customValidateMessage.Required = $"{customValidateMessage.Required}{suffix}";
            customValidateMessage.Enum = $"{customValidateMessage.Enum}{suffix}";

            customValidateMessage.Types.String = $"{customValidateMessage.Types.String}{suffix}";
            customValidateMessage.Types.Array = $"{customValidateMessage.Types.Array}{suffix}";
            customValidateMessage.Types.Object = $"{customValidateMessage.Types.Object}{suffix}";
            customValidateMessage.Types.Number = $"{customValidateMessage.Types.Number}{suffix}";
            customValidateMessage.Types.Date = $"{customValidateMessage.Types.Date}{suffix}";
            customValidateMessage.Types.Boolean = $"{customValidateMessage.Types.Boolean}{suffix}";
            customValidateMessage.Types.Integer = $"{customValidateMessage.Types.Integer}{suffix}";
            customValidateMessage.Types.Float = $"{customValidateMessage.Types.Float}{suffix}";
            customValidateMessage.Types.Regexp = $"{customValidateMessage.Types.Regexp}{suffix}";
            customValidateMessage.Types.Email = $"{customValidateMessage.Types.Email}{suffix}";
            customValidateMessage.Types.Url = $"{customValidateMessage.Types.Url}{suffix}";

            customValidateMessage.String.Len = $"{customValidateMessage.String.Len}{suffix}";
            customValidateMessage.String.Min = $"{customValidateMessage.String.Min}{suffix}";
            customValidateMessage.String.Max = $"{customValidateMessage.String.Max}{suffix}";
            customValidateMessage.String.Range = $"{customValidateMessage.String.Range}{suffix}";

            customValidateMessage.Number.Len = $"{customValidateMessage.Number.Len}{suffix}";
            customValidateMessage.Number.Min = $"{customValidateMessage.Number.Min}{suffix}";
            customValidateMessage.Number.Max = $"{customValidateMessage.Number.Max}{suffix}";
            customValidateMessage.Number.Range = $"{customValidateMessage.Number.Range}{suffix}";

            customValidateMessage.Array.Len = $"{customValidateMessage.Array.Len}{suffix}";
            customValidateMessage.Array.Min = $"{customValidateMessage.Array.Min}{suffix}";
            customValidateMessage.Array.Max = $"{customValidateMessage.Array.Max}{suffix}";
            customValidateMessage.Array.Range = $"{customValidateMessage.Array.Range}{suffix}";

            customValidateMessage.Pattern.Mismatch = $"{customValidateMessage.Pattern.Mismatch}{suffix}";

            return customValidateMessage;
        }

        private ValidationResult GetValidationResult(FormValidationRule rule, object value)
        {
            var validationContext = new FormValidationContext()
            {
                ValidateMessages = new FormValidateErrorMessages(),
                Rule = rule,
                Value = value,
                FieldName = _fieldName,
                DisplayName = _displayName,
                FieldType = rule.Type switch
                {
                    FormFieldType.String => typeof(string),
                    FormFieldType.Number => typeof(int),
                    FormFieldType.Array => typeof(string[]),
                    _ => typeof(object)
                },
                Model = new FieldsTestObj(),
            };

            return FormValidateHelper.GetValidationResult(validationContext);
        }

    }
}

internal sealed class CustomValidationAttribute : ValidationAttribute
{
    public int Max { get; set; }
    public CustomValidationAttribute(int max) : base("The field {0} should not max than {1}.")
    {
        Max = max;
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Max);
    }

    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return false;
        }

        if (!(value is int valueAsInt))
        {
            return false;
        }

        return valueAsInt < Max;
    }
}

internal sealed class FieldsTestObj
{
    public string _fieldName = "one";
    public int _fieldAge = 10;
    public string PropertyName { get; set; } = "one";
    public int PropertyAge { get; set; } = 10;
}

internal enum FormValidateHelperEnum
{
    None,
}
