import { domInfoHelper } from "./exports";
import { styleHelper } from "../styleHelper";
import { state } from "../stateProvider";
import * as enums from "../enums";

let cachedScrollBarSize: number | undefined = undefined;
const scrollIds = new Map<HTMLElement, number>();

export class manipulationHelper {
  /**
   * Safely parse CSS numeric values, returning 0 for "auto" or invalid values
   * @param element The HTML element to get CSS property from
   * @param property The CSS property name (e.g., 'marginTop', 'marginLeft')
   * @returns Parsed number or 0 if invalid
   */
  static parseNumericValue(element: HTMLElement, property: string): number;
  /**
   * Safely parse CSS numeric values, returning 0 for "auto" or invalid values
   * @param value CSS value to parse
   * @returns Parsed number or 0 if invalid
   */
  static parseNumericValue(value: string): number;
  static parseNumericValue(elementOrValue: HTMLElement | string, property?: string): number {
    // If first parameter is HTMLElement, get the style value and calculate
    if (elementOrValue instanceof HTMLElement && property) {
      const element = elementOrValue;
      const style = window.getComputedStyle(element);
      const value = style[property];

      // Check if the element's inline style has the property set to inherit
      // This is needed because in some test environments, getComputedStyle
      // returns empty string for inherit values
      const cssProperty = property.replace(/([A-Z])/g, '-$1').toLowerCase();
      const inlineValue = element.style[property] || element.style.getPropertyValue(cssProperty);
      if (inlineValue === 'inherit') {
        return this.calculateInheritValue(element, property);
      }

      if (!value) {
        return 0;
      }

      // Handle special CSS values
      if (value === 'auto') {
        return this.calculateAutoValue(element, property);
      } else if (value === 'inherit') {
        return this.calculateInheritValue(element, property);
      } else if (value === 'initial' || value === 'unset') {
        return 0;
      }

      const parsed = parseFloat(value);
      return isNaN(parsed) ? 0 : parsed;
    }

    // Legacy support: if first parameter is string, parse it directly
    if (typeof elementOrValue === 'string') {
      const value = elementOrValue;
      if (!value || value === 'auto' || value === 'inherit' || value === 'initial' || value === 'unset') {
        return 0;
      }

      const parsed = parseFloat(value);
      return isNaN(parsed) ? 0 : parsed;
    }

    return 0;
  }

  /**
   * Calculate the actual pixel value for CSS properties set to "auto"
   * @param element The HTML element
   * @param property The CSS property name (e.g., 'marginTop', 'marginLeft')
   * @returns The calculated pixel value or 0 if cannot be determined
   */
  private static calculateAutoValue(element: HTMLElement, property: string): number {
    try {
      const rect = element.getBoundingClientRect();
      const parent = element.parentElement;

      if (!parent) return 0;

      const parentRect = parent.getBoundingClientRect();

      switch (property) {
        case 'marginTop': {
          // Calculate actual top margin
          return Math.max(0, rect.top - parentRect.top - element.offsetTop);
        }

        case 'marginBottom': {
          // Calculate actual bottom margin
          const parentBottom = parentRect.bottom;
          const elementBottom = rect.bottom;
          return Math.max(0, parentBottom - elementBottom - (parent.offsetHeight - element.offsetTop - element.offsetHeight));
        }

        case 'marginLeft': {
          // For auto left margin, calculate actual left margin
          const parentLeft = parentRect.left;
          const elementLeft = rect.left;
          return Math.max(0, elementLeft - parentLeft - element.offsetLeft);
        }

        case 'marginRight': {
          // For auto right margin, calculate actual right margin
          const parentRight = parentRect.right;
          const elementRight = rect.right;
          return Math.max(0, parentRight - elementRight - (parent.offsetWidth - element.offsetLeft - element.offsetWidth));
        }

        default:
          return 0;
      }
    } catch (error) {
      // If calculation fails, return 0 as fallback
      return 0;
    }
  }

