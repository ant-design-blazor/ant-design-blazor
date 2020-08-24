using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public interface ILocale
    {
        public string locale { get; set; }
        public IPaginationLocale Pagination { get; set; }
        public object DatePicker { get; set; }
        public object TimePicker { get; set; }
        public object Calendar { get; set; }
        public object Table { get; set; }
        public object Modal { get; set; }
        public object Popconfirm { get; set; }
        public object Transfer { get; set; }
        public object Select { get; set; }
        public object Upload { get; set; }
        public object Empty { get; set; }
        public object global { get; set; }
        public object PageHeader { get; set; }
        public object Icon { get; set; }
        public object Text { get; set; }
        public object Form { get; set; }
    }
}
