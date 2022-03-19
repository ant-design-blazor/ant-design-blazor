// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AntDesign;
using AntDesign.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Reflection;
using AntDesign.Core.Reflection;

namespace AntDesign
{
    partial class DataForm<TItem> where TItem : class, new()
    {
        private EntityClassAccessor _entityClassAccessor = null;
        private int _columnsCount = 1;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        [Parameter]
        public TItem CurrentItem
        {
            get => (TItem)_entityClassAccessor?.CurrentItem;
            set
            {
                if (_entityClassAccessor == null)
                {
                    _entityClassAccessor = new EntityClassAccessor();
                }
                _entityClassAccessor.CurrentItem = value;
            }
        }

        [Parameter]
        public int ColumnsCount
        {
            get => _columnsCount; set
            {
                if (value < 1)
                {
                    _columnsCount = 1;
                }
                else if (value > 6)
                {
                    _columnsCount = 6;
                }
                else
                {
                    _columnsCount = value;
                }
            }
        }

        private RenderFragment RenderMultiItems<T>(PropertyValueAccessor accessor, System.Linq.Expressions.Expression<Func<IEnumerable<T>>> valuesExpression, EventCallback<IEnumerable<T>> valuesChanged)
        {
            if (!accessor.PropertyInfo.PropertyType.IsSubTypeOf(typeof(QueryConditionMultiItem<>)))
                return null;
            Type pType = accessor.PropertyInfo.PropertyType;
            Type valueType = null, itemType = null;
            while (pType != null)
            {
                Type[] t1 = pType.GetGenericArguments();
                if (t1?.Length == 1)
                {
                    valueType = t1[0];
                    break;
                }
                pType = pType?.BaseType;
            }

            var itemsControlAttribute = EntityClassAccessor.GetAttribute(accessor.PropertyInfo, typeof(DataSourceBindAttribute)) as DataSourceBindAttribute;
            if (itemsControlAttribute == null)
            {
                itemType = typeof(string);
            }
            else
            {
                var pi = itemsControlAttribute.BindType.GetProperty(itemsControlAttribute.DataSourcePath, BindingFlags.Static | BindingFlags.Public);
                if (pi == null)
                {
                    pi = itemsControlAttribute.BindType?.GetProperty(itemsControlAttribute.DataSourcePath);
                }
                itemType = pi?.PropertyType.GetGenericArguments()[0];
            }
            //Type t1 = propertyInfo.PropertyType.GetGenericArguments()[0];
            void Foo(RenderTreeBuilder builder)
            {
                Type ddl = typeof(AntDesign.Select<,>);
                ddl = ddl.MakeGenericType(valueType, itemType);
                builder.OpenComponent(0, ddl);
                builder.AddAttribute(1, "Mode", "multiple");
                builder.AddAttribute(2, "ValuesExpression", valuesExpression);
                builder.AddAttribute(3, "ValuesChanged", valuesChanged);
                builder.AddAttribute(4, "DataSource", accessor.GetItemsControlSource());
                if (!string.IsNullOrEmpty(itemsControlAttribute?.LabelPath))
                    builder.AddAttribute(5, "LabelName", itemsControlAttribute?.LabelPath);
                if (!string.IsNullOrEmpty(itemsControlAttribute?.ValuePath))
                    builder.AddAttribute(6, "ValueName", itemsControlAttribute?.ValuePath);
                builder.AddAttribute(7, "Disabled", (!accessor.QueryConditionItem.Checked));
                builder.AddAttribute(8, "Values", valuesExpression.Compile().Invoke());
                builder.CloseComponent();
            }

            return Foo;
        }

        private RenderFragment RenderEnumItem<T>(PropertyValueAccessor accessor, System.Linq.Expressions.Expression<Func<T>> valueExpression, EventCallback<T> valueChanged)
        {
            void Foo(RenderTreeBuilder builder)
            {
                Type ddl = typeof(AntDesign.EnumSelect<>);
                ddl = ddl.MakeGenericType(accessor.QueryConditionGenericType);
                builder.OpenComponent(0, ddl);
                builder.AddAttribute(1, "ValueExpression", valueExpression);
                builder.AddAttribute(2, "ValueChanged", valueChanged);
                builder.AddAttribute(3, "Disabled", (!accessor.QueryConditionItem.Checked));
                builder.CloseComponent();
            }

            return Foo;
        }
    }
}
