type fileInfo = {
  fileName: string,
  size: number,
  objectURL: string,
  type: string
}

export class uploadHelper {
  static addFileClickEventListener(btn: HTMLElement) {
    if (btn.addEventListener) {
      btn.addEventListener("click", uploadHelper.fileClickEvent);
    }
  }

  static removeFileClickEventListener(btn: HTMLElement) {
    btn.removeEventListener("click", uploadHelper.fileClickEvent);
  }

  private static fileClickEvent(e: MouseEvent) {
    e.stopPropagation();
    const fileId = (e.currentTarget as HTMLSpanElement).attributes["data-fileid"].nodeValue;
    const element = document.getElementById(fileId) as HTMLInputElement;
    element.click();
  }

  static clearFile(element) {
    element.setAttribute("type", "input");
    element.value = '';
    element.setAttribute("type", "file");
  }

  static getFileInfo(element: HTMLInputElement) {
    if (element.files && element.files.length > 0) {
      let fileInfo = Array<fileInfo>();
      for (var i = 0; i < element.files.length; i++) {
        let file = element.files[i];
        const objectUrl = this.getObjectURL(file);
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

  private static getObjectURL(file: File): string {
    var url = null;
    if (window.URL != undefined) {
      url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) {
      url = window.webkitURL.createObjectURL(file);
    }
    return url;
  }

  static uploadFile(element, index, data, headers, fileId, url, name, instance, percentMethod, successMethod, errorMethod, method: string) {
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
        // #1655 Any 2xx response code is okay
        if (req.status < 200 || req.status > 299) {
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
    req.open(method, url, true)
    if (headers != null) {
      for (var header in headers) {
        req.setRequestHeader(header, headers[header]);
      }
    }
    req.send(formData)
  }
}