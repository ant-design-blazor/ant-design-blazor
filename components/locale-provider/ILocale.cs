using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public interface ILocale
    {
        public string Locale { get; set; }

        public IPaginationLocale Pagination { get; set; }

        public IDateLocale DatePicker { get; set; }

        public IDateLocale TimePicker { get; set; }

        public IDateLocale Calendar { get; set; }

        public ITableLocale Table { get; set; }

        public IModalLocale Modal { get; set; }

        public IPopconfirmLocale Popconfirm { get; set; }

        public ITransferLocale Transfer { get; set; }

        public ISelectLocale Select { get; set; }

        public IUploadLocale Upload { get; set; }

        public IEmptyLocale Empty { get; set; }

        public object Global { get; set; }

        public IPageHeaderLocale PageHeader { get; set; }

        public IIconLocale Icon { get; set; }

        public ITextLocale Text { get; set; }

        public IFormLocale Form { get; set; }
    }
}
