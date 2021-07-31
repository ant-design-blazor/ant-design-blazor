import { infoHelper as domInfoHelper } from './dom/infoHelper';

export class styleHelper {
  static addCls(selector: Element | string, className: string | Array<string>) {
    let element = domInfoHelper.get(selector);
    if (element) {
      if (typeof className === "string") {
        element.classList.add(className);
      } else {
        element.classList.add(...className);
      }
    }
  }

  static removeCls(selector: Element | string, clsName: string | Array<string>) {
    let element = domInfoHelper.get(selector);
    if (element) {
      if (typeof clsName === "string") {
        element.classList.remove(clsName);
      } else {
        element.classList.remove(...clsName);
      }
    }
  }

  static addClsToFirstChild(element: Element | string, className: string): void {
    var domElement = domInfoHelper.get(element);
    if (domElement && domElement.firstElementChild) {
      domElement.firstElementChild.classList.add(className);
    }
  }

  static removeClsFromFirstChild(element: Element | string, className): void {
    var domElement = domInfoHelper.get(element);
    if (domElement && domElement.firstElementChild) {
      domElement.firstElementChild.classList.remove(className);
    }
  }

  static matchMedia(query: string): boolean {
    return window.matchMedia(query).matches;
  }

  static getStyle(element, styleProp: string) {
    if (element.currentStyle)
      return element.currentStyle[styleProp];
    else if (window.getComputedStyle)
      return document.defaultView.getComputedStyle(element, null).getPropertyValue(styleProp);
  }

  //Referenced in Caret, class Mirror
  static css(element: HTMLElement, name: string | object, value: string | null = null) {
    if (typeof name === 'string') {
      element.style[name] = value;
    } else {
      for (let key in name) {
        if (name.hasOwnProperty(key)) {
          element.style[key] = name[key];
        }
      }
    }
  }

}