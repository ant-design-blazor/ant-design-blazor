const pointerdown = "pointerdown";
const pointermove = "pointermove";
const pointerup = "pointerup";
const touchstart = "touchstart";
const NULL = null;

export class splitterHelper {
  static attach(component, container) {
    const state = { isHorizontal: 0, targetPaneIndex: 0, pos: 0, sizeOfTargetPane: 0, sizeOfContainer: 0 };
    let disposed = false;
    const splitter = container.querySelector(":scope > .ant-splitter-bar > .spliter-bar");
    const panes = [...container.querySelectorAll(":scope > .pane-of-split-container")];
    const collapseBars = [...container.querySelectorAll(":scope > .ant-splitter-bar > .ant-splitter-bar-collapse-bar")];
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

    const updateButtonVisibility = (pos) => {
      const containerRect = container.getBoundingClientRect();
      const containerSize = state.isHorizontal === 0 ? containerRect.width : containerRect.height;
      const threshold = 2; // 2px threshold for edge detection

      // Calculate the actual position relative to the container
      const relativePos = state.isHorizontal === 0
        ? pos - containerRect.left
        : pos - containerRect.top;

      collapseBars.forEach((bar, index) => {
        if (index === 0) { // Left/Top button
          if (relativePos <= threshold) {
            bar.style.display = 'none';
          } else {
            bar.style.display = '';
          }
        } else { // Right/Bottom button
          if (relativePos >= containerSize - threshold) {
            bar.style.display = 'none';
          } else {
            bar.style.display = '';
          }
        }
      });
    };

    const updateSize = (ev) => {
      const targetPaneIndex = state.targetPaneIndex;
      const resizeTarget = panes[targetPaneIndex] || NULL;
      const otherPane = panes[targetPaneIndex == 0 ? 1 : 0] || NULL;
      if (resizeTarget === NULL || otherPane === NULL)
        return ["0px", "0px"];

      const resizeTargetStyle = resizeTarget.style;
      const currentPos = getPos(ev);
      const containerRect = container.getBoundingClientRect();
      const relativePos = state.isHorizontal === 0
        ? currentPos - containerRect.left
        : currentPos - containerRect.top;
      const delta = relativePos - state.pos;
      const nextPxSize = (state.sizeOfTargetPane + (targetPaneIndex == 0 ? +1 : -1) * delta);

      // Calculate min and max sizes based on container size
      const minSize = 0;
      const maxSize = state.sizeOfContainer;

      // If dragging beyond boundaries, return current sizes
      if (nextPxSize <= minSize || nextPxSize >= maxSize) {
        return [resizeTargetStyle.width || resizeTargetStyle.height, otherPane.style.width || otherPane.style.height];
      }

      const nextSize = nextPxSize + "px";
      const otherSize = (state.sizeOfContainer - nextPxSize) + "px";

      if (state.isHorizontal === 0) {
        resizeTargetStyle.width = nextSize;
        otherPane.style.width = otherSize;
        return [nextSize, otherSize];
      } else {
        resizeTargetStyle.height = nextSize;
        otherPane.style.height = otherSize;
        return [nextSize, otherSize];
      }
    };

    const onPointerMove = (ev) => {
      // Only update sizes in JavaScript during dragging
      updateSize(ev);
      const currentPos = getPos(ev);
      updateButtonVisibility(currentPos);
    };

    const onPointerDown = (ev) => {
      if (!document.contains(splitter)) {
        dispose();
        return;
      }
      if (splitter.classList.contains('ant-splitter-bar-dragger-disabled')) {
        return;
      }
      const targetPaneIndex = panes.findIndex(p => p.style.flex === "");
      if (targetPaneIndex === -1)
        return;
      state.isHorizontal = container.classList.contains("splitter-orientation-vertical") ? 0 : 1;
      state.targetPaneIndex = targetPaneIndex;
      const containerRect = container.getBoundingClientRect();
      const currentPos = getPos(ev);
      state.pos = state.isHorizontal === 0
        ? currentPos - containerRect.left
        : currentPos - containerRect.top;
      state.sizeOfTargetPane = getSizeOfPane(targetPaneIndex);
      state.sizeOfContainer = getSize(container);
      addEventListenerToSplitter(pointermove, onPointerMove);
      splitter.setPointerCapture(ev.pointerId);
    };

    const onPointerUp = (ev) => {
      splitter.releasePointerCapture(ev.pointerId);
      removeEventListenerToSplitter(pointermove, onPointerMove);
      // Get final sizes and sync back to C#
      const sizes = updateSize(ev);
      const currentPos = getPos(ev);
      updateButtonVisibility(currentPos);
      // Only invoke C# method when drag ends
      component.invokeMethodAsync("UpdateSize", sizes);
    };

    addEventListenerToSplitter(pointerdown, onPointerDown);
    addEventListenerToSplitter(pointerup, onPointerUp);
    addEventListenerToSplitter(touchstart, preventDefault, { passive: false });
    return { dispose: dispose };
  }
}
