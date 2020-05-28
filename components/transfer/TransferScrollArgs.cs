using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class TransferScrollArgs
    {
        public string Direction { get; private set; }

        public EventArgs Args { get; private set; }

        public TransferScrollArgs(string direction, EventArgs e)
        {
            this.Direction = direction;
            this.Args = e;
        }
    }
}
