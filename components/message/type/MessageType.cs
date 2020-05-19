using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public enum MessageType:byte
    {
        Info = 0,
        Success = 1,
        Error = 2,
        Warning = 3,
        Loading = 4
    }
}
