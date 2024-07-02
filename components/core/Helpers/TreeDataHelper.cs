// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntDesign.Core.Helpers
{
    internal static class TreeDataHelper
    {
        public static List<TItem> FlattenTree<TItem>(IEnumerable<TItem> root, Func<TItem, IEnumerable<TItem>> getChildren)
        {
            List<TItem> result = new List<TItem>();

            foreach (var item in root)
            {
                result.Add(item);

                var children = getChildren(item);
                if (children.Any())
                {
                    result.AddRange(FlattenTree(children, getChildren));
                }
            }

            return result;
        }
    }
}
