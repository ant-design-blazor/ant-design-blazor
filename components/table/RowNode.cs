using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class RowNode<TData>
    {
        public TData Data { get; set; }

        public int RowIndex { get; set; }

        public bool Selected { get; set; }
    }
}
