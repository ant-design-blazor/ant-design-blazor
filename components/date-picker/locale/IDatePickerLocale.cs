using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public interface IDatePickerLocale
    {
        public IDateLocale Lang { get; }

        public ITimePickerLocale TimePickerLocale { get; }
    }
}
