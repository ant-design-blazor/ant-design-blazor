using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AntDesign.TableModels
{
    public class FilterModel<TField> : ITableFilterModel
    {
        public string FieldName { get; }

        public IEnumerable<string> SelectedValues { get; set; }

        public IList<TableFilter<TField>> Filters { get; }

        public Expression<Func<TField, TField, bool>> OnFilter { get; set; }

        public LambdaExpression GetFieldExpression { get; set; }

        public FilterModel(LambdaExpression getFieldExpression, Expression<Func<TField, TField, bool>> onFilter, IList<TableFilter<TField>> filters)
        {
            this.GetFieldExpression = getFieldExpression;
            this.FieldName = GetFieldExpression.ReturnType.Name;
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

            var sourceExpression = GetFieldExpression.Parameters[0];

            Expression invocationExpression = Expression.Constant(false, typeof(bool));

            foreach (var filter in Filters)
            {
                invocationExpression = Expression.OrElse(invocationExpression, Expression.Invoke(OnFilter, Expression.Constant(filter.Value), GetFieldExpression.Body));
            }

            var lambda = Expression.Lambda<Func<TItem, bool>>(invocationExpression, sourceExpression);

            return source.Where(lambda);
        }
    }
}
