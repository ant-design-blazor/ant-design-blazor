import { infoHelper as domInfoHelper} from '../modules/dom/infoHelper';
import { boxSize, domRect } from '../modules/dom/types';

class ResizeObserverEntry {
  borderBoxSize?: boxSize
  contentBoxSize?: boxSize
  contentRect?: domRect
  target?: Element
}

export class resizeObserver {

  static isResizeObserverSupported(): boolean {
    return "ResizeObserver" in window;
  }

  private static resizeObservers: Map<string, ResizeObserver> = new Map<string, ResizeObserver>();

  static create(key, invoker, isDotNetInvoker: boolean = true ) {
    let observer;
        
    if (isDotNetInvoker) {
      observer = new ResizeObserver((entries, observer) => resizeObserver.observerCallBack(entries, observer, invoker));
    } else {
      observer = new ResizeObserver((entries, observer) => invoker(entries, observer));
    }
    resizeObserver.resizeObservers.set(key, observer)
  }

  static observe(key: string, element) {
    const observer = resizeObserver.resizeObservers.get(key);
    if (observer) {
      const domElement = domInfoHelper.get(element);
      observer.observe(domElement);
    }
  }

  static disconnect(key: string): void {
    const observer = this.resizeObservers.get(key)
    if (observer) {
      observer.disconnect()
    }
  }

  static unobserve(key: string, element: Element): void {
    const observer = this.resizeObservers.get(key)

    if (observer) {
      const domElement = domInfoHelper.get(element);
      observer.unobserve(domElement)
    }
  }

  static dispose(key: string): void {
    this.disconnect(key)
    this.resizeObservers.delete(key)
  }

  private static observerCallBack(entries, observer, invoker) {
    if (invoker) {
      const mappedEntries = new Array<ResizeObserverEntry>()
      entries.forEach(entry => {        
        if (entry) {
          const mEntry = new ResizeObserverEntry()
          if (entry.borderBoxSize) {
            mEntry.borderBoxSize = {
              blockSize: entry.borderBoxSize.blockSize,
              inlineSize: entry.borderBoxSize.inlineSize
            }
          }

          if (entry.contentBoxSize) {
            mEntry.contentBoxSize = {
              blockSize: entry.contentBoxSize.blockSize,
              inlineSize: entry.contentBoxSize.inlineSize
            }
          }

          if (entry.contentRect) {
            mEntry.contentRect = {
              x: entry.contentRect.x,
              y: entry.contentRect.y,
              width: entry.contentRect.width,
              height: entry.contentRect.height,
              top: entry.contentRect.top,
              right: entry.contentRect.right,
              bottom: entry.contentRect.bottom,
              left: entry.contentRect.left
            }

          }

          //mEntry.target = entry.target
          mappedEntries.push(mEntry)
        }
      })

      const entriesJson = JSON.stringify(mappedEntries)
      invoker.invokeMethodAsync('Invoke', entriesJson)
    }
  }

}