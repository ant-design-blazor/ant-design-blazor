const throttle = (fn, threshold = 160) => {
  let timeout;
  let start = +new Date();
  return function (...args) {
    // eslint-disable-next-line @typescript-eslint/no-this-alias
    const context = this,
      curTime = +new Date() - 0;
    //总是干掉事件回调
    window.clearTimeout(timeout);
    if (curTime - start >= threshold) {
      //只执行一部分方法，这些方法是在某个时间段内执行一次
      fn.apply(context, args);
      start = curTime;
    } else {
      //让方法在脱离事件后也能执行一次
      timeout = window.setTimeout(() => {
        fn.apply(this, args);
      }, threshold);
    }
  };
};

const eventMap = new Map<HTMLElement, Dragger>();

const defaultOptions = {
  inViewport: true,
};

interface DraggerState {
  isInDrag: boolean;
  // mouse x of once mouseDown event
  mouseDownX: number;
  // mouse y of once mouseDown event
  mouseDownY: number;

  // during onec drag event,
  // the offset from the original x during mouseDown
  mouseDownXOffset: number;
  // during onec drag event,
  // the offset from the original x during mouseDown
  mouseDownYOffset: number;

  // the gap between dragged element and dragged element's container
  bound: {
    left: number;
    top: number;
    right: number;
    bottom: number;
  };
}

class Dragger {
  // Element of drag control
  private _trigger: HTMLElement;
  // dragged element
  private _container: HTMLElement;
  // The container where the dragged element is located
  private _draggedInContainer: HTMLElement;

  private _options: any;
  private _state: DraggerState;
  private _isFirst: boolean = true;
  private _style: string | null = null;

  constructor(
    trigger: HTMLElement,
    container: HTMLElement,
    dragInViewport: boolean = true,
    draggedIn: HTMLElement = document.documentElement
  ) {
    this._trigger = trigger;
    this._container = container;
    this._draggedInContainer = draggedIn;
    this._options = Object.assign({}, defaultOptions, {
      inViewport: dragInViewport,
    });
    this._state = {
      isInDrag: false,
      mouseDownX: 0,
      mouseDownY: 0,
      mouseDownXOffset: 0,
      mouseDownYOffset: 0,
      bound: {
        left: 0,
        top: 0,
        right: 0,
        bottom: 0,
      },
    };
  }

  /**
   * get xOffset and yOffset from container.style.translate
   * @returns [xOffset, yOffset]
   */
  getContainerTranslateOffset() {
    const translate = this._container.style.translate;
    let xOffset = 0;
    let yOffset = 0;
    if (translate) {
      const translateInfo = translate.split(" ");
      xOffset = parseInt(translateInfo[0]);
      yOffset = parseInt(translateInfo[1]);
      xOffset = Number.isNaN(xOffset) ? 0 : xOffset;
      yOffset = Number.isNaN(yOffset) ? 0 : yOffset;
    }
    return {
      xOffset: xOffset,
      yOffset: yOffset,
    };
  }

  onMousedown = (e) => {
    const state = this._state;
    state.isInDrag = true;
    state.mouseDownX = e.clientX;
    state.mouseDownY = e.clientY;
    const { xOffset, yOffset } = this.getContainerTranslateOffset();

    if (this._isFirst) {
      state.bound = getBoundPosition(this._container, this._draggedInContainer);
      if (!this._style) {
        this._style = this._container.getAttribute("style");
      }

      this._isFirst = false;
    }

    state.mouseDownXOffset = xOffset;
    state.mouseDownYOffset = yOffset;
  };

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  onMouseup = (e) => {
    const state = this._state;
    state.isInDrag = false;
  };

  onMousemove = throttle((e) => {
    const state = this._state;
    if (state.isInDrag) {
      const nowX = e.clientX, nowY = e.clientY;
      let offsetX = nowX - state.mouseDownX + state.mouseDownXOffset,
        offsetY = nowY - state.mouseDownY + state.mouseDownYOffset;

      if (this._options.inViewport) {
        if (offsetX < state.bound.left) {
          offsetX = state.bound.left;
        } else if (offsetX > state.bound.right) {
          offsetX = state.bound.right;
        }
        if (offsetY < state.bound.top) {
          offsetY = state.bound.top;
        } else if (offsetY > state.bound.bottom) {
          offsetY = state.bound.bottom;
        }
      }
      this._container.style.translate = `${offsetX}px ${offsetY}px`;
    }
  }, 10).bind(this);

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  onResize = throttle((e) => {
    this._state.bound = getBoundPosition(
      this._container,
      this._draggedInContainer
    );
  }, 30).bind(this);

