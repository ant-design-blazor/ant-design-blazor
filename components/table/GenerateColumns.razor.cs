// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AntDesign
{
    public partial class GenerateColumns<TItem> : ComponentBase
    {
        private PropertyInfo[] _propertyInfos = { };

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


        protected override void OnInitialized()
        {
            _propertyInfos = typeof(TItem).GetProperties();

            base.OnInitialized();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var i = 0;
            var showPropertys = _propertyInfos;
            if (Range != null)
            {
                showPropertys = _propertyInfos[Range.Value];
            }
            foreach (var property in showPropertys)
            {
                if (HideColumnsByName.Contains(property.Name)) continue;
                var columnType = typeof(Column<>).MakeGenericType(property.PropertyType.GetUnderlyingType());
                var instance = Activator.CreateInstance(columnType) as IFieldColumn;

                Definitions?.Invoke(property.Name, instance);

                var attributes = columnType.GetProperties()
                    .Where(x => x.GetCustomAttribute<ParameterAttribute>() != null)
                    .Where(x => !x.Name.IsIn("DataIndex"))
                    .ToDictionary(x => x.Name, x => x.GetValue(instance))
                    .Where(x => x.Value != null);

                builder.OpenComponent(++i, columnType);
                builder.AddAttribute(++i, "DataIndex", property.Name);
                builder.AddMultipleAttributes(++i, attributes);

                builder.CloseComponent();
            }
        }
    }
}
