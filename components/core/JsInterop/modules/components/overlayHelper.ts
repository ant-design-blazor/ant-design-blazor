import { domInfoHelper, eventHelper, domManipulationHelper, domTypes } from '../dom/exports'
import { Placement, TriggerBoundyAdjustMode, overlayConstraints, overlayPosition, Overlay } from './overlay'
import { state } from '../stateProvider';

function resolveOffset(element: HTMLElement, fallback: number, cssExpression?: string): number {
  if (!cssExpression) {
    return fallback;
  }

  const probe = document.createElement('div');
  probe.style.cssText = 'position:absolute;display:block;visibility:hidden;height:0;padding:0;border:0;margin:0;';
  probe.style.width = cssExpression;
  element.appendChild(probe);
  const resolved = probe.getBoundingClientRect().width;
  probe.remove();

  return Number.isFinite(resolved) && resolved > 0 ? resolved : fallback;
}

function getInheritedAntDesignTokenStyle(triggerSelector: string): string {
  const trigger = domInfoHelper.get(triggerSelector) as HTMLElement;
  if (!trigger) return '';

  const triggerStyle = window.getComputedStyle(trigger);
  const bodyStyle = window.getComputedStyle(document.body);
  const declarations: string[] = [];
  for (let index = 0; index < triggerStyle.length; index++) {
    const property = triggerStyle.item(index);
    if (!property.startsWith('--ant-')) continue;
    const value = triggerStyle.getPropertyValue(property).trim();
    if (value && value !== bodyStyle.getPropertyValue(property).trim()) {
      declarations.push(`${property}:${value}`);
    }
  }
  return declarations.join(';');
}

function prepareOverlayInContainer(overlay: HTMLElement, trigger: HTMLElement, containerSelector: string): void {
  // Measure in the same DOM context in which the popup will be displayed.
  // Select popup content can resolve different typography and sizing tokens
  // after it leaves a scoped ConfigProvider, so copy those tokens before the
  // measurement and keep the popup invisible while moving it.
  const triggerStyle = window.getComputedStyle(trigger);
  const bodyStyle = window.getComputedStyle(document.body);
  for (let index = 0; index < triggerStyle.length; index++) {
    const property = triggerStyle.item(index);
    if (!property.startsWith('--ant-')) continue;
    const value = triggerStyle.getPropertyValue(property).trim();
    if (value && value !== bodyStyle.getPropertyValue(property).trim()) {
      overlay.style.setProperty(property, value);
    }
  }

  const container = domInfoHelper.get(containerSelector) as HTMLElement;
  if (container && overlay.parentElement !== container) {
    overlay.style.visibility = 'hidden';
    overlay.style.display = 'block';
    domManipulationHelper.addElementTo(overlay, containerSelector);
  }
}

type ImmediateOpenWidthMode = 'match-trigger' | 'content' | 'custom' | '';

export class overlayHelper {
  static overlayRegistry: { [key: string]: Overlay} = {};
  static immediateOpenTriggers = new WeakSet<HTMLElement>();
  private static measuringOverlayTransitions = new WeakMap<HTMLElement, { value: string; priority: string }>();
  private static pendingOverlayRebuilds: { [key: string]: {
    element: HTMLElement;
    listener: (event: Event) => void;
    timeout: number;
    rebuild: () => void;
  } } = {};

  static getInheritedAntDesignTokenStyle(triggerSelector: string): string {
    return getInheritedAntDesignTokenStyle(triggerSelector);
  }

  private static clearPendingOverlayRebuild(blazorId: string) {
    const pending = this.pendingOverlayRebuilds[blazorId];
    if (!pending) return;

    pending.element.removeEventListener('animationend', pending.listener);
    pending.element.removeEventListener('transitionend', pending.listener);
    window.clearTimeout(pending.timeout);
    delete this.pendingOverlayRebuilds[blazorId];
  }

