### 如何实现上传服务端?

- 可以参考[Asp.Net Core WebAPI](https://github.com/ant-design-blazor/ant-design-blazor/discussions/2149)或[jQuery-File-Upload](https://github.com/blueimp/jQuery-File-Upload/wiki#server-side）关于如何实现服务器端上传接口。
- rc-upload 中有一个 [express](https://github.com/react-component/upload/blob/master/server.js) 的模拟示例。
 
### 我可以使用 FileStream 吗?

该组件没有实现FileStream，如果追求在Server端上传而忽略WebAPI，可以尝试使用官方的InputFile组件（并美化Antd的风格）

### 我想显示下载链接。

请设置`fileList`中每一项的属性`url`来控制链接的内容。

### 服务端开启了 Antiforgery 验证，如何在上传的请求中加入 AntiForgeryToken？

Upload 组件自动会寻找页面中的 `<input type="hidden" name="__RequestVerificationToken" value="... token ..." />`，如果是静态表单中使用，您不需要做什么。
如果是在 Blazor Server 中使用，则需在静态的 App.razor 中添加 `<AntiforgeryToken />` 组件，
或通过 [官方文档](https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0) 中提到的方式手动创建这样的隐藏域。

如果是通过Ajax方式获取的 AntiForgeryToken，请通过 Data 属性或 Headers 属性加入到上传请求中。