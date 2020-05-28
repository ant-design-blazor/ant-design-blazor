using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public class TransferChangeArgs
    {
        public string[] TargetKeys { get; private set; }

        public string Direction { get; private set; }

        public string[] MoveKeys { get; private set; }

        public TransferChangeArgs(string[] targetKeys, string direction, string[] moveKeys)
        {
            this.TargetKeys = targetKeys;
            this.Direction = direction;
            this.MoveKeys = moveKeys;
        }
    }
}
