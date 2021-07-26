﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AntDesign.Internal.Form.Validate
{
    internal class TypeAttribute : ValidationAttribute
    {
        internal FormFieldType Type { get; set; }
        internal TypeAttribute(FormFieldType type)
        {
            Type = type;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Type.ToString());
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            return Type switch
            {
                FormFieldType.String => value is string,
                FormFieldType.Number => IsNumber(value),
                FormFieldType.Integer => IsInteger(value),
                FormFieldType.Float => IsFloat(value),
                FormFieldType.Boolean => value is bool,
                FormFieldType.Regexp => IsRegexp(value),
                FormFieldType.Array => value is Array,
                FormFieldType.Object => value is object,
                FormFieldType.Enum => value is Enum,
                FormFieldType.Date => value is DateTime,
                FormFieldType.Url => new UrlAttribute().IsValid(value),
                FormFieldType.Email => new EmailAddressAttribute().IsValid(value),
                _ => false,
            };
        }

        private bool IsRegexp(object value)
        {
            if (!(value is string valueAsStrng))
            {
                return false;
            }

            try
            {
                new Regex(valueAsStrng);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (ArgumentException)
            {

                return false;
            }

            return true;
        }

        private bool IsNumber(object value)
        {
            Type[] numberTypes = new Type[] {
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(float),
                typeof(double),
                typeof(sbyte),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
                typeof(decimal),
            };

            if (value.GetType().IsIn(numberTypes))
            {
                return true;
            }

            return false;
        }

        private bool IsInteger(object value)
        {
            Type[] integerTypes = new Type[] {
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(sbyte),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),
            };

            if (value.GetType().IsIn(integerTypes))
            {
                return true;
            }

            return false;
        }

        private bool IsFloat(object value)
        {
            Type[] floatTypes = new Type[] {
                typeof(float),
                typeof(double),
                typeof(decimal),
            };

            if (value.GetType().IsIn(floatTypes))
            {
                return true;
            }

            return false;
        }
    }
}
