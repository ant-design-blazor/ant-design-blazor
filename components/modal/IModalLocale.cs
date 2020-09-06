using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public interface IModalLocale
    {
        public string OkText { get; }

        public string CancelText { get; }

        public string JustOkText { get; }
    }
}
