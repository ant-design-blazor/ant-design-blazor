using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AntDesign.TableModels
{
    public class QueryModel<TItem>
    {
        public int PageIndex { get; }

        public int PageSize { get; }

        public IList<ITableSortModel> SortModel { get; private set; }

        [JsonIgnore] public IQueryable<TItem> QueryableLambda { get; private set; }

        internal QueryModel(int pageIndex, int pageSize)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }

        internal void AddSortModel(ITableSortModel model)
        {
            SortModel ??= new List<ITableSortModel>();
            SortModel.Add(model);
        }

        internal void SetQueryableLambda(IQueryable<TItem> query)
        {
            this.QueryableLambda = query;
        }
    }
}