  private static isEnterMotionRunning(element: HTMLElement): boolean {
    const hasEnterClass = Array.from(element.classList)
      .some(className => className.endsWith('-enter') || className.endsWith('-enter-active'));
    if (!hasEnterClass) return false;

    const animations = element.getAnimations?.();
    if (animations?.some(animation => animation.playState === 'running')) return true;
    if (animations && animations.length > 0) return false;

    // The enter class is applied before the browser creates its Animation
    // object (usually on the next style/layout flush). Treat that startup
    // window as an active motion too, or a concurrent Blazor render can
    // dispose and recreate the positioning Overlay just before the animation
    // starts. Computed durations also let reduced-motion styles complete
    // immediately when they set both animation and transition durations to 0.
    const styles = window.getComputedStyle(element);
    const maxMotionTime = (durations: string, delays: string) => {
      const parseTimes = (value: string) => value.split(',').map(part => {
        const time = part.trim();
        const amount = Number.parseFloat(time);
        if (!Number.isFinite(amount)) return 0;
        return time.endsWith('ms') ? amount : amount * 1000;
      });
      const durationValues = parseTimes(durations);
      const delayValues = parseTimes(delays);
      return Math.max(0, ...durationValues.map((duration, index) => duration + (delayValues[index % Math.max(delayValues.length, 1)] ?? 0)));
    };

    return maxMotionTime(styles.animationDuration, styles.animationDelay) > 0
      || maxMotionTime(styles.transitionDuration, styles.transitionDelay) > 0;
  }

  private static deferOverlayRebuildUntilMotionEnds(blazorId: string, element: HTMLElement, rebuild: () => void) {
    const current = this.pendingOverlayRebuilds[blazorId];
    if (current?.element === element) {
      // Keep only the latest render's parameters while retaining the same
      // completion listener and timeout for the in-flight enter motion.
      current.rebuild = rebuild;
      return;
    }
    this.clearPendingOverlayRebuild(blazorId);

    const pending = {
      element,
      listener: null as unknown as (event: Event) => void,
      timeout: 0,
      rebuild
    };
    const finish = () => {
      if (this.pendingOverlayRebuilds[blazorId] !== pending) return;
      if (element.isConnected && this.isEnterMotionRunning(element)) {
        // The timeout is only a fallback for missing end events, not a reason
        // to interrupt a longer-running enter motion.
        pending.timeout = window.setTimeout(finish, 100);
        return;
      }
      this.clearPendingOverlayRebuild(blazorId);
      pending.rebuild();
    };
    pending.listener = event => {
      if (event.target !== element) return;
      // A popup can emit several transition/animation events. Wait for the
      // last active motion so the deferred setup cannot restart the entrance.
      const animations = element.getAnimations?.();
      if (animations?.some(animation => animation.playState === 'running')) return;
      finish();
    };
    this.pendingOverlayRebuilds[blazorId] = pending;
    element.addEventListener('animationend', pending.listener);
    element.addEventListener('transitionend', pending.listener);
    // Reduced-motion styles or detached popups may not emit an end event.
    pending.timeout = window.setTimeout(finish, 1000);
  }

