export function getDom(element) {
    if (!element) {
        element = document.body;
    } else if (typeof element === 'string') {
        element = document.querySelector(element);
    } else {
        element = null;
    }
    return element;
}

export function getDoms(element: string) {
    if (typeof element === "string") {
        return document.querySelectorAll(element);
    } else {
        return new NodeList();
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
 * 将html字符串转为element
 * @param html
 */
function htmlToElement(html) {
    var template = document.createElement('template');
    html = html.trim();
    template.innerHTML = html;
    let node = template.content.firstChild;
    return node;
}

/**
 * 将html追加到selector的末尾
 * @param html  string(css选择器) | Node
 * @param containerSelector string(css选择器) | HTMLElement
 */
export function appendHtml(html: string | Node,
    containerSelector: string | HTMLElement = null) {
    console.log("appendHtml,containerSelector", containerSelector);
    let element: Node;
    if (typeof (html) === "string") {
        element = htmlToElement(html);
    }
    else {
        element = html;
    }
    let dom: HTMLElement;
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
 * 将selector从DOM中移出
 * @param selector string(css选择器) | Element | NodeList
 */
export function removeElement(selector: string | Element | NodeList) {
    if (selector instanceof Element) {
        selector.parentNode.removeChild(selector);
    } else {
        let elements: NodeList;
        if (typeof selector === "string") {
            elements = getDoms(selector);
        } else {
            elements = selector;
        }
        if (elements) {
            for (var i = 0; i < elements.length; i++) {
                elements[i].parentNode.removeChild(elements[i]);
            }
        }
    }
}

/**
 * 清空selector中元素
 * @param selector string(css选择器) | Element | NodeList
 */
export function emptyElement(selector: string | Element | NodeList) {
    if (selector instanceof Element) {
        selector.innerHTML = "";
    }
    let elements: NodeList;

    if (typeof selector === "string") {
        elements = getDoms(selector);
    } else if (selector instanceof NodeList){
        elements = selector;
    }
    if (elements) {
        for (var i = 0; i < elements.length; i++) {
            if (elements[i] instanceof Element) {
                (<Element>elements[i]).innerHTML = "";
            }
        }
    }
}

/**
 * 获取selector的class列表
 * @param selector string(css选择器) | Element
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
 * 从selector中的class中移出className
 * @param selector string(css选择器) | Element
 * @param className string(class name) | Array<string>(class name array)
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
 * 向js端注册C#类的实例引用
 * @param instance
 * @param instanceName
 */
export function initCsInstance(instance: any, instanceName: string) {
    let csInstance = (<any>window).antBlazor.interop.csInstance;
    if (!csInstance) {
        csInstance = {};
        (<any>window).antBlazor.interop.csInstance = csInstance;
    }
    csInstance[instanceName] = instance;
}

/**
 * 删除js端注册的C#类的实例引用
 * @param instanceName
 */
export function delCsInstance(instanceName: string) {
    let csInstance = (<any>window).antBlazor.interop.csInstance;
    if (csInstance) {
        delete (<any>window).antBlazor.interop.csInstance[instanceName];
    }
}

/**
 * 获取js端注册的C#类的实例引用
 * @param instanceName
 */
export function getCsInstance(instanceName:string) {
    return (<any>window).antBlazor.interop.csInstance[instanceName];
}

/************ Notification start ************/

function getNotificationService() {
    return getCsInstance("notification");
}


/**
 * 移除通知提示框
 * @param id
 * @param element
 */
export function removeNotification(id:string, element: Element) {
    if (!element) {
        element = document.querySelector("#" + id);
    }
    removeClass(element, ["ant-notification-fade-enter", "ant-notification-fade-enter-active"]);
    element.className += " ant-notification-fade-leave ant-notification-fade-leave-active";
    window.setTimeout(() => {
        removeElement(element);
    }, 500);
    getNotificationService().invokeMethodAsync("NotificationClose", id);
}


/**
 * 添加通知提示框
 * @param htmlStr
 * @param elementSelector
 * @param id
 * @param duration
 */
export function addNotification(htmlStr, elementSelector, id, duration) {
    let spanContainer = getDom(elementSelector).children[0];
    let element = <HTMLElement>htmlToElement(htmlStr);
    appendHtml(element, spanContainer);

    let timeout;
    if (duration && duration > 0) {
        timeout = window.setTimeout(() => {
            removeNotification(id, element);
        }, duration * 1000);
    }

    element.addEventListener("click",
        () => {
            getNotificationService().invokeMethodAsync("NotificationClick", id);
        });
    let btn = (<any>element).querySelector(".ant-notification-notice-btn");
    if (btn) {
        btn.addEventListener("click",
            e => {
                window.clearTimeout(timeout);
                removeNotification(id, element);
                e.preventDefault();
                e.stopPropagation();
            });
    }

    (<any>element).querySelector(".ant-notification-notice-close")
        .addEventListener("click",
            e => {
                window.clearTimeout(timeout);
                removeNotification(id, element);
                e.preventDefault();
                e.stopPropagation();
            });
}

/************ Notification end ************/