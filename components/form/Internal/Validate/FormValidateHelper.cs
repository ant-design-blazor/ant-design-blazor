﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AntDesign.Internal.Form.Validate
{
    internal static class FormValidateHelper
    {
        public static ValidationResult GetValidationResult(FormValidationContext validationContext)
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
        private static bool RequiredIsValid(FormValidationContext validationContext, out ValidationResult result)
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

        private static bool LenIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;
            if (rule.Len != null)
            {
                ValidationAttribute attribute = null;

                if (rule.Type == FormFieldType.String)
                {
                    attribute = new StringLengthAttribute((int)rule.Len);
                    attribute.ErrorMessage = validationContext.ValidateMessages.String.Len;
                }
                if (rule.Type == FormFieldType.Number)
                {
                    attribute = new NumberAttribute((decimal)rule.Len);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Number.Len;
                }
                if (rule.Type == FormFieldType.Array)
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

        private static bool MinIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;

            if (rule.Min != null)
            {
                ValidationAttribute attribute = null;

                if (rule.Type.IsIn(FormFieldType.String))
                {
                    attribute = new MinLengthAttribute((int)rule.Min);
                    attribute.ErrorMessage = validationContext.ValidateMessages.String.Min;
                }

                if (rule.Type.IsIn(FormFieldType.Array))
                {
                    attribute = new MinLengthAttribute((int)rule.Min);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Array.Min;
                }

                if (rule.Type.IsIn(FormFieldType.Number, FormFieldType.Integer, FormFieldType.Float))
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

        private static bool MaxIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;

            if (rule.Max != null)
            {
                ValidationAttribute attribute = null;

                if (rule.Type.IsIn(FormFieldType.String))
                {
                    attribute = new MaxLengthAttribute((int)rule.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.String.Max;
                }

                if (rule.Type.IsIn(FormFieldType.Array))
                {
                    attribute = new MaxLengthAttribute((int)rule.Max);
                    attribute.ErrorMessage = validationContext.ValidateMessages.Array.Max;
                }

                if (rule.Type.IsIn(FormFieldType.Number, FormFieldType.Integer, FormFieldType.Float))
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

        private static bool PatternIsValid(FormValidationContext validationContext, out ValidationResult result)
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

        private static bool DefaultFieldIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            if (validationContext.Rule.Type == FormFieldType.Array && validationContext.Rule.DefaultField != null)
            {
                Array values = validationContext.Value as Array;

                var context = new FormValidationContext()
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

        private static bool OneOfIsValid(FormValidationContext validationContext, out ValidationResult result)
        {
            var rule = validationContext.Rule;

            if (rule.Type != FormFieldType.Array && rule.OneOf != null)
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

        private static bool IsValid(ValidationAttribute validationAttribute, FormValidationContext validationContext, out ValidationResult result)
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
    }
}
