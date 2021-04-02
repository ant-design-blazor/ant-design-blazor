using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using AntDesign.Internal;

namespace AntDesign.TableModels
{
    public class SortModel<TField> : ITableSortModel, IComparer<TField>
    {
        public int Priority { get; }

        public string FieldName { get; }

        public string Sort => _sortDirection?.Name;

        SortDirection ITableSortModel.SortDirection => _sortDirection;

        private readonly Func<TField, TField, int> _comparer;

        private SortDirection _sortDirection;

        private LambdaExpression _getFieldExpression;

        public SortModel(LambdaExpression getFieldExpression, int priority, SortDirection defaultSortOrder, Func<TField, TField, int> comparer)
        {
            this.Priority = priority;
            this._getFieldExpression = getFieldExpression;
            var member = ColumnExpressionHelper.GetReturnMemberInfo(_getFieldExpression);
            this.FieldName = member.GetCustomAttribute<DisplayAttribute>(true)?.Name ?? member.Name;
            this._comparer = comparer;
            this._sortDirection = defaultSortOrder ?? SortDirection.None;
        }

        void ITableSortModel.SetSortDirection(SortDirection sortDirection)
        {
            _sortDirection = sortDirection;
        }

        IOrderedQueryable<TItem> ITableSortModel.SortList<TItem>(IQueryable<TItem> source)
        {
            if (_sortDirection == SortDirection.None)
            {
                return source as IOrderedQueryable<TItem>;
            }

            var lambda = (Expression<Func<TItem, TField>>)_getFieldExpression;

            if (_sortDirection == SortDirection.Ascending)
            {
                return _comparer == null ? source.OrderBy(lambda) : source.OrderBy(lambda, this);
            }
            else
            {
                return _comparer == null ? source.OrderByDescending(lambda) : source.OrderByDescending(lambda, this);
            }
        }

        /// <inheritdoc />
        public int Compare(TField x, TField y)
        {
            return _comparer?.Invoke(x, y) ?? 0;
        }
    }
}
