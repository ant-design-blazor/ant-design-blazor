namespace AntDesign
{
    internal sealed class SummaryRowContext
    {
        public int ColumnIndex { get; private set; }

        public int Advance(int colSpan)
        {
            var currentIndex = ColumnIndex;
            ColumnIndex += colSpan;
            return currentIndex;
        }
    }
}
