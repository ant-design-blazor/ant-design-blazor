export function getDom(element) {
  if (!element) {
    element = document.body;
  } else if (typeof element === 'string') {
    if (element === 'document') {
      return document;
    }
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
  var absolutePosition = getElementAbsolutePos(dom);
  result["absoluteTop"] = Math.round(absolutePosition.y);
  result["absoluteLeft"] = Math.round(absolutePosition.x);

  return result;
}

function getElementAbsolutePos(element) {
  var res: any = new Object();
  res.x = 0; res.y = 0;
  if (element !== null) {
    if (element.getBoundingClientRect) {
      var viewportElement = document.documentElement;
      var box = element.getBoundingClientRect();
      var scrollLeft = viewportElement.scrollLeft;
      var scrollTop = viewportElement.scrollTop;

      res.x = box.left + scrollLeft;
      res.y = box.top + scrollTop;
    }
  }
  return res;
}

export function addFileClickEventListener(btn) {
  if ((btn as HTMLElement).addEventListener) {
    (btn as HTMLElement).addEventListener("click", fileClickEvent);
  }
}

export function removeFileClickEventListener(btn) {
  (btn as HTMLElement).removeEventListener("click", fileClickEvent);
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
    var fileInfo = [];
    for (var i = 0; i < element.files.length; i++) {
      var file = element.files[i];
      var objectUrl = getObjectURL(file);
      fileInfo.push({
        fileName: file.name,
        size: file.size,
        objectURL: objectUrl,
        type: file.type
      });
    }

    return fileInfo;
  }
}

export function getObjectURL(file: File) {
  var url = null;
  if (window.URL != undefined) {
    url = window.URL.createObjectURL(file);
  } else if (window.webkitURL != undefined) {
    url = window.webkitURL.createObjectURL(file);
  }
  return url;
}

