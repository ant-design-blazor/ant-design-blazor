﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace AntDesign.TableModels
{
    public class SortModel<TField> : ITableSortModel, IComparer<TField>, ICloneable
    {
        public int Priority { get; }

        public string FieldName { get; }

        public string Sort => _sortDirection?.Name;

        SortDirection ITableSortModel.SortDirection => _sortDirection;

        public int ColumnIndex => _columnIndex;

        private readonly Func<TField, TField, int> _comparer;

        private SortDirection _sortDirection;

        private LambdaExpression _getFieldExpression;

        private int _columnIndex;


        public SortModel(IFieldColumn column, LambdaExpression getFieldExpression, string fieldName, int priority, SortDirection defaultSortOrder, Func<TField, TField, int> comparer)
        {
            this.Priority = priority;
            this._columnIndex = column.ColIndex;
            this._getFieldExpression = getFieldExpression;
            this.FieldName = fieldName;
            this._comparer = comparer;
            this._sortDirection = defaultSortOrder ?? SortDirection.None;
        }

#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public SortModel(int columnIndex, int priority, string fieldName, string sort)
        {
            this.Priority = priority;
            this._columnIndex = columnIndex;
            this.FieldName = fieldName;
            this._sortDirection = SortDirection.Parse(sort);
        }

        void ITableSortModel.SetSortDirection(SortDirection sortDirection)
        {
            _sortDirection = sortDirection;
        }

        IQueryable<TItem> ITableSortModel.SortList<TItem>(IQueryable<TItem> source)
        {
            if (_sortDirection == SortDirection.None)
            {
                return source;
            }

            var lambda = (Expression<Func<TItem, TField>>)_getFieldExpression;

            if (source.Expression.Type == typeof(IOrderedQueryable<TItem>))
            {
                var orderedSource = source as IOrderedQueryable<TItem>;
                if (_sortDirection == SortDirection.Ascending)
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
                if (_sortDirection == SortDirection.Ascending)
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
            return new SortModel<TField>(_columnIndex, Priority, FieldName, Sort);
        }
    }
}
