using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AntDesign.TableModels
{
    public class DataIndexSortModel<TField> : ITableSortModel, IComparer<TField>
    {
        private readonly LambdaExpression _propertySelect;

        private readonly Func<TField, TField, int> _comparer;

        public int Priority { get; }

        public string FieldName { get; }

        public string Sort => _sortDirection?.Name;

        SortDirection ITableSortModel.SortDirection => _sortDirection;

        private SortDirection _sortDirection;

        public DataIndexSortModel(string dataIndex, LambdaExpression propertySelect, int priority, SortDirection sortDirection, Func<TField, TField, int> comparer)
        {
            this.FieldName = dataIndex;
            this._propertySelect = propertySelect;
            this._comparer = comparer;
            this.Priority = priority;
            this._sortDirection = sortDirection ?? SortDirection.None;
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

            var lambda = (Expression<Func<TItem, TField>>)_propertySelect;

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
