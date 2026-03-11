// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using AntDesign.Core.Helpers.MemberPath;

namespace AntDesign.TableModels
{
    public class SortModel<TField> : ITableSortModel, IComparer<TField>, ICloneable
    {
        public int Priority { get; }

        public string FieldName { get; }

        [Obsolete("Use SortDirection instead")]
        public string Sort => SortDirection.ToString();

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SortDirection SortDirection { get; set; }

        public int ColumnIndex => _columnIndex;

        private Func<TField, TField, int> _comparer;

        private LambdaExpression _getFieldExpression;

        private int _columnIndex;

        public SortModel(IFieldColumn column, LambdaExpression getFieldExpression, string fieldName, int priority, SortDirection defaultSortOrder, Func<TField, TField, int> comparer)
        {
            this.Priority = priority;
            this._columnIndex = column.ColIndex;
            this._getFieldExpression = getFieldExpression;
            this.FieldName = fieldName;
            this._comparer = comparer;
            this.SortDirection = defaultSortOrder;
        }

#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public SortModel(int columnIndex, int priority, string fieldName, SortDirection sortDirection)
        {
            this.Priority = priority;
            this._columnIndex = columnIndex;
            this.FieldName = fieldName;
            this.SortDirection = sortDirection;
        }

        void ITableSortModel.SetSortDirection(SortDirection sortDirection)
        {
            SortDirection = sortDirection;
        }

        IQueryable<TItem> ITableSortModel.SortList<TItem>(IQueryable<TItem> source)
        {
            if (SortDirection == SortDirection.None)
            {
                return source;
            }

            var lambda = (Expression<Func<TItem, TField>>)_getFieldExpression;

            if (source.Expression.Type == typeof(IOrderedQueryable<TItem>))
            {
                var orderedSource = source as IOrderedQueryable<TItem>;
                if (SortDirection == SortDirection.Ascending)
                {
                    return _comparer == null ? orderedSource.ThenBy(lambda) : orderedSource.ThenBy(lambda, this);
                }
                else
                {
                    return _comparer == null ? orderedSource.ThenByDescending(lambda) : orderedSource.ThenByDescending(lambda, this);
                }
            }
            else
            {
                if (SortDirection == SortDirection.Ascending)
                {
                    return _comparer == null ? source.OrderBy(lambda) : source.OrderBy(lambda, this);
                }
                else
                {
                    return _comparer == null ? source.OrderByDescending(lambda) : source.OrderByDescending(lambda, this);
                }
            }
        }

        /// <inheritdoc />
        public int Compare(TField x, TField y)
        {
            return _comparer?.Invoke(x, y) ?? 0;
        }

        public object Clone()
        {
            return new SortModel<TField>(_columnIndex, Priority, FieldName, SortDirection)
            {
                _getFieldExpression = this._getFieldExpression, // keep the expression instance for sorting rows outside
                _comparer = this._comparer,
            };
        }

        void ITableSortModel.BuildGetFieldExpression<TItem>()
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
