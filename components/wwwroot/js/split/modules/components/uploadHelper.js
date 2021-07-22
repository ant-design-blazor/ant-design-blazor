export class uploadHelper {
    static addFileClickEventListener(btn) {
        if (btn.addEventListener) {
            btn.addEventListener("click", this.fileClickEvent);
        }
    }
    static removeFileClickEventListener(btn) {
        btn.removeEventListener("click", this.fileClickEvent);
    }
    static fileClickEvent(e) {
        const fileId = e.currentTarget.attributes["data-fileid"].nodeValue;
        const element = document.getElementById(fileId);
        element.click();
    }
    static clearFile(element) {
        element.setAttribute("type", "input");
        element.value = '';
        element.setAttribute("type", "file");
    }
    static getFileInfo(element) {
        if (element.files && element.files.length > 0) {
            let fileInfo = Array();
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
    static getObjectURL(file) {
        var url = null;
        if (window.URL != undefined) {
            url = window.URL.createObjectURL(file);
        }
        else if (window.webkitURL != undefined) {
            url = window.webkitURL.createObjectURL(file);
        }
        return url;
    }
    static uploadFile(element, index, data, headers, fileId, url, name, instance, percentMethod, successMethod, errorMethod) {
        let formData = new FormData();
        var file = element.files[index];
        var size = file.size;
        formData.append(name, file);
        if (data != null) {
            for (var key in data) {
                formData.append(key, data[key]);
            }
        }
        const req = new XMLHttpRequest();
        req.onreadystatechange = function () {
            if (req.readyState === 4) {
                // #1655 Any 2xx response code is okay
                if (req.status < 200 || req.status > 299) {
                    instance.invokeMethodAsync(errorMethod, fileId, `{"status": ${req.status}}`);
                    return;
                }
                instance.invokeMethodAsync(successMethod, fileId, req.responseText);
            }
        };
        req.upload.onprogress = function (event) {
            var percent = Math.floor(event.loaded / size * 100);
            instance.invokeMethodAsync(percentMethod, fileId, percent);
        };
        req.onerror = function (e) {
            instance.invokeMethodAsync(errorMethod, fileId, "error");
        };
        req.open('post', url, true);
        if (headers != null) {
            for (var header in headers) {
                req.setRequestHeader(header, headers[header]);
            }
        }
        req.send(formData);
    }
}
