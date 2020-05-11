using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static string focus => $"{FUNC_PREFIX}focus";

        public static string blur => $"{FUNC_PREFIX}blur";
        public static string backTop => $"{FUNC_PREFIX}BackTop";

        public static string getFirstChildDomInfo => $"{FUNC_PREFIX}getFirstChildDomInfo";

        public static string addClsToFirstChild => $"{FUNC_PREFIX}addClsToFirstChild";

        public static string addDomEventListenerToFirstChild => $"{FUNC_PREFIX}addDomEventListenerToFirstChild";

        public static string addElementToBody => $"{FUNC_PREFIX}addElementToBody";

        public static string delElementFromBody => $"{FUNC_PREFIX}delElementFromBody";

        public static string addElementTo => $"{FUNC_PREFIX}addElementTo";

        public static string delElementFrom => $"{FUNC_PREFIX}delElementFrom";

        public static string initCsInstance => $"{FUNC_PREFIX}initCsInstance";

        public static string delCsInstance => $"{FUNC_PREFIX}delCsInstance";



        public static string appendHtml => $"{FUNC_PREFIX}appendHtml";

        public static string removeElement=> $"{FUNC_PREFIX}removeElement";

        public static string addNotification => $"{FUNC_PREFIX}addNotification";
        
        public static string removeNotification => $"{FUNC_PREFIX}removeNotification";
    }
}
