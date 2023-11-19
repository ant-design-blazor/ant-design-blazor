using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Components
{
    public static class ParameterViewExtensions
    {
        public static bool IsParameterChanged<T>(this ParameterView parameters,
            string parameterName, T value)
        {
            if (parameters.TryGetValue(parameterName, out T newValue))
            {
                if (newValue == null && value == null)
                {
                    return false;
                }

                if (newValue == null && value != null)
                {
                    return true;
                }

                if (newValue != null && value == null)
                {
                    return true;
                }

                if (newValue is string[] stringNewValue && value is string[] stringValue)
                {
                    return !stringNewValue.SequenceEqual(stringValue);
                }

                if (!EqualityComparer<T>.Default.Equals(value, newValue))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
