---
order: 9
title:
  zh-CN: Put方式上传
  en-US: Http Put Upload
---

## zh-CN

有些业务系统后端接口采用HttpPut方式接收前端上传的文件，HttpPut方式只能上传单个文件，文件流数据被存放在了Body中，我们只需要将上传组件的Method属性值设置为Put就可以实现这一功能。
阿里云的OSS产品就是采用HttpPut方式，为了安全，前端上传文件之前，先从后端接口获取到上传Url，前端通过put方式将文件流上传到该Url接口。

## en-US

Some business system back-end interfaces use HttpPut mode to receive front-end uploaded files, HttpPut mode can only upload a single file, file stream data is stored in Body, we only need to set the Method property value of the upload component to Put to achieve this function.
Alibaba Cloud's OSS products use the HttpPut method, for security, before uploading the file, the frontend obtains the upload URL from the backend interface, and the frontend uploads the file stream to the URL interface through the put mode.
