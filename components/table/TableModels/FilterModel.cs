using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.TableModels
{
    public class FilterModel<TValue> : ITableFilterModel
    {
        public string Text { get; set; }

        public TValue Value { get; set; }

        public bool Selected { get; set; }
    }
}
