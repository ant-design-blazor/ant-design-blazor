using System;

namespace AntDesign
{
    public class TransferScrollArgs
    {
        public string Direction { get; private set; }

        public EventArgs Args { get; private set; }

        public TransferScrollArgs(string direction, EventArgs e)
        {
            Direction = direction;
            Args = e;
        }
    }
}
