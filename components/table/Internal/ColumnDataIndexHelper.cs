// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using AntDesign.core.Helpers;
using AntDesign.TableModels;

namespace AntDesign.Internal
{
    internal static class ColumnDataIndexHelper<TProp>
    {
        private static readonly ConcurrentDictionary<ColumnCacheKey, ColumnCacheItem> _dataIndexCache = new();

        internal static ColumnCacheItem GetDataIndexConfig(Column<TProp> column)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            var cacheKey = ColumnCacheKey.Create(column);
            return _dataIndexCache.GetOrAdd(cacheKey, CreateDataIndexConfig);
        }

        private static ColumnCacheItem CreateDataIndexConfig(ColumnCacheKey key)
        {
            var (itemType, propType, dataIndex) = key;
            Func<RowData, TProp> getValue = null;
            LambdaExpression getFieldExpression = null;
            var properties = dataIndex?.Split(".");
            if (properties is {Length: > 0})
            {
                var canBeNull = !propType.IsValueType || Nullable.GetUnderlyingType(propType) != null;
                var rowDataType = typeof(RowData);
                var rowData1Type = typeof(RowData<>).MakeGenericType(itemType);
                var rowDataExp = Expression.Parameter(rowDataType);
                var rowData1Exp = Expression.TypeAs(rowDataExp, rowData1Type);
                var dataMemberExp = Expression.Property(rowData1Exp, nameof(RowData<object>.Data));

                var memberExp = canBeNull
                                    ? dataMemberExp.AccessNullableProperty(properties)
                                    : dataMemberExp.AccessProperty(properties);
                getValue = Expression.Lambda<Func<RowData, TProp>>(memberExp, rowDataExp).Compile();

                getFieldExpression = canBeNull
                                         ? itemType.BuildAccessNullablePropertyLambdaExpression(properties)
                                         : itemType.BuildAccessPropertyLambdaExpression(properties);
            }

            return new ColumnCacheItem(getValue, getFieldExpression);
        }

        internal readonly struct ColumnCacheKey
        {
            internal readonly Type ItemType;

            internal readonly Type PropType;

            internal readonly string DataIndex;

            internal static ColumnCacheKey Create(Column<TProp> column)
            {
                return new(column.ItemType, typeof(TProp), column.DataIndex);
            }

            internal ColumnCacheKey(Type itemType, Type propType, string dataIndex)
            {
                ItemType = itemType;
                PropType = propType;
                DataIndex = dataIndex;
            }

            internal void Deconstruct(out Type itemType, out Type propType, out string dataIndex)
            {
                itemType = ItemType;
                propType = PropType;
                dataIndex = DataIndex;
            }
        }

        internal readonly struct ColumnCacheItem
        {
            internal readonly Func<RowData, TProp> GetValue;

            internal readonly LambdaExpression GetFieldExpression;

            internal ColumnCacheItem(Func<RowData, TProp> getValue, LambdaExpression getFieldExpression)
            {
                GetValue = getValue;
                GetFieldExpression = getFieldExpression;
            }

            internal void Deconstruct(out Func<RowData, TProp> getValue, out LambdaExpression getFieldExpression)
            {
                getValue = GetValue;
                getFieldExpression = GetFieldExpression;
            }
        }
    }
}
