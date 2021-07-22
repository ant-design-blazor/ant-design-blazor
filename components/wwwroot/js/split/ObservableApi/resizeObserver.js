import { infoHelper as domInfoHelper } from '../modules/dom/infoHelper';
class ResizeObserverEntry {
}
export class resizeObserver {
    static isResizeObserverSupported() {
        return "ResizeObserver" in window;
    }
    static create(key, invoker) {
        // @ts-ignore: TS2304: Cannot find name 'ResizeObserver'
        const observer = new ResizeObserver((entries, observer) => resizeObserver.observerCallBack(entries, observer, invoker));
        resizeObserver.resizeObservers.set(key, observer);
    }
    static observe(key, element) {
        const observer = resizeObserver.resizeObservers.get(key);
        if (observer) {
            let domElement = domInfoHelper.get(element);
            observer.observe(domElement);
        }
    }
    static disconnect(key) {
        const observer = this.resizeObservers.get(key);
        if (observer) {
            observer.disconnect();
        }
    }
    static unobserve(key, element) {
        const observer = this.resizeObservers.get(key);
        if (observer) {
            let domElement = domInfoHelper.get(element);
            observer.unobserve(domElement);
        }
    }
    static dispose(key) {
        console.log("dispose", key);
        this.disconnect(key);
        this.resizeObservers.delete(key);
    }
    static observerCallBack(entries, observer, invoker) {
        console.log("observerCallBack start", entries);
        if (invoker) {
            const mappedEntries = new Array();
            entries.forEach(entry => {
                console.log("observerCallBack entry", entry);
                if (entry) {
                    const mEntry = new ResizeObserverEntry();
                    if (entry.borderBoxSize) {
                        mEntry.borderBoxSize = {
                            blockSize: entry.borderBoxSize.blockSize,
                            inlineSize: entry.borderBoxSize.inlineSize
                        };
                    }
                    if (entry.contentBoxSize) {
                        mEntry.contentBoxSize = {
                            blockSize: entry.contentBoxSize.blockSize,
                            inlineSize: entry.contentBoxSize.inlineSize
                        };
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
                        };
                    }
                    mEntry.target = entry.target;
                    mappedEntries.push(mEntry);
                }
            });
            const entriesJson = JSON.stringify(mappedEntries);
            invoker.invokeMethodAsync('Invoke', entriesJson);
        }
    }
}
// @ts-ignore: TS2304: Cannot find name 'ResizeObserver'
resizeObserver.resizeObservers = new Map();
