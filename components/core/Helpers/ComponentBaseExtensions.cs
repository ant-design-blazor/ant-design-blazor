using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public static class ComponentBaseExtensions
    {
        public static bool ParameterIsChanged<T>(this ComponentBase _, ParameterView parameters,
            string parameterName, T value)
        {
            if (parameters.TryGetValue(parameterName, out T newValue))
            {
                if (!EqualityComparer<T>.Default.Equals(value, newValue))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
