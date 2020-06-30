using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace AntDesign
{
    public class TreeEventArgs : EventArgs
    {
        public TreeEventArgs() { }
        public TreeEventArgs(Tree tree) { Tree = tree; }
        public TreeEventArgs(Tree tree, TreeNode node) { Tree = tree; Node = node; }

        public Tree Tree { get; set; }
        public TreeNode Node { get; set; }
    }
}
