using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.TableModels
{
    public class SortModel<TField> : ITableSortModel, IComparer<TField>
    {
        private PropertyInfo _propertyInfo;

        private readonly Func<TField, TField, int> _comparer;

        public int Priority { get; }

        public string FieldName { get; }

        public string Sort => _sortDirection.Name;

        SortDirection ITableSortModel.SortDirection => _sortDirection;

        private SortDirection _sortDirection;

        public SortModel(PropertyInfo propertyInfo, int priority, SortDirection defaultSortOrder, Func<TField, TField, int> comparer)
        {
            this._propertyInfo = propertyInfo;
            _comparer = comparer;
            this.Priority = priority;
            this.FieldName = propertyInfo?.Name;
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

            var sourceExpression = Expression.Parameter(typeof(TItem));

            var propertyExpression = Expression.Property(sourceExpression, _propertyInfo);

            var lambda = Expression.Lambda<Func<TItem, TField>>(propertyExpression, sourceExpression);

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
