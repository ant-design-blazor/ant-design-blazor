using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Table<TItem>
    {
        [Parameter]
        public bool ShowPagination { get; set; } = true;

        /// <summary>
        /// topLeft | topCenter | topRight |bottomLeft | bottomCenter | bottomRight
        /// </summary>
        [Parameter]
        public string PaginationPosition { get; set; } = "bottomRight";

        [Parameter]
        public int Total { get; set; }

        [Parameter]
        public int PageIndex { get; set; }

        private IEnumerable<TItem> ShowItems => DataSource.Skip(_current - 1 * _pageSize).Take(_pageSize);

        private int _pageSize = 10;
        private int _current = 1;
        private int _total => Total > 0 ? Total : DataSource.Count();

        private string PaginationClass => $"ant-table-pagination ant-table-pagination-{Regex.Replace(PaginationPosition, "bottom|top", "").ToLowerInvariant()}";
    }
}
