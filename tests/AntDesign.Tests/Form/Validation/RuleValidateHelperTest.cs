using System;
using System.Collections.Generic;
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

            var validationContext = new RuleValidationContext()
            {
                Rule = rule,
                Value = value,
                FieldName = "fieldName",
                DisplayName = "displayName",
            };

            var isValid = RuleValidateHelper.GetValidationResult(validationContext) == null;

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

            var validationContext = new RuleValidationContext()
            {
                Rule = rule,
                Value = value,
                FieldName = "fieldName",
                DisplayName = "displayName",
            };

            var isValid = RuleValidateHelper.GetValidationResult(validationContext) == null;

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

            var validationContext = new RuleValidationContext()
            {
                Rule = rule,
                Value = value,
                FieldName = "fieldName",
                DisplayName = "displayName",
            };

            var isValid = RuleValidateHelper.GetValidationResult(validationContext) == null;

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

            var validationContext = new RuleValidationContext()
            {
                Rule = rule,
                Value = value,
                FieldName = "fieldName",
                DisplayName = "displayName",
            };

            var isValid = RuleValidateHelper.GetValidationResult(validationContext) == null;

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
    }
}
