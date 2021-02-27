using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class TableFilter<TValue>
    {
        public string Text { get; set; }

        public TValue Value { get; set; }

        public bool Selected { get; set; }

        internal void SelectValue(bool selected)
        {
            this.Selected = selected;
        }
    }
}
