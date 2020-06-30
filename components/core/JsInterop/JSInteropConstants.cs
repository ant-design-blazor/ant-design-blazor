using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AntDesign
{
    public static class JSInteropConstants
    {
        private const string FUNC_PREFIX = "AntDesign.interop.";

        public static string getDomInfo => $"{FUNC_PREFIX}getDomInfo";

        public static string triggerEvent => $"{FUNC_PREFIX}triggerEvent";

        public static string addFileClickEventListener => $"{FUNC_PREFIX}addFileClickEventListener";

        public static string removeFileClickEventListener => $"{FUNC_PREFIX}removeFileClickEventListener";

        public static string clearFile => $"{FUNC_PREFIX}clearFile";

        public static string uploadFile => $"{FUNC_PREFIX}uploadFile";

        public static string getObjectURL => $"{FUNC_PREFIX}getObjectURL";

        public static string getFileInfo => $"{FUNC_PREFIX}getFileInfo";

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

        public static string getActiveElement => $"{FUNC_PREFIX}getActiveElement";

        public static string focusDialog => $"{FUNC_PREFIX}focusDialog";

        public static string getWindow = $"{FUNC_PREFIX}getWindow";

        public static string disableBodyScroll => $"{FUNC_PREFIX}disableBodyScroll";

        public static string enableModalBodyScroll => $"{FUNC_PREFIX}enableModalBodyScroll";

        public static string enableDrawerBodyScroll => $"{FUNC_PREFIX}enableDrawerBodyScroll";

        public static string CreateIconFromfontCN => $"{FUNC_PREFIX}createIconFromfontCN";

        public static string getScroll => $"{FUNC_PREFIX}getScroll";

        public static string getInnerText => $"{FUNC_PREFIX}getInnerText";

        public static string getCursorXY => $"{FUNC_PREFIX}getCursorXY";
    }
}
