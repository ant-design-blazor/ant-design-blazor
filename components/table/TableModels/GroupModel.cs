// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace AntDesign.TableModels
{
    public class GroupModel<TField> : ITableGroupModel
    {
        public int Priority { get; }

        public string FieldName { get; }

        public int ColumnIndex { get; set; }

        public TableGroupOperator GroupOperator { get; set; }

        private LambdaExpression _getFieldExpression;

#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public GroupModel(int columnIndex, int priority, string fieldName, LambdaExpression getFieldExpression = null)
        {
            this.ColumnIndex = columnIndex;
            this.Priority = priority;
            this.FieldName = fieldName;
            this._getFieldExpression = getFieldExpression;
        }

        public IQueryable<GroupData<TItem>> GroupList<TItem>(IQueryable<TItem> source)
        {
            var lambda = (Expression<Func<TItem, TField>>)_getFieldExpression;
            var result = source
                .GroupBy(lambda)
                .Select(group => new GroupData<TField, TItem>(group.Key) { Items = group.ToArray() })
                .AsQueryable();
            return result;
        }
    }

    public enum TableGroupOperator
    {
        Sum = 1,
        Count = 2,
        Average = 3,
        Max = 4,
        Min = 5,
        Join = 6,
    }
}
