const throttle = (fn, threshold = 160) => {
    let timeout;
    var start = +new Date;
    return function (...args) {
        let context = this, curTime = +new Date() - 0;
        //总是干掉事件回调
        window.clearTimeout(timeout);
        if (curTime - start >= threshold) {
            //只执行一部分方法，这些方法是在某个时间段内执行一次
            fn.apply(context, args);
            start = curTime;
        }
        else {
            //让方法在脱离事件后也能执行一次
            timeout = window.setTimeout(() => {
                //@ts-ignore
                fn.apply(this, args);
            }, threshold);
        }
    };
};
const eventMap = new Map();
const defaultOptions = {
    inViewport: true
};
class Dragger {
    constructor(triggler, container, dragInViewport) {
        this._triggler = null;
        this._container = null;
        this._options = null;
        this._state = null;
        this._isFirst = true;
        this._style = null;
        this.onMousedown = (e) => {
            const state = this._state;
            state.isInDrag = true;
            state.mX = e.clientX;
            state.mY = e.clientY;
            this._container.style.position = "absolute";
            const { left, top } = this.getContainerPos();
            if (this._isFirst) {
                state.domMaxY = document.documentElement.clientHeight
                    - this._container.offsetHeight - 1;
                state.domMaxX = document.documentElement.clientWidth
                    - this._container.offsetWidth - 1;
                this._container.style.left = left + 'px';
                this._container.style.top = top + 'px';
                if (!this._style) {
                    this._style = this._container.getAttribute("style");
                }
                this._isFirst = false;
            }
            state.domStartX = left;
            state.domStartY = top;
        };
        this.onMouseup = (e) => {
            const state = this._state;
            state.isInDrag = false;
            const { left, top } = this.getContainerPos();
            state.domStartX = left;
            state.domStartY = top;
        };
        this.onMousemove = throttle((e) => {
            const state = this._state;
            if (state.isInDrag) {
                var nowX = e.clientX, nowY = e.clientY, disX = nowX - state.mX, disY = nowY - state.mY;
                var newDomX = state.domStartX + disX;
                var newDomY = state.domStartY + disY;
                if (this._options.inViewport) {
                    if (newDomX < 0) {
                        newDomX = 0;
                    }
                    else if (newDomX > state.domMaxX) {
                        newDomX = state.domMaxX;
                    }
                    if (newDomY < 0) {
                        newDomY = 0;
                    }
                    else if (newDomY > state.domMaxY) {
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
        this.onResize = throttle((e) => {
            const state = this._state;
            state.domMaxY = document.documentElement.clientHeight
                - this._container.offsetHeight - 1;
            state.domMaxX = document.documentElement.clientWidth
                - this._container.offsetWidth - 1;
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
        this._triggler = triggler;
        this._container = container;
        this._options = Object.assign({}, defaultOptions, {
            inViewport: dragInViewport
        });
        this._state = {
            isInDrag: false,
            mX: 0,
            mY: 0,
            domStartX: 0,
            domStartY: 0,
        };
    }
    getContainerPos() {
        const rect = this._container.getBoundingClientRect();
        return {
            left: rect.left,
            top: rect.top
        };
    }
    bindDrag() {
        const triggler = this._triggler;
        const options = this._options;
        triggler.addEventListener("mousedown", this.onMousedown, false);
        window.addEventListener("mouseup", this.onMouseup, false);
        document.addEventListener("mousemove", this.onMousemove);
        if (options.inViewport) {
            window.addEventListener("resize", this.onResize, false);
        }
    }
    unbindDrag() {
        const triggler = this._triggler;
        triggler.removeEventListener("mousedown", this.onMousedown, false);
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
function enableDraggable(triggler, container, dragInViewport = true) {
    let dragger = eventMap.get(triggler);
    if (!dragger) {
        dragger = new Dragger(triggler, container, dragInViewport);
        eventMap.set(triggler, dragger);
    }
    dragger.bindDrag();
}
function disableDraggable(triggler) {
    const dragger = eventMap.get(triggler);
    if (dragger) {
        dragger.unbindDrag();
    }
}
function resetModalPosition(triggler) {
    const dragger = eventMap.get(triggler);
    if (dragger) {
        dragger.resetContainerStyle();
    }
}
export { enableDraggable, disableDraggable, resetModalPosition };
