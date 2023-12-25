using System;

namespace AntDesign
{
    public static class JSInteropConstants
    {
        private const string FUNC_PREFIX = "AntDesign.interop.";

        #region domInfo

        public static string GetWindow = DomInfoHelper.GetWindow;
        public static string GetDomInfo => DomInfoHelper.GetInfo;
        public static string GetBoundingClientRect => DomInfoHelper.GetBoundingClientRect;
        public static string GetFirstChildDomInfo => DomInfoHelper.GetFirstChildDomInfo;
        public static string GetElementsDomInfo => DomInfoHelper.GetElementsDomInfo;
        public static string GetActiveElement => DomInfoHelper.GetActiveElement;
        public static string GetScroll => DomInfoHelper.GetScroll;
        public static string HasFocus => DomInfoHelper.HasFocus;
        public static string GetInnerText => DomInfoHelper.GetInnerText;
        public static string GetMaxZIndex => DomInfoHelper.GetMaxZIndex;

        #endregion domInfo

        #region styleManipulation

        public static string AddClsToFirstChild => StyleHelper.AddClsToFirstChild;
        public static string RemoveClsFromFirstChild => StyleHelper.RemoveClsFromFirstChild;
        public static string MatchMedia => StyleHelper.MatchMedia;
        public static string GetStyle => StyleHelper.GetStyle;

        #endregion styleManipulation

        #region domManipulation

        public static string AddElementToBody => DomMainpulationHelper.AddElementToBody;
        public static string DelElementFromBody => DomMainpulationHelper.DelElementFromBody;
        public static string AddElementTo => DomMainpulationHelper.AddElementTo;
        public static string DelElementFrom => DomMainpulationHelper.DelElementFrom;
        public static string SetDomAttribute => DomMainpulationHelper.SetDomAttribute;
        public static string Copy => DomMainpulationHelper.Copy;
        public static string CopyElement => DomMainpulationHelper.CopyElement;
#if NET5_0_OR_GREATER
        [Obsolete("It will be removed in the future, because Blazor already has a native implementation.")]
#endif
        public static string Focus => DomMainpulationHelper.Focus;
        public static string Blur => DomMainpulationHelper.Blur;
        public static string ScrollTo => DomMainpulationHelper.ScrollTo;
        public static string SmoothScrollTo => DomMainpulationHelper.SmoothScrollTo;
        public static string InvokeTabKey => DomMainpulationHelper.InvokeTabKey;
        public static string DisableBodyScroll => DomMainpulationHelper.DisableBodyScroll;
        public static string EnableBodyScroll => DomMainpulationHelper.EnableBodyScroll;

        #endregion domManipulation

        #region upload

        public static string AddFileClickEventListener => UploadComponentHelper.AddFileClickEventListener;
        public static string RemoveFileClickEventListener => UploadComponentHelper.RemoveFileClickEventListener;
        public static string ClearFile => UploadComponentHelper.ClearFile;
        public static string UploadFile => UploadComponentHelper.UploadFile;
        public static string GetFileInfo => UploadComponentHelper.GetFileInfo;

        #endregion upload

        #region download

        public static string TriggerFileDownload => DownloadComponentHelper.TriggerFileDownload;

        #endregion download

        #region event

        public static string TriggerEvent => EventHelper.TriggerEvent;
        public static string AddDomEventListener => EventHelper.AddDomEventListener;
        public static string RemoveDomEventListener => EventHelper.RemoveDomEventListener;
        public static string AddDomEventListenerToFirstChild => EventHelper.AddDomEventListenerToFirstChild;
        public static string AddPreventKeys => EventHelper.AddPreventKeys;
        public static string RemovePreventKeys => EventHelper.RemovePreventKeys;

        #endregion event

        #region backtop

        public static string BackTop => BackTopComponentHelper.BackTop;

        #endregion backtop

        #region icon

        public static string CreateIconFromfontCN => IconComponentHelper.CreateIconFromfontCN;

        #endregion icon

        #region image

        public static string ImgDragAndDrop => ImageComponentHelper.ImgDragAndDrop;

        #endregion

        #region input

        public static string RegisterResizeTextArea => InputComponentHelper.RegisterResizeTextArea;
        public static string DisposeResizeTextArea => InputComponentHelper.DisposeResizeTextArea;
        public static string SetSelectionStart => InputComponentHelper.SetSelectionStart;

        #endregion input

        #region mentions

        public static string GetCursorXY => MentionsComponentHelper.GetCursorXY;
        public static string SetEditorKeyHandler => MentionsComponentHelper.SetEditorKeyHandler;
        public static string GetProp => MentionsComponentHelper.GetProp;
        public static string SetPopShowFlag => MentionsComponentHelper.SetPopShowFlag;


