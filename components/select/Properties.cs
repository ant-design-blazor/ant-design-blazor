using System;
using Microsoft.AspNetCore.Components.Web;

namespace AntDesign
{
    public class Properties
    {
        public string Value { get; set; }

        public string Label { get; set; }

        public bool Closable { get; set; }

        public Action<MouseEventArgs> OnClose { get; set; }
    }
}
