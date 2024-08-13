using System;

namespace AntDesign
{
    public class TransferScrollArgs
    {
        public TransferDirection Direction { get; private set; }

        public EventArgs Args { get; private set; }

        public TransferScrollArgs(TransferDirection direction, EventArgs e)
        {
            Direction = direction;
            Args = e;
        }
    }
}
