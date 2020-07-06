using System.Threading.Tasks;
using AntDesign.TableModels;

namespace AntDesign
{
    public interface ITable
    {
        internal ISelectionColumn HeaderSelection { get; set; }

        internal void SelectionChanged(int[] checkedIndex);

        internal void Refresh();

        QueryModel QueryModel { get; set; }

        void SetSelection(string[] keys);

        void ReloadData();
    }
}
