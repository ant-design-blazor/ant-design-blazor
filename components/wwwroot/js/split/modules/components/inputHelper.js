import { domInfoHelper } from '../dom/exports';
import { state } from '../stateProvider';
export class inputHelper {
    static getTextAreaInfo(element) {
        var result = {};
        var dom = domInfoHelper.get(element);
        if (!dom)
            return null;
        result["scrollHeight"] = dom.scrollHeight || 0;
        if (element.currentStyle) {
            result["lineHeight"] = parseFloat(element.currentStyle["line-height"]);
            result["paddingTop"] = parseFloat(element.currentStyle["padding-top"]);
            result["paddingBottom"] = parseFloat(element.currentStyle["padding-bottom"]);
            result["borderBottom"] = parseFloat(element.currentStyle["border-bottom"]);
            result["borderTop"] = parseFloat(element.currentStyle["border-top"]);
        }
        else if (window.getComputedStyle) {
            result["lineHeight"] = parseFloat(document.defaultView.getComputedStyle(element, null).getPropertyValue("line-height"));
            result["paddingTop"] = parseFloat(document.defaultView.getComputedStyle(element, null).getPropertyValue("padding-top"));
            result["paddingBottom"] = parseFloat(document.defaultView.getComputedStyle(element, null).getPropertyValue("padding-bottom"));
            result["borderBottom"] = parseFloat(document.defaultView.getComputedStyle(element, null).getPropertyValue("border-bottom"));
            result["borderTop"] = parseFloat(document.defaultView.getComputedStyle(element, null).getPropertyValue("border-top"));
        }
        //Firefox can return this as NaN, so it has to be handled here like that.
        if (Object.is(NaN, result["borderTop"]))
            result["borderTop"] = 1;
        if (Object.is(NaN, result["borderBottom"]))
            result["borderBottom"] = 1;
        return result;
    }
    static registerResizeTextArea(element, minRows, maxRows, objReference) {
        if (!objReference) {
            this.disposeResizeTextArea(element);
        }
        else {
            state.objReferenceDict[element.id] = objReference;
            state.eventCallbackRegistry[element.id + "input"] = function () { inputHelper.resizeTextArea(element, minRows, maxRows); };
            element.addEventListener("input", state.eventCallbackRegistry[element.id + "input"]);
            return this.getTextAreaInfo(element);
        }
    }
    static disposeResizeTextArea(element) {
        element.removeEventListener("input", state.eventCallbackRegistry[element.id + "input"]);
        state.objReferenceDict[element.id] = null;
        state.eventCallbackRegistry[element.id + "input"] = null;
    }
    static resizeTextArea(element, minRows, maxRows) {
        var dims = this.getTextAreaInfo(element);
        var rowHeight = dims["lineHeight"];
        var offsetHeight = dims["paddingTop"] + dims["paddingBottom"] + dims["borderTop"] + dims["borderBottom"];
        var oldHeight = parseFloat(element.style.height);
        element.style.height = 'auto';
        var rows = Math.trunc(element.scrollHeight / rowHeight);
        rows = Math.max(minRows, rows);
        var newHeight = 0;
        if (rows > maxRows) {
            rows = maxRows;
            newHeight = (rows * rowHeight + offsetHeight);
            element.style.height = newHeight + "px";
            element.style.overflowY = "visible";
        }
        else {
            newHeight = rows * rowHeight + offsetHeight;
            element.style.height = newHeight + "px";
            element.style.overflowY = "hidden";
        }
        if (oldHeight !== newHeight) {
            let textAreaObj = state.objReferenceDict[element.id];
            textAreaObj.invokeMethodAsync("ChangeSizeAsyncJs", parseFloat(element.scrollWidth), newHeight);
        }
    }
    static setSelectionStart(element, position) {
        if (position >= 0) {
            let dom = domInfoHelper.get(element);
            if (dom) {
                if (position <= dom.value.length) {
                    dom.selectionStart = position;
                    dom.selectionEnd = position;
                }
            }
        }
    }
}
