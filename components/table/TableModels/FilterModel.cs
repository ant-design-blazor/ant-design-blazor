using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AntDesign.TableModels
{
    public class FilterModel<TField> : ITableFilterModel
    {
        public string Text { get; set; }

        public TField Value { get; set; }

        public bool Selected { get; set; }

        public string FieldName { get; }

        public Expression<Func<TField, bool>> OnFilter { get; set; }

        private PropertyInfo _propertyInfo;

        public FilterModel(PropertyInfo propertyInfo)
        {
            this._propertyInfo = propertyInfo;
        }

        public IQueryable<TItem> FilterList<TItem>(IQueryable<TItem> source)
        {
            var sourceExpression = Expression.Parameter(typeof(TItem));

            var propertyExpression = Expression.Property(sourceExpression, _propertyInfo);

            var lambda = Expression.Lambda<Func<TItem, bool>>(propertyExpression, sourceExpression);

            throw new NotImplementedException();
        }
    }
}
