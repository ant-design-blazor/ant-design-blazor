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

        public PaginationLocale Pagination { get; set; } = new();

        public DatePickerLocale DatePicker { get; set; } = new();

        public TimePickerLocale TimePicker { get; set; } = new();

        public DatePickerLocale Calendar { get; set; } = new();

        public TableLocale Table { get; set; } = new();

        public ModalLocale Modal { get; set; } = new();

        public ConfirmLocale Confirm { get; set; } = new();

        public PopconfirmLocale Popconfirm { get; set; } = new();

        public TransferLocale Transfer { get; set; } = new();

        public SelectLocale Select { get; set; } = new();

        public UploadLocale Upload { get; set; } = new();

        public GlobalLocale Global { get; set; } = new();

        public PageHeaderLocale PageHeader { get; set; } = new();

        public EmptyLocale Empty { get; set; } = new();

        public IconLocale Icon { get; set; } = new();

        public TextLocale Text { get; set; } = new();

        public FormLocale Form { get; set; } = new();

        public ImageLocale Image { get; set; } = new();

        public ReuseTabsLocale ReuseTabs { get; set; } = new();
    }
}
