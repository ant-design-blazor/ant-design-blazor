using System;
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

        public IList<ITableGroupModel> GroupModel { get; private set; }

        public QueryModel()
        {
            this.SortModel = new List<ITableSortModel>();
            this.FilterModel = new List<ITableFilterModel>();
            this.GroupModel = new List<ITableGroupModel>();
        }

        internal QueryModel(int pageIndex, int pageSize)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.SortModel = new List<ITableSortModel>();
            this.FilterModel = new List<ITableFilterModel>();
            this.GroupModel = new List<ITableGroupModel>();
        }

#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public QueryModel(int pageIndex, int pageSize, IList<ITableSortModel> sortModel, IList<ITableFilterModel> filterModel, IList<ITableGroupModel> groupModel)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.SortModel = sortModel;
            this.FilterModel = filterModel;
            this.GroupModel = groupModel;
        }
    }

    public class QueryModel<TItem> : QueryModel, ICloneable
    {
        internal QueryModel(int pageIndex, int pageSize) : base(pageIndex, pageSize)
        {
        }

#if NET5_0_OR_GREATER
        [JsonConstructor]
#endif
        public QueryModel(int pageIndex, int pageSize, IList<ITableSortModel> sortModel, IList<ITableFilterModel> filterModel, IList<ITableGroupModel> groupModel) :
            base(pageIndex, pageSize, sortModel, filterModel, groupModel)
        {
        }

        internal void AddSortModel(ITableSortModel model)
        {
            SortModel.Add(model);
        }

        internal void AddFilterModel(ITableFilterModel model)
        {
            FilterModel.Add(model);
        }

        internal void AddGroupModel(ITableGroupModel model)
        {
            GroupModel.Add(model);
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

        public IQueryable<GroupData<TItem>> ExecuteGroup(IQueryable<TItem> query)
        {
            var firstGroup = GroupModel.OrderBy(x => x.Priority).FirstOrDefault();
            if (firstGroup is null) return null;

            var firstGroupResult = firstGroup.GroupList(query).ToArray();
            var currentLayerGroups = firstGroupResult.AsQueryable();
            foreach (var groupModel in GroupModel.OrderBy(x => x.Priority).Skip(1))
            {
                var nextLayerGroups = new List<GroupData<TItem>>();
                foreach (var currentGroup in currentLayerGroups)
                {
                    var subGroups = groupModel.GroupList(currentGroup.Items.AsQueryable()).ToArray();
                    currentGroup.Children = subGroups;
                    foreach (var subGroup in currentGroup.Children)
                    {
                        subGroup.Parent = currentGroup;
                    }
                    nextLayerGroups.AddRange(subGroups);
                }
                currentLayerGroups = nextLayerGroups.AsQueryable();
            }

            return firstGroupResult.AsQueryable();
        }

        public IQueryable<IEnumerable<TItem>> ExecuteGroup(IQueryable<TItem> query)
        {
            var firstGroup = GroupModel.OrderBy(x => x.Priority).FirstOrDefault();
            _ = firstGroup ?? throw new InvalidOperationException();

            var firstGroupResult = firstGroup.GroupList(query).ToArray();
            var currentLayerGroups = firstGroupResult.AsQueryable();
            foreach (var groupModel in GroupModel.OrderBy(x => x.Priority).Skip(1))
            {
                var nextLayerGroups = new List<GroupData<TItem>>();
                foreach (var currentGroup in currentLayerGroups)
                {
                    var subGroups = groupModel.GroupList(currentGroup.Items.AsQueryable()).ToArray();
                    currentGroup.Children = subGroups;
                    foreach (var subGroup in currentGroup.Children)
                    {
                        subGroup.Parent = currentGroup;
                    }
                    nextLayerGroups.AddRange(subGroups);
                }
                currentLayerGroups = nextLayerGroups.AsQueryable();
            }

            return firstGroupResult.AsQueryable();
        }

        public IQueryable<TItem> CurrentPagedRecords(IQueryable<TItem> query) => query.Skip(OffsetRecords).Take(PageSize);

        public IQueryable<GroupData<TItem>> CurrentPagedRecords(IQueryable<GroupData<TItem>> query) => query.Skip(OffsetRecords).Take(PageSize);

        public object Clone()
        {
            var sorters = this.SortModel.Select(x => x.Clone() as ITableSortModel).ToList();
            var filters = this.FilterModel.ToList();
            var groups = this.GroupModel.ToList();
            return new QueryModel<TItem>(PageIndex, PageSize, sorters, filters, groups);
        }
    }
}
