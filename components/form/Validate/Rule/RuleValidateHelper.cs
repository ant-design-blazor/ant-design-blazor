using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AntDesign.Internal
{
    internal class RuleValidateHelper
    {
        public static ValidationResult GetValidationResult(RuleValidationContext validationContext)
        {
            Func<ValidationAttribute, ValidationResult> getValidationResult = (attribute) =>
            {
                if (!IsValid(attribute, validationContext, out ValidationResult validationResult))
                {
                    return validationResult;
                }

                return null;
            };

            var rule = validationContext.Rule;

            if (rule.Required == true)
            {
                var result = getValidationResult(new RequiredAttribute());
                if (result != null) return result;
            }

            if (rule.Len != null)
            {
                if (rule.Type == RuleFieldType.String)
                {
                    var result = getValidationResult(new StringLengthAttribute((int)rule.Len));
                    if (result != null) return result;
                }
                if (rule.Type == RuleFieldType.Number)
                {
                    var result = getValidationResult(new NumberAttribute((decimal)rule.Len));
                    if (result != null) return result;
                }
                if (rule.Type == RuleFieldType.Array)
                {
                    var result = getValidationResult(new ArrayLengthAttribute((int)rule.Len));
                    if (result != null) return result;
                }
            }

            if (rule.Min != null)
            {
                if (rule.Type.IsIn(RuleFieldType.String, RuleFieldType.Array))
                {
                    var result = getValidationResult(new MinLengthAttribute((int)rule.Min));
                    if (result != null) return result;
                }
                if (rule.Type == RuleFieldType.Number)
                {
                    var type = validationContext.Value.GetType();
                    var typeMaxValue = GetMaxValueString(type);

                    var attribute = new RangeAttribute(type, ((decimal)rule.Min).ToString(), typeMaxValue);
                    var result = getValidationResult(attribute);
                    if (result != null) return result;
                }
            }

            if (rule.Max != null)
            {
                if (rule.Type.IsIn(RuleFieldType.String, RuleFieldType.Array))
                {
                    var result = getValidationResult(new MinLengthAttribute((int)rule.Max));
                    if (result != null) return result;
                }
                if (rule.Type == RuleFieldType.Number)
                {
                    var type = validationContext.Value.GetType();
                    var typeMinValue = GetMinValueString(type);

                    var attribute = new RangeAttribute(type, typeMinValue, ((decimal)rule.Max).ToString());
                    var result = getValidationResult(attribute);
                    if (result != null) return result;
                }
            }

            return null;
        }

        private static bool IsValid(ValidationAttribute validationAttribute, RuleValidationContext validationContext, out ValidationResult result)
        {
            if (validationAttribute.IsValid(validationContext.Value) == false)
            {
                result = new ValidationResult(
                    validationAttribute.FormatErrorMessage(validationContext.DisplayName), new string[] { validationContext.FieldName });

                return false;
            }

            result = null;

            return true;
        }

        private static string GetMinValueString(Type type)
        {
            if (type == typeof(byte)) return byte.MinValue.ToString();
            if (type == typeof(short)) return short.MinValue.ToString();
            if (type == typeof(float)) return float.MinValue.ToString();
            if (type == typeof(double)) return double.MinValue.ToString();
            if (type == typeof(long)) return long.MinValue.ToString();
            if (type == typeof(uint)) return uint.MinValue.ToString();
            if (type == typeof(ulong)) return ulong.MinValue.ToString();
            if (type == typeof(ushort)) return ushort.MinValue.ToString();
            if (type == typeof(decimal)) return decimal.MinValue.ToString();
            if (type == typeof(sbyte)) return sbyte.MinValue.ToString();

            return null;
        }

        private static string GetMaxValueString(Type type)
        {
            if (type == typeof(byte)) return byte.MaxValue.ToString();
            if (type == typeof(short)) return short.MaxValue.ToString();
            if (type == typeof(int)) return int.MaxValue.ToString();
            if (type == typeof(long)) return long.MaxValue.ToString();
            if (type == typeof(float)) return float.MaxValue.ToString();
            if (type == typeof(double)) return double.MaxValue.ToString();
            if (type == typeof(sbyte)) return sbyte.MaxValue.ToString();
            if (type == typeof(ushort)) return ushort.MaxValue.ToString();
            if (type == typeof(uint)) return uint.MaxValue.ToString();
            if (type == typeof(ulong)) return ulong.MaxValue.ToString();
            if (type == typeof(decimal)) return decimal.MaxValue.ToString();

            return null;
        }
    }
}
