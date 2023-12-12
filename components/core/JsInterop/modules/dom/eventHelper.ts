import { domInfoHelper } from './exports';
import { state } from '../stateProvider';

export class eventHelper {
  static triggerEvent(element: HTMLInputElement, eventType: string, eventName: string) {
    //TODO: replace with event constructors https://developer.mozilla.org/en-US/docs/Web/API/CustomEvent
    //Not used 
    const evt = document.createEvent(eventType);
    evt.initEvent(eventName);
    return element.dispatchEvent(evt);
  }

    static addDomEventListener(element, eventName: string, preventDefault: boolean, invoker: any, stopPropagation: boolean = false) {
    const callback = args => {
      const obj = {};
      for (let k in args) {
        if (k !== 'originalTarget') { //firefox occasionally raises Permission Denied when this property is being stringified
          obj[k] = args[k];
        }
      }
      const json = JSON.stringify(obj, (k, v) => {
        if (v instanceof Node) return 'Node';
        if (v instanceof Window) return 'Window';
        return v;
      }, ' ');

      setTimeout(function () { invoker.invokeMethodAsync('Invoke', json) }, 0);
      if (preventDefault === true) {
        args.preventDefault();
      }
      if (stopPropagation) {
          args.stopPropagation();
      }
    };

    const dom = domInfoHelper.get(element);
    const key = `${eventName}-${invoker._id}`;

    if (eventName === 'resize') {
      dom[`e_${key}`] = this.debounce(() => callback({ innerWidth: window.innerWidth, innerHeight: window.innerHeight }), 200, false);
    } else {
      dom[`e_${key}`] = callback;
    }

    dom[`i_${key}`] = invoker;
    (dom as HTMLElement).addEventListener(eventName, dom[`e_${key}`]);
  }

  static addDomEventListenerToFirstChild(element, eventName, preventDefault, invoker) {
    const dom = domInfoHelper.get(element);

    if (dom && dom.firstElementChild) {
      this.addDomEventListener(dom.firstElementChild, eventName, preventDefault, invoker);
    }
  }

  static removeDomEventListener(element, eventName: string, invoker) {
    const dom = domInfoHelper.get(element);
    const key = `${eventName}-${invoker._id}`;

    if (dom) {
      dom.removeEventListener(eventName, dom[`e_${key}`]);
      //dom[`i_${key}`].dispose();
    }
  }

  static addPreventKeys(inputElement, keys: string[]) {
    if (inputElement) {
      const dom = domInfoHelper.get(inputElement);
      keys = keys.map(function (x) { return x.toUpperCase(); })
      state.eventCallbackRegistry[inputElement.id + "keydown"] = (e) => this.preventKeys(e, keys);
      (dom as HTMLElement).addEventListener("keydown", state.eventCallbackRegistry[inputElement.id + "keydown"], false);
    }
  }

  static preventKeyOnCondition(e: KeyboardEvent, key: string, check: () => boolean) {
    if (e.key.toUpperCase() === key.toUpperCase() && check()) {
      e.preventDefault();
      return false;
    }
  }

  static removePreventKeys(inputElement) {
    if (inputElement) {
      const dom = domInfoHelper.get(inputElement);
      if (dom) {
        (dom as HTMLElement).removeEventListener("keydown", state.eventCallbackRegistry[inputElement.id + "keydown"]);
        state.eventCallbackRegistry[inputElement.id + "keydown"] = null;
      }
    }
  }

  private static debounce(func, wait, immediate) {
    var timeout;
    return (...args) => {
      const context = this;
      const later = () => {
        timeout = null;
        if (!immediate) func.apply(this, args);
      };
      const callNow = immediate && !timeout;
      clearTimeout(timeout);
      timeout = setTimeout(later, wait);
      if (callNow) func.apply(context, args);
    };
  };

  private static preventKeys(e: KeyboardEvent, keys: string[]) {
    if (keys.indexOf(e.key.toUpperCase()) !== -1) {
      e.preventDefault();
      return false;
    }
  }
}