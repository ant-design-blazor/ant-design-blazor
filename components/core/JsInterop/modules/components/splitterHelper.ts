const pointerdown = "pointerdown";
const pointermove = "pointermove";
const pointerup = "pointerup";
const touchstart = "touchstart";
const NULL = null;

export class splitterHelper {
  static attach(component, container) {
    const state = [0, 0, 0, 0, 0];
    let disposed = false;
    const splitter = container.querySelector(":scope > .spliter-bar");
    const panes = [...container.querySelectorAll(":scope > .pane-of-split-container")];
    const round = Math.round;
    const getPos = (ev) => round(state[0] === 0 ? ev.clientX : ev.clientY);
    const getSize = (element) => round(((rect) => state[0] === 0 ? rect.width : rect.height)(element.getBoundingClientRect()));
    const getSizeOfPane = (targetPaneIndex) => getSize(panes[targetPaneIndex]);
    const addEventListenerToSplitter = splitter.addEventListener.bind(splitter);
    const removeEventListenerToSplitter = splitter.removeEventListener.bind(splitter);
    const preventDefault = (ev) => ev.preventDefault();
    const dispose = () => {
        if (disposed)
            return;
        disposed = true;
        removeEventListenerToSplitter(pointerdown, onPointerDown);
        removeEventListenerToSplitter(pointerup, onPointerUp);
        removeEventListenerToSplitter(touchstart, preventDefault);
    };
    const updateSize = (ev) => {
        const targetPaneIndex = state[1];
        const resizeTarget = panes[targetPaneIndex] || NULL;
        if (resizeTarget === NULL)
            return [NULL, 0];
        const resizeTargetStyle = resizeTarget.style;
        const currentPos = getPos(ev);
        const delta = currentPos - state[2];
        const nextPxSize = (state[3] + (targetPaneIndex == 0 ? +1 : -1) * delta);
        const nextUnitSize = state[4] === 1 ?
            parseFloat(((nextPxSize + getSize(splitter) / 2) / getSize(container) * 100).toFixed(3)) :
            nextPxSize;
        const nextSize = state[4] === 1 ?
            `calc(${nextUnitSize}% - calc(var(--splitter-bar-size) / 2))` :
            nextUnitSize + "px";
        if (state[0] === 0)
            resizeTargetStyle.width = nextSize;
        else
            resizeTargetStyle.height = nextSize;
        return [resizeTarget, nextUnitSize];
    };
    const onPointerMove = (ev) => { updateSize(ev); };
    const onPointerDown = (ev) => {
        if (!document.contains(splitter)) {
            dispose();
            return;
        }
        const targetPaneIndex = panes.findIndex(p => p.style.flex === "");
        if (targetPaneIndex === -1)
            return;
        state[0] = container.classList.contains("splitter-orientation-vertical") ? 0 : 1;
        state[1] = targetPaneIndex;
        state[2] = getPos(ev);
        state[3] = getSizeOfPane(targetPaneIndex);
        state[4] = container.dataset.unitOfSize === "percent" ? 1 : 0;
        addEventListenerToSplitter(pointermove, onPointerMove);
        splitter.setPointerCapture(ev.pointerId);
    };
    const onPointerUp = (ev) => {
        splitter.releasePointerCapture(ev.pointerId);
        removeEventListenerToSplitter(pointermove, onPointerMove);
        const [resizeTarget, nextUnitSize] = updateSize(ev);
        component.invokeMethodAsync("UpdateSize", resizeTarget === panes[0], nextUnitSize);
    };
    addEventListenerToSplitter(pointerdown, onPointerDown);
    addEventListenerToSplitter(pointerup, onPointerUp);
    addEventListenerToSplitter(touchstart, preventDefault, { passive: false });
    return { dispose: dispose };
  }
}
