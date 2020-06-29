---
category: Components
type: 通用
title: Upload 
subtitle: 文件上传
cols: 1
---

文件上传。

## 何时使用

- 当需要文件上传时时使用, 如商品轮播图, 文章图片,用户头像等。


## API

Attribute

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Url | * 文件上传API地址 | string         | null         |
| Name | 	* Form表单提交时的input file的name属性 | string         | null         |
| Text | 	上传按钮文字 | string         | Click to upload         |
| Icon | 上传按钮图标| string         | upload         |
| MaxCount | 最大文件数量 | int         | 6         |
| FileList | 用于初始化以及返回上传的文件列表 | List<UploadFileItem>         | empty |

CallbackEvent

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| BeforeUpload | 用于文件上传前的验证. 传入参数 UploadFileItem, 返回 bool, 若返回false则中止改文件上传 | delegate bool BeforeUploadDelegate(UploadFileItem file)  | null         |
| OnSingleCompleted | 	单个文件上传完成后触发(不论成功失败) | EventCallback<UploadFileItem>  | null         |
| OnCompleted | FileList全部完成后触发(不论成功失败) | EventCallback<List<UploadFileItem>>         | null         |

