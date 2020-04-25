using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor.Internal
{
    internal class AntDatePickerStatus
    {
        internal string _initPicker = null;
        internal Stack<string> _prePickerStack = new Stack<string>();
        internal bool _hadSelectValue = false;
    }
}
