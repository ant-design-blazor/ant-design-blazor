using System.Threading.Tasks;
using AntDesign.TableModels;

namespace AntDesign
{
    public interface ITable
    {
        internal ISelectionColumn Selection { get; set; }

        internal void SelectionChanged(int[] checkedIndex);

        internal void Refresh();

        internal void ReloadAndInvokeChange();

        void SetSelection(string[] keys);

        void ReloadData();
    }
}
