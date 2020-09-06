using System;
using AntDesign.Form.Locale;

namespace AntDesign.Locale
{
    public class Default : ILocale
    {
        public string Locale => "en";

        public PaginationLocale Pagination => new PaginationLocale
        {
            ItemsPerPage = ""
        };

        public IDatePickerLocale DatePicker => new
        {
        }


        public ITimePickerLocale TimePicker => throw new NotImplementedException();

        public IDatePickerLocale Calendar => throw new NotImplementedException();

        public ITableLocale Table => throw new NotImplementedException();

        public IModalLocale Modal => throw new NotImplementedException();

        public IPopconfirmLocale Popconfirm => throw new NotImplementedException();

        public ITransferLocale Transfer => throw new NotImplementedException();

        public ISelectLocale Select => throw new NotImplementedException();

        public IUploadLocale Upload => throw new NotImplementedException();

        public IEmptyLocale Empty => throw new NotImplementedException();

        public IGlobalLocale Global => throw new NotImplementedException();

        public IPageHeaderLocale PageHeader => throw new NotImplementedException();

        public IIconLocale Icon => throw new NotImplementedException();

        public ITextLocale Text => throw new NotImplementedException();

        public IFormLocale Form => throw new NotImplementedException();
    }
}
