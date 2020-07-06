using System;
using System.Linq;
using System.Linq.Expressions;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.TableModels
{
    public class SortModel<TField> : ITableSortModel
    {
        private readonly FieldIdentifier _fieldIdentifier;

        public SortType SortType { get; private set; }

        public int Priority { get; }

        public string FieldName { get; }

        public SortModel(FieldIdentifier fieldIdentifier, int priority, string sort)
        {
            this._fieldIdentifier = fieldIdentifier;
            this.FieldName = fieldIdentifier.FieldName;
            this.Priority = priority;
            this.SortType = SortType.Parse(sort) ?? SortType.None;
        }

        IOrderedQueryable<TItem> ITableSortModel.Sort<TItem>(IQueryable<TItem> source)
        {
            if (SortType == SortType.None)
            {
                return source as IOrderedQueryable<TItem>;
            }

            var sourceExpression = Expression.Parameter(typeof(TItem));
            if (_fieldIdentifier.TryGetValidateProperty(out var property))
            {
                var propertyExpression = Expression.Property(sourceExpression, property);

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

            return source as IOrderedQueryable<TItem>;
        }

        void ITableSortModel.SwitchSortType()
        {
            if (SortType == SortType.None)
            {
                SortType = SortType.Ascending;
            }
            else if (SortType == SortType.Ascending)
            {
                SortType = SortType.Descending; ;
            }
            else
            {
                SortType = SortType.None;
            }
        }

        void ITableSortModel.SetSortType(SortType sortType)
        {
            this.SortType = SortType;
        }

        void ITableSortModel.SetSortType(string sortType)
        {
            this.SortType = SortType.Parse(sortType);
        }
    }
}
