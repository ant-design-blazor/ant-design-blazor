const pointerdown = "pointerdown";
const pointermove = "pointermove";
const pointerup = "pointerup";
const dblclick = "dblclick";
const touchstart = "touchstart";

export class splitterHelper {
  static async attach(component, container, lazy = false, paneOptions = []) {
    const draggers = [...container.querySelectorAll(":scope > .ant-splitter-bar > .ant-splitter-bar-dragger")];
    const panes = [...container.querySelectorAll(":scope > .ant-splitter-panel")];
    const dimension = container.classList.contains("ant-splitter-horizontal") ? "width" : "height";
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
    const toPercentage = (value, containerSize) => {
      if (value == null || value === "")
        return undefined;
      const text = String(value).trim();
      const parsed = Number.parseFloat(text);
      if (!Number.isFinite(parsed))
        return undefined;
      return text.endsWith("%") ? parsed / 100 : parsed / containerSize;
    };
    const fitPercentages = (sizes, minSizes, maxSizes) => {
      const min = sizes.map((size, index) => size === 0 ? 0 : (minSizes[index] ?? 0));
      const max = sizes.map((size, index) => size === 0 ? 0 : (maxSizes[index] ?? 1));
      if (min.some((value, index) => value > max[index]) || min.reduce((sum, value) => sum + value, 0) > 1 || max.reduce((sum, value) => sum + value, 0) < 1)
        return sizes;

      const result = sizes.map((size, index) => Math.min(max[index], Math.max(min[index], size)));
      const total = result.reduce((sum, value) => sum + value, 0);
      if (total === 1)
        return result;

      const grow = total < 1;
      const spaces = result.map((size, index) => grow ? max[index] - size : size - min[index]);
      const totalSpace = spaces.reduce((sum, value) => sum + value, 0);
      const remaining = 1 - total;
      return result.map((size, index) => size + remaining * (spaces[index] / totalSpace));
    };
    const autoPercentages = (sizes, minSizes, maxSizes) => {
      let currentTotal = 0;
      const undefinedIndexes = [];
      sizes.forEach((size, index) => {
        if (size === undefined)
          undefinedIndexes.push(index);
        else
          currentTotal += size;
      });

      const remaining = 1 - currentTotal;
      if (sizes.length && undefinedIndexes.length === 0 && currentTotal !== 1) {
        const scale = currentTotal === 0 ? undefined : 1 / currentTotal;
        const scaled = scale === undefined ? sizes.map(() => 1 / sizes.length) : sizes.map((size) => size * scale);
        return currentTotal === 0 ? scaled : fitPercentages(scaled, minSizes, maxSizes);
      }
      if (remaining < 0) {
        const scale = 1 / currentTotal;
        return sizes.map((size) => size === undefined ? 0 : size * scale);
      }
      if (!undefinedIndexes.length)
        return sizes;

      const minTotal = undefinedIndexes.reduce((sum, index) => sum + (minSizes[index] || 0), 0);
      const maxTotal = undefinedIndexes.reduce((sum, index) => sum + (maxSizes[index] || 1), 0);
      const minLimit = Math.max(...undefinedIndexes.map((index) => minSizes[index] || 0));
      const maxLimit = Math.min(...undefinedIndexes.map((index) => maxSizes[index] || 1));
      if (minTotal > 1 && maxTotal < 1) {
        const average = 1 / undefinedIndexes.length;
        return sizes.map((size) => size === undefined ? average : size);
      }

      const average = remaining / undefinedIndexes.length;
      if (minLimit <= average && average <= maxLimit)
        return sizes.map((size) => size === undefined ? average : size);

      const result = [...sizes];
      let remainingAfterMin = remaining - minTotal;
      for (const index of undefinedIndexes) {
        const min = minSizes[index] || 0;
        const max = maxSizes[index] || 1;
        result[index] = min;
        const add = Math.min(max - min, remainingAfterMin);
        result[index] += add;
        remainingAfterMin -= add;
      }
      return result;
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

    const containerSize = sizeOf(container);
    const hasControlledSizes = paneOptions.some((option) => option.size != null);
    const requestedSizes = paneOptions.map((option) => toPercentage(hasControlledSizes ? option.size : option.defaultSize, containerSize));
    const minPercentages = paneOptions.map((option) => toPercentage(option.min, containerSize) ?? 0);
    const maxPercentages = paneOptions.map((option) => toPercentage(option.max, containerSize) ?? 1);
    const initialSizes = containerSize
      ? autoPercentages(requestedSizes, minPercentages, maxPercentages).map((size) => size * containerSize)
      : panes.map((pane) => sizeOf(pane));
    const setAriaValues = () => {
      const containerSize = sizeOf(container);
      if (!containerSize)
        return;

      const sizes = panes.map((pane) => sizeOf(pane));
      const percentage = (value) => value / containerSize;
      const minSize = (index) => toPx(paneOptions[index]?.min, containerSize);
      const maxSize = (index) => paneOptions[index]?.max ? toPx(paneOptions[index].max, containerSize) : containerSize;
      let previousStack = 0;

      draggers.forEach((dragger, index) => {
        const currentStack = previousStack + sizes[index];
        const nextStack = currentStack + sizes[index + 1];
        const ariaMin = Math.max(previousStack + minSize(index), nextStack - maxSize(index + 1));
        const ariaMax = Math.min(previousStack + maxSize(index), nextStack - minSize(index + 1));
        const setValue = (name, value) => dragger.setAttribute(name, String(Math.round(value)));

        setValue("aria-valuenow", percentage(currentStack) * 100);
        setValue("aria-valuemin", percentage(ariaMin) * 100);
        setValue("aria-valuemax", percentage(ariaMax) * 100);
        previousStack = currentStack;
      });
    };

    panes.forEach((pane, index) => {
      const size = `${Number(initialSizes[index].toFixed(3))}px`;
      pane.style.removeProperty(dimension);
      pane.style.flexBasis = size;
      pane.style.flexGrow = "0";
    });
    setAriaValues();
    await component.invokeMethodAsync("InitializeSizes", initialSizes.map((size) => `${Number(size.toFixed(3))}px`));

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

      const leftResizable = paneOptions[index]?.resizable !== false;
      const rightResizable = paneOptions[index + 1]?.resizable !== false;
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
        minLeft: toPx(paneOptions[index]?.min, containerSize),
        maxLeft: paneOptions[index]?.max ? toPx(paneOptions[index].max, containerSize) : containerSize,
        minRight: toPx(paneOptions[index + 1]?.min, containerSize),
        maxRight: paneOptions[index + 1]?.max ? toPx(paneOptions[index + 1].max, containerSize) : containerSize,
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
      setAriaValues();
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
