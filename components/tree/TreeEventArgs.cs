using System;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class TreeEventArgs<TItem> : EventArgs
    {
        public TreeEventArgs() { }
        public TreeEventArgs(Tree<TItem> tree) { Tree = tree; }
        public TreeEventArgs(Tree<TItem> tree, TreeNode<TItem> node) { Tree = tree; Node = node; }

        public TreeEventArgs(Tree<TItem> tree, TreeNode<TItem> node, MouseEventArgs originalEvent) { Tree = tree; Node = node; OriginalEvent = originalEvent; }

        public Tree<TItem> Tree { get; set; }
        public TreeNode<TItem> Node { get; set; }

        /// <summary>
        /// 原生事件
        /// </summary>
        public MouseEventArgs OriginalEvent { get; set; }
    }
}
