using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace AntDesign.TableModels
{
    public class QueryModel
    {
        public int PageIndex { get; }

        public int PageSize { get; }

        public int OffsetRecords => (PageIndex - 1) * PageSize;

        public IList<ITableSortModel> SortModel { get; private set; }

        public IList<ITableFilterModel> FilterModel { get; private set; }

        public QueryModel()
        {
            this.SortModel = new List<ITableSortModel>();
            this.FilterModel = new List<ITableFilterModel>();
        }

        internal QueryModel(int pageIndex, int pageSize)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.SortModel = new List<ITableSortModel>();
            this.FilterModel = new List<ITableFilterModel>();
        }

#if NET5_0_OR_GREATER
        [JsonConstructor]
        public QueryModel(int pageIndex, int pageSize, IList<ITableSortModel> sortModel, IList<ITableFilterModel> filterModel)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.SortModel = sortModel;
            this.FilterModel = filterModel;
        }
#endif
    }

    public class QueryModel<TItem> : QueryModel
    {
        internal QueryModel(int pageIndex, int pageSize) : base(pageIndex, pageSize)
        {
        }

#if NET5_0_OR_GREATER
        [JsonConstructor]
        public QueryModel(int pageIndex, int pageSize, IList<ITableSortModel> sortModel, IList<ITableFilterModel> filterModel) : base(pageIndex, pageSize, sortModel, filterModel)
        {
        }
#endif

        internal void AddSortModel(ITableSortModel model)
        {
            SortModel.Add(model);
        }

        internal void AddFilterModel(ITableFilterModel model)
        {
            FilterModel.Add(model);
        }

        public IQueryable<TItem> ExecuteQuery(IQueryable<TItem> query)
        {
            foreach (var sort in SortModel.OrderBy(x => x.Priority))
            {
                query = sort.SortList(query);
            }

            foreach (var filter in FilterModel)
            {
                query = filter.FilterList(query);
            }

            return query;
        }

        public IQueryable<TItem> CurrentPagedRecords(IQueryable<TItem> query) => query.Skip(OffsetRecords).Take(PageSize);
    }
}
