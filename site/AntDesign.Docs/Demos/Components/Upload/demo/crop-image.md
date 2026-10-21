---
order: 24
title:
  zh-CN: 裁剪图片
  en-US: Crop image
---

## zh-CN

使用 `Cropper.Blazor` 在上传前裁剪图片，并通过 `Upload.TransformFile` 上传裁剪后的 PNG。点击取消会跳过裁剪并上传原图。本演示依赖外部 Blazor 裁剪库；文件字节会经过 Blazor 回调，适合较小的图片，不建议用于大文件。

## en-US

This example uses `Cropper.Blazor` to crop an image before upload and returns the cropped PNG through `Upload.TransformFile`. Canceling skips cropping and uploads the original image. The demo relies on an external Blazor cropper; file bytes travel through a Blazor callback, so it is intended for small images rather than large files.
