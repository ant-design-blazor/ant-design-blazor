
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