  static bindImmediateOpen(
    triggerSelector: string,
    overlaySelector: string,
    blazorId: string,
    hiddenClass: string,
    enterClass: string,
    overlayDisplayStyle: string,
    disabled: boolean,
    clickIgnoreSelector: string,
    openClass: string,
    placement: Placement,
    containerSelector: string,
    triggerBoundyAdjustMode: TriggerBoundyAdjustMode,
    triggerIsWrappedInDiv: boolean,
    triggerPrefixCls: string,
    verticalOffset: number,
    horizontalOffset: number,
    offsetCssExpression: string,
    arrowPointAtCenter: boolean,
    widthMode: ImmediateOpenWidthMode = '',
    width: string = '',
    maxWidth: string = ''
  ) {
    const trigger = domInfoHelper.get(triggerSelector) as HTMLElement;
    const overlay = domInfoHelper.get(overlaySelector) as HTMLElement;
    if (!trigger || !overlay || this.immediateOpenTriggers.has(trigger)) return;
    this.immediateOpenTriggers.add(trigger);
    const selectRoot = trigger.matches('.ant-select')
      ? trigger
      : trigger.querySelector<HTMLElement>('.ant-select') ?? trigger.closest<HTMLElement>('.ant-select');

    const openImmediately = (event: Event) => {
      if (disabled || selectRoot?.classList.contains('ant-select-disabled') || !overlay.classList.contains(hiddenClass)) return;
      const target = event.target as Element | null;
      if (clickIgnoreSelector && target?.closest(clickIgnoreSelector)) return;

      // Position and portal the already-rendered popup locally before the
      // Blazor Server click callback completes. Otherwise a server round trip
      // is required before the popup reaches its final position, which is
      // especially noticeable on large Select data sets.
      const widthTrigger = selectRoot ?? trigger;
      if (widthMode) {
        prepareOverlayInContainer(overlay, widthTrigger, containerSelector);
        this.prepareOverlayWidth(overlay, widthTrigger, widthMode, width, maxWidth);
      }
      this.addOverlayToContainer(
        blazorId,
        overlaySelector,
        triggerSelector,
        placement,
        containerSelector,
        triggerBoundyAdjustMode,
        triggerIsWrappedInDiv,
        triggerPrefixCls,
        verticalOffset,
        horizontalOffset,
        offsetCssExpression,
        arrowPointAtCenter
      );
      // Keep the popup invisible for one layout frame after its final width
      // and position have been applied. This prevents the browser from
      // painting the intrinsic width first and then growing it when the
      // measured width is committed.
      void this.revealMeasuredOverlay(overlay, hiddenClass, enterClass, overlayDisplayStyle);
      const stateRoot = selectRoot ?? trigger;
      if (openClass) stateRoot.classList.add(openClass);
      stateRoot.querySelectorAll<HTMLElement>('[role="combobox"]').forEach(input => {
        input.setAttribute('aria-expanded', 'true');
      });
    };

    trigger.addEventListener('pointerdown', event => {
      if (event.button === 0) openImmediately(event);
    });
    trigger.addEventListener('keydown', event => {
      const keyboardEvent = event as KeyboardEvent;
      if (keyboardEvent.key === 'Enter' || keyboardEvent.key === ' ' || keyboardEvent.key === 'ArrowDown') {
        openImmediately(event);
      }
    });
  }

  /** Reveal a width-measured popup only after its final box has been laid out. */
  static async revealMeasuredOverlay(
    overlay: HTMLElement,
    hiddenClass: string,
    enterClass: string,
    displayStyle: string
  ): Promise<void> {
    if (!overlay) return;

    if (hiddenClass) overlay.classList.remove(hiddenClass);
    overlay.style.display = displayStyle || 'inline-flex';
    overlay.style.visibility = 'hidden';
    // Force style/layout with the measured dimensions before the enter class
    // is added. The next frame is the first frame the popup can be visible.
    overlay.getBoundingClientRect();
    await new Promise<void>(resolve => requestAnimationFrame(() => resolve()));
    const transition = this.measuringOverlayTransitions.get(overlay);
    if (transition) {
      if (transition.value) {
        overlay.style.setProperty('transition', transition.value, transition.priority);
      } else {
        overlay.style.removeProperty('transition');
      }
      this.measuringOverlayTransitions.delete(overlay);
    }
    if (!overlay.isConnected) return;

    overlay.getBoundingClientRect();
    overlay.style.visibility = '';
    enterClass.split(/\s+/).filter(Boolean).forEach(className => overlay.classList.add(className));
  }

