using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntDesign.Filters;

namespace AntDesign.TableModels
{
    public interface ITableFilterModel
    {
        public string FieldName { get; }

        public int ColumnIndex { get; }

        public IEnumerable<string> SelectedValues { get; }

        public IList<TableFilter> Filters { get; }

        public IQueryable<TItem> FilterList<TItem>(IQueryable<TItem> source);
    }
}
