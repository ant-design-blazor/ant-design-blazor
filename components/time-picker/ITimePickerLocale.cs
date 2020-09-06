using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public interface ITimePickerLocale
    {
        public string Placeholder { get; }

        public string[] RangePlaceholder { get; }
    }
}
