﻿using System;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class TreeEventArgs<TItem> : EventArgs
    {
        public TreeEventArgs() { }
        public TreeEventArgs(Tree<TItem> tree) { Tree = tree; }
        public TreeEventArgs(Tree<TItem> tree, TreeNode<TItem> node) { Tree = tree; Node = node; }
        public TreeEventArgs(Tree<TItem> tree, TreeNode<TItem> node, MouseEventArgs originalEvent) { Tree = tree; Node = node; OriginalEvent = originalEvent; }
        public TreeEventArgs(Tree<TItem> tree, TreeNode<TItem> node, MouseEventArgs originalEvent, bool dropBelow ) { Tree = tree; Node = node; OriginalEvent = originalEvent; DropBelow = dropBelow; }

        public Tree<TItem> Tree { get; set; }
        public TreeNode<TItem> Node { get; set; }

        /// <summary>
        /// 目标节点
        /// </summary>
        public TreeNode<TItem> TargetNode { get; set; }

        /// <summary>
        /// 原生事件
        /// </summary>
        public MouseEventArgs OriginalEvent { get; set; }
        
        /// <summary>
        /// Whether to drop dragged node as a sibling (below) or as a child of target node.  
        /// </summary>
        public bool DropBelow { get; set; }
    }
}
