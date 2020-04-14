using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public enum RowSelectionType
    {
        None,
        CheckBox,
        Radio
    }

    public struct RowSelection<TData>
    {
        public const string DEFAULT_COLUMN_WIDTH = "40px";

        public RowSelectionType Type { get; set; }

        public CssSizeLength ColumnWidth { get; set; }

        public bool Fixed { get; set; }

        public Func<TData, CheckboxOption> GetCheckboxOptions { get; set; }

        public RowSelection(RowSelectionType type)
        {
            Type = type;
            ColumnWidth = DEFAULT_COLUMN_WIDTH;
            Fixed = default;
            GetCheckboxOptions = default;
        }

        public static implicit operator RowSelection<TData>(string value) => value.ToLowerInvariant() switch
        {
            "checkbox" => new RowSelection<TData>(RowSelectionType.CheckBox),
            "radio" => new RowSelection<TData>(RowSelectionType.Radio),
            _ => new RowSelection<TData>(RowSelectionType.None)
        };

        public static implicit operator RowSelection<TData>(RowSelectionType value) => new RowSelection<TData>(value);
    }
}
