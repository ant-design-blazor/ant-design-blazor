using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public partial class PaginationDefault : AntDomComponentBase
    {
        [Parameter]
        public string Size { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool ShowSizeChanger { get; set; }

        [Parameter]
        public bool ShowQuickJumper { get; set; }

        [Parameter]
        public int Total { get; set; } = 0;

        [Parameter]
        public int PageIndex { get; set; } = 1;

        [Parameter]
        public int PageSize { get; set; } = 10;

        [Parameter]
        public int[] PageSizeOptions { get; set; } = { 10, 20, 30, 40 };

        [Parameter]
        public EventCallback<int> PageIndexChange { get; set; }

        [Parameter]
        public EventCallback<int> PageSizeChange { get; set; }

        [CascadingParameter]
        public Pagination Pagination { get; set; }

        private OneOf<Func<PaginationTotalContext, string>, RenderFragment<PaginationTotalContext>> _showTotal;

        private (int, int) _ranges = (0, 0);

        private IEnumerable<PaginationItem> _listOfPageItem = Enumerable.Empty<PaginationItem>();

        private PaginationTotalContext TotalContext => new PaginationTotalContext { Total = Total, Range = _ranges };

        protected override void OnInitialized()
        {
            if (Pagination != null)
            {
                this._showTotal = Pagination.ShowTotal;
            }

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            this._ranges = ((this.PageIndex - 1) * this.PageSize + 1, Math.Min(this.PageIndex * this.PageSize, this.Total));
            this.BuildIndexes();

            base.OnParametersSet();
        }

        private void JumpPage(int index)
        {
            this.OnPageIndexChange(index);
        }

        private void JumpDiff(int diff)
        {
            this.JumpPage(this.PageIndex + diff);
        }

        private void OnPageIndexChange(int index)
        {
            this.PageIndexChange.InvokeAsync(index);
        }

        private void OnPageSizeChange(int size)
        {
            this.PageSizeChange.InvokeAsync(size);
        }

        private void BuildIndexes()
        {
            var lastIndex = this.GetLastIndex(this.Total, this.PageSize);
            this._listOfPageItem = this.GetListOfPageItem(this.PageIndex, lastIndex);
        }

        private int GetLastIndex(int total, int pageSize)
        {
            return (total - 1) / pageSize + 1;
        }

        private IEnumerable<PaginationItem> GetListOfPageItem(int pageIndex, int lastIndex)
        {
            IEnumerable<PaginationItem> ConcatWithPrevNext(IEnumerable<PaginationItem> listOfPage)
            {
                var prevItem = new PaginationItem() { Type = "prev", Disabled = pageIndex == 1 };
                var nextItem = new PaginationItem() { Type = "next", Disabled = pageIndex == lastIndex };
                return new[] { prevItem }.Concat(listOfPage).Concat(new[] { nextItem });
            }

            IEnumerable<PaginationItem> GeneratePage(int start, int end)
            {
                return Enumerable.Range(start, end - start + 1).Select(x => new PaginationItem() { Index = x, Type = "page" });
            }

            if (lastIndex <= 9)
            {
                return ConcatWithPrevNext(GeneratePage(1, lastIndex));
            }
            else
            {
                IEnumerable<PaginationItem> GenerateRangeItem(int selected, int last)
                {
                    var listOfRange = Enumerable.Empty<PaginationItem>();
                    var prevFiveItem = new[] { new PaginationItem() { Type = "prev_5" } };
                    var nextFiveItem = new[] { new PaginationItem() { Type = "next_5" } };
                    var firstPageItem = GeneratePage(1, 1);
                    var lastPageItem = GeneratePage(lastIndex, lastIndex);
                    if (selected < 4)
                    {
                        listOfRange = GeneratePage(2, 5).Concat(nextFiveItem);
                    }
                    else if (selected < last - 3)
                    {
                        listOfRange = prevFiveItem.Concat(GeneratePage(selected - 2, selected + 2))
                            .Concat(nextFiveItem);
                    }
                    else
                    {
                        listOfRange = prevFiveItem.Concat(GeneratePage(last - 4, last - 1));
                    }

                    return firstPageItem.Concat(listOfRange).Concat(lastPageItem);
                }

                return ConcatWithPrevNext(GenerateRangeItem(pageIndex, lastIndex));
            }
        }
    }
}