  /**
   * Measure and lock a popup's width while it is rendered but invisible. This
   * must happen before it is portaled/revealed so its first visible frame and
   * placement calculation use the final dimensions.
   */
  static prepareOverlayWidth(
    overlay: HTMLElement,
    trigger: HTMLElement,
    mode: Exclude<ImmediateOpenWidthMode, ''>,
    width: string,
    maxWidth: string
  ): number {
    const triggerWidth = trigger.getBoundingClientRect().width;
    // A theme or consumer rule using `transition: all` can animate the width
    // from its intrinsic size even while the popup is hidden. Keep measurement
    // and positioning instantaneous; restore the original transition just
    // before applying the enter classes.
    if (!this.measuringOverlayTransitions.has(overlay)) {
      this.measuringOverlayTransitions.set(overlay, {
        value: overlay.style.getPropertyValue('transition'),
        priority: overlay.style.getPropertyPriority('transition')
      });
    }
    overlay.style.setProperty('transition', 'none', 'important');
    overlay.style.visibility = 'hidden';
    overlay.style.display = 'block';
    overlay.style.position = 'absolute';
    overlay.style.left = '-9999px';
    // getBoundingClientRect() returns the border-box width. Keep that same
    // sizing model when writing the measured value back; otherwise padding
    // and borders are added a second time by content-box sizing and the popup
    // grows on its first visible frame.
    overlay.style.boxSizing = 'border-box';
    overlay.style.minWidth = `${triggerWidth}px`;
    overlay.style.maxWidth = maxWidth && maxWidth.toLowerCase() !== 'auto' ? maxWidth : '';

    if (mode === 'match-trigger') {
      overlay.style.width = `${triggerWidth}px`;
      // Commit the final width while the popup is still invisible. This is
      // also needed for the trigger-width path, which otherwise might not
      // force layout until the enter class has already been applied.
      overlay.getBoundingClientRect();
      return triggerWidth;
    }

    overlay.style.width = mode === 'custom' ? width : 'max-content';
    const measuredWidth = overlay.getBoundingClientRect().width;
    const resolvedWidth = measuredWidth > 0 ? measuredWidth : triggerWidth;
    overlay.style.width = `${resolvedWidth}px`;
    // Measure once more after locking the resolved width. The caller can now
    // position and reveal the overlay without its first visible frame
    // changing size during the entrance animation.
    overlay.getBoundingClientRect();
    return resolvedWidth;
  }

  /**
   * Prepare a popup before the Blazor open render. An immediate pointer open
   * may already have done this work; in that case reuse its locked width so we
   * don't hide or measure a popup while its enter animation is running.
   */
  static prepareOverlayWidthForShow(
    overlaySelector: string,
    triggerSelector: string,
    containerSelector: string,
    mode: ImmediateOpenWidthMode,
    width: string,
    maxWidth: string
  ): string {
    const overlay = domInfoHelper.get(overlaySelector) as HTMLElement;
    const trigger = domInfoHelper.get(triggerSelector) as HTMLElement;
    if (!overlay || !trigger || !mode) return '';

    const isVisible = overlay.isConnected
      && overlay.offsetParent !== null
      && getComputedStyle(overlay).visibility !== 'hidden';
    const selectRoot = trigger.matches('.ant-select')
      ? trigger
      : trigger.querySelector<HTMLElement>('.ant-select') ?? trigger.closest<HTMLElement>('.ant-select');
    if (mode === 'match-trigger') {
      // Keep the original fractional trigger measurement across the Blazor
      // rerender. CSSStyleDeclaration serializes widths to three decimals, so
      // parsing overlay.style.width here would lose the final 1/64px quantum.
      const triggerWidth = (selectRoot ?? trigger).getBoundingClientRect().width;
      if (!isVisible) {
        prepareOverlayInContainer(overlay, selectRoot ?? trigger, containerSelector);
        this.prepareOverlayWidth(overlay, selectRoot ?? trigger, 'match-trigger', width, maxWidth);
      }
      return String(triggerWidth);
    }
    const existingWidth = parseFloat(overlay.style.width);
    if (isVisible && Number.isFinite(existingWidth) && existingWidth > 0) {
      return String(existingWidth);
    }

    prepareOverlayInContainer(overlay, selectRoot ?? trigger, containerSelector);
    return String(this.prepareOverlayWidth(overlay, selectRoot ?? trigger, mode as Exclude<ImmediateOpenWidthMode, ''>, width, maxWidth));
  }

