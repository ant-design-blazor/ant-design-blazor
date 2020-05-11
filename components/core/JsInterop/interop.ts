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

/************ Notification start ************/
function getNotificationService() {
    return (<any>window).notificationService;
}

function htmlToElement(html) {
    var template = document.createElement('template');
    html = html.trim();
    template.innerHTML = html;
    return template.content.firstChild;
}

function removeEnterClass(element) {
    let newClassName = element.getAttribute("class").split(" ").map((item) => {
        return item.trim();
    }).filter(item => {
        return item !== "ant-notification-fade-enter" && item !== "ant-notification-fade-enter-active";
    }).join(" ");
    element.setAttribute("class", newClassName);
}

function removeNotificationHandler(id, element) {
    if (!element) {
        element = document.querySelector("#" + id);
    }
    element.className += " ant-notification-fade-leave ant-notification-fade-leave-active";
    window.setTimeout(() => {
        element.parentNode.removeChild(element);
    }, 500);
    getNotificationService().invokeMethodAsync("NotificationClose", id);
}

export function initNotification(notificationService) {
    (<any>window).notificationService = notificationService;
}

export function createNotificationContainer(htmlStr) {
    let element = htmlToElement(htmlStr);
    document.body.appendChild(element);
}

export function addNotification(htmlStr, elementSelector, id, duration) {
    let container = getDom(elementSelector);
    let spanContainer = container.children[0];
    let element = htmlToElement(htmlStr);
    spanContainer.appendChild(element);

    window.setTimeout(() => {
        removeEnterClass(element);
    }, 500);

    let timeout;
    if (duration && duration > 0) {
        timeout = window.setTimeout(() => {
            removeNotificationHandler(id, element);
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
                removeNotificationHandler(id, element);
                e.preventDefault();
                e.stopPropagation();
            });
    }

    (<any>element).querySelector(".ant-notification-notice-close")
        .addEventListener("click",
            e => {
                window.clearTimeout(timeout);
                removeNotificationHandler(id, element);
                e.preventDefault();
                e.stopPropagation();
            });
}

export function removeNotification(id) {
    let element = document.querySelector("#" + id);
    removeNotificationHandler(id, element);
}

export function destroyNotification() {
    let allContainer = document.querySelectorAll(".ant-notification");
    for (let i = allContainer.length - 1; i > -1; i--) {
        let container = allContainer[i];
        container.parentNode.removeChild(container);
    }
}


/************ Notification end ************/