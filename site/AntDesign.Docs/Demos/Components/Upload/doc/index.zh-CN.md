---
category: Components
subtitle: 上传
type: 数据录入
title: Upload
cover: https://gw.alipayobjects.com/zos/alicdn/QaeBt_ZMg/Upload.svg
---

文件选择上传和拖拽上传控件。

## 何时使用

上传是将信息（网页、文字、图片、视频等）通过网页或者上传工具发布到远程服务器上的过程。

- 当需要上传一个或一些文件时。
- 当需要展现上传的进度时。
- 当需要使用拖拽交互时。

## API

| 参数 | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| ClassNames | 为根节点、触发器、文件列表和文件项设置语义 class | `IReadOnlyDictionary<string, string>` | - |
| Styles | 为根节点、触发器、文件列表和文件项设置语义内联样式 | `IReadOnlyDictionary<string, string>` | - |
| RemoveIcon | 自定义移除操作的内容 | `RenderFragment<UploadFileItem>` | 删除图标 |
| PreviewIcon | 自定义预览操作的内容 | `RenderFragment<UploadFileItem>` | 预览图标 |
| DownloadIcon | 自定义下载操作的内容 | `RenderFragment<UploadFileItem>` | 下载图标 |
| Extra | 在文件名旁按文件渲染的附加内容 | `RenderFragment<UploadFileItem>` | - |
| DragSort | 允许通过拖动重新排列文件列表 | bool | false |

| 参数 | 说明 | 类型 | 默认值 | 版本 |
| --- | --- | --- | --- | --- |
| Accept | 接受上传的文件类型, 详见 [input accept Attribute](https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input/file#accept) | string | 无 |  |
| Action | 上传的地址 | string\|(file) => `Promise` | 无 |  |
| Directory | 支持上传文件夹（[caniuse](https://caniuse.com/#feat=input-file-directory)） | boolean | false |  |
| BeforeUpload | 上传文件之前的钩子，参数为上传的文件，若返回 `false` 则停止上传。支持返回一个 Promise 对象，Promise 对象 reject 时则停止上传，resolve 时开始上传（ resolve 传入 `File` 或 `Blob` 对象则上传 resolve 传入对象）。**注意：IE9 不支持该方法**。 | (file, fileList) => `boolean | Promise` | 无 |  |
| TransformFile | 上传前异步替换文件内容。文件字节会经 Blazor 回调传输，适用于较小文件。 | `Func<UploadFileTransformContext, Task<UploadFileTransformResult>>` | 无 |  |
| BeforeAllUpload & BeforeAllUploadAsync | 上传文件之前的钩子，参数为上传的文件，若返回 `false` 则停止上传。支持返回一个 Promise 对象，Promise 对象 reject 时则停止上传，resolve 时开始上传（ resolve 传入 `File` 或 `Blob` 对象则上传 resolve 传入对象）。**注意：IE9 不支持该方法**。 | (file, fileList) => `boolean | Promise` | 无 |  |
| Data | 上传所需额外参数或返回上传额外参数的方法 | object\|(file) => object | 无 |  |
| DefaultFileList | 默认已经上传的文件列表 | object\[] | 无 |  |
| Disabled | 是否禁用 | boolean | false |  |
| FileList | 已经上传的文件列表（受控），使用此参数时，如果遇到 `onChange` 只调用一次的问题，请参考 [#2423](https://github.com/ant-design/ant-design/issues/2423) | object\[] | 无 |  |
| Headers | 设置上传的请求头部，IE10 以上有效 | object | 无 |  |
| ListType | 上传列表的内建样式：`text`、`picture`、`picture-card` 或 `picture-circle` | string | 'text' | `picture-circle` (5.2.0+) |
| Method | 上传请求的 http method | string | 'post' |  |
| Multiple | 是否支持多选文件，`ie10+` 支持。开启后按住 ctrl 可选择多个文件 | boolean | false |  |
| MaxCount | 文件列表的最大数量；设置为 `1` 时，新文件会替换当前文件 | number | - |  |
| ProgressStrokeColor | 上传列表进度条的渐变色节点 | `Dictionary<string, string>` | - |  |
| ProgressStrokeWidth | 上传列表进度条的像素宽度 | `int?` | `2` |  |
| ProgressFormat | 格式化进度条旁显示的上传百分比 | `Func<double, string>` | - |  |
| ProgressShowInfo | 是否在进度条旁显示格式化后的百分比 | `bool` | `false` |  |
| Name | 发到后台的文件参数名 | string | 'file' |  |
| OnChange | 上传文件改变时的状态，详见 [onChange](#onChange) | Function | 无 |  |
| OnCompleted | 回调函数，当所有上传成功或失败时执行 | Function(file) | - |  |
| OnDownload | 点击下载文件时的回调，如果没有指定，则默认跳转到文件 url 对应的标签页。 | Function(file): void | 跳转新标签页 |  |
| OnPreview | 点击文件链接或预览图标时的回调 | Function(file) | 无 |  |
| OnRemove   | 点击移除文件时的回调，返回值为 false 时不移除。支持返回一个 Promise 对象，Promise 对象 resolve(false) 或 reject 时不移除。               | Function(file): `boolean | Promise` | 无   |  |
| OnSingleCompleted | 回调函数，上传完成后执行 | Function(file) | - |  |
| ShowUploadList | 是否展示文件列表 | Boolean | true |  |
| ShowDownloadIcon | 是否展示下载按钮 | Boolean | true |  |
| ShowPreviewIcon | 是否展示预览按钮 | Boolean | true |  |
| ShowRemoveIcon | 是否展示移除按钮 | Boolean | true |  |
| Defer | 是否使用手动上传模式 | Boolean | false |  |

### onChange

> 上传中、完成、失败都会调用这个函数。

文件状态改变的回调，返回为：

```js
{
  file: { /* ... */ },
  fileList: [ /* ... */ ],
  event: { /* ... */ },
}
```

1. `file` 当前操作的文件对象。

   ```js
   {
      uid: 'uid',      // 文件唯一标识，建议设置为负数，防止和内部产生的 id 冲突
      name: 'xx.png'   // 文件名
      status: 'done', // 状态有：uploading done error removed
      response: '{"status": "success"}', // 服务端响应内容
      linkProps: '{"download": "image"}', // 下载链接额外的 HTML 属性
   }
   ```

2. `fileList` 当前的文件列表。
3. `event` 上传中的服务端响应内容，包含了上传进度等信息，高级浏览器支持。

## FAQ

### 服务端如何实现？

- 服务端上传接口实现可以参考[Asp.Net Core WebAPI](https://github.com/ant-design-blazor/ant-design-blazor/discussions/2149) [jQuery-File-Upload](https://github.com/blueimp/jQuery-File-Upload/wiki#server-side)。
- 如果要做本地 mock 可以参考这个 [express 的例子](https://github.com/react-component/upload/blob/master/server.js)。

### 如何获取FileStream

目前并未实现FileStream,如果你追求在Server Side完成上传操作并省略WebAPI,你可以尝试使用官方的InputFile组件(并使用Antd的样式进行美化)

### 如何显示下载链接？

请使用 fileList 属性设置数组项的 url 属性进行展示控制。

### `customRequest` 怎么使用？

请参考 <https://github.com/react-component/upload#customrequest>。

## 设计 Token

| Token | 说明 | 类型 | 默认值 |
| --- | --- | --- | --- |
| ActionsColor | 上传列表操作图标的颜色 | string | `colorIcon` |
| PictureCardSize | picture-card 和 picture-circle 上传项的尺寸 | number | `controlHeightLG * 2.55` |