  /**
   * Calculate the actual pixel value for CSS properties set to "inherit"
   * @param element The HTML element
   * @param property The CSS property name
   * @returns The inherited value from parent element or 0 if cannot be determined
   */
  private static calculateInheritValue(element: HTMLElement, property: string): number {
    try {
      const parent = element.parentElement;
      if (!parent) return 0;

      // First try to get the parent's computed style value
      const parentStyle = window.getComputedStyle(parent);
      const parentValue = parentStyle[property];

      // If parent has a valid numeric value, parse it directly
      if (parentValue && parentValue !== 'inherit' && parentValue !== 'auto' && parentValue !== 'initial' && parentValue !== 'unset' && parentValue.trim() !== '') {
        const parsed = parseFloat(parentValue);
        if (!isNaN(parsed)) {
          return parsed;
        }
      }

      // If getComputedStyle doesn't work (empty string in test environments),
      // try to get the value from the parent's inline style
      if (parent instanceof HTMLElement && parent.style) {
        const inlineValue = parent.style[property];
        if (inlineValue && inlineValue !== 'inherit' && inlineValue !== 'auto' && inlineValue !== 'initial' && inlineValue !== 'unset') {
          const parsed = parseFloat(inlineValue);
          if (!isNaN(parsed)) {
            return parsed;
          }
        }
      }

      // If parent value is also inherit/auto/etc, recursively parse it
      if (parentValue === 'inherit' || parentValue === 'auto' || parentValue === 'initial' || parentValue === 'unset') {
        return this.parseNumericValue(parent, property);
      }

      // If we still can't get the parent value, return 0 as fallback
      return 0;
    } catch (error) {
      return 0;
    }
  }

  static addElementToBody(element) {
    document.body.appendChild(element);
  }

  static delElementFromBody(element) {
    document.body.removeChild(element);
  }

  static addElementTo(addElement, elementSelector, prepend = false): boolean {
    const parent = domInfoHelper.get(elementSelector);
    if (parent && addElement) {
      if (parent instanceof Node && addElement instanceof Node) {
        if (prepend) parent.insertBefore(addElement, parent.firstChild);
        else parent.appendChild(addElement);
        return true;
      } else {
        console.log("does not implement node", parent, addElement);
      }
    }
    return false;
  }

  static delElementFrom(delElementSelector, elementSelector, delay = 0) {
    setTimeout(() => {
      const delElement = domInfoHelper.get(delElementSelector);
      const parent = domInfoHelper.get(elementSelector);
      if (parent && delElement) {
        parent.removeChild(delElement);
      }
    }, delay);
  }

  static setDomAttribute(element, attributes) {
    const dom: HTMLElement = domInfoHelper.get(element);
    if (dom) {
      for (const key in attributes) {
        dom.setAttribute(key, attributes[key]);
      }
    }
  }

  static copyElement(element) {
    if (!this.copyElementAsRichText(element)) {
      this.copy(element.innerText);
    }
  }

  private static copyElementAsRichText(element) {
    const selection = document.getSelection();
    if (selection.rangeCount > 0) {
      selection.removeAllRanges();
    }
    const range = document.createRange();
    range.selectNode(element);
    selection.addRange(range);
    try {
      const successful = document.execCommand('copy');
      selection.removeAllRanges();
      return successful;
    } catch (err) {
      selection.removeAllRanges();
      return false;
    }
  }

  static copy(text) {
    if (!navigator.clipboard) {
      this.fallbackCopyTextToClipboard(text);
      return;
    }
    navigator.clipboard.writeText(text).then(
      function () {
        console.log("Async: Copying to clipboard was successful!");
      },
      function (err) {
        console.error("Async: Could not copy text: ", err);
      }
    );
  }

  private static fallbackCopyTextToClipboard(text) {
    const textArea = document.createElement("textarea");
    textArea.value = text;

    // Avoid scrolling to bottom
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.position = "fixed";

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
      const successful = document.execCommand('copy');
      const msg = successful ? 'successful' : 'unsuccessful';
      console.log('Fallback: Copying text command was ' + msg);
    } catch (err) {
      console.error("Fallback: Oops, unable to copy", err);
    }

