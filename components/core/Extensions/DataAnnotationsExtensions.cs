using System;
using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Internal
{
    internal static class DataAnnotationsExtensions
    {
        private static readonly ConcurrentDictionary<(Type ModelType, string FieldName), PropertyInfo>
            _propertyInfoCache = new ConcurrentDictionary<(Type, string), PropertyInfo>();

        internal static bool TryGetValidateProperty(this FieldIdentifier fieldIdentifier, out PropertyInfo propertyInfo)
        {
            var cacheKey = (ModelType: fieldIdentifier.Model.GetType(), fieldIdentifier.FieldName);
            propertyInfo =
                _propertyInfoCache.GetOrAdd(cacheKey, key => cacheKey.ModelType.GetProperty(cacheKey.FieldName));
            return propertyInfo != null;
        }

        internal static PropertyInfo GetProperty(this FieldIdentifier fieldIdentifier)
        {
            return fieldIdentifier.TryGetValidateProperty(out var property) ? property : null;
        }
    }
}
