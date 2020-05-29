var startLoader = function (progressElement, textElement) {
  console.log('loading..')
  /*
   * Loader/splash screen
   * */
  if (!window.XMLHttpRequest) {
    console.log('no XMLHttpRequest..')
    return;
  }
  var loading = {};
  var files = [];
  var total = 0;
  var loaded = 0;
  var proxied = window.XMLHttpRequest.prototype.open;
  window.XMLHttpRequest.prototype.open = function () {
    console.log('open..')
    var file = arguments[1];
    files.push(file);
    total++;
    loading[file] = 1;
    this.addEventListener("load", function () {
      console.log(`Loaded ${file} ...`);
      delete (loading[file]);
      loaded++;
      var progress = Math.floor(((loaded / total) * 100));
      if (progressElement) {
        progressElement.max = total;
        progressElement.value = loaded;
      }
      if (textElement) {
        textElement.innerHTML = `Loaded ${file} ..., (${progress}%).`;
      }

      if (loaded == total) {
        // Reset override.
        window.XMLHttpRequest.prototype.open = proxied;
        if (textElement) {
          textElement.innerHTML = "Loading 100%, opening application...";
        }
      }
    });
    return proxied.apply(this, [].slice.call(arguments));
  };
};