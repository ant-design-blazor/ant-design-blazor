import { infoHelper as domInfoHelper } from '../modules/dom/infoHelper';

export class intersectionObserver {
  // @ts-ignore: TS2304: Cannot find name 'IntersectionObserver'
  private static intersectionObservers: Map<string, IntersectionObserver> = new Map<string, IntersectionObserver>();


  static create(key: string, invoker, isDotNetInvoker: boolean = true) {
    // @ts-ignore: TS2304: Cannot find name 'IntersectionObserver'
    let observer;

    if (isDotNetInvoker) {
      observer = new IntersectionObserver(mutations => intersectionObserver.observerCallback(mutations, invoker))
    } else {
      observer = new IntersectionObserver(mutations => invoker(mutations))
    }
    intersectionObserver.intersectionObservers.set(key, observer)
  }

  static observe(key: string, element, options?: IntersectionObserverInit) {
    const observer = intersectionObserver.intersectionObservers.get(key);
    if (observer) {
      let domElement = domInfoHelper.get(element);
      observer.observe(domElement);
    }
  }

  static disconnect(key: string): void {
    const observer = this.intersectionObservers.get(key)
    if (observer) {
      observer.disconnect()
    }
  }

  static dispose(key: string): void {
    this.disconnect(key)
    this.intersectionObservers.delete(key)
  }

  private static observerCallback(mutations, invoker) {
    //TODO: serialize a proper object (check resizeObserver.ts for sample)
    const entriesJson = JSON.stringify(mutations)
    invoker.invokeMethodAsync('Invoke', entriesJson)
  }
}