    document.body.removeChild(textArea);
  }

  static focus(selector, noScroll: boolean = false, option: enums.FocusBehavior = enums.FocusBehavior.FocusAtLast) {
    const dom = domInfoHelper.get(selector);

    if (!(dom instanceof HTMLElement))
      throw new Error("Unable to focus on invalid element.");

    dom.focus({
      preventScroll: noScroll,
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
    const dom = domInfoHelper.get(selector);
    if (dom) {
      dom.blur();
    }
  }

  static scrollTo(selector: Element | string, parentElement?: HTMLElement) {
    const element = domInfoHelper.get(selector);
    if (parentElement && element && element instanceof HTMLElement) {
      parentElement.scrollTop = element.offsetTop;
    } else if (element && element instanceof HTMLElement) {
      element.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'start' });
    }
  }

  static smoothScrollTo(
    selector: Element | string,
    parentElement: HTMLElement,
    duration: number
  ) {
    const element = domInfoHelper.get(selector);
    const to = element.offsetTop;
    if (scrollIds.get(parentElement)) {
      cancelAnimationFrame(scrollIds.get(parentElement)!);
    }
    // jump to target if duration zero
    if (duration <= 0) {
      scrollIds.set(
        parentElement,
        requestAnimationFrame(() => {
          parentElement.scrollTop = to;
        }),
      );

      return;
    }
    const difference = to - parentElement.scrollTop;
    const perTick = (difference / duration) * 10;

    scrollIds.set(
      parentElement,
      requestAnimationFrame(() => {
        parentElement.scrollTop += perTick;
        if (parentElement.scrollTop !== to) {
          manipulationHelper.smoothScrollTo(selector, parentElement, duration - 10);
        }
      }),
    );
  }

  static slideTo(targetPageY) {
    const timer = setInterval(function () {
      const currentY =
        document.documentElement.scrollTop || document.body.scrollTop;
      const distance =
        targetPageY > currentY
          ? targetPageY - currentY
          : currentY - targetPageY;
      const speed = Math.ceil(distance / 10);
      if (currentY === targetPageY) {
        clearInterval(timer);
      } else {
        window.scrollTo(
          0,
          targetPageY > currentY ? currentY + speed : currentY - speed
        );
      }
    }, 10);
  }

  //copied from https://www.telerik.com/forums/trigger-tab-key-when-enter-key-is-pressed
  static invokeTabKey() {
    const currInput = document.activeElement;
    if (currInput.tagName.toLowerCase() == "input") {
      const inputs = document.getElementsByTagName("input");
      for (let i = 0; i < inputs.length; i++) {
        if (inputs[i] == currInput) {
          const next = inputs[i + 1];
          if (next && next.focus) {
            next.focus();
          }
          break;
        }
      }
    }
  }

  static disableBodyScroll() {
    const body = document.body;
    const oldBodyCache = {};
    ["position", "width", "overflow"].forEach((key) => {
      oldBodyCache[key] = body.style[key];
    });
    state.oldBodyCacheStack.push(oldBodyCache);
    const scrollBarSize = this.getScrollBarSize();
    styleHelper.css(body, {
      position: "relative",
      width:
        this.hasScrollbar() && scrollBarSize > 0
          ? `calc(100% - ${scrollBarSize}px)`
          : null,
      overflow: "hidden",
    });
    styleHelper.addCls(document.body, "ant-scrolling-effect");
  }

  static enableBodyScroll(force: boolean | undefined) {
    if (force) {
      state.oldBodyCacheStack = [];
    }

    const oldBodyCache = state.oldBodyCacheStack.length > 0 ? state.oldBodyCacheStack.pop() : {};
    styleHelper.css(document.body,
      {
        "position": oldBodyCache["position"] ?? null,
        "width": oldBodyCache["width"] ?? null,
        "overflow": oldBodyCache["overflow"] ?? null
      });
    styleHelper.removeCls(document.body, "ant-scrolling-effect");
  }

  static hasScrollbar = () => {
    const overflow = document.body.style.overflow;
    if (overflow && overflow === "hidden") return false;
    return (
      document.body.scrollHeight >
      (window.innerHeight || document.documentElement.clientHeight)
    );
  };

  /**
   * getScrollBarSize
   * source https://github.com/react-component/util/blob/master/src/getScrollBarSize.tsx
   *
   * @param fresh force get scrollBar size and don't use cache
   * @returns
   */
  static getScrollBarSize = (fresh: boolean = false) => {
    if (typeof document === "undefined") {
      return 0;
    }

    if (fresh || cachedScrollBarSize === undefined) {
      const inner = document.createElement("div");
      inner.style.width = "100%";
      inner.style.height = "200px";

      const outer = document.createElement("div");
      const outerStyle = outer.style;

      outerStyle.position = "absolute";
      outerStyle.top = "0";
      outerStyle.left = "0";
      outerStyle.pointerEvents = "none";
      outerStyle.visibility = "hidden";
      outerStyle.width = "200px";
      outerStyle.height = "150px";
      outerStyle.overflow = "hidden";

      outer.appendChild(inner);

      document.body.appendChild(outer);

      const widthContained = inner.offsetWidth;
      outer.style.overflow = "scroll";
      let widthScroll = inner.offsetWidth;

      if (widthContained === widthScroll) {
        widthScroll = outer.clientWidth;
      }

      document.body.removeChild(outer);
      cachedScrollBarSize = widthContained - widthScroll;
    }

    if (manipulationHelper.hasScrollbar()) {
      document.documentElement.style.setProperty('--ant-scrollbar-width', `${cachedScrollBarSize}px`);
    }

    return cachedScrollBarSize;
  };
}
