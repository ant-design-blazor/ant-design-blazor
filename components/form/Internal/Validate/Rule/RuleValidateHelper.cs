using System;
using System.ComponentModel.DataAnnotations;

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
            if (!PatternIsValid(validationContext, out result)) return result;
            if (!ValidatorIsValid(validationContext, out result)) return result;
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
                var attribute = new RequiredAttribute();
                attribute.ErrorMessage = validationContext.ValidateMessages.Required;

                if (!IsValid(attribute, validationContext, out ValidationResult validationResult))
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
                ValidationAttribute attribute = null;

                if (rule.Type == RuleFieldType.String)
                {
                    attribute = new StringLengthAttribute((int)rule.Len);
                    attribute.ErrorMessage = validationContext.ValidateMessages.String.Len;
                }
                if (rule.Type == RuleFieldType.Number)
                {
                    attribute = new NumberAttribute((decimal)rule.Len);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Number.Len;
                }
                if (rule.Type == RuleFieldType.Array)
                {
                    attribute = new ArrayLengthAttribute((int)rule.Len);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Array.Len;
                }

                if (attribute != null && !IsValid(attribute, validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
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
                ValidationAttribute attribute = null;

                if (rule.Type.IsIn(RuleFieldType.String))
                {
                    attribute = new MinLengthAttribute((int)rule.Min);
                    attribute.ErrorMessage = validationContext.ValidateMessages.String.Min;
                }

                if (rule.Type.IsIn(RuleFieldType.Array))
                {
                    attribute = new MinLengthAttribute((int)rule.Min);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Array.Min;
                }

                if (rule.Type.IsIn(RuleFieldType.Number, RuleFieldType.Integer, RuleFieldType.Float))
                {
                    attribute = new NumberMinAttribute((decimal)rule.Min);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Number.Min;
                }

                if (attribute != null && !IsValid(attribute, validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
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
                ValidationAttribute attribute = null;

                if (rule.Type.IsIn(RuleFieldType.String))
                {
                    attribute = new MaxLengthAttribute((int)rule.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.String.Max;
                }

                if (rule.Type.IsIn(RuleFieldType.Array))
                {
                    attribute = new MaxLengthAttribute((int)rule.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Array.Max;
                }

                if (rule.Type.IsIn(RuleFieldType.Number, RuleFieldType.Integer, RuleFieldType.Float))
                {
                    attribute = new NumberMaxAttribute((decimal)rule.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Number.Max;
                }

                if (attribute != null && !IsValid(attribute, validationContext, out ValidationResult validationResult))
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
                var attribute = new RegularExpressionAttribute(rule.Pattern);
                attribute.ErrorMessage = validationContext.ValidateMessages.Pattern.Mismatch;

                if (!IsValid(attribute, validationContext, out ValidationResult validationResult))
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

            var attribute = new TypeAttribute(rule.Type);
            attribute.ErrorMessage = validationContext.ValidateMessages.GetTypeMessage(rule.Type);

            if (!IsValid(attribute, validationContext, out ValidationResult validationResult))
            {
                result = validationResult;

                return false;
            }

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
                    ValidateMessages = validationContext.ValidateMessages,
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
                ValidateMessages = validationContext.ValidateMessages,
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
                var attribute = new OneOfAttribute(rule.OneOf);
                attribute.ErrorMessage = validationContext.ValidateMessages.OneOf;

                if (!IsValid(attribute, validationContext, out ValidationResult validationResult))
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
                Console.WriteLine($"isvalid validationContext.FieldName:{validationContext.FieldName}");
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
