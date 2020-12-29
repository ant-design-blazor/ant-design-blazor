using System.Globalization;
using System.Text.Json.Serialization;
using AntDesign.Form.Locale;

namespace AntDesign.Locales
{
    public class Locale
    {
        [JsonPropertyName("locale")]
        public string LocaleName { get; set; }

        public CultureInfo CurrentCulture => new CultureInfo(LocaleName);

        public PaginationLocale Pagination { get; set; }

        public DatePickerLocale DatePicker { get; set; }

        public TimePickerLocale TimePicker { get; set; }

        public DatePickerLocale Calendar { get; set; }

        public TableLocale Table { get; set; }

        public ModalLocale Modal { get; set; }

        public ConfirmLocale Confirm { get; set; }

        public PopconfirmLocale Popconfirm { get; set; }

        public TransferLocale Transfer { get; set; }

        public SelectLocale Select { get; set; }

        public UploadLocale Upload { get; set; }

        public GlobalLocale Global { get; set; }

        public PageHeaderLocale PageHeader { get; set; }

        public EmptyLocale Empty { get; set; }

        public IconLocale Icon { get; set; }

        public TextLocale Text { get; set; }

        public FormLocale Form { get; set; }
    }
}
