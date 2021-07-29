﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public partial class TreeIndent<TItem> : ComponentBase
    {
        /// <summary>
        /// Root Tree
        /// </summary>
        [CascadingParameter(Name = "Tree")]
        public Tree<TItem> TreeComponent { get; set; }

        /// <summary>
        /// Current Node
        /// </summary>
        [CascadingParameter(Name = "SelfNode")]
        public TreeNode<TItem> SelfNode { get; set; }

        [Parameter]
        public int TreeLevel { get; set; }

        /// <summary>
        /// To find specific level parent node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private static TreeNode<TItem> GetParentNode(TreeNode<TItem> node, int level)
        {
            if (level > 0 && node.ParentNode != null)
            {
                return GetParentNode(node.ParentNode, level - 1);
            }

            return node;
        }
    }
}
