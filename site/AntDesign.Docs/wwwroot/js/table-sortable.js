const ROW_SELECTOR = 'tr.ant-table-row[data-row-key]';
const HANDLE_SELECTOR = '.table-sort-handle';
const TRANSITION = 'transform 200ms ease';

export function initializeTableSortable(root, dotNet, handleOnly) {
  if (!root) return;

  let drag = null;
  const originalStyles = new WeakMap();
  const getRows = () => [...root.querySelectorAll(ROW_SELECTOR)];
  const keyOf = row => row.getAttribute('data-row-key');

  const decorateRows = () => {
    const descriptionId = root.closest('.table-parity-demo')?.querySelector('[id$="-instructions"]')?.id;
    for (const row of getRows()) {
      row.setAttribute('role', 'button');
      row.setAttribute('tabindex', '0');
      row.setAttribute('aria-disabled', 'false');
      row.setAttribute('aria-roledescription', 'sortable');
      if (descriptionId) row.setAttribute('aria-describedby', descriptionId);
      const handle = row.querySelector(HANDLE_SELECTOR);
      originalStyles.set(row, {
        transform: row.style.transform,
        transition: row.style.transition,
        position: row.style.position,
        zIndex: row.style.zIndex,
        willChange: row.style.willChange,
        cursor: handleOnly ? row.style.cursor : 'move',
      });
      if (!handleOnly) row.style.cursor = 'move';
      if (handle && window.matchMedia('(pointer: coarse)').matches) handle.style.touchAction = 'none';
    }
  };

  const createDrag = (row, handle, event, keyboard = false) => {
    const rows = getRows();
    const rects = rows.map(item => item.getBoundingClientRect());
    const index = rows.indexOf(row);
    if (index < 0) return null;
    return {
      rows,
      rects,
      activeKey: keyOf(row),
      overKey: keyOf(row),
      startX: keyboard ? rects[index].left + rects[index].width / 2 : event.clientX,
      startY: keyboard ? rects[index].top + rects[index].height / 2 : event.clientY,
      handle,
      keyboard,
    };
  };

  const applyTransforms = (current, targetIndex, deltaY = 0, pointerMode = false) => {
    const activeIndex = current.rows.findIndex(row => keyOf(row) === current.activeKey);
    if (activeIndex < 0) return;
    targetIndex = Math.max(0, Math.min(targetIndex, current.rows.length - 1));

    const ordered = [...current.rows];
    const [active] = ordered.splice(activeIndex, 1);
    ordered.splice(targetIndex, 0, active);
    current.overKey = keyOf(current.rows[targetIndex]);
    const slotIndex = new Map(ordered.map((row, index) => [row, index]));

    current.rows.forEach((row, index) => {
      const original = current.rects[index];
      const slot = current.rects[slotIndex.get(row)];
      row.style.transition = row === active && pointerMode ? 'none' : TRANSITION;
      row.style.willChange = 'transform';
      if (row === active) {
        row.style.position = 'relative';
        row.style.zIndex = '9999';
        row.style.transform = pointerMode
          ? `translate3d(0px, ${deltaY}px, 0)`
          : `translate3d(0px, ${slot.top - original.top}px, 0)`;
      } else {
        row.style.transform = `translate3d(0px, ${slot.top - original.top}px, 0)`;
      }
    });
  };

  const reset = () => {
    if (!drag) return;
    for (const row of drag.rows) {
      const saved = originalStyles.get(row);
      if (!saved) continue;
      row.style.transform = saved.transform;
      row.style.transition = saved.transition;
      row.style.position = saved.position;
      row.style.zIndex = saved.zIndex;
      row.style.willChange = saved.willChange;
      row.style.cursor = saved.cursor;
    }
    drag = null;
  };

  const finish = async (commit) => {
    if (!drag) return;
    const activeKey = drag.activeKey;
    const overKey = drag.overKey;
    if (!drag.keyboard && commit && activeKey !== overKey) {
      const index = drag.rows.findIndex(row => keyOf(row) === activeKey);
      const target = drag.rows.findIndex(row => keyOf(row) === overKey);
      applyTransforms(drag, target);
      await new Promise(resolve => setTimeout(resolve, 210));
    }
    if (commit && activeKey !== overKey) {
      try {
        await dotNet.invokeMethodAsync('OnSortableDrop', activeKey, overKey);
        await new Promise(resolve => requestAnimationFrame(() => requestAnimationFrame(resolve)));
      } catch (error) {
        console.error('Table row sorting update failed.', error);
      }
    }
    reset();
  };

  const onPointerDown = event => {
    if (event.button !== 0 || !event.isPrimary || drag) return;
    const row = event.target.closest(ROW_SELECTOR);
    if (!row || !root.contains(row)) return;
    let handle = row;
    if (handleOnly) {
      handle = event.target.closest(HANDLE_SELECTOR);
      if (!handle || !row.contains(handle)) return;
    }
    event.preventDefault();
    drag = createDrag(row, handle, event);
  };

  const onPointerMove = event => {
    if (!drag || drag.keyboard) return;
    const deltaY = event.clientY - drag.startY;
    if (Math.abs(deltaY) < 1) return;
    let targetIndex = 0;
    let distance = Number.POSITIVE_INFINITY;
    drag.rects.forEach((rect, index) => {
      const next = Math.abs(event.clientY - (rect.top + rect.height / 2));
      if (next < distance) {
        distance = next;
        targetIndex = index;
      }
    });
    applyTransforms(drag, targetIndex, deltaY, true);
  };

  const onKeyDown = event => {
    const row = event.target.closest(ROW_SELECTOR);
    if (!row || !root.contains(row)) return;
    if (handleOnly && !event.target.closest(HANDLE_SELECTOR)) return;

    if (!drag && (event.code === 'Space' || event.code === 'Enter')) {
      event.preventDefault();
      drag = createDrag(row, handleOnly ? event.target.closest(HANDLE_SELECTOR) : row, event, true);
      return;
    }
    if (!drag || !drag.keyboard || keyOf(row) !== drag.activeKey) return;
    if (event.code === 'Escape') {
      event.preventDefault();
      finish(false);
      return;
    }
    if (event.code === 'Space' || event.code === 'Enter') {
      event.preventDefault();
      finish(true);
      return;
    }
    if (event.code === 'ArrowUp' || event.code === 'ArrowDown') {
      event.preventDefault();
      const index = drag.rows.findIndex(item => keyOf(item) === drag.overKey);
      applyTransforms(drag, index + (event.code === 'ArrowDown' ? 1 : -1));
    }
  };

  const onPointerUp = () => {
    if (drag && !drag.keyboard) finish(true);
  };

  decorateRows();
  root.addEventListener('pointerdown', onPointerDown);
  root.addEventListener('keydown', onKeyDown);
  document.addEventListener('pointermove', onPointerMove);
  document.addEventListener('pointerup', onPointerUp);
  document.addEventListener('pointercancel', onPointerUp);

  root.__tableSortableDispose = () => {
    reset();
    root.removeEventListener('pointerdown', onPointerDown);
    root.removeEventListener('keydown', onKeyDown);
    document.removeEventListener('pointermove', onPointerMove);
    document.removeEventListener('pointerup', onPointerUp);
    document.removeEventListener('pointercancel', onPointerUp);
    delete root.__tableSortableDispose;
  };
}

export function disposeTableSortable(root) {
  root?.__tableSortableDispose?.();
}
