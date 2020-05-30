export function getDom(element) {
    if (!element) {
        element = document.body;
    } else if (typeof element === 'string') {
        element = document.querySelector(element);
    }
    return element;
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

export function css(element: HTMLElement, name: string | object, value: string | null = null) {
    if (typeof name === 'string') {
        element.style[name] = value;
    } else {
        for (let key in name) {
            if (name.hasOwnProperty(key)) {
                element.style[key] = name[key];
            }
        }
    }

}

export function addClassName(element: Element, clsName: string | Array<string>) {
    if (typeof clsName === "string") {
        if (!containsClassName(element, clsName)) {
            element.classList.add(clsName);
        }
    } else {
        let clsNameArr = clsName.filter((item: string) => {
            return !containsClassName(element, item);
        });
        element.classList.add(...clsNameArr);
    }
   
    return element;
}

export function removeClassName(element: Element, clsName: string | Array<string>) {
    if (typeof clsName === "string") {
        if (containsClassName(element, clsName)) {
            element.classList.remove(clsName);
        }
    } else {
        let clsNameArr = clsName.filter((item: string) => {
            return containsClassName(element, item);
        });
        element.classList.remove(...clsNameArr);
    }
    return element;
}

export function containsClassName(element: Element, clsName: string) {
    return element.classList.contains(clsName);
}

export function hide(element: string | HTMLElement) {
    if (typeof element === "string") {
        element = getDom(element);
    }

    css((<HTMLElement>element), "display", "none");
}

export function show(element: string | HTMLElement)  {
    if (typeof element === "string") {
        element = getDom(element);
    }

    css((<HTMLElement>element), "display", null);
}

export function find(element: Element, css:string) {
    return element.querySelectorAll(css);
}



export function hideModal(element: HTMLElement) {
    let mask = find(element, ".ant-modal-mask")[0];
    if (containsClassName(mask, "ant-modal-mask-hidden")) {
        return;
    }
    let antModalWrap = find(element, ".ant-modal-wrap")[0];
    let antModal = find(antModalWrap, ".ant-modal")[0];

    addClassName(antModal, ["zoom-leave", "zoom-leave-active" ]);
    addClassName(mask, ["fade-leave", "fade-leave-active"]);

    setTimeout(() => {
        addClassName(mask, "ant-modal-mask-hidden");
        hide(antModalWrap as HTMLElement);

        let length = document.querySelectorAll(".ant-modal-mask:not(.ant-modal-mask-hidden)").length;
        if (length === 0) {
            let body = getDom("body");
            css(body,
                {
                    "position": null,
                    "width": null,
                    "overflow": null
                });
            removeClassName(body, "ant-scrolling-effect");
        }
        
        removeClassName(antModal, ["zoom-leave", "zoom-leave-active"]);
        removeClassName(mask, ["fade-leave", "fade-leave-active"]);

    }, 200);
}

let mousePosition: { x: number; y: number } | null;
const getClickPosition = (e: MouseEvent) => {
    mousePosition = {
        x: e.pageX,
        y: e.pageY,
    };
    // 100ms 内发生过点击事件，则从点击位置动画展示
    // 否则直接 zoom 展示
    // 这样可以兼容非点击方式展开
    setTimeout(() => (mousePosition = null), 100);
};

if (typeof window !== 'undefined' && window.document && window.document.documentElement) {
    document.documentElement.addEventListener('click', getClickPosition);
}

function getScroll(w: any, top?: boolean) {
    let ret = w[`page${top ? 'Y' : 'X'}Offset`];
    const method = `scroll${top ? 'Top' : 'Left'}`;
    if (typeof ret !== 'number') {
        const d = w.document;
        ret = d.documentElement[method];
        if (typeof ret !== 'number') {
            ret = d.body[method];
        }
    }
    return ret;
}

function offset(el: any) {
    const rect = el.getBoundingClientRect();
    const pos = {
        left: rect.left,
        top: rect.top,
    };
    const doc = el.ownerDocument;
    const w = doc.defaultView || doc.parentWindow;
    pos.left += getScroll(w);
    pos.top += getScroll(w, true);
    return pos;
}

function setTransformOrigin(node: any, value: string) {
    const style = node.style;
    ['Webkit', 'Moz', 'Ms', 'ms'].forEach((prefix: string) => {
        style[`${prefix}TransformOrigin`] = value;
    });
    style[`transformOrigin`] = value;
}

export function showModal(element: HTMLElement) {
    let mask = find(element, ".ant-modal-mask")[0];
    if (!containsClassName(mask, "ant-modal-mask-hidden")) {
        return;
    }

    let antModalWrap = find(element, ".ant-modal-wrap")[0];
    let antModal = find(antModalWrap, ".ant-modal")[0];
    removeClassName(mask, "ant-modal-mask-hidden");
    show(antModalWrap as HTMLElement);

    let body = getDom("body");
    css(body,
        {
            "position": "relative",
            "width": "calc(100% - 17px)",
            "overflow": "hidden"
        });
    addClassName(body, "ant-scrolling-effect");

    if (mousePosition) {
        const elOffset = offset(antModal);
        setTransformOrigin(
            antModal,
            `${mousePosition.x - elOffset.left}px ${mousePosition.y - elOffset.top}px`,
        );
    } else {
        setTransformOrigin(antModal, '');
    }
    
    addClassName(antModal, ["zoom-enter", "zoom-enter-active"]);
    addClassName(mask, ["fade-enter", "fade-enter-active"]);

    let defaultFocus = find(antModal, "div:first-child")[0];
    focus(defaultFocus);

    setTimeout(() => {
        removeClassName(antModal, ["zoom-enter", "zoom-enter-active"]);
        removeClassName(mask, ["fade-enter", "fade-enter-active"]);
    }, 200);
}

export function getActiveElement() {
    let element = document.activeElement;
    let id = element.getAttribute("id") || "";
    return id;
}

export function destoryDialog(delElement, elementSelector) {
    if (elementSelector) {
        let parent = getDom(elementSelector);
        if (parent && delElement) {
            parent.removeChild(delElement);
        }
    }
}

export function focusConfirmBtn(element: string,count:number = 0) {
    setTimeout(() => {
        focus(element);
        let curId = "#" + getActiveElement();
        if (curId !== element) {
            if (count < 10) {
                focusConfirmBtn(element, count++);
            }
        }
    },50);
}
