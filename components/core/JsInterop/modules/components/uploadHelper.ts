// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

type fileInfo = {
  fileName: string,
  size: number,
  objectURL: string,
  type: string
}

const CALLBACK_METHODS = {
  PERCENT: "UploadChanged",
  SUCCESS: "UploadSuccess",
  ERROR: "UploadError",
  PASTE_RESULT: "OnPasteResult"
} as const;

interface UploadConfig {
  element: HTMLInputElement;
  index: number;
  data: any;
  headers: any;
  fileIds: string[];
  url: string;
  name: string;
  instance: any;
  method: string;
  withCredentials?: boolean;
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

  static addPasteEventListener(element: HTMLElement, input: HTMLInputElement, instance: any) {
    const inputTarget = element.querySelector("input");
    const textareaTarget = element.querySelector("textarea");
    
    if (inputTarget && inputTarget.addEventListener) {
      inputTarget.addEventListener("paste", (e: ClipboardEvent) => uploadHelper.handlePaste(e, input, instance));
    }
    
    if (textareaTarget && textareaTarget.addEventListener) {
      textareaTarget.addEventListener("paste", (e: ClipboardEvent) => uploadHelper.handlePaste(e, input, instance));
    }
  }

  static removePasteEventListener(element: HTMLElement) {
    const inputTarget = element.querySelector("input");
    const textareaTarget = element.querySelector("textarea");
    
    if (inputTarget) {
      inputTarget.removeEventListener("paste", uploadHelper.handlePaste as any);
    }
    
    if (textareaTarget) {
      textareaTarget.removeEventListener("paste", uploadHelper.handlePaste as any);
    }
  }

  private static fileClickEvent(e: MouseEvent) {
    e.stopPropagation();
    const fileId = (e.currentTarget as HTMLSpanElement).attributes["data-fileid"].nodeValue;
    const element = document.getElementById(fileId) as HTMLInputElement;
    element.click();
  }

  private static async handlePaste(e: ClipboardEvent, input: HTMLInputElement, instance: any) {
    e.preventDefault();
    const items = e.clipboardData?.items;
    if (!items) return;

    const files: File[] = [];
    for (let i = 0; i < items.length; i++) {
      const item = items[i];
      if (item.kind === 'file') {
        const file = item.getAsFile();
        if (file) {
          files.push(file);
        }
      }
    }

    if (files.length > 0) {
      const target = e.target as any;
      if (!target.dataTransfer) {
        target.dataTransfer = new DataTransfer();
      }
      const dataTransfer = target.dataTransfer as DataTransfer;
      files.forEach(file => dataTransfer.items.add(file));
      input.files = dataTransfer.files;

      const fileInfo = files.map(file => ({
        fileName: file.name,
        size: file.size,
        objectURL: uploadHelper.getObjectURL(file),
        type: file.type
      }));

      await instance.invokeMethodAsync(CALLBACK_METHODS.PASTE_RESULT, fileInfo);
    }
  }

  static clearFile(element) {
    element.setAttribute("type", "input");
    element.value = '';
    element.setAttribute("type", "file");
  }

  static getFileInfo(element: HTMLInputElement) {
    if (element.files && element.files.length > 0) {
      const fileInfo = Array<fileInfo>();
      for (let i = 0; i < element.files.length; i++) {
        const file = element.files[i];
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
    let url = null;
    if (window.URL != undefined) {
      url = window.URL.createObjectURL(file);
    } else if (window.webkitURL != undefined) {
      url = window.webkitURL.createObjectURL(file);
    }
    return url;
  }

  static uploadFile({ element, index, data, headers, fileIds, url, name, instance, method, withCredentials }: UploadConfig) {
    const formData = new FormData();
    const files = index === -1 ? Array.from(element.files) : [element.files[index]];

    files.forEach((file, i) => {
      formData.append(name, file);
      // Add file ID to track individual file progress
      formData.append(`${name}_id`, fileIds[i]);
    });

    if (data != null) {
      for (const key in data) {
        formData.append(key, data[key]);
      }
    }

    if (!formData['__RequestVerificationToken']) {
      const antiforgeryToken = document.querySelector('[name="__RequestVerificationToken"]');
      if (antiforgeryToken) {
        formData.append('__RequestVerificationToken', antiforgeryToken.getAttribute('value'));
      }
    }

    const req = new XMLHttpRequest()
    req.onreadystatechange = function () {
      if (req.readyState === 4) {
        // #1655 Any 2xx response code is okay
        if (req.status < 200 || req.status > 299) {
          // #2857 should get error raw response
          fileIds.forEach(id => {
            instance.invokeMethodAsync(CALLBACK_METHODS.ERROR, id, req.responseText);
          });
          return;
        }
        fileIds.forEach(id => {
          instance.invokeMethodAsync(CALLBACK_METHODS.SUCCESS, id, req.responseText);
        });
      }
    }
    req.upload.onprogress = function (event) {
      const percent = Math.floor(event.loaded / event.total * 100);
      fileIds.forEach(id => {
        instance.invokeMethodAsync(CALLBACK_METHODS.PERCENT, id, percent);
      });
    }
    req.onerror = function (e: ProgressEvent) {
      const errorMessage = `Upload failed: ${e.type} (${req.status} ${req.statusText})${req.responseText ? ` - ${req.responseText}` : ''}`;
      fileIds.forEach(id => {
        instance.invokeMethodAsync(CALLBACK_METHODS.ERROR, id, errorMessage);
      });
    }
    req.open(method, url, true)
    // Set withCredentials if provided
    if (withCredentials === true) {
      req.withCredentials = true;
    }
    if (headers != null) {
      for (const header in headers) {
        req.setRequestHeader(header, headers[header]);
      }
    }
    req.send(formData)
  }
}