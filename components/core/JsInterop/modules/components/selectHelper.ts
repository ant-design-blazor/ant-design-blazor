const pendingScrolls = new WeakMap<HTMLElement, () => void>();
const virtualLists = new WeakMap<HTMLElement, VirtualListState>();

type DotNetReference = {
  invokeMethodAsync(method: string, ...args: number[]): Promise<unknown>;
};

type VirtualListState = {
  dotNet: DotNetReference;
  itemHeight: number;
  total: number;
  lastStart: number;
  lastEnd: number;
  frame: number;
  hideThumbTimeout: number;
  scrollbarObserver?: ResizeObserver;
  popupObserver?: MutationObserver;
  scrollbarFrame: number;
  pending: boolean;
  queued: boolean;
  detached: boolean;
  onScroll: () => void;
  onWheel: (event: WheelEvent) => void;
};

export class selectHelper {
  static attachVirtualList(holder: HTMLElement, dotNet: DotNetReference, itemHeight: number, visibleRows: number, total: number) {
    this.detachVirtualList(holder);
    if (!holder || !dotNet || !Number.isFinite(total) || total <= 0 || itemHeight <= 0) return;
    let updateScrollbar = () => {};

    const state: VirtualListState = {
      dotNet,
      itemHeight,
      total,
      lastStart: -1,
      lastEnd: -1,
      frame: 0,
      hideThumbTimeout: 0,
      scrollbarFrame: 0,
      pending: false,
      queued: false,
      detached: false,
      onScroll: () => {
        scheduleUpdate();
        updateScrollbar();
      },
      onWheel: event => {
        if (event.shiftKey || event.deltaY === 0) return;
        const maximum = Math.max(0, holder.scrollHeight - holder.clientHeight);
        const modeScale = event.deltaMode === WheelEvent.DOM_DELTA_LINE
          ? itemHeight
          : event.deltaMode === WheelEvent.DOM_DELTA_PAGE ? holder.clientHeight : 1;
        const next = Math.max(0, Math.min(maximum, holder.scrollTop + event.deltaY * modeScale));
        if (next === holder.scrollTop) return;
        event.preventDefault();
        holder.scrollTop = next;
        updateScrollbar();
      },
    };
    virtualLists.set(holder, state);

    const dropdown = holder.closest<HTMLElement>('.ant-select-dropdown');
    const scrollbar = holder.parentElement?.querySelector<HTMLElement>('.ant-select-dropdown-list-scrollbar');
    const thumb = scrollbar?.querySelector<HTMLElement>('.ant-select-dropdown-list-scrollbar-thumb');
    updateScrollbar = () => {
      if (!scrollbar || !thumb) return;
      const viewportHeight = holder.clientHeight;
      if (!viewportHeight || holder.getClientRects().length === 0 || dropdown?.getClientRects().length === 0) return;
      const scrollHeight = Math.max(holder.scrollHeight, total * itemHeight);
      const thumbHeight = Math.max(20, Math.round(viewportHeight * viewportHeight / scrollHeight));
      const maximumScroll = Math.max(1, scrollHeight - viewportHeight);
      const thumbRange = Math.max(0, viewportHeight - thumbHeight);
      thumb.style.height = `${thumbHeight}px`;
      thumb.style.top = `${Math.round(holder.scrollTop / maximumScroll * thumbRange)}px`;
      scrollbar.classList.add('ant-select-dropdown-list-scrollbar-visible');
      scrollbar.style.visibility = 'visible';
      window.clearTimeout(state.hideThumbTimeout);
      state.hideThumbTimeout = window.setTimeout(() => {
        scrollbar.classList.remove('ant-select-dropdown-list-scrollbar-visible');
        scrollbar.style.visibility = 'hidden';
      }, 3000);
    };
    const updateRange = () => {
      if (state.detached || !holder.isConnected) return;
      const viewportHeight = holder.clientHeight || parseFloat(getComputedStyle(holder).maxHeight) || visibleRows * itemHeight;
      const firstVisible = Math.max(0, Math.ceil(holder.scrollTop / itemHeight) - 1);
      // rc-virtual-list includes the first row just beyond the viewport and one
      // additional row for motion, yielding ten rows for an 8-row popup.
      const end = Math.min(total, Math.floor((holder.scrollTop + viewportHeight) / itemHeight) + 2);
      if (firstVisible === state.lastStart && end === state.lastEnd) return;
      state.lastStart = firstVisible;
      state.lastEnd = end;
      if (state.pending) { state.queued = true; return; }
      state.pending = true;
      dotNet.invokeMethodAsync('UpdateVirtualWindow', firstVisible, end)
        .catch(() => { state.detached = true; })
        .finally(() => {
          state.pending = false;
          if (state.queued && !state.detached) {
            state.queued = false;
            scheduleUpdate();
          }
        });
    };
    function scheduleUpdate() {
      if (state.detached || state.frame) return;
      state.frame = window.requestAnimationFrame(() => {
        state.frame = 0;
        updateRange();
      });
    }
    holder.addEventListener('scroll', state.onScroll, { passive: true });
    holder.addEventListener('wheel', state.onWheel, { passive: false });
    // Overlays may attach the virtual list before their entry transition makes
    // the holder measurable. Recompute once it receives its visible dimensions
    // so the thumb does not stay at the 20px fallback until the first scroll.
    if (typeof ResizeObserver !== 'undefined') {
      state.scrollbarObserver = new ResizeObserver(updateScrollbar);
      state.scrollbarObserver.observe(holder);
    }
    // The list can be attached while its overlay is still hidden. Observe the
    // popup and its ancestor wrappers so opening it starts the same transient
    // scrollbar visibility window as React's virtual list.
    if (typeof MutationObserver !== 'undefined' && dropdown) {
      let dropdownWasVisible = dropdown.getClientRects().length > 0;
      state.popupObserver = new MutationObserver(() => {
        if (state.detached || !dropdown.isConnected) return;
        const dropdownIsVisible = dropdown.getClientRects().length > 0;
        if (!dropdownIsVisible) {
          dropdownWasVisible = false;
          return;
        }
        // Overlay position updates can mutate style/class while the popup stays
        // open. Only treat a hidden-to-visible transition as a popup opening;
        // otherwise selection and focus updates keep the scrollbar flashing.
        if (dropdownWasVisible) return;
        dropdownWasVisible = true;
        scheduleUpdate();
        if (state.scrollbarFrame) window.cancelAnimationFrame(state.scrollbarFrame);
        state.scrollbarFrame = window.requestAnimationFrame(() => {
          state.scrollbarFrame = 0;
          updateScrollbar();
        });
      });
      for (let element: HTMLElement | null = dropdown; element && element !== document.body; element = element.parentElement) {
        state.popupObserver.observe(element, { attributes: true, attributeFilter: ['class', 'style'] });
      }
    }
    // rc-virtual-list shows its scrollbar briefly on mount, then hides it
    // three seconds after the latest scroll offset change.
    updateScrollbar();
    scheduleUpdate();
  }

