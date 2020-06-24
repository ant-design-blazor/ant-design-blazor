namespace AntDesign
{
    public interface ITable
    {
        internal ISelectionColumn HeaderSelection { get; set; }

        internal void SelectionChanged(int[] checkedIndex);

        internal void Refresh();

        void SetSelection(string[] keys);
    }
}
