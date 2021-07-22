import { domInfoHelper } from './exports';
import { state } from '../stateProvider';
export class eventHelper {
    static triggerEvent(element, eventType, eventName) {
        //TODO: replace with event constructors https://developer.mozilla.org/en-US/docs/Web/API/CustomEvent
        //Not used 
        var evt = document.createEvent(eventType);
        evt.initEvent(eventName);
        return element.dispatchEvent(evt);
    }
    static addDomEventListener(element, eventName, preventDefault, invoker) {
        let callback = args => {
            const obj = {};
            for (let k in args) {
                if (k !== 'originalTarget') { //firefox occasionally raises Permission Denied when this property is being stringified
                    obj[k] = args[k];
                }
            }
            let json = JSON.stringify(obj, (k, v) => {
                if (v instanceof Node)
                    return 'Node';
                if (v instanceof Window)
                    return 'Window';
                return v;
            }, ' ');
            setTimeout(function () { invoker.invokeMethodAsync('Invoke', json); }, 0);
            if (preventDefault === true) {
                args.preventDefault();
            }
        };
        if (element == 'window') {
            if (eventName == 'resize') {
                window.addEventListener(eventName, this.debounce(() => callback({ innerWidth: window.innerWidth, innerHeight: window.innerHeight }), 200, false));
            }
            else {
                window.addEventListener(eventName, callback);
            }
        }
        else {
            let dom = domInfoHelper.get(element);
            if (dom) {
                dom.addEventListener(eventName, callback);
            }
        }
    }
    static addDomEventListenerToFirstChild(element, eventName, preventDefault, invoker) {
        var dom = domInfoHelper.get(element);
        if (dom && dom.firstElementChild) {
            this.addDomEventListener(dom.firstElementChild, eventName, preventDefault, invoker);
        }
    }
    static addPreventKeys(inputElement, keys) {
        if (inputElement) {
            let dom = domInfoHelper.get(inputElement);
            keys = keys.map(function (x) { return x.toUpperCase(); });
            state.eventCallbackRegistry[inputElement.id + "keydown"] = (e) => this.preventKeys(e, keys);
            dom.addEventListener("keydown", state.eventCallbackRegistry[inputElement.id + "keydown"], false);
        }
    }
    static preventKeyOnCondition(e, key, check) {
        if (e.key.toUpperCase() === key.toUpperCase() && check()) {
            e.preventDefault();
            return false;
        }
    }
    static removePreventKeys(inputElement) {
        if (inputElement) {
            let dom = domInfoHelper.get(inputElement);
            if (dom) {
                dom.removeEventListener("keydown", state.eventCallbackRegistry[inputElement.id + "keydown"]);
                state.eventCallbackRegistry[inputElement.id + "keydown"] = null;
            }
        }
    }
    static debounce(func, wait, immediate) {
        var timeout;
        return () => {
            const context = this, args = arguments;
            const later = () => {
                timeout = null;
                if (!immediate)
                    func.apply(this, args);
            };
            const callNow = immediate && !timeout;
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
            if (callNow)
                func.apply(context, args);
        };
    }
    ;
    static preventKeys(e, keys) {
        if (keys.indexOf(e.key.toUpperCase()) !== -1) {
            e.preventDefault();
            return false;
        }
    }
}
