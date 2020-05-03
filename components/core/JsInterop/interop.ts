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

  for (var key in dom) {
    var item = dom[key];
    if (!item) continue;
    if (typeof item === 'string' || (typeof item === 'number' && !isNaN(item))) result[key] = item;
  }

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
