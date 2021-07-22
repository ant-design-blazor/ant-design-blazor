import { domInfoHelper, eventHelper } from '../dom/exports';
import { state } from '../stateProvider';
export class overlayHelper {
    static addPreventEnterOnOverlayVisible(element, overlayElement) {
        if (element && overlayElement) {
            let dom = domInfoHelper.get(element);
            if (dom) {
                state.eventCallbackRegistry[element.id + "keydown:Enter"] = (e) => eventHelper.preventKeyOnCondition(e, "enter", () => overlayElement.offsetParent !== null);
                dom.addEventListener("keydown", state.eventCallbackRegistry[element.id + "keydown:Enter"], false);
            }
        }
    }
    static removePreventEnterOnOverlayVisible(element) {
        if (element) {
            let dom = domInfoHelper.get(element);
            if (dom) {
                dom.removeEventListener("keydown", state.eventCallbackRegistry[element.id + "keydown:Enter"]);
                state.eventCallbackRegistry[element.id + "keydown:Enter"] = null;
            }
        }
    }
    static getMaxZIndex() {
        return [...document.all].reduce((r, e) => Math.max(r, +window.getComputedStyle(e).zIndex || 0), 0);
    }
}
