const HEADER_SELECTOR = 'thead th[id]';
const CELL_SELECTOR = 'tbody td[id]';
const TRANSITION = 'transform 200ms ease';

export function initializeTableColumnSortable(root, dotNet) {
  if (!root) return;

  let drag = null;
  const originalHeaderStyles = new WeakMap();
  const originalCellStyles = new WeakMap();
  const headers = () => [...root.querySelectorAll(HEADER_SELECTOR)];
  const cells = () => [...root.querySelectorAll(CELL_SELECTOR)];
  const idOf = element => element.id;

  const rememberStyles = () => {
    headers().forEach(header => originalHeaderStyles.set(header, {
      transform: header.style.transform,
      transition: header.style.transition,
      position: header.style.position,
      zIndex: header.style.zIndex,
      userSelect: header.style.userSelect,
      backgroundColor: header.style.backgroundColor,
      opacity: header.style.opacity,
    }));
    cells().forEach(cell => originalCellStyles.set(cell, {
      backgroundColor: cell.style.backgroundColor,
      opacity: cell.style.opacity,
      borderInlineStart: cell.style.borderInlineStart,
      borderInlineEnd: cell.style.borderInlineEnd,
    }));
  };

  const createOverlay = header => {
    const rect = header.getBoundingClientRect();
    const overlay = document.createElement('div');
    overlay.className = 'table-column-drag-overlay';
    overlay.style.transform = `translate3d(${rect.left}px, ${rect.top}px, 0)`;
    overlay.innerHTML = '<table><thead><tr><th></th></tr></thead></table>';
    overlay.querySelector('th').textContent = header.textContent.trim();
    document.body.appendChild(overlay);
    return overlay;
  };

  const begin = (header, event, keyboard = false) => {
    const currentHeaders = headers();
    const index = currentHeaders.indexOf(header);
    if (index < 0) return;
    rememberStyles();
    const rect = header.getBoundingClientRect();
    drag = {
      headers: currentHeaders,
      rects: currentHeaders.map(item => item.getBoundingClientRect()),
      activeId: idOf(header),
      overId: idOf(header),
      startX: keyboard ? rect.left + rect.width / 2 : event.clientX,
      startY: rect.top + rect.height / 2,
      pointerX: keyboard ? rect.left + rect.width / 2 : event.clientX,
      keyboard,
      overlay: createOverlay(header),
    };
    header.setAttribute('aria-pressed', 'true');
  };

  const nearestHeaderIndex = x => {
    let nearest = 0;
    let distance = Number.POSITIVE_INFINITY;
    drag.rects.forEach((rect, index) => {
      const next = Math.abs(x - (rect.left + rect.width / 2));
      if (next < distance) {
        nearest = index;
        distance = next;
      }
    });
    return nearest;
  };

  const paint = overIndex => {
    const activeIndex = drag.headers.findIndex(header => idOf(header) === drag.activeId);
    overIndex = Math.max(0, Math.min(overIndex, drag.headers.length - 1));
    const overId = idOf(drag.headers[overIndex]);
    const direction = overIndex > activeIndex ? 'right' : 'left';
    drag.overId = overId;

    drag.headers.forEach((header, index) => {
      const original = drag.rects[index];
      let shift = 0;
      if (index === activeIndex) {
        shift = drag.pointerX - drag.startX;
        header.style.transition = 'none';
        header.style.position = 'relative';
        header.style.zIndex = '9999';
        header.style.userSelect = 'none';
      } else {
        if (activeIndex < overIndex && index > activeIndex && index <= overIndex) {
          shift = -drag.rects[activeIndex].width;
        } else if (activeIndex > overIndex && index >= overIndex && index < activeIndex) {
          shift = drag.rects[activeIndex].width;
        }
        header.style.transition = TRANSITION;
      }
      header.style.transform = `translate3d(${shift}px, 0, 0)`;
    });

    headers().forEach(header => {
      const active = idOf(header) === drag.activeId;
      const over = idOf(header) === overId && !active;
      header.style.backgroundColor = active ? 'gray' : '';
      header.style.opacity = active ? '0.5' : '';
      if (over) {
        header.style[direction === 'right' ? 'borderInlineEnd' : 'borderInlineStart'] = '1px dashed gray';
      }
    });

    cells().forEach(cell => {
      const active = idOf(cell) === drag.activeId;
      const over = idOf(cell) === overId && !active;
      cell.style.backgroundColor = active ? 'gray' : '';
      cell.style.opacity = active ? '0.5' : '';
      if (over) {
        cell.style[direction === 'right' ? 'borderInlineEnd' : 'borderInlineStart'] = '1px dashed gray';
      }
    });

    const overlayHeader = drag.overlay.querySelector('th');
    overlayHeader.style.backgroundColor = 'gray';
    overlayHeader.style.padding = '16px';
    drag.overlay.style.transform = `translate3d(${drag.rects[activeIndex].left + drag.pointerX - drag.startX}px, ${drag.rects[activeIndex].top}px, 0)`;
  };

  const reset = () => {
    if (!drag) return;
    headers().forEach(header => {
      const original = originalHeaderStyles.get(header);
      header.removeAttribute('aria-pressed');
      if (!original) return;
      header.style.transform = original.transform;
      header.style.transition = original.transition;
      header.style.position = original.position;
      header.style.zIndex = original.zIndex;
      header.style.userSelect = original.userSelect;
      header.style.backgroundColor = original.backgroundColor;
      header.style.opacity = original.opacity;
      header.style.borderInlineStart = '';
      header.style.borderInlineEnd = '';
    });
    cells().forEach(cell => {
      const original = originalCellStyles.get(cell);
      if (!original) return;
      cell.style.backgroundColor = original.backgroundColor;
      cell.style.opacity = original.opacity;
      cell.style.borderInlineStart = original.borderInlineStart;
      cell.style.borderInlineEnd = original.borderInlineEnd;
    });
    drag.overlay.remove();
    drag = null;
  };

  const finish = async commit => {
    if (!drag) return;
    const { activeId, overId } = drag;
    if (commit && activeId !== overId) {
      try {
        await dotNet.invokeMethodAsync('OnColumnSortDrop', activeId, overId);
        await new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)));
      } catch (error) {
        console.error('Table column sorting update failed.', error);
      }
    }
    reset();
  };

  const onPointerDown = event => {
    if (event.button !== 0 || !event.isPrimary || drag) return;
    const header = event.target.closest(HEADER_SELECTOR);
    if (!header || !root.contains(header)) return;
    event.preventDefault();
    begin(header, event);
  };

  const onPointerMove = event => {
    if (!drag || drag.keyboard) return;
    const deltaX = event.clientX - drag.startX;
    if (Math.abs(deltaX) < 1) return;
    drag.pointerX = event.clientX;
    paint(nearestHeaderIndex(event.clientX));
  };

  const onPointerUp = () => {
    if (drag && !drag.keyboard) finish(true);
  };

  const onKeyDown = event => {
    const header = event.target.closest(HEADER_SELECTOR);
    if (!header || !root.contains(header)) return;
    if (!drag && (event.code === 'Space' || event.code === 'Enter')) {
      event.preventDefault();
      begin(header, event, true);
      paint(drag.headers.indexOf(header));
      return;
    }
    if (!drag || !drag.keyboard || idOf(header) !== drag.activeId) return;
    if (event.code === 'Escape') {
      event.preventDefault();
      finish(false);
    } else if (event.code === 'Space' || event.code === 'Enter') {
      event.preventDefault();
      finish(true);
    } else if (event.code === 'ArrowLeft' || event.code === 'ArrowRight') {
      event.preventDefault();
      const current = drag.headers.findIndex(item => idOf(item) === drag.overId);
      const next = current + (event.code === 'ArrowRight' ? 1 : -1);
      if (next >= 0 && next < drag.headers.length) {
        drag.pointerX = drag.rects[next].left + drag.rects[next].width / 2;
        paint(next);
      }
    }
  };

  root.addEventListener('pointerdown', onPointerDown);
  root.addEventListener('keydown', onKeyDown);
  document.addEventListener('pointermove', onPointerMove);
  document.addEventListener('pointerup', onPointerUp);
  document.addEventListener('pointercancel', onPointerUp);

  root.__tableColumnSortableDispose = () => {
    reset();
    root.removeEventListener('pointerdown', onPointerDown);
    root.removeEventListener('keydown', onKeyDown);
    document.removeEventListener('pointermove', onPointerMove);
    document.removeEventListener('pointerup', onPointerUp);
    document.removeEventListener('pointercancel', onPointerUp);
    delete root.__tableColumnSortableDispose;
  };
}

export function disposeTableColumnSortable(root) {
  root?.__tableColumnSortableDispose?.();
}
