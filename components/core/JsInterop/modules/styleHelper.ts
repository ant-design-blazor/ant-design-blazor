import { infoHelper as domInfoHelper } from './dom/infoHelper';

export class styleHelper {
  static addCls(selector: Element | string, className: string | Array<string>) {
    const element = domInfoHelper.get(selector);
    if (element) {
      if (typeof className === "string") {
        element.classList.add(className);
      } else {
        element.classList.add(...className);
      }
    }
  }

  static removeCls(selector: Element | string, clsName: string | Array<string>) {
    const element = domInfoHelper.get(selector);
    if (element) {
      if (typeof clsName === "string") {
        element.classList.remove(clsName);
      } else {
        element.classList.remove(...clsName);
      }
    }
  }

  static addClsToFirstChild(element: Element | string, className: string): void {
    const domElement = domInfoHelper.get(element);
    if (domElement && domElement.firstElementChild) {
      domElement.firstElementChild.classList.add(className);
    }
  }

  static removeClsFromFirstChild(element: Element | string, className): void {
    const domElement = domInfoHelper.get(element);
    if (domElement && domElement.firstElementChild) {
      domElement.firstElementChild.classList.remove(className);
    }
  }

  static addSpaceCompactClasses(root: Element, itemClasses: string[]): void {
    const components: [string, string][] = [
      [".ant-btn", "ant-btn"],
      [".ant-input, .ant-input-search", "ant-input"],
      [".ant-input-number", "ant-input-number"],
      [".ant-select", "ant-select"],
      [".ant-picker", "ant-picker"],
      [".ant-cascader-picker", "ant-cascader"],
    ];

    components.forEach(([selector, prefix]) => {
      root.querySelectorAll(selector).forEach((element) => {
        if (element.matches(".ant-input") && element.closest(".ant-input-search")) {
          return;
        }
        const compactClasses = [
          "compact-item",
          "compact-first-item",
          "compact-last-item",
          "compact-horizontal-item",
          "compact-horizontal-first-item",
          "compact-horizontal-last-item",
          "compact-vertical-item",
          "compact-vertical-first-item",
          "compact-vertical-last-item",
          "compact-item-rtl",
        ].map((cls) => `${prefix}-${cls}`);
        element.classList.remove(...compactClasses);
        element.classList.add(...itemClasses.map((cls) => `${prefix}-${cls}`));
      });
    });
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
      if (value === null) {
        const style = name;
        const cssAttributes = style.split(";");
        for (let i = 0; i < cssAttributes.length; i++) {
          const cssAttribute = cssAttributes[i];
          if (!cssAttribute) continue;
          const attribute = cssAttribute.split(":");
          element.style.setProperty(attribute[0], attribute[1]);
        }
        return;
      }
      element.style.setProperty(name, value);
    } else {
      for (const key in name) {
        if (Object.prototype.hasOwnProperty.call(name, key)) {
          element.style.setProperty(key, name[key]);
        }
      }
    }
  }
}
