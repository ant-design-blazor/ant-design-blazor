using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.TableModels
{
    public class SortModel<TField> : ITableSortModel
    {
        private PropertyInfo _propertyInfo;

        public SortType SortType { get; private set; }

        public int Priority { get; }

        public string FieldName { get; }

        public SortModel(PropertyInfo propertyInfo, int priority, string sort)
        {
            this._propertyInfo = propertyInfo;
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

            var sourceExpression = Expression.Parameter(typeof(TItem));

            var propertyExpression = Expression.Property(sourceExpression, _propertyInfo);

            var lambda = Expression.Lambda<Func<TItem, TField>>(propertyExpression, sourceExpression);

            if (SortType == SortType.Ascending)
            {
                return source.OrderBy(lambda);
            }
            else
            {
                return source.OrderByDescending(lambda);
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
                return SortType.Descending; ;
            }
            else
            {
                return SortType.None;
            }
        }
    }
}
