---
category: Components
type: General
title: Upload
cols: 1
---

File upload.

## When To Use

- It is used when file upload is needed, such as product picture, article picture, user avatar or other files. 


## API

Attribute

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| Url | * upload api url | string         | null         |
| Name | 	* The name attribute of the input element | string         | null         |
| Text | 	Button's Text | string         | Click to upload         |
| Icon | Button's icon| string         | upload         |
| MaxCount | Max file count | int         | 6         |
| FileList | List of files used for initialization or return | List<UploadFileItem>         | empty |

CallbackEvent

| 参数             | 说明                                         | 类型          | 默认值    |
| ---------------- | -------------------------------------------- | ------------- | --------- |
| BeforeUpload | Used for verification before uploading, if it returns false, cancel uploading this file | delegate bool BeforeUploadDelegate(UploadFileItem file)  | null         |
| OnSingleCompleted | 	Triggered after a single file is uploaded (success or failure) | EventCallback<UploadFileItem>  | null         |
| OnCompleted | Triggered after the FileList was all completed (success or failure) | EventCallback<List<UploadFileItem>>         | null         |

