export function getSelection(container, positioningContainer) {
  const selection = window.getSelection();
  const text = selection?.toString().trim();

  if (!selection || !text || selection.rangeCount === 0) {
    return null;
  }

  const range = selection.getRangeAt(0);
  if (!container.contains(range.commonAncestorContainer)) {
    return null;
  }

  const rect = range.getBoundingClientRect();
  if (!rect.width || !rect.height) {
    return null;
  }

  const containerRect = positioningContainer.getBoundingClientRect();
  return {
    text,
    x: rect.left + rect.width / 2 - containerRect.left - positioningContainer.clientLeft + positioningContainer.scrollLeft,
    y: rect.bottom + 4 - containerRect.top - positioningContainer.clientTop + positioningContainer.scrollTop,
  };
}

export function clearSelection() {
  window.getSelection()?.removeAllRanges();
}
