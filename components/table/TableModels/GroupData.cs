// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign.TableModels
{
    public class GroupData<TItem>
    {
        public TItem Overall { get; set; }

        public IEnumerable<TItem> Items { get; set; }

        public IEnumerable<GroupData<TItem>> Children { get; set; }

        public GroupData()
        {
        }
    }

    public class GroupData<TKey, TItem> : GroupData<TItem>
    {
        public TKey Key { get; set; }

        public GroupData() : base()
        {
        }

        public GroupData(TKey key)
        {
            this.Key = key;
        }
    }
}
