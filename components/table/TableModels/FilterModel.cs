using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AntDesign.TableModels
{
    public class FilterModel<TField> : ITableFilterModel
    {
        public string Text { get; set; }

        public TField Value { get; set; }

        public string FieldName { get; }

        public Expression<Func<TField, TField, bool>> OnFilter { get; set; }

        public bool Selected { get; set; }

        private PropertyInfo _propertyInfo;

        public FilterModel(PropertyInfo propertyInfo, string text, TField value, Expression<Func<TField, TField, bool>> onFilter, bool selected)
        {
            this._propertyInfo = propertyInfo;
            this.Text = text;
            this.Value = value;
            this.OnFilter = onFilter;
            this.Selected = selected;
        }

        public IQueryable<TItem> FilterList<TItem>(IQueryable<TItem> source)
        {
            var sourceExpression = Expression.Parameter(typeof(TItem));
            var propertyExpression = Expression.Property(sourceExpression, _propertyInfo);

            var invocationExpression = Expression.Invoke(OnFilter, Expression.Constant(Value), propertyExpression);
            var lambda = Expression.Lambda<Func<TItem, bool>>(invocationExpression, sourceExpression);

            return source.Where(lambda);
        }
    }
}
