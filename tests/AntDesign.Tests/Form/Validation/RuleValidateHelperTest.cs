using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AntDesign;
using AntDesign.Internal;
using Xunit;

namespace AntDesign.Tests.Form.Validation
{
    public class RuleValidateHelperTest
    {
        [Theory]
        [InlineData("hello", true, true)]
        [InlineData("hello", false, true)]
        [InlineData(null, true, false)]
        [InlineData(null, false, true)]
        [InlineData("", true, false)]
        [InlineData("", false, true)]
        [InlineData(123, true, true)]
        [InlineData(123, false, true)]
        public void RuleValidate_Required(object value, bool required, bool expectedValid)
        {
            var rule = new Rule()
            {
                Required = required,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        [Theory]
        [InlineData(" ", true, false)]
        [InlineData(" ", false, true)]
        [InlineData(null, true, true)]
        [InlineData(null, false, true)]
        [InlineData("", true, true)]
        [InlineData("", false, true)]
        [InlineData(123, true, true)]
        [InlineData(123, false, true)]
        public void RuleValidate_Whitespace(object value, bool whitespace, bool expectedValid)
        {
            var rule = new Rule()
            {
                Whitespace = whitespace,
            };

            var isValid = GetValidationResult(rule, value) == null;

            Assert.Equal(expectedValid, isValid);
        }

        [Theory]
        [MemberData(nameof(RuleValidate_Len_Values))]
        public void RuleValidate_Len(RuleFieldType type, object value, decimal len, bool expectedValid)
        {
            var rule = new Rule()
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
            var rule = new Rule()
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
            var rule = new Rule()
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
            var rule = new Rule()
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
            var rule = new Rule()
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
            var rule = new Rule()
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
            new object[] { "", null, "The displayName field is required." },
        };


        [Fact]
        public void RuleValidate_Validator()
        {
            const int MAX = 10;
            int value = 15;

            var rule = new Rule()
            {
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


        private ValidationResult GetValidationResult(Rule rule, object value)
        {
            var validationContext = new RuleValidationContext()
            {
                Rule = rule,
                Value = value,
                FieldName = "fieldName",
                DisplayName = "displayName",
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
