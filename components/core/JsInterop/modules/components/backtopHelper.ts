import { domInfoHelper } from '../dom/exports'

export class backtopHelper {
  private static readonly visibilityDurationMs = 200;
  private static animationFrames = new WeakMap<object, number>();
  private static bindings = new WeakMap<HTMLElement, { target: any; click: () => void; scroll: () => void; resize: () => void }>();
  private static originalScrollBehaviors = new WeakMap<object, string>();
  private static visibilityTimers = new WeakMap<HTMLElement, number>();
  private static visibilityFrames = new WeakMap<HTMLElement, number>();
  private static visibilityStates = new WeakMap<HTMLElement, boolean>();

  static backTop(target: string, duration = 450) {
    const container = domInfoHelper.get(target || 'window') || window;
    this.scrollToTop(container, duration);
  }

  static bindBackTop(button: HTMLElement, target: string, duration = 450, visibilityHeight = 400, showProgress = false) {
    if (!button || this.bindings.has(button)) return;
    const container = domInfoHelper.get(target || 'window') || window;
    const sync = () => {
      const { offset, maxScroll } = this.getScrollState(container);
      const visible = offset >= visibilityHeight;
      const previousVisibility = this.visibilityStates.get(button);
      if (previousVisibility !== visible) {
        const visibilityFrame = this.visibilityFrames.get(button);
        if (visibilityFrame !== undefined) {
          cancelAnimationFrame(visibilityFrame);
          this.visibilityFrames.delete(button);
        }
        const visibilityTimer = this.visibilityTimers.get(button);
        if (visibilityTimer !== undefined) {
          window.clearTimeout(visibilityTimer);
          this.visibilityTimers.delete(button);
        }
        if (visible) {
          button.style.visibility = 'visible';
          button.style.pointerEvents = '';
          // Make the visible/hidden change take effect before starting the
          // opacity transition. Otherwise the browser can coalesce both style
          // writes and the BackTop button appears without fading in.
          const currentOpacity = Number.parseFloat(getComputedStyle(button).opacity);
          this.setOpacity(button, Number.isFinite(currentOpacity) ? currentOpacity : 0);
          // Reversing a fade-out starts from its current opacity and continues
          // toward visible without jumping back to transparent.
          const frame = requestAnimationFrame(() => {
            this.visibilityFrames.delete(button);
            if (this.visibilityStates.get(button)) this.setOpacity(button, 1);
          });
          this.visibilityFrames.set(button, frame);
        } else {
          button.style.pointerEvents = 'none';
          this.setOpacity(button, 0);
        }
        this.visibilityStates.set(button, visible);
        if (!visible && button.style.visibility !== 'hidden') {
          const reducedMotion = window.matchMedia?.('(prefers-reduced-motion: reduce)').matches;
          if (reducedMotion) {
            button.style.visibility = 'hidden';
          } else {
            const timer = window.setTimeout(() => {
              button.style.visibility = 'hidden';
              this.visibilityTimers.delete(button);
            }, this.visibilityDurationMs);
            this.visibilityTimers.set(button, timer);
          }
        }
      }
      if (showProgress) {
        button.style.setProperty('--ant-float-btn-progress', `${maxScroll > 0 ? Math.min(Math.max(offset / maxScroll, 0), 1) : 0}turn`);
      }
    };
    const click = () => this.scrollToTop(container, duration);
    const resize = () => sync();
    button.addEventListener('click', click);
    container.addEventListener('scroll', sync, { passive: true });
    if (showProgress) window.addEventListener('resize', resize, { passive: true });
    this.bindings.set(button, { target: container, click, scroll: sync, resize });
    sync();
  }