        #endregion mentions

        #region modal

        public static string FocusDialog => ModalComponentHelper.FocusDialog;
        public static string DestroyAllDialog => ModalComponentHelper.DestroyAllDialog;
        public static string EnableDraggable => $"{FUNC_PREFIX}enableDraggable";
        public static string DisableDraggable => $"{FUNC_PREFIX}disableDraggable";
        public static string ResetModalPosition => $"{FUNC_PREFIX}resetModalPosition";

        #endregion modal

        #region overlay

        public static string AddPreventEnterOnOverlayVisible => OverlayComponentHelper.AddPreventEnterOnOverlayVisible;
        public static string RemovePreventEnterOnOverlayVisible => OverlayComponentHelper.RemovePreventEnterOnOverlayVisible;
        //public static string AddOverlayToContainer => OverlayComponentHelper.AddOverlayToContainer;

        #endregion overlay

        #region table

        public static string BindTableScroll => TableComponentHelper.BindTableScroll;
        public static string UnbindTableScroll => TableComponentHelper.UnbindTableScroll;

        #endregion table

        public static string DisposeObj => $"{FUNC_PREFIX}state.disposeObj";
        public static string Log => $"{FUNC_PREFIX}log";
        public static string GenerateColor => $"{FUNC_PREFIX}generateColor";

        public static class DomInfoHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "domInfoHelper.";
            public static string GetWindow = $"{FUNC_PREFIX}getWindow";
            public static string GetInfo => $"{FUNC_PREFIX}getInfo";

            public static string GetElementsDomInfo => $"{FUNC_PREFIX}getElementsInfo";
            public static string GetBoundingClientRect => $"{FUNC_PREFIX}getBoundingClientRect";
            public static string GetFirstChildDomInfo => $"{FUNC_PREFIX}getFirstChildDomInfo";
            public static string GetActiveElement => $"{FUNC_PREFIX}getActiveElement";
            public static string GetScroll => $"{FUNC_PREFIX}getScroll";
            public static string HasFocus => $"{FUNC_PREFIX}hasFocus";
            public static string GetInnerText => $"{FUNC_PREFIX}getInnerText";
            public static string GetMaxZIndex => $"{FUNC_PREFIX}getMaxZIndex";
        }

        public static class EventHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "eventHelper.";
            public static string TriggerEvent => $"{FUNC_PREFIX}triggerEvent";
            public static string AddDomEventListener => $"{FUNC_PREFIX}addDomEventListener";
            public static string RemoveDomEventListener => $"{FUNC_PREFIX}removeDomEventListener";
            public static string AddDomEventListenerToFirstChild => $"{FUNC_PREFIX}addDomEventListenerToFirstChild";
            public static string AddPreventKeys => $"{FUNC_PREFIX}addPreventKeys";
            public static string RemovePreventKeys => $"{FUNC_PREFIX}removePreventKeys";
        }

        public static class DomMainpulationHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "domManipulationHelper.";
            public static string AddElementToBody => $"{FUNC_PREFIX}addElementToBody";
            public static string DelElementFromBody => $"{FUNC_PREFIX}delElementFromBody";
            public static string AddElementTo => $"{FUNC_PREFIX}addElementTo";
            public static string DelElementFrom => $"{FUNC_PREFIX}delElementFrom";
            public static string SetDomAttribute => $"{FUNC_PREFIX}setDomAttribute";
            public static string Copy => $"{FUNC_PREFIX}copy";
            public static string CopyElement => $"{FUNC_PREFIX}copyElement";
#if NET5_0_OR_GREATER
            [Obsolete("It will be removed in the future, because Blazor already has a native implementation.")]
