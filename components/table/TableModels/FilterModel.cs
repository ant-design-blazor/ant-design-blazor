using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.TableModels
{
    public class FilterModel<TField> : ITableFilterModel
    {
        public string FieldName { get; }

        public IEnumerable<string> SelectedValues { get; set; }

        public IList<TableFilter<TField>> Filters { get; }

        public Expression<Func<TField, TField, bool>> OnFilter { get; set; }

        private PropertyInfo _propertyInfo;

        public FilterModel(PropertyInfo propertyInfo, Expression<Func<TField, TField, bool>> onFilter, IList<TableFilter<TField>> filters)
        {
            this._propertyInfo = propertyInfo;
            this.FieldName = _propertyInfo.Name;
            this.OnFilter = onFilter;
            this.SelectedValues = filters.Select(x => x.Value.ToString());
            this.Filters = filters;
        }

        public IQueryable<TItem> FilterList<TItem>(IQueryable<TItem> source)
        {
            if (Filters?.Any() != true)
            {
                return source;
            }

            var sourceExpression = Expression.Parameter(typeof(TItem));
            var propertyExpression = Expression.Property(sourceExpression, _propertyInfo);

            Expression invocationExpression = Expression.Invoke((Expression<Func<bool>>)(() => false));

            foreach (var filter in Filters)
            {
                invocationExpression = Expression.OrElse(invocationExpression, Expression.Invoke(OnFilter, Expression.Constant(filter.Value), propertyExpression));
            }

            var lambda = Expression.Lambda<Func<TItem, bool>>(invocationExpression, sourceExpression);

            return source.Where(lambda);
        }
    }
}
