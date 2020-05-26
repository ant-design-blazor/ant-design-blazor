using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public class TransferSearchArgs
    {
        public string Direction { get; private set; }

        public string Value { get; private set; }

        public TransferSearchArgs(string direction, string value)
        {
            this.Direction = direction;
            this.Value = value;
        }
    }
}