  static async revealOverlayForShow(
    overlaySelector: string,
    hiddenClass: string,
    enterClass: string,
    displayStyle: string
  ): Promise<void> {
    const overlay = domInfoHelper.get(overlaySelector) as HTMLElement;
    if (!overlay) return;

    if ((!hiddenClass || !overlay.classList.contains(hiddenClass)) && getComputedStyle(overlay).visibility !== 'hidden') {
      return;
    }
    await this.revealMeasuredOverlay(overlay, hiddenClass, enterClass, displayStyle);
  }

  static addOverlayToContainer(blazorId: string, 
    overlaySelector, triggerSelector, placement: Placement,  containerSelector: string,
    triggerBoundyAdjustMode: TriggerBoundyAdjustMode, triggerIsWrappedInDiv: boolean, triggerPrefixCls: string,
    verticalOffset: number, horizontalOffset: number, offsetCssExpression: string, arrowPointAtCenter: boolean,
    overlayTop?: number, overlayLeft?: number
  ): overlayPosition {      
    const overlayElement = domInfoHelper.get(overlaySelector) as HTMLDivElement;    
    const containerElement = domInfoHelper.get(containerSelector) as HTMLElement;
    const triggerElement = domInfoHelper.get(triggerSelector) as HTMLElement;    

    const previousOverlay = this.overlayRegistry[blazorId];
    if (previousOverlay) {
      const verticalResolvedOffset = resolveOffset(overlayElement, verticalOffset, offsetCssExpression);
      const horizontalResolvedOffset = resolveOffset(overlayElement, horizontalOffset, offsetCssExpression);
      if (previousOverlay.matchesContext(
        overlayElement,
        containerElement,
        triggerElement,
        triggerBoundyAdjustMode,
        triggerIsWrappedInDiv,
        triggerPrefixCls
      )) {
        this.clearPendingOverlayRebuild(blazorId);
        // Immediate-open triggers create the positioning observer on pointerdown,
        // before the Blazor Server callback returns. Reuse it on the ensuing
        // render so the entering popup keeps its DOM, observers, and animation.
        previousOverlay.updateOffsets(verticalResolvedOffset, horizontalResolvedOffset);
        previousOverlay.updatePlacement(placement, arrowPointAtCenter);
        const overlayPresets = overlayTop != null || overlayLeft != null
          ? { x: overlayLeft, y: overlayTop }
          : undefined;
        setTimeout(() => {
          if (this.overlayRegistry[blazorId] === previousOverlay) {
            previousOverlay.calculatePosition(true, false, overlayPresets);
          }
        }, 10);
        return previousOverlay.calculatePosition(false, false, overlayPresets);
      }

      if (this.isEnterMotionRunning(overlayElement)) {
        // Blazor may render a new trigger/popup reference while the current
        // popup is entering. Preserve that motion; apply the latest context
        // only after it settles instead of disposing and recreating Overlay.
        this.deferOverlayRebuildUntilMotionEnds(blazorId, overlayElement, () => {
          this.addOverlayToContainer(
            blazorId,
            overlaySelector,
            triggerSelector,
            placement,
            containerSelector,
            triggerBoundyAdjustMode,
            triggerIsWrappedInDiv,
            triggerPrefixCls,
            verticalOffset,
            horizontalOffset,
            offsetCssExpression,
            arrowPointAtCenter,
            overlayTop,
            overlayLeft
          );
        });
        return previousOverlay.calculatePosition(false, false);
      }

      this.clearPendingOverlayRebuild(blazorId);
      previousOverlay.dispose();
      delete this.overlayRegistry[blazorId];
    }

    if (!domManipulationHelper.addElementTo(overlaySelector, containerElement)) {
      console.log("Failed to add overlay. Details:", {
        triggerPrefixCls: triggerPrefixCls,
        overlaySelector: overlaySelector,
        containerElement: containerElement
      } );
      return null;
    }    

    let overlayPresets: domTypes.position;
    if (overlayTop != null || overlayLeft != null) {
      overlayPresets = { x: overlayLeft, y: overlayTop };
    }

    const overlayConstraints: overlayConstraints = {
      verticalOffset: resolveOffset(overlayElement, verticalOffset, offsetCssExpression),
      horizontalOffset: resolveOffset(overlayElement, horizontalOffset, offsetCssExpression),
      arrowPointAtCenter: arrowPointAtCenter
    };

    const overlay = new Overlay(blazorId, overlayElement, containerElement, triggerElement, placement, triggerBoundyAdjustMode, triggerIsWrappedInDiv, triggerPrefixCls, overlayConstraints);   
    //register object in store, so it can be retrieved during update/dispose
    this.overlayRegistry[blazorId] = overlay;

    setTimeout(() => {
      // We need to calculate the position again to ensure that the overlay is correctly placed. Needs to be optimized twice to once
      overlay.calculatePosition(true, false, overlayPresets);
    }, 10);
    return overlay.calculatePosition(false, true, overlayPresets);
  }


