using System.Collections.Generic;
using System.Linq;
using AntDesign.Internal;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class Selection : ColumnBase, ISelectionColumn
    {
        [Parameter] public string Type { get; set; } = "checkbox";

        [Parameter] public bool Disabled { get; set; }

        [Parameter] public string Key { get; set; }

        [Parameter] public bool CheckStrictly { get; set; }

        [Parameter]
        public virtual RenderFragment<CellData> CellRender { get; set; }

        //private bool _checked;

        private bool Indeterminate => IsHeader
                                   && Table.AnySelected
                                   && !Table.AllSelected;

        public IList<ISelectionColumn> RowSelections { get; set; } = new List<ISelectionColumn>();

        //private int[] _selectedIndexes;

        private bool IsHeaderDisabled => RowSelections.Any() && RowSelections.All(x => x.Disabled);

        public bool Selected => DataItem.Selected;

        private bool? _selected;

        private void OnCheckedChange(bool selected)
        {
            if (IsHeader)
            {
                if (selected)
                {
                    Table.SelectAll();
                }
                else
                {
                    Table.UnselectAll();
                }
            }
            else if (IsBody)
            {
                if (Type == "radio")
                {
                    Table.SetSelection(this);
                }
                else
                {
                    RowData.SetSelected(selected, CheckStrictly);
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (Table == null)
            {
                return;
            }

            if (IsHeader)
            {
                Table.Selection = this;
                Context.HeaderColumnInitialed(this);
            }
            else if (IsBody)
            {
                Table?.Selection?.RowSelections.Add(this);
                DataItem.Disabled = Disabled;
            }
        }

        // fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/3312
        // fixed https://github.com/ant-design-blazor/ant-design-blazor/issues/3417
        private void HandleSelected()
        {
            // avoid check the disabled one but allow default checked
            if (Disabled && _selected.HasValue)
            {
                DataItem.SetSelected(_selected.Value);
            }

            _selected = DataItem.Selected;
        }

        void ISelectionColumn.StateHasChanged()
        {
            if (IsHeader && Type == "checkbox")
            {
                StateHasChanged();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsHeader)
            {
                Table?.Selection?.RowSelections?.Remove(this);
            }

            base.Dispose(disposing);
        }
    }
}
