---
order: 22
title:
  zh-CN: 阿里云 OSS
  en-US: Aliyun OSS
---

## zh-CN

演示将 OSS 的 `key`、`OSSAccessKeyId`、`policy` 和 `Signature` 作为额外表单字段随文件上传。页面使用 mock 上传地址和占位凭证；生产环境应由服务端签发短期凭证，并在凭证过期时重新获取，切勿把真实密钥放在浏览器中。

## en-US

Attach the OSS `key`, `OSSAccessKeyId`, `policy`, and `Signature` as extra multipart fields. This page uses a mock upload endpoint and placeholder credentials; production applications should obtain short-lived credentials from a trusted server and refresh them when expired. Never expose real secrets in browser code.
