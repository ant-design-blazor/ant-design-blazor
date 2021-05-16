import { getDom } from '../interop';

const resizeObservers: Map<string, ResizeObserver> = new Map<string, ResizeObserver>();

export function create(key, invoker) {
  const observer = new ResizeObserver((entries, observer) => observerCallBack(entries, observer, invoker));
  resizeObservers.set(key, observer)
}

export function observe(key: string, element) {
  const observer = resizeObservers.get(key);
  if (observer) {
    let dom = getDom(element);
    observer.observe(dom);
  }
}

export function disconnect(key: string): void {
  const observer = this.resizeObservers.get(key)
  if (observer) {
    observer.disconnect()
  }
}

export function unobserve(key: string, element: Element): void {
  const observer = this.resizeObservers.get(key)

  if (observer) {
    let dom = getDom(element);
    observer.unobserve(dom)
  }
}

export function dispose(key: string): void {
  console.log("dispose", key);
  disconnect(key)
  resizeObservers.delete(key)
}

class BoxSize {
  public blockSize?: number
  public inlineSize?: number
}

class DomRect {
  x?: number
  y?: number
  width?: number
  height?: number
  top?: number
  right?: number
  bottom?: number
  left?: number
}

class ResizeObserverEntry {
  borderBoxSize?: BoxSize
  contentBoxSize?: BoxSize
  contentRect?: DomRect
  target?: Element
}

function observerCallBack(entries, observer, invoker) {
  if (invoker) {
    const mappedEntries = new Array<ResizeObserverEntry>()
    entries.forEach(entry => {
      if (entry) {
        const mEntry = new ResizeObserverEntry()
        if (entry.borderBoxSize) {
          mEntry.borderBoxSize = new BoxSize()
          mEntry.borderBoxSize.blockSize = entry.borderBoxSize.blockSize
          mEntry.borderBoxSize.inlineSize = entry.borderBoxSize.inlineSize
        }

        if (entry.contentBoxSize) {
          mEntry.contentBoxSize = new BoxSize()
          mEntry.contentBoxSize.blockSize = entry.contentBoxSize.blockSize
          mEntry.contentBoxSize.inlineSize = entry.contentBoxSize.inlineSize
        }

        if (entry.contentRect) {
          mEntry.contentRect = new DomRect()
          mEntry.contentRect.x = entry.contentRect.x
          mEntry.contentRect.y = entry.contentRect.y
          mEntry.contentRect.width = entry.contentRect.width
          mEntry.contentRect.height = entry.contentRect.height
          mEntry.contentRect.top = entry.contentRect.top
          mEntry.contentRect.right = entry.contentRect.right
          mEntry.contentRect.bottom = entry.contentRect.bottom
          mEntry.contentRect.left = entry.contentRect.left
        }

        mEntry.target = entry.target
        mappedEntries.push(mEntry)
      }
    })

    const entriesJson = JSON.stringify(mappedEntries)
    invoker.invokeMethodAsync('Invoke', entriesJson)
  }
}
