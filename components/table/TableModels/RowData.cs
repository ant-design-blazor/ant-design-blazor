using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.TableModels
{
    public class RowData<TItem>
    {
        public int RowIndex { get; set; }

        public bool Selected { get; set; }

        public TItem Data { get; set; }

        public RowData(int rowIndex, TItem data)
        {
            this.RowIndex = rowIndex;
            this.Data = data;
        }
    }
}
