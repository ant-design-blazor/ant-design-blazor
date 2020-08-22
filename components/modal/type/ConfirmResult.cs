using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    /// <summary>
    /// which the confirm button is clicked
    /// </summary>
    public enum ConfirmResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Abort = 4,
        Retry = 8,
        Ignore = 16,
        Yes = 32,
        No = 64
    }
}
