using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign.Forms
{
    public interface IControlValueAccessor
    {
        internal FieldIdentifier FieldIdentifier { get; }
        void OnValidated(string[] validationMessages);
        internal void Reset();
    }
}
