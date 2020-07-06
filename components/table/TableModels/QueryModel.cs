using System.Collections.Generic;

namespace AntDesign.TableModels
{
    public class QueryModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public IList<ITableSortModel> SortModel { get; set; } = new List<ITableSortModel>();
    }
}
