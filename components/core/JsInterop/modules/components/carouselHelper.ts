interface TouchOptions {
    element: HTMLElement;
    dotNetHelper: any;
    minSwipeDistance?: number;
}

export class carouselHelper {
    static initializeTouch(options: TouchOptions) {
        const { element, dotNetHelper, minSwipeDistance = 50 } = options;
        if (!element) return;

        let startX = 0;
        let startY = 0;

        function handleTouchStart(e: TouchEvent) {
            startX = e.touches[0].clientX;
            startY = e.touches[0].clientY;
        }

        function handleTouchEnd(e: TouchEvent) {
            if (!e.changedTouches?.[0]) return;

            const endX = e.changedTouches[0].clientX;
            const endY = e.changedTouches[0].clientY;
            const deltaX = endX - startX;
            const deltaY = endY - startY;

            // Only handle horizontal swipes
            if (Math.abs(deltaX) > Math.abs(deltaY) && Math.abs(deltaX) > minSwipeDistance) {
                if (deltaX > 0) {
                    dotNetHelper.invokeMethodAsync('HandleSwipe', 'right');
                } else {
                    dotNetHelper.invokeMethodAsync('HandleSwipe', 'left');
                }
            }
        }

        element.addEventListener('touchstart', handleTouchStart, { passive: true });
        element.addEventListener('touchend', handleTouchEnd, { passive: true });

        return {
            dispose: () => {
                element.removeEventListener('touchstart', handleTouchStart);
                element.removeEventListener('touchend', handleTouchEnd);
            }
        };
    }
} 