const ITEM_SELECTOR = '.ant-list-item[id^="list-sortable-item-"]';
const HANDLE_SELECTOR = 'button.ant-btn';
// Matches @dnd-kit/sortable's default transition used by the React demos.
const TRANSITION = 'transform 200ms ease';

export function initializeSortableList(root, dotNet, handleOnly, grid, contextId) {
  if (!root) return;

  let drag = null;
  let pendingPointer = null;
  const originalBodyUserSelect = document.body.style.userSelect;
  const description = document.getElementById(contextId);
  const originals = new WeakMap();
  const getItems = () => [...root.querySelectorAll(ITEM_SELECTOR)];
  const keyOf = (item) => Number(item.id.slice(item.id.lastIndexOf('-') + 1));

  const decorateItems = () => {
    for (const item of getItems()) {
      const handle = handleOnly
        ? item.querySelector(HANDLE_SELECTOR)
        : (!grid ? item.querySelector(':scope > div') : item);
      if (!handle) continue;

      if (!handle.hasAttribute('role')) handle.setAttribute('role', 'button');
      if (!handle.hasAttribute('tabindex')) handle.setAttribute('tabindex', '0');
      handle.setAttribute('aria-disabled', 'false');
      handle.setAttribute('aria-roledescription', 'sortable');
      if (description) handle.setAttribute('aria-describedby', description.id);
      if (window.matchMedia('(pointer: coarse)').matches) handle.style.touchAction = 'none';
      originals.set(item, {
        transform: item.style.transform,
        transition: item.style.transition,
        position: item.style.position,
        zIndex: item.style.zIndex,
        willChange: item.style.willChange,
      });
    }
  };

  const applyTransforms = (current, targetIndex, deltaX = 0, deltaY = 0, pointerMode = false) => {
    const items = current.items;
    const activeIndex = items.findIndex((item) => keyOf(item) === current.activeKey);
    if (activeIndex < 0) return;
    targetIndex = Math.max(0, Math.min(targetIndex, items.length - 1));

    const ordered = [...items];
    const [active] = ordered.splice(activeIndex, 1);
    ordered.splice(targetIndex, 0, active);
    current.overKey = keyOf(items[targetIndex]);

    const slotIndex = new Map(ordered.map((item, index) => [item, index]));
    items.forEach((item, index) => {
      const original = current.rects[index];
      const slot = current.rects[slotIndex.get(item)];
      item.style.transition = item === active && pointerMode ? 'none' : TRANSITION;
      item.style.willChange = 'transform';
      if (item === active) {
        item.style.position = 'relative';
        item.style.zIndex = '9999';
        item.classList.add('ant-list-item-dragging');
        item.style.transform = pointerMode
          ? `translate3d(${deltaX}px, ${deltaY}px, 0)`
          : `translate3d(${slot.left - original.left}px, ${slot.top - original.top}px, 0)`;
      } else {
        item.style.transform = `translate3d(${slot.left - original.left}px, ${slot.top - original.top}px, 0)`;
      }
    });
  };

  const createDrag = (item, handle, event, keyboard = false, snapshot = null) => {
    const items = snapshot?.items ?? getItems();
    const rects = snapshot?.rects ?? items.map((node) => node.getBoundingClientRect());
    const index = items.indexOf(item);
    if (index < 0) return null;
    const startRect = rects[index];
    item.classList.add('ant-list-item-dragging');
    handle.setAttribute('aria-pressed', 'true');
    return {
      items,
      rects,
      activeKey: keyOf(item),
      overKey: keyOf(item),
      startX: keyboard ? startRect.left + startRect.width / 2 : event.clientX,
      startY: keyboard ? startRect.top + startRect.height / 2 : event.clientY,
      startRect,
      handle,
      keyboard,
      pointerId: keyboard ? null : event.pointerId,
      originalBodyUserSelect,
    };
  };

  const nearestIndex = (current, x, y) => {
    let result = 0;
    let distance = Number.POSITIVE_INFINITY;
    current.rects.forEach((rect, index) => {
      const dx = grid ? x - (rect.left + rect.width / 2) : 0;
      const dy = y - (rect.top + rect.height / 2);
      const next = dx * dx + dy * dy;
      if (next < distance) {
        distance = next;
        result = index;
      }
    });
    return result;
  };

  const reset = () => {
    pendingPointer = null;
    if (!drag) return;
    drag.handle.removeAttribute('aria-pressed');
    document.body.style.userSelect = drag.originalBodyUserSelect;
    for (const item of getItems()) {
      const saved = originals.get(item);
      if (!saved) continue;
      item.style.transform = saved.transform;
      item.style.transition = saved.transition;
      item.style.position = saved.position;
      item.style.zIndex = saved.zIndex;
      item.style.willChange = saved.willChange;
      item.classList.remove('ant-list-item-dragging');
    }
    drag = null;
  };

  const finish = async (commit) => {
    if (!drag) return;
    const activeKey = drag.activeKey;
    const overKey = drag.overKey;
    const shouldCommit = commit && activeKey !== overKey;

    // Keep the sortable preview alive through the drop animation. React's
    // useSortable animates the active item and the displaced siblings into
    // their destination slots before the new order is rendered.
    if (activeKey !== overKey) {
      const landingKey = commit ? overKey : activeKey;
      const landingIndex = drag.items.findIndex((item) => keyOf(item) === landingKey);
      if (landingIndex >= 0) {
        applyTransforms(drag, landingIndex);
        await new Promise((resolve) => setTimeout(resolve, 200));
      }
    }

    if (shouldCommit) {
      try {
        await dotNet.invokeMethodAsync('OnSortableDrop', activeKey, overKey);
        await new Promise((resolve) => requestAnimationFrame(() => requestAnimationFrame(resolve)));
      } catch (error) {
        console.error('List sorting update failed.', error);
      }
    }
    reset();
  };

  const onPointerDown = (event) => {
    if (event.button !== 0 || !event.isPrimary || drag || pendingPointer) return;
    const item = event.target.closest(ITEM_SELECTOR);
    if (!item || !root.contains(item)) return;
    let handle;
    if (handleOnly) {
      handle = event.target.closest(HANDLE_SELECTOR);
      if (!handle || !item.contains(handle)) return;
    } else if (grid) {
      handle = item;
    } else {
      const content = item.querySelector(':scope > div');
      // The React demo's listener wrapper fills the available list-item width.
      // Blazor can leave some of that width on the <li> itself, so treat the
      // whole row as the pointer sensor while retaining the content wrapper as
      // the keyboard activator.
      handle = content?.contains(event.target) ? content : item;
    }
    // Match dnd-kit PointerSensor's 1px activation constraint: clicks and text
    // selection remain native until the pointer actually starts moving.
    const items = getItems();
    pendingPointer = {
      item,
      handle,
      pointerId: event.pointerId,
      startX: event.clientX,
      startY: event.clientY,
      rects: items.map((node) => node.getBoundingClientRect()),
      items,
    };
  };

  const onPointerMove = (event) => {
    if (!drag) {
      if (!pendingPointer || pendingPointer.pointerId !== event.pointerId) return;
      const dx = event.clientX - pendingPointer.startX;
      const dy = event.clientY - pendingPointer.startY;
      if (Math.hypot(dx, dy) < 1) return;

      event.preventDefault();
      drag = createDrag(pendingPointer.item, pendingPointer.handle, {
        clientX: pendingPointer.startX,
        clientY: pendingPointer.startY,
        pointerId: pendingPointer.pointerId,
      }, false, pendingPointer);
      document.body.style.userSelect = 'none';
      pendingPointer = null;
    }
    if (drag.keyboard || drag.pointerId !== event.pointerId) return;
    // React's vertical-list demos restrict dragging to the Y axis; grid demos
    // intentionally allow both axes so cards follow the pointer freely.
    const deltaX = grid ? event.clientX - drag.startX : 0;
    const deltaY = event.clientY - drag.startY;
    if (Math.abs(deltaX) + Math.abs(deltaY) < 1) return;
    const index = nearestIndex(drag, event.clientX, event.clientY);
    applyTransforms(drag, index, deltaX, deltaY, true);
  };

  const onPointerUp = (event) => {
    if (drag?.pointerId != null && drag.pointerId !== event.pointerId) return;
    if (pendingPointer && pendingPointer.pointerId !== event.pointerId) return;
    if (drag && !drag.keyboard) finish(true);
    else pendingPointer = null;
  };

  const onPointerCancel = (event) => {
    if (pendingPointer?.pointerId === event.pointerId) pendingPointer = null;
    if (drag?.pointerId === event.pointerId) finish(false);
  };

  const onKeyDown = (event) => {
    const item = event.target.closest(ITEM_SELECTOR);
    if (!item || !root.contains(item)) return;
    const handle = handleOnly ? event.target.closest(HANDLE_SELECTOR) : (!grid ? item.querySelector(':scope > div') : item);
    if (!handle || (handleOnly && !item.contains(handle))) return;

    if (!drag && (event.code === 'Space' || event.code === 'Enter')) {
      event.preventDefault();
      drag = createDrag(item, handle, event, true);
      document.body.style.userSelect = 'none';
      return;
    }
    if (!drag || !drag.keyboard || keyOf(item) !== drag.activeKey) return;

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

    const currentIndex = drag.items.findIndex((node) => keyOf(node) === drag.overKey);
    let nextIndex = currentIndex;
    if (event.code === 'ArrowUp') nextIndex = currentIndex - (grid ? Math.max(1, Math.round(root.clientWidth / (drag.rects[0].width || 1))) : 1);
    if (event.code === 'ArrowDown') nextIndex = currentIndex + (grid ? Math.max(1, Math.round(root.clientWidth / (drag.rects[0].width || 1))) : 1);
    if (grid && event.code === 'ArrowLeft') nextIndex = currentIndex - 1;
    if (grid && event.code === 'ArrowRight') nextIndex = currentIndex + 1;
    if (nextIndex !== currentIndex) {
      event.preventDefault();
      applyTransforms(drag, nextIndex);
    }
  };

  decorateItems();
  // Listen in capture phase so ListItem's click handling and nested controls
  // cannot swallow the pointer sensor before a drag has been activated.
  root.addEventListener('pointerdown', onPointerDown, true);
  root.addEventListener('keydown', onKeyDown);
  document.addEventListener('pointermove', onPointerMove);
  document.addEventListener('pointerup', onPointerUp);
  document.addEventListener('pointercancel', onPointerCancel);

  root.__listSortableDispose = () => {
    reset();
    root.removeEventListener('pointerdown', onPointerDown, true);
    root.removeEventListener('keydown', onKeyDown);
    document.removeEventListener('pointermove', onPointerMove);
    document.removeEventListener('pointerup', onPointerUp);
    document.removeEventListener('pointercancel', onPointerCancel);
    delete root.__listSortableDispose;
  };
}

export function disposeSortableList(root) {
  root?.__listSortableDispose?.();
}
