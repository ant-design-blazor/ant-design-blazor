using System.Collections.Generic;
using AntDesign.Internal;
using Microsoft.AspNetCore.Components.Forms;

namespace AntDesign
{
    public class FormProviderFinishEventArgs
    {
        public Dictionary<string, IForm> Forms { get; set; }
        public IForm FinishForm { get; set; }
    }
}
