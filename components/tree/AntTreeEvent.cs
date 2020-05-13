using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace AntBlazor
{
    public class AntTreeEventArgs : EventArgs
    {
        public AntTreeEventArgs() { }
        public AntTreeEventArgs(AntTree tree) { Tree = tree; }
        public AntTreeEventArgs(AntTree tree, AntTreeNode node) { Tree = tree;Node = node; }


        public AntTree Tree { get; set; }
        public AntTreeNode Node { get; set; }
    }
}
