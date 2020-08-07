using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public static class ComponentBaseExtensions
    {
        public static bool ParameterIsChanged<T>(this ComponentBase cmp, ParameterView parameters,
            string parameterName, T value)
        {
            T newValue;
            if (parameters.TryGetValue(parameterName, out newValue))
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
