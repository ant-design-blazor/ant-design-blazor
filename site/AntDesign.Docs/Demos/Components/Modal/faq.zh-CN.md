### 为什么继承自FeedbackComponent<>的自定义组件在关闭`ModalService.CreateModalAsync<>`创建的Modal时不执行`Dispose`方法？

Modal关闭后默认不会从DOM中移除，所以不会执行自定义组件的`Dispose`方法。 您可以通过设置 modalOptions.DestroyOnClose=true 来更改此默认行为。