  static detachVirtualList(holder: HTMLElement) {
    const state = virtualLists.get(holder);
    if (!state) return;
    state.detached = true;
    holder.removeEventListener('scroll', state.onScroll);
    holder.removeEventListener('wheel', state.onWheel);
    state.scrollbarObserver?.disconnect();
    state.popupObserver?.disconnect();
    if (state.frame) window.cancelAnimationFrame(state.frame);
    if (state.scrollbarFrame) window.cancelAnimationFrame(state.scrollbarFrame);
    window.clearTimeout(state.hideThumbTimeout);
    virtualLists.delete(holder);
  }

  static cancelScroll(holder: HTMLElement) {
    pendingScrolls.get(holder)?.();
  }

  static scrollToIndex(holder: HTMLElement, index: number, label?: string) {
    if (!holder || !Number.isInteger(index) || index < 0) return;
    this.cancelScroll(holder);
    const scroll = () => {
      const item = holder.querySelector<HTMLElement>('.ant-select-item-option');
      const height = item?.offsetHeight
        || parseFloat(getComputedStyle(holder).getPropertyValue('--ant-select-option-height')) || 32;
      const top = index * height;
      const bottom = top + height;
      if (top < holder.scrollTop) holder.scrollTop = top;
      else if (bottom > holder.scrollTop + holder.clientHeight)
        holder.scrollTop = bottom - holder.clientHeight;
      return { top, bottom };
    };
    if (!label) { scroll(); return; }

    // Virtualize can revise spacer heights after popup motion or a resize.
    // Reapply the requested position as its DOM arrives, until the target row
    // really is visible. Never keep fighting a user's wheel/touch interaction.
    const win = holder.ownerDocument.defaultView;
    const observer = new win.MutationObserver(() => update());
    const cancel = () => {
      if (pendingScrolls.get(holder) !== cancel) return;
      observer.disconnect();
      win.clearTimeout(timeout);
      holder.removeEventListener('wheel', cancel);
      holder.removeEventListener('pointerdown', cancel);
      holder.removeEventListener('touchstart', cancel);
      pendingScrolls.delete(holder);
    };
    const update = () => {
      if (pendingScrolls.get(holder) !== cancel) return;
      if (!holder.isConnected) { cancel(); return; }
      const range = scroll();
      const bounds = holder.getBoundingClientRect();
      const target = Array.from(holder.querySelectorAll<HTMLElement>('.ant-select-item-option'))
        .find(option => option.title === label);
      if (target) {
        const rect = target.getBoundingClientRect();
        if (rect.height > 0 && rect.top >= bounds.top && rect.bottom <= bounds.bottom
          && holder.scrollTop <= range.top && holder.scrollTop + holder.clientHeight >= range.bottom) cancel();
      }
    };
    const timeout = win.setTimeout(cancel, 10000);
    pendingScrolls.set(holder, cancel);
    observer.observe(holder, { subtree: true, childList: true, attributes: true, attributeFilter: ['style'] });
    holder.addEventListener('wheel', cancel, { passive: true });
    holder.addEventListener('pointerdown', cancel);
    holder.addEventListener('touchstart', cancel, { passive: true });
    update();
  }
}
