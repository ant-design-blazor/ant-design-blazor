using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AntDesign
{
    public static class JSInteropConstants
    {
        private const string FUNC_PREFIX = "AntDesign.interop.";

        public static string GetDomInfo => $"{FUNC_PREFIX}getDomInfo";

        public static string TriggerEvent => $"{FUNC_PREFIX}triggerEvent";

        public static string AddFileClickEventListener => $"{FUNC_PREFIX}addFileClickEventListener";

        public static string RemoveFileClickEventListener => $"{FUNC_PREFIX}removeFileClickEventListener";

        public static string ClearFile => $"{FUNC_PREFIX}clearFile";

        public static string UploadFile => $"{FUNC_PREFIX}uploadFile";

        public static string GetObjectURL => $"{FUNC_PREFIX}getObjectURL";

        public static string GetFileInfo => $"{FUNC_PREFIX}getFileInfo";

        public static string GetBoundingClientRect => $"{FUNC_PREFIX}getBoundingClientRect";

        public static string AddDomEventListener => $"{FUNC_PREFIX}addDomEventListener";

        public static string MatchMedia => $"{FUNC_PREFIX}matchMedia";

        public static string Copy => $"{FUNC_PREFIX}copy";

        public static string Log => $"{FUNC_PREFIX}log";

        public static string Focus => $"{FUNC_PREFIX}focus";

        public static string Blur => $"{FUNC_PREFIX}blur";

        public static string BackTop => $"{FUNC_PREFIX}backTop";

        public static string ScrollTo => $"{FUNC_PREFIX}scrollTo";

        public static string GetFirstChildDomInfo => $"{FUNC_PREFIX}getFirstChildDomInfo";

        public static string AddClsToFirstChild => $"{FUNC_PREFIX}addClsToFirstChild";

        public static string RemoveClsFromFirstChild => $"{FUNC_PREFIX}removeClsFromFirstChild";

        public static string AddDomEventListenerToFirstChild => $"{FUNC_PREFIX}addDomEventListenerToFirstChild";

        public static string AddElementToBody => $"{FUNC_PREFIX}addElementToBody";

        public static string DelElementFromBody => $"{FUNC_PREFIX}delElementFromBody";

        public static string AddElementTo => $"{FUNC_PREFIX}addElementTo";

        public static string DelElementFrom => $"{FUNC_PREFIX}delElementFrom";

        public static string GetActiveElement => $"{FUNC_PREFIX}getActiveElement";

        public static string FocusDialog => $"{FUNC_PREFIX}focusDialog";

        public static string GetWindow = $"{FUNC_PREFIX}getWindow";

        public static string DisableBodyScroll => $"{FUNC_PREFIX}disableBodyScroll";

        public static string EnableBodyScroll => $"{FUNC_PREFIX}enableBodyScroll";

        public static string DestroyAllDialog => $"{FUNC_PREFIX}destroyAllDialog";

        public static string CreateIconFromfontCN => $"{FUNC_PREFIX}createIconFromfontCN";

        public static string GetScroll => $"{FUNC_PREFIX}getScroll";

        public static string GetInnerText => $"{FUNC_PREFIX}getInnerText";

        public static string GetMaxZIndex => $"{FUNC_PREFIX}getMaxZIndex";

        public static string GetCursorXY => $"{FUNC_PREFIX}getCursorXY";

        public static string DisposeObj => $"{FUNC_PREFIX}disposeObj";

        public static string ElementScrollIntoView => $"{FUNC_PREFIX}elementScrollIntoView";

        public static string BindTableHeaderAndBodyScroll => $"{FUNC_PREFIX}bindTableHeaderAndBodyScroll";

        public static string UnbindTableHeaderAndBodyScroll => $"{FUNC_PREFIX}unbindTableHeaderAndBodyScroll";

        public static string AddPreventKeys => $"{FUNC_PREFIX}addPreventKeys";

        public static string RemovePreventKeys => $"{FUNC_PREFIX}removePreventKeys";

        public static string AddPreventEnterOnOverlayVisible => $"{FUNC_PREFIX}addPreventEnterOnOverlayVisible";

        public static string RemovePreventEnterOnOverlayVisible => $"{FUNC_PREFIX}removePreventEnterOnOverlayVisible";

        public static string GetStyle => $"{FUNC_PREFIX}getStyle";

        public static string RegisterResizeTextArea => $"{FUNC_PREFIX}registerResizeTextArea";

        public static string DisposeResizeTextArea => $"{FUNC_PREFIX}disposeResizeTextArea";

        public static string SetDomAttribute => $"{FUNC_PREFIX}setDomAttribute";

        #region Draggable Modal

        public static string EnableDraggable => $"{FUNC_PREFIX}enableDraggable";

        public static string DisableDraggable => $"{FUNC_PREFIX}disableDraggable";

        public static string ResetModalPosition => $"{FUNC_PREFIX}resetModalPosition";

        #endregion Draggable Modal
    }
}
