// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Components
{
    public static class ParameterViewExtensions
    {
        public static bool IsParameterChanged<T>(this ParameterView parameters,
            string parameterName, T currentValue)
        {
            return IsParameterChanged(parameters, parameterName, currentValue, out _);
        }

        public static bool IsParameterChanged<T>(this ParameterView parameters,
        string parameterName, IEnumerable<T> currentValue)
        {
            return IsParameterChanged(parameters, parameterName, currentValue, out _);
        }

        public static bool IsParameterChanged<T>(this ParameterView parameters, string parameterName, IEnumerable<T> currentValue, out IEnumerable<T> newValue)
        {
            if (parameters.TryGetValue<IEnumerable<T>>(parameterName, out newValue))
            {
                if (newValue == null && currentValue == null)
                {
                    return false;
                }

                if (newValue == null && currentValue != null)
                {
                    return true;
                }

                if (newValue != null && currentValue == null)
                {
                    return true;
                }

                return !currentValue.SequenceEqual(newValue);
            }

            return false;
        }

        public static bool IsParameterChanged<T>(this ParameterView parameters,
            string parameterName, T currentValue, out T newValue)
        {
            if (parameters.TryGetValue(parameterName, out newValue))
            {
                if (newValue == null && currentValue == null)
                {
                    return false;
                }

                if (newValue == null && currentValue != null)
                {
                    return true;
                }

                if (newValue != null && currentValue == null)
                {
                    return true;
                }

                if (newValue is string[] stringNewValue && currentValue is string[] stringValue)
                {
                    return !stringNewValue.SequenceEqual(stringValue);
                }

                if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