#endif
            public static string Focus => $"{FUNC_PREFIX}focus";
            public static string Blur => $"{FUNC_PREFIX}blur";
            public static string ScrollTo => $"{FUNC_PREFIX}scrollTo";
            public static string SmoothScrollTo => $"{FUNC_PREFIX}smoothScrollTo";
            public static string InvokeTabKey => $"{FUNC_PREFIX}invokeTabKey";
            public static string DisableBodyScroll => $"{FUNC_PREFIX}disableBodyScroll";
            public static string EnableBodyScroll => $"{FUNC_PREFIX}enableBodyScroll";
        }

        public static class StyleHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "styleHelper.";
            public static string AddClsToFirstChild => $"{FUNC_PREFIX}addClsToFirstChild";
            public static string AddCls => $"{FUNC_PREFIX}addCls";
            public static string RemoveCls => $"{FUNC_PREFIX}removeCls";
            public static string RemoveClsFromFirstChild => $"{FUNC_PREFIX}removeClsFromFirstChild";
            public static string MatchMedia => $"{FUNC_PREFIX}matchMedia";
            public static string GetStyle => $"{FUNC_PREFIX}getStyle";
            public static string SetStyle => $"{FUNC_PREFIX}css";
        }

        public static class ObserverConstants
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "observable.";

            public static class Resize
            {
                private const string FUNC_PREFIX = ObserverConstants.FUNC_PREFIX + "resize.";
                public static string IsResizeObserverSupported => $"{FUNC_PREFIX}isResizeObserverSupported";
                public static string Create = $"{FUNC_PREFIX}create";
                public static string Observe = $"{FUNC_PREFIX}observe";
                public static string Unobserve = $"{FUNC_PREFIX}unobserve";
                public static string Disconnect = $"{FUNC_PREFIX}disconnect";
                public static string Dispose = $"{FUNC_PREFIX}dispose";
            }
        }

        public static class BackTopComponentHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "backtopHelper.";
            public static string BackTop => $"{FUNC_PREFIX}backTop";
        }

        public static class IconComponentHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "iconHelper.";
            public static string CreateIconFromfontCN => $"{FUNC_PREFIX}createIconFromfontCN";
        }

        public static class ImageComponentHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "imageHelper.";
            public static string ImgDragAndDrop => $"{FUNC_PREFIX}imgDragAndDrop";
        }

        public static class InputComponentHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "inputHelper.";
            public static string RegisterResizeTextArea => $"{FUNC_PREFIX}registerResizeTextArea";
            public static string GetTextAreaInfo => $"{FUNC_PREFIX}getTextAreaInfo";
            public static string DisposeResizeTextArea => $"{FUNC_PREFIX}disposeResizeTextArea";
            public static string SetSelectionStart => $"{FUNC_PREFIX}setSelectionStart";
            public static string ResizeTextArea => $"{FUNC_PREFIX}resizeTextArea";
        }

        public static class MentionsComponentHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "mentionsHelper.";
            public static string GetCursorXY => $"{FUNC_PREFIX}getCursorXY";
            public static string SetEditorKeyHandler => $"{FUNC_PREFIX}setEditorKeyHandler";
            public static string GetProp => $"{FUNC_PREFIX}getProp";
            public static string SetPopShowFlag => $"{FUNC_PREFIX}setPopShowFlag";
        }

        public static class ModalComponentHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "modalHelper.";
            public static string FocusDialog => $"{FUNC_PREFIX}focusDialog";
            public static string DestroyAllDialog => $"{FUNC_PREFIX}destroyAllDialog";
            public static string EnableDraggable => $"{FUNC_PREFIX}enableDraggable";
            public static string DisableDraggable => $"{FUNC_PREFIX}disableDraggable";
            public static string ResetModalPosition => $"{FUNC_PREFIX}resetModalPosition";
        }

        public static class OverlayComponentHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "overlayHelper.";
            public static string AddPreventEnterOnOverlayVisible => $"{FUNC_PREFIX}addPreventEnterOnOverlayVisible";
            public static string RemovePreventEnterOnOverlayVisible => $"{FUNC_PREFIX}removePreventEnterOnOverlayVisible";
            public static string AddOverlayToContainer => $"{FUNC_PREFIX}addOverlayToContainer";
            public static string UpdateOverlayPosition => $"{FUNC_PREFIX}updateOverlayPosition";
            public static string DeleteOverlayFromContainer => $"{FUNC_PREFIX}deleteOverlayFromContainer";
        }

        public static class TableComponentHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "tableHelper.";
            public static string BindTableScroll => $"{FUNC_PREFIX}bindTableScroll";
            public static string UnbindTableScroll => $"{FUNC_PREFIX}unbindTableScroll";
        }

        public static class UploadComponentHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "uploadHelper.";
            public static string AddFileClickEventListener => $"{FUNC_PREFIX}addFileClickEventListener";
            public static string RemoveFileClickEventListener => $"{FUNC_PREFIX}removeFileClickEventListener";
            public static string ClearFile => $"{FUNC_PREFIX}clearFile";
            public static string UploadFile => $"{FUNC_PREFIX}uploadFile";
            public static string GetFileInfo => $"{FUNC_PREFIX}getFileInfo";
        }

        public static class DownloadComponentHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "downloadHelper.";
            public static string TriggerFileDownload => $"{FUNC_PREFIX}triggerFileDownload";
        }

        public static class WatermarkHelper
        {
            private const string FUNC_PREFIX = JSInteropConstants.FUNC_PREFIX + "watermarkHelper.";

            public static string GenerateBase64Url => $"{FUNC_PREFIX}generateBase64Url";
        }
    }
}
