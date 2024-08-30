---
order: 9
title:
  zh-CN: 下载按钮
  en-US: Download Buttons
---

## zh-CN

下载按钮提供了文件下载的[简单实现](https://learn.microsoft.com/zh-cn/aspnet/core/blazor/file-downloads#download-from-a-url)，可以满足多数情况下的需求。

`Url`参数为空或null时，该按钮不会触发文件下载。

`FileName`参数实际是`<a/>`标签的`download`属性。关于其描述和限制请参阅[MDN文档](https://developer.mozilla.org/zh-CN/docs/Web/HTML/Element/a#attr-download)。

## en-US

`DownloadButton` provides a [simple implementation](https://learn.microsoft.com/en-us/aspnet/core/blazor/file-downloads#download-from-a-url) of file download, which may meet the needs of most situations.

When `Url` is null or empty, this button won't trigger file download.

`FileName` is actually the `download` attribute of `<a/>`. About its description and limit, please read [MDN Doc](https://developer.mozilla.org/en-US/docs/Web/HTML/Element/a#attr-download).