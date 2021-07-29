﻿import { infoHelper as domInfoHelper} from '../modules/dom/infoHelper';
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

  // @ts-ignore: TS2304: Cannot find name 'ResizeObserver'
  private static resizeObservers: Map<string, ResizeObserver> = new Map<string, ResizeObserver>();

  static create(key, invoker) {
    // @ts-ignore: TS2304: Cannot find name 'ResizeObserver'
    const observer = new ResizeObserver((entries, observer) => resizeObserver.observerCallBack(entries, observer, invoker));
    resizeObserver.resizeObservers.set(key, observer)
  }

  static observe(key: string, element) {
    const observer = resizeObserver.resizeObservers.get(key);
    if (observer) {
      let domElement = domInfoHelper.get(element);
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
      let domElement = domInfoHelper.get(element);
      observer.unobserve(domElement)
    }
  }

  static dispose(key: string): void {
    console.log("dispose", key);
    this.disconnect(key)
    this.resizeObservers.delete(key)
  }

  private static observerCallBack(entries, observer, invoker) {
    console.log("observerCallBack start", entries)
    if (invoker) {
      const mappedEntries = new Array<ResizeObserverEntry>()
      entries.forEach(entry => {
        console.log("observerCallBack entry", entry)
        if (entry) {
          //let mEntry: resizeObserverEntry; // = new ResizeObserverEntry()
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

          mEntry.target = entry.target
          mappedEntries.push(mEntry)
        }
      })

      const entriesJson = JSON.stringify(mappedEntries)
      invoker.invokeMethodAsync('Invoke', entriesJson)
    }
  }

}

//export function isResizeObserverSupported(): boolean {
//  return "ResizeObserver" in window;
//}

//// @ts-ignore: TS2304: Cannot find name 'ResizeObserver'
//const resizeObservers: Map<string, ResizeObserver> = new Map<string, ResizeObserver>();

//export function create(key, invoker) {
//  // @ts-ignore: TS2304: Cannot find name 'ResizeObserver'
//  const observer = new ResizeObserver((entries, observer) => observerCallBack(entries, observer, invoker));
//  resizeObservers.set(key, observer)
//}

//export function observe(key: string, element) {
//  const observer = resizeObservers.get(key);
//  if (observer) {
//    let domElement = domInfoHelper.get(element);
//    observer.observe(domElement);
//  }
//}

//export function disconnect(key: string): void {
//  const observer = this.resizeObservers.get(key)
//  if (observer) {
//    observer.disconnect()
//  }
//}

//export function unobserve(key: string, element: Element): void {
//  const observer = this.resizeObservers.get(key)

//  if (observer) {
//    let domElement = domInfoHelper.get(element);
//    observer.unobserve(domElement)
//  }
//}

//export function dispose(key: string): void {
//  console.log("dispose", key);
//  disconnect(key)
//  resizeObservers.delete(key)
//}

//export type resizeObserverEntry = {
//  borderBoxSize?: boxSize
//  contentBoxSize?: boxSize
//  contentRect?: domRect
//  target?: Element
//}

//class ResizeObserverEntry {
//  borderBoxSize?: boxSize
//  contentBoxSize?: boxSize
//  contentRect?: domRect
//  target?: Element
//}

//function observerCallBack(entries, observer, invoker) {
//  console.log("observerCallBack start", entries)
//  if (invoker) {
//    const mappedEntries = new Array<ResizeObserverEntry>()
//    entries.forEach(entry => {
//      console.log("observerCallBack entry", entry)
//      if (entry) {
//        //let mEntry: resizeObserverEntry; // = new ResizeObserverEntry()
//        const mEntry = new ResizeObserverEntry()
//        if (entry.borderBoxSize) {
//          mEntry.borderBoxSize = {
//            blockSize: entry.borderBoxSize.blockSize,
//            inlineSize: entry.borderBoxSize.inlineSize
//          }
//        }

//        if (entry.contentBoxSize) {
//          mEntry.contentBoxSize = {
//            blockSize: entry.contentBoxSize.blockSize,
//            inlineSize: entry.contentBoxSize.inlineSize
//          }
//        }

//        if (entry.contentRect) {
//          mEntry.contentRect = {
//            x: entry.contentRect.x,
//            y: entry.contentRect.y,
//            width: entry.contentRect.width,
//            height: entry.contentRect.height,
//            top: entry.contentRect.top,
//            right: entry.contentRect.right,
//            bottom: entry.contentRect.bottom,
//            left: entry.contentRect.left
//          }

//        }

//        mEntry.target = entry.target
//        mappedEntries.push(mEntry)
//      }
//    })

//    const entriesJson = JSON.stringify(mappedEntries)
//    invoker.invokeMethodAsync('Invoke', entriesJson)
//  }
//}