  bindDrag() {
    const trigger = this._trigger;
    const options = this._options;

    trigger.addEventListener("mousedown", this.onMousedown, false);
    window.addEventListener("mouseup", this.onMouseup, false);
    document.addEventListener("mousemove", this.onMousemove);
    if (options.inViewport) {
      window.addEventListener("resize", this.onResize, false);
    }
  }

  unbindDrag() {
    const trigger = this._trigger;

    trigger.removeEventListener("mousedown", this.onMousedown, false);
    window.removeEventListener("mouseup", this.onMouseup, false);
    document.removeEventListener("mousemove", this.onMousemove);
    if (this._options.inViewport) {
      window.removeEventListener("resize", this.onResize, false);
    }
  }

  resetContainerStyle() {
    if (this._style !== null) {
      this._isFirst = true;
      this._container.setAttribute("style", this._style);
    }
  }
}

/**
 *
 * @param childNode The child of parentNode
 * @param parentNode The container of childNode
 * @returns
 */
function getBoundPosition(childNode: HTMLElement, parentNode: HTMLElement) {
  const childComputedStyle = window.getComputedStyle(parentNode);
  const parentComputedStyle = window.getComputedStyle(parentNode);

  const bounds = {
    left:
      -childNode.offsetLeft +
      parseInt(childComputedStyle.marginLeft) +
      parseInt(parentComputedStyle.paddingLeft),
    top:
      -childNode.offsetTop +
      parseInt(childComputedStyle.marginTop) +
      parseInt(parentComputedStyle.paddingTop),
    right:
      innerWidth(parentNode, parentComputedStyle) -
      outerWidth(childNode, childComputedStyle) -
      childNode.offsetLeft +
      parseInt(parentComputedStyle.paddingRight) -
      parseInt(childComputedStyle.marginRight),
    bottom:
      innerHeight(parentNode, parentComputedStyle) -
      outerHeight(childNode, childComputedStyle) -
      childNode.offsetTop +
      parseInt(parentComputedStyle.paddingBottom) -
      parseInt(childComputedStyle.marginBottom),
  };

  return bounds;
}

function outerHeight(
  node: HTMLElement,
  computedStyle: CSSStyleDeclaration | null = null
): number {
  if (!computedStyle) {
    computedStyle = window.getComputedStyle(node);
  }
  let height = node.clientHeight;
  height += parseInt(computedStyle.borderTopWidth);
  height += parseInt(computedStyle.borderBottomWidth);
  return height;
}

function outerWidth(
  node: HTMLElement,
  computedStyle: CSSStyleDeclaration | null = null
): number {
  if (!computedStyle) {
    computedStyle = window.getComputedStyle(node);
  }
  let width = node.clientWidth;
  width += parseInt(computedStyle.borderLeftWidth);
  width += parseInt(computedStyle.borderRightWidth);
  return width;
}

function innerHeight(
  node: HTMLElement,
  computedStyle: CSSStyleDeclaration | null = null
): number {
  if (!computedStyle) {
    computedStyle = window.getComputedStyle(node);
  }
  let height = node.clientHeight;
  height -= parseInt(computedStyle.paddingTop);
  height -= parseInt(computedStyle.paddingBottom);
  return height;
}

function innerWidth(
  node: HTMLElement,
  computedStyle: CSSStyleDeclaration | null = null
): number {
  if (!computedStyle) {
    computedStyle = window.getComputedStyle(node);
  }
  let width = node.clientWidth;
  width -= parseInt(computedStyle.paddingLeft);
  width -= parseInt(computedStyle.paddingRight);
  return width;
}

function enableDraggable(
  trigger: HTMLElement,
  container: HTMLElement,
  dragInViewport: boolean = true,
  draggedIn: HTMLElement = document.documentElement
) {
  let dragger = eventMap.get(trigger);
  if (!dragger) {
    dragger = new Dragger(trigger, container, dragInViewport, draggedIn);
    eventMap.set(trigger, dragger);
  }
  dragger.bindDrag();
}

function disableDraggable(trigger: HTMLElement) {
  const dragger = eventMap.get(trigger);
  if (dragger) {
    dragger.unbindDrag();
  }
}

function resetModalPosition(trigger: HTMLElement) {
  const dragger = eventMap.get(trigger);
  if (dragger) {
    dragger.resetContainerStyle();
  }
}

export { enableDraggable, disableDraggable, resetModalPosition };
