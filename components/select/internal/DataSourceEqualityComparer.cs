// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
                if (SelectParent._getValue is null)
                {
                    return x.ToString() == y.ToString();
                }
                return x.ToString() == y.ToString()
                    && EqualityComparer<TItemValue>.Default.Equals(SelectParent._getValue(x), SelectParent._getValue(y));
            }
            if (SelectParent._getValue is null)
            {
                return SelectParent._getLabel(x) == SelectParent._getLabel(y);
            }
            return SelectParent._getLabel(x) == SelectParent._getLabel(y)
                && EqualityComparer<TItemValue>.Default.Equals(SelectParent._getValue(x), SelectParent._getValue(y));
        }

        public int GetHashCode(TItem obj)
        {
            return obj.GetHashCode();
        }
    }
}
