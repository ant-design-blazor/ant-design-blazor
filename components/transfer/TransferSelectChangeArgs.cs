using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class TransferSelectChangeArgs
    {
        public string[] SourceSelectedKeys { get; private set; }

        public string[] TargetSelectedKeys { get; private set; }

        public TransferSelectChangeArgs(string[] sourceSelectedKeys, string[] targetSelectedKeys)
        {
            this.SourceSelectedKeys = sourceSelectedKeys;
            this.TargetSelectedKeys = targetSelectedKeys;
        }
    }
}
