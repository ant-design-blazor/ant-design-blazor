using System;
using System.Collections.Generic;
using System.Text;

namespace AntBlazor
{
    public static class JSInteropConstants
    {
        private const string FUNC_PREFIX = "antBlazor.interop.";

        public static string getDomInfo => $"{FUNC_PREFIX}getDomInfo";

        public static string getBoundingClientRect => $"{FUNC_PREFIX}getBoundingClientRect";

        public static string addDomEventListener => $"{FUNC_PREFIX}addDomEventListener";

        public static string matchMedia => $"{FUNC_PREFIX}matchMedia";

        public static string copy => $"{FUNC_PREFIX}copy";

        public static string log => $"{FUNC_PREFIX}log";
    }
}