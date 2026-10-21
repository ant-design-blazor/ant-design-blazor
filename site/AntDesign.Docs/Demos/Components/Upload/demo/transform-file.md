---
order: 23
title:
  zh-CN: 上传前转换文件
  en-US: Transform file before upload
---

## zh-CN

`TransformFile` 在上传前获取文件内容并返回替换内容。本例通过 Canvas 为图片添加水印；文件内容会经过 Blazor 回调往返，适合较小的文件，不建议直接用于大文件。

## en-US

`TransformFile` can replace file contents before upload. This example adds a watermark to an image with Canvas. File bytes travel through the Blazor callback, so this is intended for small files and is not recommended for large uploads.
