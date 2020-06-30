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

export function addFileClickEventListener(btn) {
  (btn as HTMLSpanElement).addEventListener("click", fileClickEvent);
}

export function removeFileClickEventListener(btn) {
  (btn as HTMLSpanElement).removeEventListener("click", fileClickEvent);
}

export function fileClickEvent() {
  var fileId = this.attributes["data-fileid"].nodeValue;
  var element = document.getElementById(fileId);
  (element as HTMLInputElement).click();
}

export function clearFile(element) {
  element.setAttribute("type", "input");
  element.value = '';
  element.setAttribute("type", "file");
}

export function getFileInfo(element) {
  if (element.files && element.files.length > 0) {
    var file = element.files[0];
    var objectUrl = getObjectURL(element);
    var fileInfo = {
      fileName: file.name,
      size: file.size,
      objectURL: objectUrl,
      type: file.type
    };
    return fileInfo;
  }
}

export function getObjectURL(element) {
  var url = null;
  var file = element.files[0];
  if (window.URL != undefined) {
    url = window.URL.createObjectURL(file);
  } else if (window.webkitURL != undefined) {
    url = window.webkitURL.createObjectURL(file);
  }
  return url;
}

export function uploadFile(element, headers, fileId, url, name, instance, percentMethod, successMethod, errorMethod) {
  let formData = new FormData();
  var file = element.files[0];
  var size = file.size;
  formData.append(name, file)
  const req = new XMLHttpRequest()
  req.onreadystatechange = function () {
    if (req.readyState === 4) {
      if (req.responseText == null || req.responseText.length == 0) {
        instance.invokeMethodAsync(errorMethod, fileId, "error");
        return;
      }
      instance.invokeMethodAsync(successMethod, fileId, req.responseText);
    }
  }
  req.upload.onprogress = function (event) {
    var percent = Math.floor(event.loaded / size * 100);
    instance.invokeMethodAsync(percentMethod, fileId, percent);
  }
  req.onerror = function (e) {
    instance.invokeMethodAsync(errorMethod, fileId, "error");
  }
  req.open('post', url, true)
  if (headers != null) {
    for (var header in headers) {
      req.setRequestHeader(header, headers[header]);
    }
  }
  req.send(formData)
}

export function triggerEvent(element, eventType, eventName) {
  var dom = element as HTMLInputElement;
  var evt = document.createEvent(eventType);
  evt.initEvent(eventName);
  return dom.dispatchEvent(evt);
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
    if (eventName == 'resize') {
      window.addEventListener(eventName, debounce(() => callback({ innerWidth: window.innerWidth, innerHeight: window.innerHeight }), 200, false));
    } else {
      window.addEventListener(eventName, callback);
    }
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
  let parent = getDom(elementSelector);
  if (parent && addElement) {
    parent.appendChild(addElement);
  }
}

export function delElementFrom(delElement, elementSelector) {
  let parent = getDom(elementSelector);
  if (parent && delElement) {
    parent.removeChild(delElement);
  }
}

export function getActiveElement() {
  let element = document.activeElement;
  let id = element.getAttribute("id") || "";
  return id;
}

export function focusDialog(selector: string, count: number = 0) {
  let ele = <HTMLElement>document.querySelector(selector);
  if (ele && !ele.hasAttribute("disabled")) {
    setTimeout(() => {
      ele.focus();
      let curId = "#" + getActiveElement();
      if (curId !== selector) {
        if (count < 10) {
          focusDialog(selector, count + 1);
        }
      }
    }, 10);
  }
}

export function getWindow() {
  return {
    innerWidth: window.innerWidth,
    innerHeight: window.innerHeight
  };
}

function debounce(func, wait, immediate) {
  var timeout;
  return () => {
    const context = this, args = arguments;
    const later = () => {
      timeout = null;
      if (!immediate) func.apply(this, args);
    };
    const callNow = immediate && !timeout;
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
    if (callNow) func.apply(context, args);
  };
};

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

export function addCls(selector: Element | string, clsName: string | Array<string>) {
  let element = getDom(selector);

  if (typeof clsName === "string") {
    element.classList.add(clsName);
  } else {
    element.classList.add(...clsName);
  }
}

export function removeCls(selector: Element | string, clsName: string | Array<string>) {
  let element = getDom(selector);

  if (typeof clsName === "string") {
    element.classList.remove(clsName);
  } else {
    element.classList.remove(...clsName);
  }
}

export function disableBodyScroll() {
  css(document.body,
    {
      "position": "relative",
      "width": "calc(100% - 17px)",
      "overflow": "hidden"
    });
  addCls(document.body, "ant-scrolling-effect");
}

function enableBodyScroll(selector, filter = null) {
  let length = 0;
  let queryElements = document.querySelectorAll(selector);
  if (typeof filter === "function") {
    queryElements.forEach((value, key, parent) => {
      if (!filter(value, key, parent)) {
        length += 1;
      }
    });
  } else {
    length = queryElements.length;
  }
  if (length === 0) {
    css(document.body,
      {
        "position": null,
        "width": null,
        "overflow": null
      });
    removeCls(document.body, "ant-scrolling-effect");
  }
}

export function enableModalBodyScroll() {
  enableBodyScroll(".ant-modal-mask:not(.ant-modal-mask-hidden)");
}

export function enableDrawerBodyScroll() {
  enableBodyScroll(".ant-drawer-open",
    (value, key, parent) => { return value.style.position === "absolute" });
}

export function createIconFromfontCN(scriptUrl) {
  if (document.querySelector(`[data-namespace="${scriptUrl}"]`)) {
    return;
  }
  const script = document.createElement('script');
  script.setAttribute('src', scriptUrl);
  script.setAttribute('data-namespace', scriptUrl);
  document.body.appendChild(script);
}

export function getScroll() {
  return { x: window.pageXOffset, y: window.pageYOffset };
}

export function getInnerText(element) {
  let dom = getDom(element);
  return dom.innerText;
}

 // return { x: dom.getCursorPosition.x, y: dom.getCursorPosition.y };
export function getCursorXY(element) {
    var rect = getBoundingClientRect(element), 
        scrollTop = document.body.scrollTop || document.documentElement.scrollTop,
        scrollLeft = document.body.scrollLeft || document.documentElement.scrollLeft,
        isIE = !(!document.all) ? 2 : 0;
    var position;
    position.left = rect.left - isIE + scrollLeft;
    position.top = rect.top - isIE + scrollTop;
    return position;
    
}
