using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.TableModels
{
    public class DataIndexSortModel<TField> : ITableSortModel, IComparer<TField>
    {
        private readonly PropertyInfo _propertyInfo;

        private readonly LambdaExpression _propertySelect;

        private readonly Func<TField, TField, int> _comparer;

        public SortType SortType { get; private set; }

        public int Priority { get; }

        public string FieldName { get; }

        public DataIndexSortModel(PropertyInfo propertyInfo, LambdaExpression propertySelect, int priority, string sort, Func<TField, TField, int> comparer)
        {
            this._propertyInfo = propertyInfo;
            this._propertySelect = propertySelect;
            this._comparer = comparer;
            this.Priority = priority;
            this.FieldName = propertyInfo?.Name;
            this.SortType = SortType.Parse(sort) ?? SortType.None;
        }

        IOrderedQueryable<TItem> ITableSortModel.Sort<TItem>(IQueryable<TItem> source)
        {
            if (SortType == SortType.None)
            {
                return source as IOrderedQueryable<TItem>;
            }

            var lambda = (Expression<Func<TItem, TField>>)_propertySelect;

            if (SortType == SortType.Ascending)
            {
                return _comparer == null ? source.OrderBy(lambda) : source.OrderBy(lambda, this);
            }
            else
            {
                return _comparer == null ? source.OrderByDescending(lambda) : source.OrderByDescending(lambda, this);
            }
        }

        void ITableSortModel.SwitchSortType()
        {
            SortType = GetNextType();
        }

        void ITableSortModel.SetSortType(SortType sortType)
        {
            this.SortType = sortType;
        }

        void ITableSortModel.SetSortType(string sortType)
        {
            this.SortType = SortType.Parse(sortType);
        }

        SortType ITableSortModel.NextType()
        {
            return GetNextType();
        }

        private SortType GetNextType()
        {
            if (SortType == SortType.None)
            {
                return SortType.Ascending;
            }
            else if (SortType == SortType.Ascending)
            {
                return SortType.Descending;
            }
            else
            {
                return SortType.None;
            }
        }

        /// <inheritdoc />
        public int Compare(TField x, TField y)
        {
            return _comparer?.Invoke(x, y) ?? 0;
        }
    }
}
