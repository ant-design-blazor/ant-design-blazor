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
            if (!EnumIsValid(validationContext, out result)) return result;
            if (!DefaultFieldIsValid(validationContext, out result)) return result;
            if (!FieldsIsValid(validationContext, out result)) return result;
            if (!OneOfIsValid(validationContext, out result)) return result;

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

        private static bool EnumIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            // TODO
            //if (validationContext.Rule.Enum != null)
            //{
            //    if (!IsValid(new EnumDataTypeAttribute(), validationContext, out ValidationResult validationResult))
            //    {
            //        result = validationResult;

            //        return false;
            //    }
            //}

            result = null;

            return true;
        }

        private static bool DefaultFieldIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            if (validationContext.Rule.Type == RuleFieldType.Array && validationContext.Rule.DefaultField != null)
            {
                Array values = validationContext.Value as Array;

                var context = new RuleValidationContext()
                {
                    Rule = validationContext.Rule.DefaultField,
                    FieldName = validationContext.FieldName,
                };

                int index = 0;
                foreach (var value in values)
                {
                    context.Value = value;
                    context.DisplayName = $"{validationContext.DisplayName}[{index}]";

                    result = GetValidationResult(context);
                    if (result != null)
                    {
                        return false;
                    }

                    index++;
                }
            }

            result = null;

            return true;
        }

        private static bool FieldsIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;

            if (!rule.Type.IsIn(RuleFieldType.Array, RuleFieldType.Object) || rule.Fields == null)
            {
                result = null;

                return true;
            }

            var context = new RuleValidationContext()
            {
                DisplayName = validationContext.DisplayName,
                FieldName = validationContext.FieldName,
            };

            Array arrValues = validationContext.Value as Array;
            var objectType = validationContext.Value.GetType();

            foreach (var key in rule.Fields.Keys)
            {
                if (rule.Type == RuleFieldType.Array)
                {
                    int index = (int)key;

                    // out of range, ignore validation
                    if (index >= arrValues.Length)
                    {
                        continue;
                    }

                    context.Value = arrValues.GetValue(index);
                    context.DisplayName = $"{validationContext.DisplayName}[{index}]";
                }
                else
                {
                    var propertyValue = objectType.GetProperty(key.ToString());
                    if (propertyValue != null)
                    {
                        context.Value = propertyValue.GetValue(validationContext.Value);
                    }
                    else
                    {
                        var fieldValue = objectType.GetField(key.ToString());

                        // field not exists, ignore validation
                        if (fieldValue == null)
                        {
                            continue;
                        }

                        context.Value = fieldValue.GetValue(validationContext.Value);
                    }

                    context.DisplayName = $"{validationContext.DisplayName}.{key}";
                }

                context.Rule = rule.Fields[key];

                result = GetValidationResult(context);
                if (result != null)
                {
                    return false;
                }
            }

            result = null;

            return true;
        }

        private static bool OneOfIsValid(RuleValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;

            if (rule.Type != RuleFieldType.Array && rule.OneOf != null)
            {
                if (!IsValid(new OneOfAttribute(rule.OneOf), validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
                }
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
