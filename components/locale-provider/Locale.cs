// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Text.Json.Serialization;
using AntDesign.Form.Locale;

namespace AntDesign.Locales
{
    public record Locale
    {
        private CultureInfo _currentCulture;

        internal void SetCultureInfo(string cultureName)
        {
            LocaleName = cultureName;
            _currentCulture = new(cultureName);
            this.DatePicker.GetCultureInfo = () => _currentCulture;
            this.DatePicker.Lang.GetCultureInfo = () => _currentCulture;
        }

        [JsonPropertyName("locale")]
        public string LocaleName { get; private set; }

        public CultureInfo CurrentCulture => _currentCulture;

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

        public DraftLocale Draft { get; set; } = new();
    }
}
