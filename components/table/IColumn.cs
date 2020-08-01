using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public interface IColumn
    {
        public ITable Table { get; set; }

        public bool IsHeader { get; set; }

        public bool IsPlaceholder { get; set; }

        public bool IsColGroup { get; set; }

        public int ColIndex { get; set; }

        public int RowIndex { get; }

        public string Fixed { get; set; }

        public int CacheKey { get; set; }

        public string Title { get; set; }

        public int Width { get; set; }
    }
}
