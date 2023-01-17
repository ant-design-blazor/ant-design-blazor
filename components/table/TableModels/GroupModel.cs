using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace AntDesign.TableModels
{
    public class GroupModel<TField> : ITableGroupModel
    {
        public int Priority { get; }

        public string FieldName { get; }

        public int ColumnIndex { get; set; }

        private LambdaExpression _getFieldExpression;

#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public GroupModel(int columnIndex, int priority, string fieldName, LambdaExpression getFieldExpression = null)
        {
            this.ColumnIndex = columnIndex;
            this.Priority = priority;
            this.FieldName = fieldName;
            this._getFieldExpression = getFieldExpression;
        }

        public IQueryable<IEnumerable<TItem>> GroupList<TItem>(IQueryable<TItem> source)
        {
            var lambda = (Expression<Func<TItem, TField>>)_getFieldExpression;
            var result = source.GroupBy(lambda);
            return result;
        }
    }
}
