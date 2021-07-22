export class infoHelper {
    static getWindow() {
        return {
            innerWidth: window.innerWidth,
            innerHeight: window.innerHeight
        };
    }
    static get(element) {
        if (!element) {
            element = document.body;
        }
        else if (typeof element === 'string') {
            if (element === 'document') {
                return document;
            }
            element = document.querySelector(element);
        }
        return element;
    }
    static getInfo(element) {
        let domElement = this.get(element);
        if (!domElement) {
            domElement = {};
        }
        const absolutePosition = this.getElementAbsolutePos(domElement);
        const result = {
            offsetTop: domElement.offsetTop || 0,
            offsetLeft: domElement.offsetLeft || 0,
            offsetWidth: domElement.offsetWidth || 0,
            offsetHeight: domElement.offsetHeight || 0,
            scrollHeight: domElement.scrollHeight || 0,
            scrollWidth: domElement.scrollWidth || 0,
            scrollLeft: domElement.scrollLeft || 0,
            scrollTop: domElement.scrollTop || 0,
            clientTop: domElement.clientTop || 0,
            clientLeft: domElement.clientLeft || 0,
            clientHeight: domElement.clientHeight || 0,
            clientWidth: domElement.clientWidth || 0,
            selectionStart: domElement.selectionStart || 0,
            absoluteTop: Math.round(absolutePosition.y),
            absoluteLeft: Math.round(absolutePosition.x)
        };
        return result;
    }
    static getElementAbsolutePos(element) {
        let res = {
            x: 0,
            y: 0
        };
        if (element !== null) {
            if (element.getBoundingClientRect) {
                const viewportElement = document.documentElement;
                const box = element.getBoundingClientRect();
                const scrollLeft = viewportElement.scrollLeft;
                const scrollTop = viewportElement.scrollTop;
                res.x = box.left + scrollLeft;
                res.y = box.top + scrollTop;
            }
        }
        return res;
    }
    static getBoundingClientRect(element) {
        const domElement = this.get(element);
        if (domElement && domElement.getBoundingClientRect) {
            let rect = domElement.getBoundingClientRect();
            // Fixes #1468. This wrapping is necessary for old browsers. Remove this when one day we no longer support them.
            return {
                width: rect.width,
                height: rect.height,
                top: rect.top,
                right: rect.right,
                bottom: rect.bottom,
                left: rect.left,
                x: rect.x,
                y: rect.y
            };
        }
        return null;
    }
    static getFirstChildDomInfo(element) {
        const domElement = this.get(element);
        if (domElement) {
            if (domElement.firstElementChild) {
                return this.getInfo(domElement.firstElementChild);
            }
            else {
                return this.getInfo(domElement);
            }
        }
        return null;
    }
    static getActiveElement() {
        const element = document.activeElement;
        const id = element.getAttribute("id") || "";
        return id;
    }
    static getScroll() {
        return { x: window.pageXOffset, y: window.pageYOffset };
    }
    static hasFocus(selector) {
        let dom = this.get(selector);
        return (document.activeElement === dom);
    }
    static getInnerText(element) {
        let dom = this.get(element);
        if (dom)
            return dom.innerText;
        return null;
    }
}
