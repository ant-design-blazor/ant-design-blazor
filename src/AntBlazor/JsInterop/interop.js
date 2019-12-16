window.GetDomInfo = function (element) {
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

window.addDomEventListener = function (eventName, invoker) {
  window.addEventListener(eventName, function (args) {
    invoker.invokeMethodAsync('Invoke');
  });
}

window.antMatchMedia = function (query) {
  return window.matchMedia(query).matches;
}