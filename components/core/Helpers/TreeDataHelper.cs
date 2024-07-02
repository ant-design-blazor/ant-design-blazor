// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign.Core.Helpers
{
    internal static class TreeDataHelper
    {
        public static List<TItem> FlattenTree<TItem>(IEnumerable<TItem> root, Func<TItem, IEnumerable<TItem>> getChildren)
        {
            List<TItem> result = new List<TItem>();

            Stack<TItem> stack = new Stack<TItem>();
            foreach (var item in root)
            {
                stack.Push(item);
            }

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                result.Add(current);
                foreach (var child in getChildren(current))
                {
                    stack.Push(child);
                }
            }

            return result;
        }
    }
}
