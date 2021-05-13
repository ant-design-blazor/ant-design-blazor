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
            validationContext.Value = validationContext.Rule.Transform(validationContext.Value);

            ValidationResult result;

            if (!RequiredIsValid(validationContext, out result)) return result;
            if (!TypeIsValid(validationContext, out result)) return result;
            if (!LenIsValid(validationContext, out result)) return result;
            if (!MinIsValid(validationContext, out result)) return result;
            if (!MaxIsValid(validationContext, out result)) return result;
            if (!WhitespaceIsValid(validationContext, out result)) return result;
            if (!PatternIsValid(validationContext, out result)) return result;
            if (!ValidatorIsValid(validationContext, out result)) return result;

            return null;
        }

        #region Validations
        private static bool RequiredIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            if (validationContext.Rule.Required == true)
            {
                if (!IsValid(new RequiredAttribute(), validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
                }
            }

            result = null;

            return true;
        }

        private static bool LenIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;
            if (rule.Len != null)
            {
                if (rule.Type == RuleFieldType.String)
                {
                    if (!IsValid(new StringLengthAttribute((int)rule.Len), validationContext, out ValidationResult validationResult))
                    {
                        result = validationResult;

                        return false;
                    }
                }
                if (rule.Type == RuleFieldType.Number)
                {
                    if (!IsValid(new NumberAttribute((decimal)rule.Len), validationContext, out ValidationResult validationResult))
                    {
                        result = validationResult;

                        return false;
                    }
                }
                if (rule.Type == RuleFieldType.Array)
                {
                    if (!IsValid(new ArrayLengthAttribute((int)rule.Len), validationContext, out ValidationResult validationResult))
                    {
                        result = validationResult;

                        return false;
                    }
                }
            }

            result = null;

            return true;
        }

        private static bool MinIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;

            if (rule.Min != null)
            {
                if (rule.Type.IsIn(RuleFieldType.String, RuleFieldType.Array))
                {
                    if (!IsValid(new MinLengthAttribute((int)rule.Min), validationContext, out ValidationResult validationResult))
                    {
                        result = validationResult;

                        return false;
                    }
                }
                if (rule.Type == RuleFieldType.Number)
                {
                    var type = validationContext.Value.GetType();
                    var typeMaxValue = GetMaxValueString(type);

                    var attribute = new RangeAttribute(type, ((decimal)rule.Min).ToString(), typeMaxValue);

                    if (!IsValid(attribute, validationContext, out ValidationResult validationResult))
                    {
                        result = validationResult;

                        return false;
                    }
                }
            }

            result = null;

            return true;
        }

        private static bool MaxIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;

            if (rule.Max != null)
            {
                if (rule.Type.IsIn(RuleFieldType.String, RuleFieldType.Array))
                {
                    if (!IsValid(new MaxLengthAttribute((int)rule.Max), validationContext, out ValidationResult validationResult))
                    {
                        result = validationResult;

                        return false;
                    }
                }
                if (rule.Type == RuleFieldType.Number)
                {
                    var type = validationContext.Value.GetType();
                    var typeMinValue = GetMinValueString(type);

                    var attribute = new RangeAttribute(type, typeMinValue, ((decimal)rule.Max).ToString());

                    if (!IsValid(attribute, validationContext, out ValidationResult validationResult))
                    {
                        result = validationResult;

                        return false;
                    }
                }
            }

            result = null;

            return true;
        }

        private static bool WhitespaceIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            if (validationContext.Rule.Whitespace == true)
            {
                if (!IsValid(new WhitespaceAttribute(), validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
                }
            }

            result = null;

            return true;
        }

        private static bool PatternIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;
            if (!string.IsNullOrEmpty(rule.Pattern))
            {
                if (!IsValid(new RegularExpressionAttribute(rule.Pattern), validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
                }
            }

            result = null;

            return true;
        }

        private static bool ValidatorIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;
            if (rule.Validator != null)
            {
                result = rule.Validator(validationContext);

                if (result != null)
                {
                    return false;
                }
            }

            result = null;

            return true;
        }

        private static bool TypeIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;
            if (!IsValid(new TypeAttribute(rule.Type), validationContext, out ValidationResult validationResult))
            {
                result = validationResult;

                return false;
            }

            result = null;

            return true;
        }

        #endregion Validations

        private static bool IsValid(ValidationAttribute validationAttribute, RuleValidationContext validationContext, out ValidationResult result)
        {
            if (validationAttribute.IsValid(validationContext.Value) == false)
            {
                if (validationContext.Rule.Message != null)
                {
                    validationAttribute.ErrorMessage = validationContext.Rule.Message;
                }

                string errorMessage = validationAttribute.FormatErrorMessage(validationContext.DisplayName);

                result = new ValidationResult(errorMessage, new string[] { validationContext.FieldName });

                return false;
            }

            result = null;

            return true;
        }

        private static string GetMinValueString(Type type)
        {
            if (type == typeof(byte)) return byte.MinValue.ToString();
            if (type == typeof(short)) return short.MinValue.ToString();
            if (type == typeof(int)) return int.MinValue.ToString();
            if (type == typeof(long)) return long.MinValue.ToString();
            if (type == typeof(float)) return float.MinValue.ToString();
            if (type == typeof(double)) return double.MinValue.ToString();
            if (type == typeof(sbyte)) return sbyte.MinValue.ToString();
            if (type == typeof(ushort)) return ushort.MinValue.ToString();
            if (type == typeof(uint)) return uint.MinValue.ToString();
            if (type == typeof(ulong)) return ulong.MinValue.ToString();
            if (type == typeof(decimal)) return decimal.MinValue.ToString();

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
