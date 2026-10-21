const pointerdown = "pointerdown";
const pointermove = "pointermove";
const pointerup = "pointerup";
const dblclick = "dblclick";
const touchstart = "touchstart";

export class splitterHelper {
  static attach(component, container, lazy = false) {
    const draggers = [...container.querySelectorAll(":scope > .ant-splitter-bar > .spliter-bar")];
    const panes = [...container.querySelectorAll(":scope > .pane-of-split-container")];
    const dimension = container.classList.contains("splitter-orientation-vertical") ? "width" : "height";
    const position = dimension === "width" ? "clientX" : "clientY";
    const sizeOf = (element) => element.getBoundingClientRect()[dimension];
    const toPx = (value, containerSize) => {
      if (!value)
        return 0;
      const parsed = Number.parseFloat(value);
      if (!Number.isFinite(parsed))
        return 0;
      return value.trim().endsWith("%") ? containerSize * parsed / 100 : parsed;
    };
    let disposed = false;
    let dragState = null;

    const dispose = () => {
      if (disposed)
        return;
      disposed = true;
      draggers.forEach((dragger) => {
        dragger.removeEventListener(pointerdown, onPointerDown);
        dragger.removeEventListener(pointerup, onPointerUp);
        dragger.removeEventListener(dblclick, onDoubleClick);
        dragger.removeEventListener(touchstart, preventDefault);
      });
    };

    const preventDefault = (event) => event.preventDefault();

    const onPointerDown = (event) => {
      const dragger = event.currentTarget;
      if (!container.isConnected) {
        dispose();
        return;
      }
      if (dragger.classList.contains("ant-splitter-bar-dragger-disabled"))
        return;

      const index = draggers.indexOf(dragger);
      const left = panes[index];
      const right = panes[index + 1];
      if (!left || !right)
        return;

      const leftResizable = left.getAttribute("data-resizable") !== "false";
      const rightResizable = right.getAttribute("data-resizable") !== "false";
      const targetSide = leftResizable ? "left" : rightResizable ? "right" : null;
      if (!targetSide)
        return;

      const containerSize = sizeOf(container);
      const leftSize = sizeOf(left);
      const rightSize = sizeOf(right);
      dragState = {
        index,
        dragger,
        left,
        right,
        targetSide,
        startPosition: event[position],
        containerSize,
        leftSize,
        pairSize: leftSize + rightSize,
        minLeft: toPx(left.getAttribute("data-min"), containerSize),
        maxLeft: left.getAttribute("data-max") ? toPx(left.getAttribute("data-max"), containerSize) : containerSize,
        minRight: toPx(right.getAttribute("data-min"), containerSize),
        maxRight: right.getAttribute("data-max") ? toPx(right.getAttribute("data-max"), containerSize) : containerSize,
      };

      component.invokeMethodAsync("SetDragging", true, index);
      dragger.addEventListener(pointermove, onPointerMove);
      dragger.setPointerCapture(event.pointerId);
    };

    const updateSize = (event, apply = true) => {
      if (!dragState)
        return;

      const { left, right, dragger, leftSize, pairSize, startPosition, minLeft, maxLeft, minRight, maxRight } = dragState;
      const delta = event[position] - startPosition;
      const nextLeft = Math.max(minLeft, Math.min(maxLeft, Math.min(pairSize - minRight, Math.max(pairSize - maxRight, leftSize + delta))));
      const nextRight = pairSize - nextLeft;

      if (!apply) {
        const preview = dragger.parentElement.querySelector(":scope > .ant-splitter-bar-preview");
        if (preview) {
          preview.style.setProperty("--ant-splitter-bar-preview-offset", `${delta}px`);
          preview.classList.toggle("ant-splitter-bar-preview-active", delta !== 0);
        }
        return;
      }

      left.style[dimension] = `${nextLeft}px`;
      left.style.flex = `0 0 ${nextLeft}px`;
      right.style[dimension] = `${nextRight}px`;
      right.style.flex = `0 0 ${nextRight}px`;
    };

    const onPointerMove = (event) => updateSize(event, !lazy);

    const onDoubleClick = (event) => {
      const index = draggers.indexOf(event.currentTarget);
      component.invokeMethodAsync("DraggerDoubleClick", index);
    };

    const onPointerUp = async (event) => {
      if (!dragState)
        return;

      const { dragger, index } = dragState;
      updateSize(event, true);
      dragger.removeEventListener(pointermove, onPointerMove);
      if (dragger.hasPointerCapture(event.pointerId))
        dragger.releasePointerCapture(event.pointerId);

      const sizes = panes.map((pane) => `${Math.round(sizeOf(pane))}px`);
      const preview = dragger.parentElement.querySelector(":scope > .ant-splitter-bar-preview");
      preview?.classList.remove("ant-splitter-bar-preview-active");
      dragState = null;
      await component.invokeMethodAsync("SetDragging", false, index);
      await component.invokeMethodAsync("UpdateSize", sizes);
    };

    draggers.forEach((dragger) => {
      dragger.addEventListener(pointerdown, onPointerDown);
      dragger.addEventListener(pointerup, onPointerUp);
      dragger.addEventListener(dblclick, onDoubleClick);
      dragger.addEventListener(touchstart, preventDefault, { passive: false });
    });

    return { dispose };
  }
}
