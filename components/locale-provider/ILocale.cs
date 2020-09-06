﻿using AntDesign.Form.Locale;
using AntDesign.pagination.locale;
using Microsoft.Extensions.Localization;

namespace AntDesign
{
    public interface ILocale
    {
        public string Locale { get; }

        public IStringLocalizer<PaginationLocale> Pagination { get; }

        public DatePickerLocale DatePicker { get; }

        public TimePickerLocale TimePicker { get; }

        public DatePickerLocale Calendar { get; }

        public TableLocale Table { get; }

        public ModalLocale Modal { get; }

        public PopconfirmLocale Popconfirm { get; }

        public TransferLocale Transfer { get; }

        public SelectLocale Select { get; }

        public UploadLocale Upload { get; }

        public EmptyLocale Empty { get; }

        public GlobalLocale Global { get; }

        public PageHeaderLocale PageHeader { get; }

        public IconLocale Icon { get; }

        public TextLocale Text { get; }

        public FormLocale Form { get; }
    }
}
