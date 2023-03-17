// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign.TableModels
{
    public class GroupData
    {
        public object Key { get; set; }

        public bool Expanded { get; set; } = true;

        public GroupData()
        {
        }

        public GroupData(object key)
        {
            this.Key = key;
        }

        public void ToggleTreeNode()
        {
            this.Expanded = !this.Expanded;
        }
    }

    public class GroupData<TItem> : GroupData
    {
        public GroupData<TItem> Parent { get; set; }

        public IEnumerable<TItem> Items { get; set; }

        public IEnumerable<GroupData<TItem>> Children { get; set; }

        public Dictionary<TItem, RowData<TItem>> Cache { get; set; } = new Dictionary<TItem, RowData<TItem>>();

        public GroupData() : base()
        {
        }

        public GroupData(object key) : base(key)
        {
        }
    }
}
