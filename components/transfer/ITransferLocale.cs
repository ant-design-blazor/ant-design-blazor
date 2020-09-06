using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public interface ITransferLocale
    {
        public string NotFoundContent { get; set; }
        public string SearchPlaceholder { get; }
        public string ItemUnit { get; }
        public string ItemsUnit { get; }
        public string Remove { get; }
        public string SelectCurrent { get; }
        public string RemoveCurrent { get; }
        public string SelectAll { get; }
        public string RemoveAll { get; }
        public string SelectInvert { get; }
    }
}
