export function getDomInfo(element) {
  var result = {};

  if (!element) {
    element = document.body;
  } else if (typeof element === 'string') {
    element = document.querySelector(element);
  }

  for (var key in element) {
    var item = element[key];
    if (!item) continue;
    if (typeof item === 'string' || (typeof item === 'number' && !isNaN(item)))
      result[key] = item;
  }

  return result;
}

export function getBoundingClientRect(element) {
  if (!element) {
    element = document.body;
  } else if (typeof element === 'string') {
    element = document.querySelector(element);
  }

  return element.getBoundingClientRect();
}

export function addDomEventListener(dom, eventName, invoker) {
  let callback = (args) => {
    invoker.invokeMethodAsync('Invoke', args);
  };

  if (dom == 'window') {
    window.addEventListener(eventName, callback);
  } else {
    (document.querySelector(dom) as HTMLElement).addEventListener(eventName, callback);
  }
}

export function antMatchMedia(query) {
  return window.matchMedia(query).matches;
}

export function copy(text): Promise<any> {
  return new Promise<any>((resolve, reject) => {
    let copyTextArea = null;
    try {
      copyTextArea = this.document.createElement('textarea');
      copyTextArea.style.all = 'unset';
      copyTextArea.style.position = 'fixed';
      copyTextArea.style.top = '0';
      copyTextArea.style.clip = 'rect(0, 0, 0, 0)';
      copyTextArea.style.whiteSpace = 'pre';
      copyTextArea.style.webkitUserSelect = 'text';
      copyTextArea.style.MozUserSelect = 'text';
      copyTextArea.style.msUserSelect = 'text';
      copyTextArea.style.userSelect = 'text';
      this.document.body.appendChild(copyTextArea);
      copyTextArea.value = text;
      copyTextArea.select();

      const successful = this.document.execCommand('copy');
      if (!successful) {
        reject(text);
      }
      resolve(text);
    } finally {
      if (copyTextArea) {
        this.document.body.removeChild(copyTextArea);
      }
    }
  });
}