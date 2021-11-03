import { infoHelper } from "./infoHelper";
import { eventHelper } from "./eventHelper";

const getDom = infoHelper.get;
const throttle = eventHelper.throttle;


const eventMap = new Map<HTMLElement, Dragger>();

const defaultOptions = {
  inViewport: true,
};

class Dragger {
  private _trigger: HTMLElement;
  private _container: HTMLElement;
  private _options: any = null;
  private _state: any = null;
  private _isFirst: boolean = true;
  private _style: string | null = null;

  constructor(
    trigger: HTMLElement,
    container: HTMLElement,
    dragInViewport: boolean
  ) {
    this._trigger = trigger;
    this._container = container;
    this._options = Object.assign({}, defaultOptions, {
      inViewport: dragInViewport,
    });
    this._state = {
      isInDrag: false,
      mX: 0, // mouse x
      mY: 0, // mouse y
      domStartX: 0, // on mousedown, the mouse x
      domStartY: 0, // on mousedown, the mouse y
    };
  }

  getContainerPos() {
    const rect = this._container.getBoundingClientRect();
    return {
      left: rect.left,
      top: rect.top,
    };
  }

  onMousedown = (e) => {
    e.stopPropagation = true;

    const state = this._state;
    state.isInDrag = true;
    state.mX = e.clientX;
    state.mY = e.clientY;
    const { left, top } = this.getContainerPos();
    this._container.style.position = "absolute";

    if (this._isFirst) {
      state.domMaxY =
        document.documentElement.clientHeight -
        this._container.offsetHeight -
        1;
      state.domMaxX =
        document.documentElement.clientWidth - this._container.offsetWidth - 1;
      state.domMaxY = state.domMaxY < 0 ? 0 : state.domMaxY;
      state.domMaxX = state.domMaxX < 0 ? 0 : state.domMaxX;

      this._container.style.left = left + "px";
      this._container.style.top = top + "px";

      if (!this._style) {
        this._style = this._container.getAttribute("style");
      }

      this._isFirst = false;
    }

    state.domStartX = left;
    state.domStartY = top;
  };

  onMouseup = (e) => {
    e.stopPropagation = true;

    const state = this._state;

    state.isInDrag = false;

    const { left, top } = this.getContainerPos();
    state.domStartX = left;
    state.domStartY = top;
  };

  onMousemove = throttle((e) => {
    const state = this._state;
    if (state.isInDrag) {
      var nowX = e.clientX,
        nowY = e.clientY,
        disX = nowX - state.mX,
        disY = nowY - state.mY;

      var newDomX = state.domStartX + disX;
      var newDomY = state.domStartY + disY;
      if (this._options.inViewport) {
        if (newDomX < 0) {
          newDomX = 0;
        } else if (newDomX > state.domMaxX) {
          newDomX = state.domMaxX;
        }
        if (newDomY < 0) {
          newDomY = 0;
        } else if (newDomY > state.domMaxY) {
          newDomY = state.domMaxY;
        }
      }
      this._container.style.position = "absolute";
      this._container.style.margin = "0";
      this._container.style.paddingBottom = "0";
      this._container.style.left = newDomX + "px";
      this._container.style.top = newDomY + "px";
    }
  }, 10).bind(this);

  onResize = throttle((e) => {
    const state = this._state;

    state.domMaxY =
      document.documentElement.clientHeight - this._container.offsetHeight - 1;
    state.domMaxX =
      document.documentElement.clientWidth - this._container.offsetWidth - 1;
    state.domMaxY = state.domMaxY < 0 ? 0 : state.domMaxY;
    state.domMaxX = state.domMaxX < 0 ? 0 : state.domMaxX;

    state.domStartY = parseInt(this._container.style.top);
    state.domStartX = parseInt(this._container.style.left);
    if (state.domStartY > state.domMaxY) {
      if (state.domMaxY > 0) {
        this._container.style.top = state.domMaxY + "px";
      }
    }
    if (state.domStartX > state.domMaxX) {
      this._container.style.left = state.domMaxX + "px";
    }
  }, 10).bind(this);

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


function enableDraggable(
  trigger: HTMLElement | string,
  container: HTMLElement | string,
  dragInViewport: boolean = true
) {
  let triggerEle = getDom(trigger);
  if (triggerEle != null) {
    let dragger = eventMap.get(triggerEle);
    if (!dragger) {
      dragger = new Dragger(triggerEle, getDom(container)!, dragInViewport);
      eventMap.set(triggerEle, dragger);
    }
    dragger.bindDrag();
  }
}


function disableDraggable(trigger: HTMLElement | string) {
  let triggerEle = getDom(trigger);
  if (triggerEle != null) {
    const dragger = eventMap.get(triggerEle);
    if (dragger) {
      dragger.unbindDrag();
    }
  }
}


function resetModalPosition(trigger: HTMLElement | string) {
  let triggerEle = getDom(trigger);
  if (triggerEle != null) {
    const dragger = eventMap.get(triggerEle);
    if (dragger) {
      dragger.resetContainerStyle();
    }
  }
}

export { enableDraggable, disableDraggable, resetModalPosition };
