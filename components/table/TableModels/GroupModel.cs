// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace AntDesign.TableModels
{
    public abstract class GroupModel : ITableGroupModel
    {
        public int Priority { get; set; }

        public string FieldName { get; }

        public int ColumnIndex { get; set; }

        public TableGroupOperator GroupOperator { get; set; }

        protected LambdaExpression _getFieldExpression;

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

        public GroupModel(GroupModel model)
        {
            this.ColumnIndex = model.ColumnIndex;
            this.Priority = model.Priority;
            this.FieldName = model.FieldName;
            this._getFieldExpression = model._getFieldExpression;
        }

        public abstract IQueryable<GroupData<TItem>> GroupList<TItem>(IQueryable<TItem> source);
    }

    public class GroupModel<TField> : GroupModel
    {
#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public GroupModel(int columnIndex, int priority, string fieldName, LambdaExpression getFieldExpression = null)
            : base(columnIndex, priority, fieldName, getFieldExpression)
        {
        }

        public override IQueryable<GroupData<TItem>> GroupList<TItem>(IQueryable<TItem> source)
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
