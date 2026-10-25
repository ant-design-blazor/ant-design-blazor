const pendingScrolls = new WeakMap<HTMLElement, () => void>();

export class selectHelper {
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
