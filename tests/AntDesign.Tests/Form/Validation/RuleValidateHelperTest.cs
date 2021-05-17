using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;
using AntDesign;
using AntDesign.Internal;
using Xunit;

namespace AntDesign.Tests.Form.Validation
{
    public class RuleValidateHelperTest
    {
        private static readonly ValidateMessages _defValidateMsgs = new();
        private static readonly string _customSuffix = "_custom";
        private static readonly string _fieldName = "fieldName";
        private static readonly string _displayName = "displayName";

        [Theory]
        [InlineData(RuleFieldType.String, "hello", true, true)]
        [InlineData(RuleFieldType.String, "hello", false, true)]
        [InlineData(RuleFieldType.String, null, true, false)]
        [InlineData(RuleFieldType.String, null, false, true)]
        [InlineData(RuleFieldType.String, "", true, false)]
        [InlineData(RuleFieldType.String, "", false, true)]
        [InlineData(RuleFieldType.Number, 123, true, true)]
        [InlineData(RuleFieldType.Integer, 123, false, true)]
        public void RuleValidate_Required(RuleFieldType type, object value, bool required, bool expectedValid)
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
        public void RuleValidate_Len(RuleFieldType type, object value, decimal len, bool expectedValid)
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
            new object[] { RuleFieldType.String, "hello", 5m, true },
            new object[] { RuleFieldType.String, "hello", 1m, false },
            new object[] { RuleFieldType.Number, 123m, 123m, true },
            new object[] { RuleFieldType.Number, 123m, 2m, false },
            new object[] { RuleFieldType.Array, new int[] { 1, 2, 3 }, 3m, true },
            new object[] { RuleFieldType.Array, new int[] { 1, 2, 3 }, 0m, false },
        };


