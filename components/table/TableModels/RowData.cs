using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.TableModels
{
    public class RowData<TItem>
    {
        public int RowIndex { get; set; }

        public int PageIndex { get; set; }

        public bool Selected { get; set; }

        public bool Expanded { get; set; }

        public TItem Data { get; set; }

        public RowData(int rowIndex, int pageIndex, TItem data)
        {
            this.RowIndex = rowIndex;
            this.PageIndex = pageIndex;
            this.Data = data;
        }
    }
}
