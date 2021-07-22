import { domInfoHelper } from './exports';
import { styleHelper } from '../styleHelper';
import { state } from '../stateProvider';
import * as enums from '../enums';
export class manipulationHelper {
    static addElementToBody(element) {
        document.body.appendChild(element);
    }
    static delElementFromBody(element) {
        document.body.removeChild(element);
    }
    static addElementTo(addElement, elementSelector) {
        let parent = domInfoHelper.get(elementSelector);
        if (parent && addElement) {
            parent.appendChild(addElement);
        }
    }
    static delElementFrom(delElement, elementSelector) {
        let parent = domInfoHelper.get(elementSelector);
        if (parent && delElement) {
            parent.removeChild(delElement);
        }
    }
    static setDomAttribute(element, attributes) {
        let dom = domInfoHelper.get(element);
        if (dom) {
            for (let key in attributes) {
                dom.setAttribute(key, attributes[key]);
            }
        }
    }
    static copy(text) {
        if (!navigator.clipboard) {
            this.fallbackCopyTextToClipboard(text);
            return;
        }
        navigator.clipboard.writeText(text).then(function () {
            console.log('Async: Copying to clipboard was successful!');
        }, function (err) {
            console.error('Async: Could not copy text: ', err);
        });
    }
    static fallbackCopyTextToClipboard(text) {
        var textArea = document.createElement("textarea");
        textArea.value = text;
        // Avoid scrolling to bottom
        textArea.style.top = "0";
        textArea.style.left = "0";
        textArea.style.position = "fixed";
        document.body.appendChild(textArea);
        textArea.focus();
        textArea.select();
        try {
            var successful = document.execCommand('copy');
            var msg = successful ? 'successful' : 'unsuccessful';
            console.log('Fallback: Copying text command was ' + msg);
        }
        catch (err) {
            console.error('Fallback: Oops, unable to copy', err);
        }
        document.body.removeChild(textArea);
    }
    static focus(selector, noScroll = false, option = enums.FocusBehavior.FocusAtLast) {
        let dom = domInfoHelper.get(selector);
        if (!(dom instanceof HTMLElement))
            throw new Error("Unable to focus on invalid element.");
        dom.focus({
            preventScroll: noScroll
        });
        if (dom instanceof HTMLInputElement || dom instanceof HTMLTextAreaElement) {
            switch (option) {
                case enums.FocusBehavior.FocusAndSelectAll:
                    dom.select();
                    break;
                case enums.FocusBehavior.FocusAtFirst:
                    dom.setSelectionRange(0, 0);
                    break;
                case enums.FocusBehavior.FocusAtLast:
                    dom.setSelectionRange(-1, -1);
                    break;
            }
        }
    }
    static blur(selector) {
        let dom = domInfoHelper.get(selector);
        if (dom) {
            dom.blur();
        }
    }
    static scrollTo(selector) {
        let element = domInfoHelper.get(selector);
        if (element && element instanceof HTMLElement) {
            element.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'start' });
        }
    }
    static slideTo(targetPageY) {
        var timer = setInterval(function () {
            var currentY = document.documentElement.scrollTop || document.body.scrollTop;
            var distance = targetPageY > currentY ? targetPageY - currentY : currentY - targetPageY;
            var speed = Math.ceil(distance / 10);
            if (currentY == targetPageY) {
                clearInterval(timer);
            }
            else {
                window.scrollTo(0, targetPageY > currentY ? currentY + speed : currentY - speed);
            }
        }, 10);
    }
    //copied from https://www.telerik.com/forums/trigger-tab-key-when-enter-key-is-pressed
    static invokeTabKey() {
        var currInput = document.activeElement;
        if (currInput.tagName.toLowerCase() == "input") {
            var inputs = document.getElementsByTagName("input");
            var currInput = document.activeElement;
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i] == currInput) {
                    var next = inputs[i + 1];
                    if (next && next.focus) {
                        next.focus();
                    }
                    break;
                }
            }
        }
    }
    static disableBodyScroll() {
        let body = document.body;
        const oldBodyCache = {};
        ["position", "width", "overflow"].forEach((key) => {
            oldBodyCache[key] = body.style[key];
        });
        state.oldBodyCacheStack.push(oldBodyCache);
        styleHelper.css(body, {
            "position": "relative",
            "width": this.hasScrollbar() ? "calc(100% - 17px)" : null,
            "overflow": "hidden"
        });
        styleHelper.addCls(document.body, "ant-scrolling-effect");
    }
    static enableBodyScroll() {
        var _a, _b, _c;
        let oldBodyCache = state.oldBodyCacheStack.length > 0 ? state.oldBodyCacheStack.pop() : {};
        styleHelper.css(document.body, {
            "position": (_a = oldBodyCache["position"]) !== null && _a !== void 0 ? _a : null,
            "width": (_b = oldBodyCache["width"]) !== null && _b !== void 0 ? _b : null,
            "overflow": (_c = oldBodyCache["overflow"]) !== null && _c !== void 0 ? _c : null
        });
        styleHelper.removeCls(document.body, "ant-scrolling-effect");
    }
}
manipulationHelper.hasScrollbar = () => {
    let overflow = document.body.style.overflow;
    if (overflow && overflow === "hidden")
        return false;
    return document.body.scrollHeight > (window.innerHeight || document.documentElement.clientHeight);
};
