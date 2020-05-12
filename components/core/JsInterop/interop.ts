export {initCsInstance, delCsInstance} from "./modules/csInstance";
export * from "./modules/notification";


export function getDom(element, parent = null) {
    if (!element) {
        element = document.body;
    }
    if (element instanceof Node) {
        return element;
    }
    
    if (typeof element === 'string') {
        let parentElement;
        if (!parent) {
            parentElement = document;
        } else {
            if (typeof parent === "string") {
                parentElement = document.querySelector(element);
            } else if (parent instanceof Node) {
                parentElement = parent;
            } else {
                parentElement = document;
            }
        }

        element = parentElement.querySelector(element);
    } else {
        element = null;
    }
    return element;
}

export function getDoms(element: string | Array<Element>, parent:string|Element = null) {
    let parentElement: Element | Document;
    if (parent) {
        if (parent instanceof Element) {
            parentElement = parent;
        } else {
            parentElement = getDom(parent);
        }
    } else {
        parentElement = document;
    }

    if (typeof element === "string") {
        return Array.from(parentElement.querySelectorAll(element));
    } else if (element instanceof Array) {
        return element;
    } else {
        return [];
    }
}

export function getDomInfo(element) {
    var result = {};

    var dom = getDom(element);

    result["offsetTop"] = dom.offsetTop || 0;
    result["offsetLeft"] = dom.offsetLeft || 0;
    result["offsetWidth"] = dom.offsetWidth || 0;
    result["offsetHeight"] = dom.offsetHeight || 0;
    result["scrollHeight"] = dom.scrollHeight || 0;
    result["scrollWidth"] = dom.scrollWidth || 0;
    result["scrollLeft"] = dom.scrollLeft || 0;
    result["scrollTop"] = dom.scrollTop || 0;
    result["clientTop"] = dom.clientTop || 0;
    result["clientLeft"] = dom.clientLeft || 0;
    result["clientHeight"] = dom.clientHeight || 0;
    result["clientWidth"] = dom.clientWidth || 0;

    result["absoluteTop"] = getAbsoluteTop(dom);
    result["absoluteLeft"] = getAbsoluteLeft(dom);

    return result;
}

export function getBoundingClientRect(element) {
    let dom = getDom(element);
    return dom.getBoundingClientRect();
}

export function addDomEventListener(element, eventName, invoker) {
    let callback = args => {
        const obj = {};
        for (let k in args) {
            obj[k] = args[k];
        }
        let json = JSON.stringify(obj, (k, v) => {
            if (v instanceof Node) return 'Node';
            if (v instanceof Window) return 'Window';
            return v;
        }, ' ');
        invoker.invokeMethodAsync('Invoke', json);
    };

    if (element == 'window') {
        window.addEventListener(eventName, callback);
    } else {
        let dom = getDom(element);
        (dom as HTMLElement).addEventListener(eventName, callback);
    }
}

export function matchMedia(query) {
    return window.matchMedia(query).matches;
}

function fallbackCopyTextToClipboard(text) {
    var textArea = document.createElement("textarea");
    textArea.value = text;

    // Avoid scrolling to bottom
    textArea.style.top = "0";
    textArea.style.left = "0";
    textArea.style.position = "fixed";

    document.body.appendChild(textArea);
    textArea.focus();
    textArea.select();

    try {
        var successful = document.execCommand('copy');
        var msg = successful ? 'successful' : 'unsuccessful';
        console.log('Fallback: Copying text command was ' + msg);
    } catch (err) {
        console.error('Fallback: Oops, unable to copy', err);
    }

    document.body.removeChild(textArea);
}
export function copy(text) {
    if (!navigator.clipboard) {
        fallbackCopyTextToClipboard(text);
        return;
    }
    navigator.clipboard.writeText(text).then(function () {
        console.log('Async: Copying to clipboard was successful!');
    }, function (err) {
        console.error('Async: Could not copy text: ', err);
    });
}

export function focus(selector) {
    let dom = getDom(selector);
    dom.focus();
}

export function blur(selector) {
    let dom = getDom(selector);
    dom.blur();
}

export function log(text) {
    console.log(text);
}

export function BackTop(element) {
    let dom = document.getElementById("BodyContainer");
    dom.scrollTo(0, 0);
}

export function getFirstChildDomInfo(element) {
    var dom = getDom(element);
    return getDomInfo(dom.firstElementChild);
}

export function addClsToFirstChild(element, className) {
    var dom = getDom(element);
    if (dom.firstElementChild) {
        dom.firstElementChild.classList.add(className);
    }
}

export function addDomEventListenerToFirstChild(element, eventName, invoker) {
    var dom = getDom(element);

    if (dom.firstElementChild) {
        addDomEventListener(dom.firstElementChild, eventName, invoker);
    }
}

