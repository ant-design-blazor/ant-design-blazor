using System;
using System.Collections.Generic;
using System.Text;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public interface IColumn
    {
        public ITable Table { get; set; }

        public int ColIndex { get; set; }

        public string Fixed { get; set; }

        public string Title { get; set; }

        public string Width { get; set; }

        //public RowData RowData { get; set; }

        public int ColSpan { get; set; }

        public int RowSpan { get; set; }

        public int HeaderColSpan { get; set; }
    }
}