  static unbindBackTop(button: HTMLElement) {
    const binding = button && this.bindings.get(button);
    if (!binding) return;
    button.removeEventListener('click', binding.click);
    binding.target.removeEventListener('scroll', binding.scroll);
    window.removeEventListener('resize', binding.resize);
    const visibilityTimer = this.visibilityTimers.get(button);
    if (visibilityTimer !== undefined) window.clearTimeout(visibilityTimer);
    this.visibilityTimers.delete(button);
    const visibilityFrame = this.visibilityFrames.get(button);
    if (visibilityFrame !== undefined) cancelAnimationFrame(visibilityFrame);
    this.visibilityFrames.delete(button);
    this.visibilityStates.delete(button);
    this.bindings.delete(button);
  }

  private static setOpacity(button: HTMLElement, targetOpacity: number) {
    const reducedMotion = window.matchMedia?.('(prefers-reduced-motion: reduce)').matches;
    button.style.transition = reducedMotion ? 'none' : `opacity ${this.visibilityDurationMs}ms linear`;
    button.style.opacity = String(targetOpacity);
  }

  private static scrollToTop(container: any, duration: number) {
    const isWindow = container === window;
    const isDocument = container === document;
    const element = isDocument ? document.documentElement : container as HTMLElement;
    const scrollTop = isWindow
      ? window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0
      : element?.scrollTop || 0;
    const scroll = (value: number) => {
      if (isWindow) {
        window.scrollTo(window.pageXOffset, value);
      } else if (isDocument) {
        document.documentElement.scrollTop = value;
      } else if (element) {
        element.scrollTop = value;
      }
    };

    const previousAnimation = this.animationFrames.get(container);
    if (previousAnimation) {
      cancelAnimationFrame(previousAnimation);
      this.animationFrames.delete(container);
    }

    const behaviorElement = isWindow || isDocument ? document.documentElement : element;
    if (behaviorElement && !this.originalScrollBehaviors.has(behaviorElement)) {
      this.originalScrollBehaviors.set(behaviorElement, behaviorElement.style.scrollBehavior);
    }
    if (behaviorElement) {
      // Global scroll-behavior:smooth would start a second native animation
      // for every animation frame and make this fixed-duration scroll lag.
      behaviorElement.style.scrollBehavior = 'auto';
    }

    const prefersReducedMotion = window.matchMedia?.('(prefers-reduced-motion: reduce)').matches;
    const animationDuration = prefersReducedMotion ? 0 : Math.max(0, duration);
    if (animationDuration === 0 || scrollTop === 0) {
      scroll(0);
      this.restoreScrollBehavior(behaviorElement);
      return;
    }

    const startTime = Date.now();
    const easeInOutCubic = (time: number, start: number, end: number, total: number) => {
      const distance = end - start;
      time /= total / 2;
      if (time < 1) {
        return (distance / 2) * time * time * time + start;
      }
      time -= 2;
      return (distance / 2) * (time * time * time + 2) + start;
    };

    const frame = () => {
      const elapsed = Math.min(Date.now() - startTime, animationDuration);
      scroll(easeInOutCubic(elapsed, scrollTop, 0, animationDuration));
      if (elapsed < animationDuration) {
        this.animationFrames.set(container, requestAnimationFrame(frame));
      } else {
        this.animationFrames.delete(container);
        scroll(0);
        this.restoreScrollBehavior(behaviorElement);
      }
    };
    this.animationFrames.set(container, requestAnimationFrame(frame));
  }

  private static getScrollState(container: any) {
    const isWindow = container === window;
    const isDocument = container === document;
    const element = isWindow || isDocument ? document.documentElement : container as HTMLElement;
    const offset = isWindow
      ? window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0
      : isDocument ? document.documentElement.scrollTop : element?.scrollTop || 0;
    const maxScroll = Math.max((element?.scrollHeight || 0) - (element?.clientHeight || 0), 0);
    return { offset, maxScroll };
  }

  private static restoreScrollBehavior(element: HTMLElement | null) {
    if (!element) return;
    element.style.scrollBehavior = this.originalScrollBehaviors.get(element) || '';
    this.originalScrollBehaviors.delete(element);
  }
}