        [Theory]
        [MemberData(nameof(RuleValidate_Min_Values))]
        public void RuleValidate_Min(RuleFieldType type, object value, decimal min, bool expectedValid)
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
            new object[] { RuleFieldType.String, "hello", 5m, true },
            new object[] { RuleFieldType.String, "hello", 6m, false },
            new object[] { RuleFieldType.Number, 123m, 123m, true },
            new object[] { RuleFieldType.Number, 123m, 124m, false },
            new object[] { RuleFieldType.Number, (int)100, 100m, true }, // int value
            new object[] { RuleFieldType.Number, (short)100, 100m, true }, // short value
            new object[] { RuleFieldType.Number, (long)100, 100m, true }, // long value
            new object[] { RuleFieldType.Number, (decimal)100, 100m, true }, // decimal value
            new object[] { RuleFieldType.Number, (byte)1, 1m, true }, // byte value
            new object[] { RuleFieldType.Array, new int[] { 1, 2, 3 }, 3m, true },
            new object[] { RuleFieldType.Array, new int[] { 1, 2, 3 }, 10m, false },
        };


        [Theory]
        [MemberData(nameof(RuleValidate_Max_Values))]
        public void RuleValidate_Max(RuleFieldType type, object value, decimal max, bool expectedValid)
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
            new object[] { RuleFieldType.String, "hello", 5m, true },
            new object[] { RuleFieldType.String, "hello", 3m, false },
            new object[] { RuleFieldType.Number, 123m, 123m, true },
            new object[] { RuleFieldType.Number, 123m, 122m, false },
            new object[] { RuleFieldType.Number, (int)100, 100m, true }, // int value
            new object[] { RuleFieldType.Number, (short)100, 100m, true }, // short value
            new object[] { RuleFieldType.Number, (long)100, 100m, true }, // long value
            new object[] { RuleFieldType.Number, (decimal)100, 100m, true }, // decimal value
            new object[] { RuleFieldType.Number, (byte)1, 1m, true }, // byte value
            new object[] { RuleFieldType.Array, new int[] { 1, 2, 3 }, 3m, true },
            new object[] { RuleFieldType.Array, new int[] { 1, 2, 3 }, 2m, false },
        };

        [Fact]
        public void RuleValidate_Transform()
        {
            var rule = new FormValidationRule()
            {
                Transform = value => 100,
                Max = 10,
                Type = RuleFieldType.Number,
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
            new object[] { "", null, string.Format(new ValidateMessages().Required, _displayName) },
        };

        [Fact]
        public void RuleValidate_Validator()
        {
            const int MAX = 10;
            int value = 15;

            var rule = new FormValidationRule()
            {
                Type = RuleFieldType.Integer,
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
        public void RuleValidate_Type(RuleFieldType type, object value, bool expectedValid)
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
            new object[] { RuleFieldType.String, "string", true },
            new object[] { RuleFieldType.String, 100, false },
            new object[] { RuleFieldType.String, new string[] { }, false },

            new object[] { RuleFieldType.Number, 100, true },
            new object[] { RuleFieldType.Number, 100m, true },
            new object[] { RuleFieldType.Number, 100.1, true },
            new object[] { RuleFieldType.Number, "100", false },
            new object[] { RuleFieldType.Number, new int[] { }, false },

            new object[] { RuleFieldType.Array, new int[] { }, true },
            new object[] { RuleFieldType.Array, new string[] { }, true },
            new object[] { RuleFieldType.Array, "string", false },
            new object[] { RuleFieldType.Array, 100, false },

            new object[] { RuleFieldType.Boolean, true, true },
            new object[] { RuleFieldType.Boolean, false, true },
            new object[] { RuleFieldType.Boolean, 12312, false },
            new object[] { RuleFieldType.Boolean, "true", false },

            new object[] { RuleFieldType.Date, DateTime.Parse("2021-01-02"), true },
            new object[] { RuleFieldType.Date, "2021-01-01", false },
            new object[] { RuleFieldType.Date, 100, false },

            new object[] { RuleFieldType.Email, "email@blazor.cn", true },
            new object[] { RuleFieldType.Email, "email@blazor.cn.com", true },
            new object[] { RuleFieldType.Email, "email#blazor.cn", false },

            new object[] { RuleFieldType.Integer, 100, true },
            new object[] { RuleFieldType.Integer, 100.9, false },
            new object[] { RuleFieldType.Integer, "100", false },

            new object[] { RuleFieldType.Float, 100.9, true },
            new object[] { RuleFieldType.Float, 100, false },
            new object[] { RuleFieldType.Float, "100", false },

            new object[] { RuleFieldType.Enum, RuleValidateHelperEnum.None, true },
            new object[] { RuleFieldType.Enum, 100, false },

            new object[] { RuleFieldType.Regexp, "\\d", true },
            new object[] { RuleFieldType.Regexp, "^????---))%%%$$3#@^^", false },

            new object[] { RuleFieldType.Url, "http://blazor.cn", true },
            new object[] { RuleFieldType.Url, "http://www.blazor.cn", true },
            new object[] { RuleFieldType.Url, "www.blazor.cn", false },
            new object[] { RuleFieldType.Url, "http:@www-blazor-cn", false },

        };

        [Theory]
        [MemberData(nameof(RuleValidate_DefaultField_Values))]
        public void RuleValidate_DefaultField(RuleFieldType type, object value, FormValidationRule defaultField, bool expectedValid)
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
            new object[] { RuleFieldType.Array, new string[] { "one", "there", "nine" }, new FormValidationRule { Type = RuleFieldType.String, Max = 5 }, true },
            new object[] { RuleFieldType.Array, new string[] { "one", "there", "nine" }, new FormValidationRule { Type = RuleFieldType.String, Max = 3 }, false },
        };

        [Theory]
        [MemberData(nameof(RuleValidate_Fields_Values))]
        public void RuleValidate_Fields(RuleFieldType type, object value, Dictionary<object, FormValidationRule> fields, bool expectedValid)
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
                RuleFieldType.Array,
                new string[] { "one", "there", "nine" },
                new Dictionary<object, FormValidationRule>{
                    { 0, new FormValidationRule { Type = RuleFieldType.String, Max = 5 } },
                    { 1, new FormValidationRule { Type = RuleFieldType.String, Len=5 } },
                    { 2, new FormValidationRule { Type = RuleFieldType.String, Max = 4 } },
                },
                true
            },
            new object[] {
                RuleFieldType.Array,
                new string[] { "one", "there", "nine" },
                new Dictionary<object, FormValidationRule>{
                    { 0, new FormValidationRule { Type = RuleFieldType.String, Max = 2 } },
                    { 1, new FormValidationRule { Type = RuleFieldType.String, Len=5 } },
                    { 2, new FormValidationRule { Type = RuleFieldType.String, Max = 4 } },
                },
                false
            },
            new object[] {
                RuleFieldType.Object,
                new FieldsTestObj(),
                new Dictionary<object, FormValidationRule>{
                    { "_fieldName", new FormValidationRule { Type = RuleFieldType.String, Max = 3 } },
                    { "_fieldAge", new FormValidationRule { Type = RuleFieldType.Integer, Len=10 } },
                    { "PropertyName", new FormValidationRule { Type = RuleFieldType.String, Max = 3 } },
                    { "PropertyAge", new FormValidationRule { Type = RuleFieldType.Integer, Len = 10 } },
                },
                true
            },
        };

        [Theory]
        [MemberData(nameof(RuleValidate_OneOf_Values))]
        public void RuleValidate_OneOf(RuleFieldType type, object value, object[] oneOf, bool expectedValid)
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
            new object[] { RuleFieldType.String, "one", new object[] { "one", "there", "nine" }, true },
            new object[] { RuleFieldType.String, "there", new object[] { "one", "there", "nine" }, true },
            new object[] { RuleFieldType.String, "nine", new object[] { "one", "there", "nine" }, true },
            new object[] { RuleFieldType.String, "hello", new object[] { "one", "there", "nine" }, false },

            new object[] { RuleFieldType.Integer, 1, new object[] { 1, 2, 3 }, true },
            new object[] { RuleFieldType.Integer, 2, new object[] { 1, 2, 3 }, true },
            new object[] { RuleFieldType.Integer, 3, new object[] { 1, 2, 3 }, true },
            new object[] { RuleFieldType.Integer, 4, new object[] { 1, 2, 3 }, false },
        };

        [Theory]
        [MemberData(nameof(RuleValidate_ValidateMessages_Values))]
        public void RuleValidate_ValidateMessages(FormValidationRule rule, object value, string expectedMessage)
        {
            var validationContext = new RuleValidationContext()
            {
                ValidateMessages = GetCustomValidateMessages(),
                Rule = rule,
                Value = value,
                FieldName = _fieldName,
                DisplayName = _displayName,
            };

            var resultMessage = RuleValidateHelper.GetValidationResult(validationContext)?.ErrorMessage;

            Assert.Equal(expectedMessage, resultMessage);
        }

        public static List<object[]> RuleValidate_ValidateMessages_Values => new()
        {
            new object[] { new FormValidationRule { Required = true }, null, string.Format($"{_defValidateMsgs.Required}{_customSuffix}", _displayName) },
            new object[] {
                new FormValidationRule { Type= RuleFieldType.Integer, OneOf = new object[] { 1, 2 } },
                0,
                string.Format($"{_defValidateMsgs.OneOf}{_customSuffix}", _displayName, JsonSerializer.Serialize(new object[] { 1, 2 }))
            },
            new object[] { new FormValidationRule { Type = RuleFieldType.String }, 123, string.Format($"{_defValidateMsgs.Types.String }{_customSuffix}", _displayName, RuleFieldType.String) },
            new object[] { new FormValidationRule { Type = RuleFieldType.Array }, 123, string.Format($"{_defValidateMsgs.Types.Array  }{_customSuffix}", _displayName, RuleFieldType.Array) },
            new object[] { new FormValidationRule { Type = RuleFieldType.Enum }, 123, string.Format($"{_defValidateMsgs.Types.Enum   }{_customSuffix}", _displayName, RuleFieldType.Enum) },
            new object[] { new FormValidationRule { Type = RuleFieldType.Number }, "str", string.Format($"{_defValidateMsgs.Types.Number }{_customSuffix}", _displayName, RuleFieldType.Number) },
            new object[] { new FormValidationRule { Type = RuleFieldType.Date }, 123, string.Format($"{_defValidateMsgs.Types.Date   }{_customSuffix}", _displayName, RuleFieldType.Date) },
            new object[] { new FormValidationRule { Type = RuleFieldType.Boolean }, "str", string.Format($"{_defValidateMsgs.Types.Boolean}{_customSuffix}", _displayName, RuleFieldType.Boolean) },
            new object[] { new FormValidationRule { Type = RuleFieldType.Integer }, 1.0, string.Format($"{_defValidateMsgs.Types.Integer}{_customSuffix}", _displayName, RuleFieldType.Integer) },
            new object[] { new FormValidationRule { Type = RuleFieldType.Float }, 123, string.Format($"{_defValidateMsgs.Types.Float  }{_customSuffix}", _displayName, RuleFieldType.Float) },
            new object[] { new FormValidationRule { Type = RuleFieldType.Regexp }, 123, string.Format($"{_defValidateMsgs.Types.Regexp }{_customSuffix}", _displayName, RuleFieldType.Regexp) },
            new object[] { new FormValidationRule { Type = RuleFieldType.Email }, "123", string.Format($"{_defValidateMsgs.Types.Email  }{_customSuffix}", _displayName, RuleFieldType.Email) },
            new object[] { new FormValidationRule { Type = RuleFieldType.Url }, "123", string.Format($"{_defValidateMsgs.Types.Url    }{_customSuffix}", _displayName, RuleFieldType.Url) },
        };

        private ValidateMessages GetCustomValidateMessages()
        {
            var suffix = _customSuffix;

            var customValidateMessage = new ValidateMessages();
            customValidateMessage.Default = $"{customValidateMessage.Default}{suffix}";
            customValidateMessage.Required = $"{customValidateMessage.Required}{suffix}";
            customValidateMessage.OneOf = $"{customValidateMessage.OneOf}{suffix}";

            customValidateMessage.Types.String = $"{customValidateMessage.Types.String}{suffix}";
            customValidateMessage.Types.Array = $"{customValidateMessage.Types.Array}{suffix}";
            customValidateMessage.Types.Object = $"{customValidateMessage.Types.Object}{suffix}";
            customValidateMessage.Types.Enum = $"{customValidateMessage.Types.Enum}{suffix}";
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
            var validationContext = new RuleValidationContext()
            {
                ValidateMessages = new ValidateMessages(),
                Rule = rule,
                Value = value,
                FieldName = _fieldName,
                DisplayName = _displayName,
            };

            return RuleValidateHelper.GetValidationResult(validationContext);
        }

    }
}

class CustomValidationAttribute : ValidationAttribute
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

    public override bool IsValid(object value)
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

class FieldsTestObj
{
    public string _fieldName = "one";
    public int _fieldAge = 10;
    public string PropertyName { get; set; } = "one";
    public int PropertyAge { get; set; } = 10;
}

enum RuleValidateHelperEnum
{
    None,
}
