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
            string parameterName, T value)
        {
            return IsParameterChanged(parameters, parameterName, value, out _);
        }

        public static bool IsParameterChanged<T>(this ParameterView parameters,
            string parameterName, T value, out T newValue)
        {
            if (parameters.TryGetValue(parameterName, out newValue))
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
