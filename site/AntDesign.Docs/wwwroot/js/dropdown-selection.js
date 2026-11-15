const selectionTrackers = new WeakMap();

function getSelectionRect(range, container, positionContainer) {
  if (!range || !container.contains(range.commonAncestorContainer)) {
    return null;
  }

  const rect = range.getBoundingClientRect();
  if (!rect.width || !rect.height) {
    return null;
  }

  const containerRect = positionContainer.getBoundingClientRect();
  return {
    x: rect.left + rect.width / 2 - containerRect.left - positionContainer.clientLeft + positionContainer.scrollLeft,
    y: rect.bottom - containerRect.top - positionContainer.clientTop + positionContainer.scrollTop + 4,
  };
}

export async function getSelection(container, positionContainer) {
  // Match React's setTimeout(handleSelect) after mouse-up. This also lets the
  // browser finalize the Selection range before Blazor Server reads it.
  await new Promise(resolve => setTimeout(resolve, 0));

  const selection = window.getSelection();
  const text = selection?.toString().trim();

  if (!selection || !text || selection.rangeCount === 0) {
    return null;
  }

  const range = selection.getRangeAt(0);
  if (!range || typeof range.getBoundingClientRect !== "function") {
    return null;
  }

  const position = getSelectionRect(range, container, positionContainer);
  if (!position) return null;

  return {
    text,
    ...position,
  };
}

// Keep the virtual Dropdown trigger attached to the live browser selection.
// This runs in the browser's scroll capture phase, before Overlay's scroll
// listener recalculates popup placement, avoiding a Blazor Server round trip.
function getTrigger(positionContainer) {
  return positionContainer.querySelector('.dropdown-selection-trigger');
}

export function startTrackingSelection(container, positionContainer) {
  const trigger = getTrigger(positionContainer);
  if (!trigger) return;
  stopTrackingSelection(positionContainer);
  const selection = window.getSelection();
  if (!selection || selection.rangeCount === 0) return;

  const range = selection.getRangeAt(0).cloneRange();
  const update = () => {
    const position = getSelectionRect(range, container, positionContainer);
    if (!position) return;
    trigger.style.left = `${position.x}px`;
    trigger.style.top = `${position.y}px`;
  };

  selectionTrackers.set(trigger, update);
  update();
  window.addEventListener('scroll', update, true);
  window.addEventListener('resize', update);
}

export function stopTrackingSelection(positionContainer) {
  const trigger = positionContainer instanceof Element ? getTrigger(positionContainer) : positionContainer;
  if (!trigger) return;
  const update = selectionTrackers.get(trigger);
  if (!update) return;
  window.removeEventListener('scroll', update, true);
  window.removeEventListener('resize', update);
  selectionTrackers.delete(trigger);
}

export function clearSelection() {
  window.getSelection()?.removeAllRanges();
}
