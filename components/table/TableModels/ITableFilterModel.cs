using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntDesign.TableModels
{
    public interface ITableFilterModel
    {
        public string FieldName { get; }

        public IEnumerable<string> SelectedValues { get; }

        public IQueryable<TItem> FilterList<TItem>(IQueryable<TItem> source);
    }
}