  static updateOverlayPosition(blazorId: string, overlaySelector, triggerSelector, placement: Placement,  containerSelector: string,
    triggerBoundyAdjustMode: TriggerBoundyAdjustMode, triggerIsWrappedInDiv: boolean, triggerPrefixCls: string,
    verticalOffset: number, horizontalOffset: number, offsetCssExpression: string, arrowPointAtCenter: boolean,
    overlayTop?: number, overlayLeft?: number): overlayPosition {
    const overlay = this.overlayRegistry[blazorId];
    if (overlay){
      const overlayElement = domInfoHelper.get(overlaySelector) as HTMLElement;
      overlay.updateOffsets(
        resolveOffset(overlayElement, verticalOffset, offsetCssExpression),
        resolveOffset(overlayElement, horizontalOffset, offsetCssExpression)
      );
      let overlayPresets: domTypes.position;
      if (overlayTop || overlayLeft) {
        overlayPresets = { x: overlayLeft, y: overlayTop };
      }      
      setTimeout(() => {
        // We need to calculate the position again to ensure that the overlay is correctly placed. Needs to be optimized twice to once
        overlay.calculatePosition(true, false, overlayPresets);
      }, 10);
      return overlay.calculatePosition(false, false, overlayPresets);      
    } else {
      //When page is slow, it may happen that rendering of an overlay may not happen, even if 
      //blazor thinks it did happen. In such a case, when overlay object is not found, just try
      //to render it again.
      return overlayHelper.addOverlayToContainer(blazorId, overlaySelector, triggerSelector, placement,  containerSelector,triggerBoundyAdjustMode, triggerIsWrappedInDiv, triggerPrefixCls, 
        verticalOffset, horizontalOffset, offsetCssExpression, arrowPointAtCenter,
        overlayTop, overlayLeft);      
    }    
  }

  static deleteOverlayFromContainer(blazorId: string) {
    this.clearPendingOverlayRebuild(blazorId);
    const overlay = this.overlayRegistry[blazorId];
    if (overlay) {      
      overlay.dispose();
      delete this.overlayRegistry[blazorId];
    }
  }

  static addPreventEnterOnOverlayVisible(element, overlayElement) {
    if (element && overlayElement) {
      const dom: HTMLElement = domInfoHelper.get(element);
      if (dom) {
        state.eventCallbackRegistry[element.id + "keydown:Enter"] = (e) => eventHelper.preventKeyOnCondition(e, "enter", () => overlayElement.offsetParent !== null);
        dom.addEventListener("keydown", state.eventCallbackRegistry[element.id + "keydown:Enter"], false);
      }
    }
  }

  static removePreventEnterOnOverlayVisible(element) {
    if (element) {
      const dom: HTMLElement = domInfoHelper.get(element);
      if (dom) {
        dom.removeEventListener("keydown", state.eventCallbackRegistry[element.id + "keydown:Enter"]);
        state.eventCallbackRegistry[element.id + "keydown:Enter"] = null; 
      }
    }
  }
}
