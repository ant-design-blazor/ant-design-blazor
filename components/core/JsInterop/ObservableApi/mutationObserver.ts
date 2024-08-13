import { infoHelper as domInfoHelper } from '../modules/dom/infoHelper';

export class mutationObserver {
  private static mutationObservers: Map<string, MutationObserver> = new Map<string, MutationObserver>();

  static create(key: string, invoker, isDotNetInvoker: boolean = true) {
    let observer;

    if (isDotNetInvoker) {
      observer = new MutationObserver(mutations => mutationObserver.observerCallback(mutations, invoker))
    } else {
      observer = new MutationObserver(mutations => invoker(mutations))
    }
    mutationObserver.mutationObservers.set(key, observer)
  }

  static observe(key: string, element, options?: MutationObserverInit) {
    const observer = mutationObserver.mutationObservers.get(key);
    if (observer) {
      const domElement = domInfoHelper.get(element);
      observer.observe(domElement, options);
    }
  }

  static disconnect(key: string): void {
    const observer = this.mutationObservers.get(key)
    if (observer) {
      observer.disconnect()
    }
  }

  static dispose(key: string): void {
    this.disconnect(key)
    this.mutationObservers.delete(key)
  }

  private static observerCallback(mutations, invoker) {
    //TODO: serialize a proper object (check resizeObserver.ts for sample)
    const entriesJson = JSON.stringify(mutations)
    invoker.invokeMethodAsync('Invoke', entriesJson)
  }
}