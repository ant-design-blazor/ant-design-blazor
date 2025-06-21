import { domTypes } from './exports'
import { manipulationHelper } from './manipulationHelper'

export class infoHelper {

  static getWindow() {
    return {
      innerWidth: window.innerWidth,
      innerHeight: window.innerHeight
    };
  }

  static get(element: any) {
    if (!element) {
      element = document.body;
    } else if (typeof element === 'string') {
      if (element === 'window') {
        return window;
      } else if (element === 'document') {
        return document;
      }
      element = document.querySelector(element!)
    }
    return element;
  }

  static getInfo(element: any): domTypes.domInfo {
    let domElement = this.get(element);
    if (!domElement) {
      domElement = {};
    }
    const absolutePosition = this.getElementAbsolutePos(domElement);
    let elementMarginTop = 0;
    let elementMarginBottom = 0;
    let elementMarginLeft = 0;
    let elementMarginRight = 0;
    if (domElement instanceof HTMLElement) {
      elementMarginTop = manipulationHelper.parseNumericValue(domElement, 'marginTop');
      elementMarginBottom = manipulationHelper.parseNumericValue(domElement, 'marginBottom');
      elementMarginLeft = manipulationHelper.parseNumericValue(domElement, 'marginLeft');
      elementMarginRight = manipulationHelper.parseNumericValue(domElement, 'marginRight');
    }

    const result: domTypes.domInfo = {
      offsetTop: domElement.offsetTop || 0,
      offsetLeft: domElement.offsetLeft || 0,
      offsetWidth: domElement.offsetWidth || 0,
      offsetHeight: domElement.offsetHeight || 0,
      scrollHeight: domElement.scrollHeight || 0,
      scrollWidth: domElement.scrollWidth || 0,
      scrollLeft: domElement.scrollLeft || 0,
      scrollTop: domElement.scrollTop || 0,
      clientTop: domElement.clientTop || 0,
      clientLeft: domElement.clientLeft || 0,
      clientHeight: domElement.clientHeight || 0,
      clientWidth: domElement.clientWidth || 0,
      selectionStart: domElement.selectionStart || 0,
      absoluteTop: Math.round(absolutePosition.y),
      absoluteLeft: Math.round(absolutePosition.x),
      marginTop: elementMarginTop,
      marginBottom: elementMarginBottom,
      marginLeft: elementMarginLeft,
      marginRight: elementMarginRight
    };
    return result;
  }

  static getElementAbsolutePos(element: any): domTypes.position {
    const res: domTypes.position = {
      x: 0,
      y: 0
    };
    if (element !== null) {
      if (element.getBoundingClientRect) {
        const viewportElement = document.documentElement;
        const box = element.getBoundingClientRect();
        const scrollLeft = viewportElement.scrollLeft;
        const scrollTop = viewportElement.scrollTop;

        res.x = box.left + scrollLeft;
        res.y = box.top + scrollTop;
      }
    }
    return res;
  }

  static getBoundingClientRect(element: any): domTypes.domRect {
    const domElement = this.get(element);
    if (domElement && domElement.getBoundingClientRect) {
      const rect = domElement.getBoundingClientRect();
      // Fixes #1468. This wrapping is necessary for old browsers. Remove this when one day we no longer support them.
      return {
        width: rect.width,
        height: rect.height,
        top: rect.top,
        right: rect.right,
        bottom: rect.bottom,
        left: rect.left,
        x: rect.x,
        y: rect.y
      };
    }
    return null;
  }

  static getFirstChildDomInfo(element: any) {
    const domElement = this.get(element);
    if (domElement) {
      if (domElement.firstElementChild) {
        return this.getInfo(domElement.firstElementChild);
      } else {
        return this.getInfo(domElement);
      }
    }
    return null;
  }

  static getActiveElement() {
    const element = document.activeElement;
    const id = element.getAttribute("id") || "";
    return id;
  }

  static getScroll() {
    return { x: window.pageXOffset, y: window.pageYOffset };
  }

  static hasFocus(selector) {
    const dom = this.get(selector);
    return (document.activeElement === dom);
  }

  static getInnerText(element) {
    const dom = this.get(element);
    if (dom) return dom.innerText;
    return null;
  }

  static getMaxZIndex(): number {
    return Array.from(document.querySelectorAll("*")).reduce((r, e) => Math.max(r, +window.getComputedStyle(e).zIndex || 0), 0)
  }

  static isFixedPosition(element) {
    let node = this.get(element);
    while (node && node.nodeName.toLowerCase() !== 'body') {
      if (window.getComputedStyle(node).getPropertyValue('position').toLowerCase() === 'fixed') { return true; }
      node = node.parentNode;
    }
    return false;
  }

  static findAncestorWithZIndex(element: HTMLElement): number {
    let node = this.get(element);
    let zIndexAsString: string;
    let zIndex: number;
    while (node && node.nodeName.toLowerCase() !== 'body') {
      zIndexAsString = window.getComputedStyle(node).zIndex;
      zIndex = Number.parseInt(zIndexAsString);
      if (!Number.isNaN(zIndex)) {
        return zIndex;
      }
      node = node.parentNode;
    }
    return null;
  }

  static getElementsInfo(elements: any[]): any {
    const infos = {};
    elements.forEach(el => {
      infos[el.id] = this.getInfo(el);
    })

    return infos;
  }

  /**
   * Get all scrollable parents of an element
   * @param element
   * @returns
   */
  static getScrollableParents(element): HTMLElement[] {
    const parents = [];
    let node = this.get(element);

    while (node && node.nodeName.toLowerCase() !== 'body') {
      const overflowY = window.getComputedStyle(node).overflowY;
      if (overflowY === 'auto' || overflowY === 'scroll') {
        parents.push(node);
      }
      node = node.parentNode;
    }
    return parents;
  }
}