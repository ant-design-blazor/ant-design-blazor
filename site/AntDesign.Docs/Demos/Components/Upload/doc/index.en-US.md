---
category: Components
type: Data Entry
title: Upload
cover: https://gw.alipayobjects.com/zos/alicdn/QaeBt_ZMg/Upload.svg
---

Upload file by selecting or dragging.

## When To Use

Uploading is the process of publishing information (web pages, text, pictures, video, etc.) to a remote server via a web page or upload tool.

- When you need to upload one or more files.
- When you need to show the process of uploading.
- When you need to upload files by dragging and dropping.

## API

| Property | Description | Type | Default | Version |
| --- | --- | --- | --- | --- |
| Accept | File types that can be accepted. See [input accept Attribute](https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/file#accept) | string | - |  |
| Action | Uploading URL | string\|(file) => `Promise` | - |  |
| Directory | support upload whole directory ([caniuse](https://caniuse.com/#feat=input-file-directory)) | boolean | false |  |
| BeforeUpload | Hook function which will be executed before uploading each file. Uploading will be stopped with `false` or a rejected Promise returned. **Warning：this function is not supported in IE9**。 | (file, fileList) => `boolean | Promise` | - |  |
| BeforeAllUpload & BeforeAllUploadAsync  | Hook function which will be executed before uploading. Uploading will be stopped with `false` or a rejected Promise returned. **Warning：this function is not supported in IE9**。 | (file, fileList) => `boolean | Promise` | - |  |
| Data | Uploading extra params or function which can return uploading extra params. | object\|function(file) | - |  |
| DefaultFileList | Default list of files that have been uploaded. | object\[] | - |  |
| Disabled | disable upload button | boolean | false |  |
| FileList | List of files that have been uploaded (controlled). Here is a common issue [#2423](https://github.com/ant-design/ant-design/issues/2423) when using it | object\[] | - |  |
| Headers | Set request headers, valid above IE10. | object | - |  |
| ListType | Built-in stylesheets, support for three types: `text`, `picture` or `picture-card` | string | 'text' |  |
| Method | http method of upload request | string | 'post' |  |
| Multiple | Whether to support selected multiple file. `IE10+` supported. You can select multiple files with CTRL holding down while multiple is set to be true | boolean | false |  |
| Name | The name of uploading file | string | 'file' |  |
| OnChange | A callback function, can be executed when uploading state is changing, see [onChange](#onChange) | Function | - |  |
| OnCompleted | A callback function, will be executed when all uploads have succeeded or failed | Function(file) | - |  |
| OnDownload | Click the method to download the file, pass the method to perform the method logic, do not pass the default jump to the new TAB. | Function(file): void | Jump to new TAB |  |
| OnPreview | A callback function, will be executed when file link or preview icon is clicked | Function(file) | - |  |
| OnRemove | A callback function, will be executed when removing file button is clicked, remove event will be prevented when return value is `false` or a Promise which resolve(false) or reject | Function(file): `boolean | Promise` | - |  |
| OnSingleCompleted | A callback function, will be executed when an upload is complete | Function(file) | - |  |
| ShowUploadList | Whether to show default upload list | true |  |
| ShowDownloadIcon | Whether to show download button | Boolean | true |  |
| ShowPreviewIcon | Whether to show preview button | Boolean | true |  |
| ShowRemoveIcon | Whether to show remove button | Boolean | true |  |


### onChange

> The function will be called when uploading is in progress, completed or failed.

When uploading state change, it returns:

```js
{
  file: { /* ... */ },
  fileList: [ /* ... */ ],
  event: { /* ... */ },
}
```

1. `file` File object for the current operation.

   ```js
   {
      uid: 'uid',      // unique identifier, negative is recommend, to prevent interference with internal generated id
      name: 'xx.png',   // file name
      status: 'done', // options：uploading, done, error, removed
      response: '{"status": "success"}', // response from server
      linkProps: '{"download": "image"}', // additional html props of file link
      xhr: 'XMLHttpRequest{ ... }', // XMLHttpRequest Header
   }
   ```

2. `fileList` current list of files
3. `event` response from server, including uploading progress, supported by advanced browsers.

## FAQ

### How to implement upload server side?

- You can consult [Asp.Net Core WebAPI](https://github.com/ant-design-blazor/ant-design-blazor/discussions/2149) or [jQuery-File-Upload](https://github.com/blueimp/jQuery-File-Upload/wiki#server-side) about how to implement server side upload interface.
- There is a mock example of [express](https://github.com/react-component/upload/blob/master/server.js) in rc-upload.
 
### Can I use FileStream?

FileStream is not implemented in this component, if you pursue uploading on the Server Side and omit WebAPI, you can try to use the official InputFile component (and beautify it with Antd's style)

### I want to display download links.

Please set property `url` of each item in `fileList` to control content of link.

### How to use `customRequest`?

See <https://github.com/react-component/upload#customrequest>.
