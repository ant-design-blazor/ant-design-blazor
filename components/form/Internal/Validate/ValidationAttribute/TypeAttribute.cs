using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AntDesign.Internal
{
    internal class TypeAttribute : ValidationAttribute
    {
        internal RuleFieldType Type { get; set; }
        internal TypeAttribute(RuleFieldType type) : base("The field {0} should be type of {1}")// TODO: localizable
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
                RuleFieldType.String => value is string,
                RuleFieldType.Number => IsNumber(value),
                RuleFieldType.Integer => IsInteger(value),
                RuleFieldType.Float => IsFloat(value),
                RuleFieldType.Boolean => value is bool,
                RuleFieldType.Regexp => IsRegexp(value),
                RuleFieldType.Array => value is Array,
                RuleFieldType.Object => value is object,
                RuleFieldType.Enum => value is Enum,
                RuleFieldType.Date => value is DateTime,
                RuleFieldType.Url => new UrlAttribute().IsValid(value),
                RuleFieldType.Email => new EmailAddressAttribute().IsValid(value),
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
