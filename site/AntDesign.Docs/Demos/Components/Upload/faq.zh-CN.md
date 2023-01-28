### 如何实现上传服务端?

- 可以参考[Asp.Net Core WebAPI](https://github.com/ant-design-blazor/ant-design-blazor/discussions/2149)或[jQuery-File-Upload](https://github .com/blueimp/jQuery-File-Upload/wiki#server-side）关于如何实现服务器端上传接口。
- rc-upload 中有一个 [express](https://github.com/react-component/upload/blob/master/server.js) 的模拟示例。
 
### 我可以使用 FileStream 吗?

该组件没有实现FileStream，如果追求在Server端上传而忽略WebAPI，可以尝试使用官方的InputFile组件（并美化Antd的风格）

### 我想显示下载链接。

请设置`fileList`中每一项的属性`url`来控制链接的内容。