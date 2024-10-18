### How to implement upload server side?

- You can consult [Asp.Net Core WebAPI](https://github.com/ant-design-blazor/ant-design-blazor/discussions/2149) or [jQuery-File-Upload](https://github.com/blueimp/jQuery-File-Upload/wiki#server-side) about how to implement server side upload interface.
- There is a mock example of [express](https://github.com/react-component/upload/blob/master/server.js) in rc-upload.
 
### Can I use FileStream?

FileStream is not implemented in this component, if you pursue uploading on the Server Side and omit WebAPI, you can try to use the official InputFile component (and beautify it with Antd's style)

### I want to display download links.

Please set property `url` of each item in `fileList` to control content of link.

### Antiforgery verification is enabled on the server. How to add AntiForgeryToken to the uploaded request?

The Upload component automatically looks for the '<input type="hidden" name="__RequestVerificationToken" value="... token ..."  /> ', if it is used in a static form, you do not need to do anything.
If you are using the Blazor Server, you need to add the '<AntiforgeryToken />' component to the static App.razor,
Or through [the official documentation](https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0) The way mentioned in the manual creation of such hidden domains.

If the `AntiForgeryToken` is obtained through Ajax, add it to the upload request using the `Data` or `Headers` parameter.