export function uploadFile(element, index, data, headers, fileId, url, name, instance, percentMethod, successMethod, errorMethod) {
  let formData = new FormData();
  var file = element.files[index];
  var size = file.size;
  formData.append(name, file);
  if (data != null) {
    for (var key in data) {
      formData.append(key, data[key]);
    }
  }
  const req = new XMLHttpRequest()
  req.onreadystatechange = function () {
    if (req.readyState === 4) {
      if (req.status != 200) {
        instance.invokeMethodAsync(errorMethod, fileId, `{"status": ${req.status}}`);
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
  if (dom && dom.getBoundingClientRect) {
    return dom.getBoundingClientRect();
  }
  return null;
}

export function addDomEventListener(element, eventName, preventDefault, invoker) {
  let callback = args => {
    const obj = {};
    for (let k in args) {
      if (k !== 'originalTarget') { //firefox occasionally raises Permission Denied when this property is being stringified
        obj[k] = args[k];
      }
    }
    let json = JSON.stringify(obj, (k, v) => {
      if (v instanceof Node) return 'Node';
      if (v instanceof Window) return 'Window';
      return v;
    }, ' ');
    invoker.invokeMethodAsync('Invoke', json);
    if (preventDefault === true) {
      args.preventDefault();
    }
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

export function focus(selector, noScroll: boolean=false) {
  let dom = getDom(selector);     
  if (!(dom instanceof HTMLElement))
      throw new Error("Unable to focus an invalid element.");
  dom.focus({
      preventScroll: noScroll
  })
}

export function hasFocus(selector) {
  let dom = getDom(selector);
  return (document.activeElement === dom);
}

export function blur(selector) {
  let dom = getDom(selector);
  dom.blur();
}

export function log(text) {
  console.log(text);
}

export function backTop(target: string) {
  let dom = getDom(target);
  if (dom) {
    slideTo(dom.scrollTop);
  } else {
    slideTo(0);
  }
}

function slideTo(targetPageY) {
  var timer = setInterval(function () {
    var currentY = document.documentElement.scrollTop || document.body.scrollTop;
    var distance = targetPageY > currentY ? targetPageY - currentY : currentY - targetPageY;
    var speed = Math.ceil(distance / 10);
    if (currentY == targetPageY) {
      clearInterval(timer);
    } else {
      window.scrollTo(0, targetPageY > currentY ? currentY + speed : currentY - speed);
    }
  }, 10);
}

export function scrollTo(target) {
  let dom = getDom(target);
  if (dom instanceof HTMLElement) {
    dom.scrollIntoView({
      behavior: "smooth",
      block: "start",
      inline: "nearest"
    });
  }
}

export function getFirstChildDomInfo(element) {
  var dom = getDom(element);
  if (dom.firstElementChild)
    return getDomInfo(dom.firstElementChild);
  return getDomInfo(dom);
}

export function addClsToFirstChild(element, className) {
  var dom = getDom(element);
  if (dom.firstElementChild) {
    dom.firstElementChild.classList.add(className);
  }
}

export function removeClsFromFirstChild(element, className) {
  var dom = getDom(element);
  if (dom.firstElementChild) {
    dom.firstElementChild.classList.remove(className);
  }
}

export function addDomEventListenerToFirstChild(element, eventName, preventDefault, invoker) {
  var dom = getDom(element);

  if (dom.firstElementChild) {
    addDomEventListener(dom.firstElementChild, eventName, preventDefault, invoker);
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

export function elementScrollIntoView(selector: Element | string) {
  let element = getDom(selector);

  if (!element)
    return;

  element.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'start' });
}

const oldBodyCacheStack = [];

const hasScrollbar = () => {
  let overflow = document.body.style.overflow;
  if (overflow && overflow === "hidden") return false;
  return document.body.scrollHeight > (window.innerHeight || document.documentElement.clientHeight);
}

export function disableBodyScroll() {
  let body = document.body;
  const oldBodyCache = {};
  ["position", "width", "overflow"].forEach((key) => {
    oldBodyCache[key] = body.style[key];
  });
  oldBodyCacheStack.push(oldBodyCache);
  css(body,
    {
      "position": "relative",
      "width": hasScrollbar() ? "calc(100% - 17px)" : null,
      "overflow": "hidden"
    });
  addCls(document.body, "ant-scrolling-effect");
}

export function enableBodyScroll() {
  let oldBodyCache = oldBodyCacheStack.length > 0 ? oldBodyCacheStack.pop() : {};

  css(document.body,
    {
      "position": oldBodyCache["position"] ?? null,
      "width": oldBodyCache["width"] ?? null,
      "overflow": oldBodyCache["overflow"] ?? null
    });
  removeCls(document.body, "ant-scrolling-effect");
}

export function destroyAllDialog() {
  document.querySelectorAll('.ant-modal-root')
    .forEach(e => document.body.removeChild(e.parentNode));
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

export function getMaxZIndex() {
  return [...document.all].reduce((r, e) => Math.max(r, +window.getComputedStyle(e).zIndex || 0), 0)
}

export function getStyle(element, styleProp) {        
  if (element.currentStyle)
    return element.currentStyle[styleProp];
  else if (window.getComputedStyle)
    return document.defaultView.getComputedStyle(element, null).getPropertyValue(styleProp);
}

export function getTextAreaInfo(element) {
    var result = {};
    var dom = getDom(element);
    result["scrollHeight"] = dom.scrollHeight || 0;

  if (element.currentStyle) {
    result["lineHeight"] = parseFloat(element.currentStyle["line-height"]);
    result["paddingTop"] = parseFloat(element.currentStyle["padding-top"]);
    result["paddingBottom"] = parseFloat(element.currentStyle["padding-bottom"]);
    result["borderBottom"] = parseFloat(element.currentStyle["border-bottom"]);
    result["borderTop"] = parseFloat(element.currentStyle["border-top"]);
  }
  else if (window.getComputedStyle) {
    result["lineHeight"] = parseFloat(document.defaultView.getComputedStyle(element, null).getPropertyValue("line-height"));
    result["paddingTop"] = parseFloat(document.defaultView.getComputedStyle(element, null).getPropertyValue("padding-top"));
    result["paddingBottom"] = parseFloat(document.defaultView.getComputedStyle(element, null).getPropertyValue("padding-bottom"));
    result["borderBottom"] = parseFloat(document.defaultView.getComputedStyle(element, null).getPropertyValue("border-bottom"));
    result["borderTop"] = parseFloat(document.defaultView.getComputedStyle(element, null).getPropertyValue("border-top"));
  }
  //Firefox can return this as NaN, so it has to be handled here like that.
  if (Object.is(NaN, result["borderTop"]))
      result["borderTop"] = 1;
  if (Object.is(NaN, result["borderBottom"]))
      result["borderBottom"] = 1;
  return result;
}

const funcDict = {};

export function registerResizeTextArea(element, minRows, maxRows, objReference) {
  if (!objReference) {
    disposeResizeTextArea(element);
  }
  else {
    objReferenceDict[element.id] = objReference;
    funcDict[element.id + "input"] = function () { resizeTextArea(element, minRows, maxRows); }
    element.addEventListener("input", funcDict[element.id + "input"]);
    return getTextAreaInfo(element);
  }
}

export function disposeResizeTextArea(element) {
  element.removeEventListener("input", funcDict[element.id + "input"]);
  objReferenceDict[element.id] = null;
  funcDict[element.id + "input"] = null;

}

export function resizeTextArea(element, minRows, maxRows) {
  var dims = getTextAreaInfo(element);
  var rowHeight = dims["lineHeight"];
  var offsetHeight = dims["paddingTop"] + dims["paddingBottom"] + dims["borderTop"] + dims["borderBottom"];
  var oldHeight = parseFloat(element.style.height);
  element.style.height = 'auto';

  var rows = Math.trunc(element.scrollHeight / rowHeight);
  rows = Math.max(minRows, rows);

  var newHeight = 0;
  if (rows > maxRows) {
    rows = maxRows;

    newHeight = (rows * rowHeight + offsetHeight);
    element.style.height = newHeight + "px";
    element.style.overflowY = "visible";
  }
  else {
    newHeight = rows * rowHeight + offsetHeight;
    element.style.height = newHeight + "px";
    element.style.overflowY = "hidden";
  }
  if (oldHeight !== newHeight) {
    let textAreaObj = objReferenceDict[element.id];
    textAreaObj.invokeMethodAsync("ChangeSizeAsyncJs", parseFloat(element.scrollWidth), newHeight);
  }
}



const objReferenceDict = {};
export function disposeObj(objReferenceName) {
  delete objReferenceDict[objReferenceName];
}

//#region mentions

import getOffset from "./modules/Caret";

export function getCursorXY(element, objReference) {
  objReferenceDict["mentions"] = objReference;
  window.addEventListener("click", mentionsOnWindowClick);

  var offset = getOffset(element);

  return [offset.left, offset.top + offset.height + 14];
}

function mentionsOnWindowClick(e) {
  let mentionsObj = objReferenceDict["mentions"];
  if (mentionsObj) {
    mentionsObj.invokeMethodAsync("CloseMentionsDropDown");
  } else {
    window.removeEventListener("click", mentionsOnWindowClick);
  }
}

//#endregion

export { enableDraggable, disableDraggable, resetModalPosition } from "./modules/dragHelper";

export function bindTableHeaderAndBodyScroll(bodyRef, headerRef) {
  bodyRef.bindScrollLeftToHeader = () => {
    headerRef.scrollLeft = bodyRef.scrollLeft;
  }
  bodyRef.addEventListener('scroll', bodyRef.bindScrollLeftToHeader);
}

export function unbindTableHeaderAndBodyScroll(bodyRef) {
  if (bodyRef) {
    bodyRef.removeEventListener('scroll', bodyRef.bindScrollLeftToHeader);
  }
}

function preventKeys(e, keys: string[]) {
  if (keys.indexOf(e.key.toUpperCase()) !== -1) {
    e.preventDefault();
    return false;
  }
}

export function addPreventKeys(inputElement, keys: string[]) {
  if (inputElement) {
    let dom = getDom(inputElement);
    keys = keys.map(function (x) { return x.toUpperCase(); })
    funcDict[inputElement.id + "keydown"] = (e) => preventKeys(e, keys);
    (dom as HTMLElement).addEventListener("keydown", funcDict[inputElement.id + "keydown"], false);
  }
}

export function removePreventKeys(inputElement) {
  if (inputElement) {
    let dom = getDom(inputElement);
    (dom as HTMLElement).removeEventListener("keydown", funcDict[inputElement.id + "keydown"]);
    funcDict[inputElement.id + "keydown"] = null;
  }
}

function preventKeyOnCondition(e, key: string, check: () => boolean) {
  if (e.key.toUpperCase() === key.toUpperCase() && check()) {
    e.preventDefault();
    return false;
  }
}

export function addPreventEnterOnOverlayVisible(element, overlayElement) {
  if (element && overlayElement) {
    let dom = getDom(element);
    funcDict[element.id + "keydown:Enter"] = (e) => preventKeyOnCondition(e, "enter", () => overlayElement.offsetParent !== null);
    (dom as HTMLElement).addEventListener("keydown", funcDict[element.id + "keydown:Enter"], false);
  }
}

export function removePreventEnterOnOverlayVisible(element) {
  if (element) {
    let dom = getDom(element);
    (dom as HTMLElement).removeEventListener("keydown", funcDict[element.id + "keydown:Enter"]);
    funcDict[element.id + "keydown:Enter"] = null;
  }
}