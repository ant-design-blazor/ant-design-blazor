using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public interface ITextLocale
    {
        public string Edit { get; }
        public string Copy { get; }
        public string Copied { get; }
        public string Expand { get; }
    }
}
