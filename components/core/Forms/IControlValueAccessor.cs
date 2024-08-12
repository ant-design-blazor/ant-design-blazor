using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Forms
{
    public interface IControlValueAccessor
    {
        internal void Reset();
    }
}
