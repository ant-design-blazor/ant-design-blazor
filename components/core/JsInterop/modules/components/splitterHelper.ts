import { stat } from "fs";

const pointerdown = "pointerdown";
const pointermove = "pointermove";
const pointerup = "pointerup";
const touchstart = "touchstart";
const NULL = null;

export class splitterHelper {
  static attach(component, container) {
    const state = { isHorizontal: 0, targetPaneIndex: 0, pos: 0, sizeOfTargetPane: 0, sizeOfContainer: 0 };
    let disposed = false;
    const splitter = container.querySelector(":scope > .spliter-bar");
    const panes = [...container.querySelectorAll(":scope > .pane-of-split-container")];
    const round = Math.round;
    const getPos = (ev) => round(state.isHorizontal === 0 ? ev.clientX : ev.clientY);
    const getSize = (element) => round(((rect) => state.isHorizontal === 0 ? rect.width : rect.height)(element.getBoundingClientRect()));
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
        const targetPaneIndex = state.targetPaneIndex;
        const resizeTarget = panes[targetPaneIndex] || NULL;
        const otherPane = panes[targetPaneIndex == 0 ? 1 : 0] || NULL;
        if (resizeTarget === NULL || otherPane === NULL)
            return [0, 0];
        const resizeTargetStyle = resizeTarget.style;
        const currentPos = getPos(ev);
        const delta = currentPos - state.pos;
        const nextPxSize = (state.sizeOfTargetPane + (targetPaneIndex == 0 ? +1 : -1) * delta);
        const nextSize = nextPxSize + "px";
        const otherSize= (state.sizeOfContainer - nextPxSize) + "px";

        if (state.isHorizontal === 0){
            resizeTargetStyle.width = nextSize;
            otherPane.style.width = otherSize;
            return [nextSize, otherSize]
        } else {
            resizeTargetStyle.height = nextSize;
            otherPane.style.height = otherSize;
            return [otherSize, nextSize]
        }
    };
    const onPointerMove = (ev) => { updateSize(ev); };
    const onPointerDown = (ev) => {
        if (!document.contains(splitter)) {
            dispose();
            return;
        }
        if (splitter.classList.contains('ant-splitter-bar-dragger-disabled')){
            return;
        }
        const targetPaneIndex = panes.findIndex(p => p.style.flex === "");
        if (targetPaneIndex === -1)
            return;
        state.isHorizontal = container.classList.contains("splitter-orientation-vertical") ? 0 : 1;
        state.targetPaneIndex = targetPaneIndex;
        state.pos = getPos(ev);
        state.sizeOfTargetPane = getSizeOfPane(targetPaneIndex);
        state.sizeOfContainer = getSize(container);
        addEventListenerToSplitter(pointermove, onPointerMove);
        splitter.setPointerCapture(ev.pointerId);
    };
    const onPointerUp = (ev) => {
        splitter.releasePointerCapture(ev.pointerId);
        removeEventListenerToSplitter(pointermove, onPointerMove);
        const paneSizes = updateSize(ev);
        console.log(paneSizes);
        component.invokeMethodAsync("UpdateSize", paneSizes);
    };
    addEventListenerToSplitter(pointerdown, onPointerDown);
    addEventListenerToSplitter(pointerup, onPointerUp);
    addEventListenerToSplitter(touchstart, preventDefault, { passive: false });
    return { dispose: dispose };
  }
}
