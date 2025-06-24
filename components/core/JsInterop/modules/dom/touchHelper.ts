interface TouchOptions {
    element: HTMLElement;
    dotNetHelper: any;
    minSwipeDistance?: number;
    vertical?: boolean;
}

export class touchHelper {
  static initializeTouch(options: TouchOptions) {
    const { element, dotNetHelper, minSwipeDistance = 50, vertical = false } = options;

    let startX = 0;
    let startY = 0;
    let isProcessingSwipe = false;

    function handleTouchStart(e: TouchEvent) {
      if (!e.touches?.[0]) return;
      startX = e.touches[0].clientX;
      startY = e.touches[0].clientY;
      isProcessingSwipe = false;
    }

    function handleTouchEnd(e: TouchEvent) {
      if (!e.changedTouches?.[0] || isProcessingSwipe) return;

      const endX = e.changedTouches[0].clientX;
      const endY = e.changedTouches[0].clientY;
      const deltaX = endX - startX;
      const deltaY = endY - startY;

      if (vertical) {
        if (Math.abs(deltaY) > Math.abs(deltaX) && Math.abs(deltaY) > minSwipeDistance) {
          isProcessingSwipe = true;
          dotNetHelper.invokeMethodAsync('HandleSwipe', deltaY > 0 ? 'down' : 'up')
            .then(() => { isProcessingSwipe = false; })
            .catch(() => { isProcessingSwipe = false; });
        }
      } else {
        if (Math.abs(deltaX) > Math.abs(deltaY) && Math.abs(deltaX) > minSwipeDistance) {
          isProcessingSwipe = true;
          dotNetHelper.invokeMethodAsync('HandleSwipe', deltaX > 0 ? 'right' : 'left')
            .then(() => { isProcessingSwipe = false; })
            .catch(() => { isProcessingSwipe = false; });
        }
      }
    }

        // 移除可能存在的旧事件监听器
    element.removeEventListener('touchstart', handleTouchStart);
    element.removeEventListener('touchend', handleTouchEnd);

        // 添加新的事件监听器
    element.addEventListener('touchstart', handleTouchStart, { passive: true });
    element.addEventListener('touchend', handleTouchEnd, { passive: true });

    element['dispose'] = () => {
      try {
        element.removeEventListener('touchstart', handleTouchStart);
        element.removeEventListener('touchend', handleTouchEnd);
      } catch (e) {
        console.warn('Error disposing touch events:', e);
      }
    }
  }

  static dispose(element: HTMLElement) {
    element['dispose']();
  }
} 