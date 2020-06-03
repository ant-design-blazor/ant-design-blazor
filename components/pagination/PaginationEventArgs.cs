using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class PaginationEventArgs
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int Total { get; set; }
    }
}
