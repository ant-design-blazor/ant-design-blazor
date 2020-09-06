using AntDesign.Form.Locale;

namespace AntDesign
{
    public interface ILocale
    {
        public string Locale { get; }

        public PaginationLocale Pagination { get; }

        public IDatePickerLocale DatePicker { get; }

        public ITimePickerLocale TimePicker { get; }

        public IDatePickerLocale Calendar { get; }

        public ITableLocale Table { get; }

        public IModalLocale Modal { get; }

        public IPopconfirmLocale Popconfirm { get; }

        public ITransferLocale Transfer { get; }

        public ISelectLocale Select { get; }

        public IUploadLocale Upload { get; }

        public IEmptyLocale Empty { get; }

        public IGlobalLocale Global { get; }

        public IPageHeaderLocale PageHeader { get; }

        public IIconLocale Icon { get; }

        public ITextLocale Text { get; }

        public IFormLocale Form { get; }
    }
}
