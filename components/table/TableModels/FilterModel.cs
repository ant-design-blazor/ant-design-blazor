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

        private PropertyInfo _propertyInfo;

        private TableFilterType FilterType { get; set; } = TableFilterType.List;

        public FilterModel(PropertyInfo propertyInfo, string fieldName, Expression<Func<TField, TField, bool>> onFilter, IList<TableFilter<TField>> filters, TableFilterType filterType)
        {
            this._propertyInfo = propertyInfo;
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
            var sourceExpression = Expression.Parameter(typeof(TItem));
            var propertyExpression = Expression.Property(sourceExpression, _propertyInfo);

            Expression lambda = null;
            if (this.FilterType == TableFilterType.List)
            {
                lambda = Expression.Invoke((Expression<Func<bool>>)(() => false));
            }

            IFilterExpression filterExpression = null;
            if (FilterType == TableFilterType.FeildType)
            {
                filterExpression = _filterExpressionResolver.GetFilterExpression();
            }
            foreach (var filter in Filters)
            {
                if (filter.Value == null && (filter.FilterCompareOperator != TableFilterCompareOperator.IsNull && filter.FilterCompareOperator != TableFilterCompareOperator.IsNotNull)) continue;
                if (this.FilterType == TableFilterType.List)
                {
                    lambda = Expression.OrElse(lambda, Expression.Invoke(OnFilter, Expression.Constant(filter.Value, typeof(TField)), propertyExpression));
                }
                else
                {

                    Expression constantExpression = null;
                    if (filter.FilterCompareOperator == TableFilterCompareOperator.IsNull || filter.FilterCompareOperator == TableFilterCompareOperator.IsNotNull)
                    {
                        constantExpression = Expression.Constant(null, _propertyInfo.PropertyType);
                    }
                    else
                    {
                        constantExpression = Expression.Constant(filter.Value, typeof(TField));
                    }
                    var expression = filterExpression.GetFilterExpression(filter.FilterCompareOperator, propertyExpression, constantExpression);
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
