using System;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class TreeEventArgs : EventArgs
    {
        public TreeEventArgs() { }
        public TreeEventArgs(Tree tree) { Tree = tree; }
        public TreeEventArgs(Tree tree, TreeNode node) { Tree = tree; Node = node; }

        public TreeEventArgs(Tree tree, TreeNode node, MouseEventArgs originalEvent) { Tree = tree; Node = node; OriginalEvent = originalEvent; }

        public Tree Tree { get; set; }
        public TreeNode Node { get; set; }

        /// <summary>
        /// 原生事件
        /// </summary>
        public MouseEventArgs OriginalEvent { get; set; }
    }
}
