// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using AntDesign.Core.Helpers;
using AntDesign.Core.Helpers.MemberPath;
using AntDesign.Filters;

namespace AntDesign.TableModels
{
    public class FilterModel<TField> : ITableFilterModel
    {
        public string FieldName { get; }

        public IEnumerable<string> SelectedValues { get; }

        public IList<TableFilter> Filters { get; }

        [JsonIgnore]
        public Expression<Func<TField, TField, bool>> OnFilter { get; private set; }

        public int ColumnIndex => _columnIndex;

        public TableFilterType FilterType { get; private set; } = TableFilterType.List;

        private LambdaExpression _getFieldExpression;
        private readonly int _columnIndex;
        private IFieldFilterType _fieldFilterType;

#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public FilterModel(int columnIndex, string fieldName, IEnumerable<string> selectedValues, IList<TableFilter> filters, TableFilterType filterType)
        {
            this.FieldName = fieldName;
            this.SelectedValues = selectedValues;
            this.Filters = filters;
            this._columnIndex = columnIndex;
            this.OnFilter = (value, field) => field.Equals(value);
            this._fieldFilterType = InternalFieldFilterTypeResolver.Resolve<TField>();
            this.FilterType = filterType;
        }

        public FilterModel(IFieldColumn column, LambdaExpression getFieldExpression, string fieldName,
            Expression<Func<TField, TField, bool>> onFilter, IList<TableFilter> filters, TableFilterType filterType, IFieldFilterType fieldFilterType)
        {
            this._getFieldExpression = getFieldExpression;
            this.FieldName = fieldName;
            if (onFilter == null)
            {
                this.OnFilter = (value, field) => field.Equals(value);
            }
            else
            {
                this.OnFilter = onFilter;
            }

            this.SelectedValues = filters.Select(x => x.Value?.ToString());
            this.Filters = filters;
            this.FilterType = filterType;
            this._columnIndex = column.ColIndex;
            this._fieldFilterType = fieldFilterType;
        }

        public IQueryable<TItem> FilterList<TItem>(IQueryable<TItem> source)
        {
            if (Filters?.Any() != true)
            {
                return source;
            }
            return source.Where(FilterExpression<TItem>());
        }

        public Expression<Func<TItem, bool>> FilterExpression<TItem>()
        {
            var sourceExpression = _getFieldExpression.Parameters[0];
            if (Filters?.Any() != true)
            {
                return Expression.Lambda<Func<TItem, bool>>(Expression.Constant(false, typeof(bool)), sourceExpression);
            }

            Expression lambda = null;
            if (this.FilterType == TableFilterType.List)
            {
                lambda = Expression.Constant(false, typeof(bool));
            }

            foreach (var filter in Filters)
            {
                if (filter.Value is JsonElement jsonElement)
                {
                    filter.Value = JsonElementHelper<TField>.GetValue(jsonElement);
                }

                if (this.FilterType == TableFilterType.List)
                {
                    lambda = Expression.OrElse(lambda!, Expression.Invoke(OnFilter, Expression.Constant(filter.Value, typeof(TField)), _getFieldExpression.Body));
                }
                else // TableFilterType.FieldType
                {
                    if (filter.Value == null
                     && filter.FilterCompareOperator is not (TableFilterCompareOperator.IsNull or TableFilterCompareOperator.IsNotNull))
                        continue;

                    Expression constantExpression = Expression.Constant(
                        filter.FilterCompareOperator is TableFilterCompareOperator.IsNull
                            or TableFilterCompareOperator.IsNotNull
                            ? null
                            : filter.Value);
                    var expression = _fieldFilterType?.GetFilterExpression(filter.FilterCompareOperator, _getFieldExpression.Body, constantExpression);
                    if (lambda == null)
                    {
                        lambda = expression;
                    }
                    else
                    {
                        if (filter.FilterCondition == TableFilterCondition.And)
                        {
                            lambda = Expression.AndAlso(lambda, expression);
                        }
                        else
                        {
                            lambda = Expression.OrElse(lambda, expression);
                        }
                    }
                }
            }

            return Expression.Lambda<Func<TItem, bool>>(lambda, sourceExpression);
        }

        void ITableFilterModel.BuildGetFieldExpression<TItem>()
        {
            if (_getFieldExpression != null)
            {
                return;
            }

            if (string.IsNullOrEmpty(FieldName))
            {
                throw new InvalidOperationException("FieldName must be set before initializing the field expression");
            }

            try
            {
                // Use PathHelper to build an expression for accessing the field
                var lambda = PathHelper.GetLambda(FieldName, typeof(TItem), typeof(TItem), typeof(TField), false);
                _getFieldExpression = lambda;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create expression for field '{FieldName}' of type '{typeof(TItem).Name}': {ex.Message}", ex);
            }
        }
    }
}
