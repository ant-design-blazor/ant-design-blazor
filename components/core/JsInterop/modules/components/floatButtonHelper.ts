type DragState = {
  button: HTMLElement;
  pointerId: number;
  startX: number;
  startY: number;
  startLeft: number;
  startTop: number;
  startRight: number;
  startBottom: number;
  offsetX: number;
  offsetY: number;
  deltaX: number;
  deltaY: number;
  dragging: boolean;
  originalTransform: string;
  originalTransition: string;
  originalCursor: string;
  originalTouchAction: string;
  originalUserSelect: string;
  pointerDown: (event: PointerEvent) => void;
  pointerMove: (event: PointerEvent) => void;
  pointerUp: (event: PointerEvent) => void;
};

export class floatButtonHelper {
  private static bindings = new WeakMap<HTMLElement, DragState>();

  static bindDraggable(container: HTMLElement) {
    if (!container || this.bindings.has(container)) return;

    const button = container.matches('.ant-float-btn')
      ? container
      : container.querySelector<HTMLElement>('.ant-float-btn');
    if (!button) return;

    const state: DragState = {
      button,
      pointerId: -1,
      startX: 0,
      startY: 0,
      startLeft: 0,
      startTop: 0,
      startRight: 0,
      startBottom: 0,
      offsetX: 0,
      offsetY: 0,
      deltaX: 0,
      deltaY: 0,
      dragging: false,
      originalTransform: button.style.transform,
      originalTransition: button.style.transition,
      originalCursor: button.style.cursor,
      originalTouchAction: button.style.touchAction,
      originalUserSelect: button.style.userSelect,
      pointerDown: () => {},
      pointerMove: () => {},
      pointerUp: () => {},
    };

    const setTransform = (x: number, y: number) => {
      const translate = `translate3d(${x}px, ${y}px, 0)`;
      button.style.transform = state.originalTransform && state.originalTransform !== 'none'
        ? `${translate} ${state.originalTransform}`
        : translate;
    };

    state.pointerDown = (event: PointerEvent) => {
      if (!event.isPrimary || (event.pointerType === 'mouse' && event.button !== 0)) return;
      state.pointerId = event.pointerId;
      state.startX = event.clientX;
      state.startY = event.clientY;
      const bounds = button.getBoundingClientRect();
      state.startLeft = bounds.left;
      state.startTop = bounds.top;
      state.startRight = bounds.right;
      state.startBottom = bounds.bottom;
      state.deltaX = 0;
      state.deltaY = 0;
      state.dragging = false;
      button.setPointerCapture?.(event.pointerId);
    };

    state.pointerMove = (event: PointerEvent) => {
      if (event.pointerId !== state.pointerId) return;
      const deltaX = event.clientX - state.startX;
      const deltaY = event.clientY - state.startY;
      if (!state.dragging && Math.hypot(deltaX, deltaY) < 10) return;

      state.dragging = true;
      state.deltaX = Math.max(-state.startLeft, Math.min(deltaX, window.innerWidth - state.startRight));
      state.deltaY = Math.max(-state.startTop, Math.min(deltaY, window.innerHeight - state.startBottom));
      button.style.transition = 'none';
      button.style.cursor = 'grabbing';
      button.style.userSelect = 'none';
      setTransform(state.offsetX + state.deltaX, state.offsetY + state.deltaY);
      event.preventDefault();
    };

    state.pointerUp = (event: PointerEvent) => {
      if (event.pointerId !== state.pointerId) return;
      if (state.dragging) {
        state.offsetX += state.deltaX;
        state.offsetY += state.deltaY;
        setTransform(state.offsetX, state.offsetY);
      }
      state.pointerId = -1;
      state.dragging = false;
      button.style.cursor = state.originalCursor || 'grab';
      button.style.userSelect = state.originalUserSelect;
      button.style.transition = state.originalTransition || 'transform 0.2s';
    };

    button.addEventListener('pointerdown', state.pointerDown);
    // Capture keeps pointer movement routed to the button while the pointer
    // leaves it, including when it is being dragged inside a demo iframe.
    button.addEventListener('pointermove', state.pointerMove, { passive: false });
    button.addEventListener('pointerup', state.pointerUp);
    button.addEventListener('pointercancel', state.pointerUp);
    button.style.touchAction = 'none';
    button.style.cursor = 'grab';
    this.bindings.set(container, state);
  }

  static unbindDraggable(container: HTMLElement) {
    const state = container && this.bindings.get(container);
    if (!state) return;
    const { button } = state;
    button.removeEventListener('pointerdown', state.pointerDown);
    button.removeEventListener('pointermove', state.pointerMove);
    button.removeEventListener('pointerup', state.pointerUp);
    button.removeEventListener('pointercancel', state.pointerUp);
    if (state.pointerId !== -1 && button.hasPointerCapture?.(state.pointerId)) {
      button.releasePointerCapture(state.pointerId);
    }
    button.style.cursor = state.originalCursor;
    button.style.touchAction = state.originalTouchAction;
    button.style.userSelect = state.originalUserSelect;
    button.style.transition = state.originalTransition;
    this.bindings.delete(container);
  }
}
