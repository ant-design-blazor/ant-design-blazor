using System.Collections.Generic;

namespace AntDesign
{
    internal class DataSourceEqualityComparer<TItemValue, TItem> : IEqualityComparer<TItem>
    {
        private Select<TItemValue, TItem> SelectParent { get; }

        public DataSourceEqualityComparer(Select<TItemValue, TItem> selectParent)
        {
            SelectParent = selectParent;
        }

        public bool Equals(TItem x, TItem y)
        {
            if (SelectParent._getLabel is null)
            {
                return x.ToString().Equals(y.ToString())
                    && SelectParent._getValue(x).Equals(SelectParent._getValue(y));
            }
            return SelectParent._getLabel(x).Equals(SelectParent._getLabel(y))
                && SelectParent._getValue(x).Equals(SelectParent._getValue(y));
        }

        public int GetHashCode(TItem obj)
        {
            return obj.GetHashCode();
        }
    }
}