export function getAbsoluteTop(e) {
    var offset = e.offsetTop;
    if (e.offsetParent != null) {
        offset += getAbsoluteTop(e.offsetParent);
    }
    return offset;
}

export function getAbsoluteLeft(e) {
    var offset = e.offsetLeft;
    if (e.offsetParent != null) {
        offset += getAbsoluteLeft(e.offsetParent);
    }
    return offset;
}

export function addElementToBody(element) {
    document.body.appendChild(element);

}

export function delElementFromBody(element) {
    document.body.removeChild(element);
}

export function addElementTo(addElement, elementSelector) {
    getDom(elementSelector).appendChild(addElement);
}

export function delElementFrom(delElement, elementSelector) {
    getDom(elementSelector).removeChild(delElement);
}

/**
 * Convert html string to a node
 * @param html {string} closing the HTML fragment
 */
export function htmlToElement(html:string) {
    var template = document.createElement('template');
    html = html.trim();
    template.innerHTML = html;
    let node = template.content.firstChild;
    return node;
}

/**
 * Append the HTML to the end of the selector
 * @param html {string | Node} HTML string or node that to be append
 * @param containerSelector {string | Element} parent element that should be append
 */
export function appendHtml(html: string | Node,
    containerSelector: string | Element = null) {
    let element: Node;
    if (typeof (html) === "string") {
        element = htmlToElement(html);
    }
    else {
        element = html;
    }
    let dom: Element;
    if (containerSelector) {
        if (typeof (containerSelector) === "string") {
            dom = getDom(containerSelector);
        } else {
            dom = containerSelector;
        }
    } else {
        dom = document.body;
    }
   
    if (dom) {
        dom.appendChild(element);
    }
}

/**
 * Removing the selector from the DOM
 * @param selector {string | Element | Array<Node> | NodeList}
 */
export function removeElement(selector: string | Node | Array<Node> | NodeList) {
    if (selector instanceof Node) {
        selector.parentNode.removeChild(selector);
    } else {
        let elements: Array<Node>;
        if (typeof selector === "string") {
            elements = getDoms(selector);
        } else if (selector instanceof Array){
            elements = selector;
        }else if (selector instanceof NodeList) {
            elements = Array.from(selector);
        }
        if (elements) {
            for (var i = 0; i < elements.length; i++) {
                elements[i].parentNode.removeChild(elements[i]);
            }
        }
    }
}

/**
 * Empty the elements in the selector
 * @param selector {string | Element | Array<Element> | NodeList}
 * @return {string | Element | Array<Element> | NodeList} Element or elements that have been emptied of content
 */
export function emptyElement(selector: string | Element | Array<Element> | NodeList) {
    if (selector instanceof Element) {
        selector.innerHTML = "";
        return selector;
    }
    let elements: Array<Node>;

    if (typeof selector === "string") {
        elements = getDoms(selector);
    } else if (selector instanceof NodeList){
        elements = Array.from(selector);
    } else if (selector instanceof Array) {
        elements = selector;
    }
    if (elements) {
        for (var i = 0; i < elements.length; i++) {
            if (elements[i] instanceof Element) {
                (<Element>elements[i]).innerHTML = "";
            }
        }
    }
    return elements;
}

/**
 * Get the selector's class array
 * @param selector {string | Element}
 * @return class name array
 */
export function getClass(selector: string | Element) {
    let element: Element;
    if (typeof selector === "string") {
        element = getDom(selector);
    } else {
        element = selector;
    }
    let classNameArr = element.getAttribute("class").split(" ").map((item) => {
        return item.trim();
    });
    return classNameArr;
}

/**
 * Remove className from the class in the selector
 * @param selector {string | Element} element to be manipulated, css selector or element
 * @param className {string | Array<string>} class name or class name array
 */
export function removeClass(selector: string | Element,
    className: string | Array<string>) {
    let element: Element;
    if (typeof selector === "string") {
        element = getDom(selector);
    } else {
        element = selector;
    }

    let classNameArr = getClass(element);
    if (typeof className === "string") {
        classNameArr = classNameArr.filter((item: string) => {
            return item !== className;
        });
    } else {
        classNameArr = classNameArr.filter((item: string) => {
            return !className.includes(item);
        });
    }
    let newClassName = classNameArr.join(" ");
    element.setAttribute("class", newClassName);
    return element;
}

/**
 * add className for the selector
 * @param selector {string | Element} element to be manipulated, css selector or element
 * @param className {string | Array<string>} class name or class name array
 */
export function addClass(selector: string | Element,
    className: string | Array<string>) {
    let element: Element;
    if (typeof selector === "string") {
        element = getDom(selector);
    } else {
        element = selector;
    }

    let addClassName: string;
    if (typeof className === "string") {
        addClassName = className;
    } else {
        addClassName = className.join(" ");
    }
    element.className += " " + addClassName;
    return element;
}
