using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    internal static class FieldIdentifierExtensions
    {
        private static readonly ConcurrentDictionary<(Type ModelType, string FieldName), string> _displayNameCache =
            new ConcurrentDictionary<(Type, string), string>();

        public static string GetDisplayName(this FieldIdentifier fieldIdentifier)
        {
            var cacheKey = (Type: fieldIdentifier.Model.GetType(), fieldIdentifier.FieldName);

            var displayName = _displayNameCache.GetOrAdd(cacheKey, key =>
            {
                if (fieldIdentifier.TryGetValidateProperty(out var propertyInfo))
                {
                    var displayNameAttribute = propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    if (displayNameAttribute.Length > 0)
                    {
                        return ((DisplayNameAttribute)displayNameAttribute[0]).DisplayName ?? key.FieldName;
                    }
                }

                return key.FieldName;
            });

            return displayName;
        }
    }
}
