// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign.FilterExpression;

namespace AntDesign.TableModels
{
    public class FilterModel<TField> : ITableFilterModel
    {
        public string FieldName { get; }

        public IEnumerable<string> SelectedValues { get; set; }

        public IList<TableFilter<TField>> Filters { get; }

        public Expression<Func<TField, TField, bool>> OnFilter { get; set; }

        private readonly FilterExpressionResolver<TField> _filterExpressionResolver = new FilterExpressionResolver<TField>();

        private LambdaExpression _getFieldExpression;

        private TableFilterType FilterType { get; set; } = TableFilterType.List;

        public FilterModel(LambdaExpression getFieldExpression, string fieldName, Expression<Func<TField, TField, bool>> onFilter, IList<TableFilter<TField>> filters, TableFilterType filterType)
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
        }

        public IQueryable<TItem> FilterList<TItem>(IQueryable<TItem> source)
        {
            if (Filters?.Any() != true)
            {
                return source;
            }

            var sourceExpression = _getFieldExpression.Parameters[0];

            Expression lambda = null;
            if (this.FilterType == TableFilterType.List)
            {
                lambda = Expression.Constant(false, typeof(bool));
            }

            IFilterExpression filterExpression = null;
            if (FilterType == TableFilterType.FieldType)
            {
                filterExpression = _filterExpressionResolver.GetFilterExpression();
            }
            foreach (var filter in Filters)
            {
                if (filter.Value == null && (filter.FilterCompareOperator != TableFilterCompareOperator.IsNull && filter.FilterCompareOperator != TableFilterCompareOperator.IsNotNull)) continue;
                if (this.FilterType == TableFilterType.List)
                {
                    lambda = Expression.OrElse(lambda!, Expression.Invoke(OnFilter, Expression.Constant(filter.Value, typeof(TField)), _getFieldExpression.Body));
                }
                else // TableFilterType.FieldType
                {
                    Expression constantExpression = null;
                    if (filter.FilterCompareOperator == TableFilterCompareOperator.IsNull || filter.FilterCompareOperator == TableFilterCompareOperator.IsNotNull)
                    {
                        constantExpression = Expression.Constant(null, typeof(TField));
                    }
                    else
                    {
                        constantExpression = Expression.Constant(filter.Value, typeof(TField));
                    }
                    var expression = filterExpression!.GetFilterExpression(filter.FilterCompareOperator, _getFieldExpression.Body, constantExpression);
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
            if (lambda == null)
            {
                return source;
            }
            else
            {
                return source.Where(Expression.Lambda<Func<TItem, bool>>(lambda, sourceExpression));
            }

        }


    }
}
