// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AntDesign.Core.Documentation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public partial class GenerateColumns<TItem> : ComponentBase
    {
        private static readonly PropertyInfo[] _typeProperties = typeof(TItem).GetProperties();
        private static readonly Dictionary<(string PropertyName, Type PropertyType), ColumnTypeInfo> _columnTypeCache = new();

        private class ColumnTypeInfo
        {
            public Type ColumnType { get; set; }
            public PropertyInfo[] Parameters { get; set; }
            public Func<IFieldColumn> CreateInstance { get; set; }
        }

        /// <summary>
        /// Specific the range of the columns that need to display.
        /// </summary>
        [Parameter]
        public Range? Range { get; set; }

        /// <summary>
        /// Hide the columns by the property name.
        /// </summary>
        [Parameter]
        public IEnumerable<string> HideColumnsByName { get; set; } = new List<string>();

        /// <summary>
        /// An Action to defined each column
        /// </summary>
        /// <param name="propertyName">The name of the property binding the column. </param>
        /// <param name="column">The column instance, you need to explicitly cast to a concrete Column type. </param>
        [Parameter]
        public Action<string, IFieldColumn> Definitions { get; set; }

        /// <summary>
        /// Specify start column index, use it if auto indexes disabled and there are columns before generated ones
        /// </summary>
        [Parameter]
        [PublicApi("1.1.0")]
        public int StartColumnIndex { get; set; }

        private static ColumnTypeInfo GetColumnTypeInfo(string propertyName, Type propertyType)
        {
            var cacheKey = (propertyName, propertyType);
            if (_columnTypeCache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var underlyingType = propertyType.GetUnderlyingType();
            var columnType = typeof(Column<>).MakeGenericType(underlyingType);
            var parameters = columnType.GetProperties()
                .Where(x => x.GetCustomAttribute<ParameterAttribute>() != null)
                .Where(x => !x.Name.IsIn("DataIndex"))
                .ToArray();

            // Create a compiled factory delegate
            var constructor = columnType.GetConstructor(Type.EmptyTypes);
            var createInstance = () => (IFieldColumn)constructor.Invoke(null);

            var result = new ColumnTypeInfo
            {
                ColumnType = columnType,
                Parameters = parameters,
                CreateInstance = createInstance
            };

            _columnTypeCache[cacheKey] = result;
            return result;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var i = 0;
            var showProperties = Range != null ? _typeProperties[Range.Value] : _typeProperties;

            var colIndex = StartColumnIndex;
            foreach (var property in showProperties)
            {
                if (HideColumnsByName?.Contains(property.Name) == true) continue;

                var typeInfo = GetColumnTypeInfo(property.Name, property.PropertyType);
                var instance = typeInfo.CreateInstance();

                if (instance != null)
                {
                    instance.ColIndex = colIndex;
                }

                colIndex++;

                Definitions?.Invoke(property.Name, instance);

                var attributes = typeInfo.Parameters
                    .ToDictionary(x => x.Name, x => x.GetValue(instance))
                    .Where(x => x.Value != null);

                builder.OpenComponent(++i, typeInfo.ColumnType);
                builder.AddAttribute(++i, "DataIndex", property.Name);
                builder.AddMultipleAttributes(++i, attributes);

                builder.CloseComponent();
            }
        }
    }
}
