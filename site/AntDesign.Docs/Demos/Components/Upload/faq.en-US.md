### How to implement upload server side?

- You can consult [Asp.Net Core WebAPI](https://github.com/ant-design-blazor/ant-design-blazor/discussions/2149) or [jQuery-File-Upload](https://github.com/blueimp/jQuery-File-Upload/wiki#server-side) about how to implement server side upload interface.
- There is a mock example of [express](https://github.com/react-component/upload/blob/master/server.js) in rc-upload.
 
### Can I use FileStream?

FileStream is not implemented in this component, if you pursue uploading on the Server Side and omit WebAPI, you can try to use the official InputFile component (and beautify it with Antd's style)

### I want to display download links.

Please set property `url` of each item in `fileList` to control content of link.