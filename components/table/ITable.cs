using System.Threading.Tasks;
using AntDesign.TableModels;

namespace AntDesign
{
    public interface ITable
    {
        PaginationModel Pagination { get; }

        internal ISelectionColumn Selection { get; set; }

        internal void SelectionChanged();

        internal void Refresh();

        internal void ReloadAndInvokeChange();

        void SetSelection(string[] keys);

        internal int[] GetSelectedIndex();

        void ReloadData();
    }
}
