using Microsoft.AspNetCore.Components;
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

    public sealed class RowSelection<TData>
    {
        public const string DEFAULTCOLUMNWIDTH = "40px";

        public RowSelectionType Type { get; set; }

        public CssSizeLength ColumnWidth { get; set; } = DEFAULTCOLUMNWIDTH;

        public bool Fixed { get; set; } = false;

        public Func<TData, CheckboxOption> GetCheckboxOptions { get; set; } = _ => new CheckboxOption();

        public bool HideDefaultSelections { get; set; } = false;

        public Func<bool, TData, int, RenderFragment, RenderFragment> RenderCell { get; set; } = (c, d, i, o) => o;

        public IList<TData> SelectedRows { get; } = new List<TData>();

        public IList<(RenderFragment text, Func<TData> onSelect)> Selections { get; set; } = new List<(RenderFragment, Func<TData>)>();

        public RowSelection(RowSelectionType type)
        {
            Type = type;
        }

        public static implicit operator RowSelection<TData>(string value) => value?.ToUpperInvariant() switch
        {
            "CHECKBOX" => new RowSelection<TData>(RowSelectionType.CheckBox),
            "RADIO" => new RowSelection<TData>(RowSelectionType.Radio),
            _ => new RowSelection<TData>(RowSelectionType.None)
        };

        public static implicit operator RowSelection<TData>(RowSelectionType value) => new RowSelection<TData>(value);
    }
}
