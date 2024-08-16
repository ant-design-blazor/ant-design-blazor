using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AntDesign.Internal.Form.Validate
{
    internal static class FormValidateHelper
    {
        public static ValidationResult GetValidationResult(FormValidationContext validationContext)
        {
            validationContext.Value = validationContext.Rule.Transform(validationContext.Value);

            ValidationResult result;

            if (!AttributeIsValid(validationContext, out result)) return result;
            if (!RequiredIsValid(validationContext, out result)) return result;
            if (!TypeIsValid(validationContext, out result)) return result;
            if (!LenIsValid(validationContext, out result)) return result;
            if (!MinIsValid(validationContext, out result)) return result;
            if (!MaxIsValid(validationContext, out result)) return result;
            if (!RangeIsValid(validationContext, out result)) return result;
            if (!PatternIsValid(validationContext, out result)) return result;
            if (!ValidatorIsValid(validationContext, out result)) return result;
            if (!DefaultFieldIsValid(validationContext, out result)) return result;
            if (!FieldsIsValid(validationContext, out result)) return result;
            if (!OneOfIsValid(validationContext, out result)) return result;

            return null;
        }

        #region Validations

        private static bool AttributeIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            result = null;

            if (validationContext.Rule.ValidationAttribute is null)
            {
                return true;
            }

            ValidationAttribute attribute = validationContext.Rule.ValidationAttribute;

            var templates = validationContext.ValidateMessages;

            var compareMessage = validationContext.FieldType switch
            {
                Type t when t == typeof(string) => templates.String,
                Type t when THelper.IsNumericType(t) => templates.Number,
                Type t when THelper.IsEnumerable(t) => templates.Array,
                _ => templates.String
            };

            attribute.ErrorMessage = attribute switch
            {
                // if user has set the ErrorMessage, we will use it directly
                { ErrorMessage.Length: > 0 } or { ErrorMessageResourceName: not null } => attribute.ErrorMessage,
                RequiredAttribute => ReplaceLabel(templates.Required),
                RangeAttribute => ReplaceLength(compareMessage.Range, max: 2),
                MinLengthAttribute => ReplaceLength(compareMessage.Min),
                MaxLengthAttribute => ReplaceLength(compareMessage.Max),
                StringLengthAttribute => ReplaceLength(templates.String.Range, max: 2),
                _ => attribute.ErrorMessage,
            };

            return IsValid(validationContext.Rule.ValidationAttribute, validationContext, out result);
        }

        private static bool RequiredIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            if (validationContext.Rule.Required == true)
            {
                var attribute = new RequiredAttribute();
                attribute.ErrorMessage = ReplaceLabel(validationContext.ValidateMessages.Required);

                if (!IsValid(attribute, validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
                }
            }

            result = null;

            return true;
        }

        private static bool LenIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            result = null;
            var rule = validationContext.Rule;
            var fieldType = validationContext.FieldType;
            if (rule.Len != null)
            {
                ValidationAttribute attribute = null;

                if (fieldType == typeof(string))
                {
                    attribute = new StringLengthAttribute((int)rule.Len);
                    attribute.ErrorMessage = validationContext.ValidateMessages.String.Len;
                }
                else if (THelper.IsNumericType(fieldType))
                {
                    attribute = new NumberAttribute((decimal)rule.Len);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Number.Len;
                }
                else if (THelper.IsEnumerable(fieldType))
                {
                    attribute = new ArrayLengthAttribute((int)rule.Len);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Array.Len;
                }

                if (attribute is null)
                {
                    return true;
                }

                attribute.ErrorMessage = ReplaceLength(attribute.ErrorMessage);

                if (attribute != null && !IsValid(attribute, validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;
                    return false;
                }
            }

            return true;
        }

        private static bool MinIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            result = null;
            var rule = validationContext.Rule;
            var fieldType = validationContext.FieldType;
            if (rule.Min != null)
            {
                ValidationAttribute attribute = null;

                if (fieldType == typeof(string))
                {
                    attribute = new MinLengthAttribute((int)rule.Min);
                    attribute.ErrorMessage = validationContext.ValidateMessages.String.Min;
                }
                else if (THelper.IsEnumerable(fieldType))
                {
                    attribute = new MinLengthAttribute((int)rule.Min);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Array.Min;
                }
                else if (THelper.IsNumericType(fieldType))
                {
                    attribute = new NumberMinAttribute((decimal)rule.Min);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Number.Min;
                }

                if (attribute is null)
                {
                    return true;
                }

                attribute.ErrorMessage = ReplaceLength(attribute.ErrorMessage);

                if (attribute != null && !IsValid(attribute, validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
                }
            }

            return true;
        }

        private static bool MaxIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            result = null;
            var rule = validationContext.Rule;
            var fieldType = validationContext.FieldType;

            if (rule.Max != null)
            {
                ValidationAttribute attribute = null;

                if (fieldType == typeof(string))
                {
                    attribute = new MaxLengthAttribute((int)rule.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.String.Max;
                }
                else if (THelper.IsEnumerable(fieldType))
                {
                    attribute = new MaxLengthAttribute((int)rule.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Array.Max;
                }
                else if (THelper.IsNumericType(fieldType))
                {
                    attribute = new NumberMaxAttribute((decimal)rule.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Number.Max;
                }

                if (attribute is null)
                {
                    return true;
                }

                attribute.ErrorMessage = ReplaceLength(attribute.ErrorMessage);

                if (attribute != null && !IsValid(attribute, validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
                }
            }

            return true;
        }

        private static bool RangeIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            result = null;
            var rule = validationContext.Rule;
            var fieldType = validationContext.FieldType;

            if (rule.Range != null)
            {
                ValidationAttribute attribute = null;

                if (fieldType == typeof(string))
                {
                    attribute = new StringRangeAttribute((int)rule.Range.Value.Min, (int)rule.Range.Value.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.String.Range;
                }
                else if (THelper.IsEnumerable(fieldType))
                {
                    attribute = new ArrayRangeAttribute((int)rule.Range.Value.Min, (int)rule.Range.Value.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Array.Range;
                }
                else if (THelper.IsNumericType(fieldType))
                {
                    attribute = new RangeAttribute(rule.Range.Value.Min, rule.Range.Value.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Number.Range;
                    validationContext.Value ??= 0;
                }

                if (attribute is null)
                {
                    return true;
                }

                attribute.ErrorMessage = ReplaceLength(attribute.ErrorMessage, max: 2);

                if (attribute != null && !IsValid(attribute, validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
                }
            }

            return true;
        }

        private static bool PatternIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;
            if (!string.IsNullOrEmpty(rule.Pattern))
            {
                var attribute = new RegularExpressionAttribute(rule.Pattern);
                attribute.ErrorMessage = ReplacePattern(validationContext.ValidateMessages.Pattern.Mismatch);

                if (!IsValid(attribute, validationContext, out ValidationResult validationResult))
                {
                    result = validationResult;

                    return false;
                }
            }

            result = null;

            return true;
        }

        private static bool ValidatorIsValid(FormValidationContext validationContext, out ValidationResult result)
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

        private static bool TypeIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            result = null;
            var rule = validationContext.Rule;

            if (rule.Type == null)
            {
                return true;
            }

            var attribute = new TypeAttribute(rule.Type.Value);
            attribute.ErrorMessage = ReplaceType(validationContext.ValidateMessages.GetTypeMessage(rule.Type.Value));

            if (!IsValid(attribute, validationContext, out ValidationResult validationResult))
            {
                result = validationResult;

                return false;
            }

            return true;
        }

        private static bool DefaultFieldIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            if (THelper.IsEnumerable(validationContext.FieldType) && validationContext.Rule.DefaultField != null)
            {
                Array values = validationContext.Value as Array;

                var context = new FormValidationContext()
                {
                    ValidateMessages = validationContext.ValidateMessages,
                    Rule = validationContext.Rule.DefaultField,
                    FieldName = validationContext.FieldName,
                    FieldType = validationContext.FieldType,
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

        private static bool FieldsIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;

            if (!rule.Type.IsIn(FormFieldType.Array, FormFieldType.Object) || rule.Fields == null)
            {
                result = null;

                return true;
            }

            var context = new FormValidationContext()
            {
                ValidateMessages = validationContext.ValidateMessages,
                DisplayName = validationContext.DisplayName,
                FieldName = validationContext.FieldName,
                FieldType = validationContext.FieldType,
            };

            Array arrValues = validationContext.Value as Array;
            var objectType = validationContext.Value.GetType();

            foreach (var key in rule.Fields.Keys)
            {
                if (rule.Type == FormFieldType.Array)
                {
                    int index = (int)key;

                    // out of range, ignore validation
                    if (index >= arrValues.Length)
                    {
                        continue;
                    }

                    context.Value = arrValues.GetValue(index);
                    context.DisplayName = $"{validationContext.DisplayName}[{index}]";
                    context.FieldType = context.Value.GetType();
                }
                else
                {
                    var propertyValue = objectType.GetProperty(key.ToString());
                    if (propertyValue != null)
                    {
                        context.Value = propertyValue.GetValue(validationContext.Value);
                        context.FieldType = propertyValue.GetType();
                    }
                    else
                    {
                        var fieldValue = objectType.GetField(key.ToString());

                        // field not exists, ignore validation
                        if (fieldValue == null)
                        {
                            continue;
                        }

                        context.FieldType = fieldValue.GetType();
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

        private static bool OneOfIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;

            object[] values = [];
            string[] labels = null;

            if (rule.Type != FormFieldType.Array)
            {
                if (rule.OneOf != null)
                {
                    values = rule.OneOf;
                }
                else if (rule.Enum != null)
                {
                    var enumValues = Enum.GetValues(rule.Enum);
                    values = enumValues.Cast<object>().ToArray();
                    labels = values.Select(v => EnumHelper.GetDisplayName(rule.Enum, v)).ToArray();
                }
            }

            if (values.Length > 0)
            {
                var attribute = new OneOfAttribute(values, labels);
                attribute.ErrorMessage = ReplaceEnum(validationContext.ValidateMessages.Enum);

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

        private static bool IsValid(ValidationAttribute validationAttribute, FormValidationContext validationContext, out ValidationResult result)
        {
            if (validationAttribute?.IsValid(validationContext.Value) == false)
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

        #region message replacement
        private static string ReplaceTypeMessage(string message) => message.Replace("${label}", "{0}").Replace("${type}", "{1}");

        private static string ReplaceLabel(string message) => message.Replace("${label}", "{0}");

        private static string ReplaceLength(string message, int min = 1, int max = 1, int len = 1)
            => message
            .Replace("${label}", "{0}")
            .Replace("${len}", $"{{{len}}}")
            .Replace("${min}", $"{{{min}}}")
            .Replace("${max}", $"{{{max}}}");

        private static string ReplacePattern(string message)
            => message
            .Replace("${label}", "{0}")
            .Replace("${pattern}", "{1}");

        private static string ReplaceType(string message)
            => message
            .Replace("${label}", "{0}")
            .Replace("${type}", "{1}");

        private static string ReplaceEnum(string message)
            => message
            .Replace("${label}", "{0}")
            .Replace("${enum}", "{1}");

        #endregion
    }
}
