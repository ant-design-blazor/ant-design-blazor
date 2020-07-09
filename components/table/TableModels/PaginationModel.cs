using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.TableModels
{
    public class PaginationModel
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int Total { get; set; }
    }
}
