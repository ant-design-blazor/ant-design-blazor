### Why does the custom component inherited from `FeedbackComponent<>` not execute the `Dispose` method when a Modal created by `ModalService.CreateModalAsync<>` is closed?

Modal will not be removed from DOM by default after closing, so the `Dispose` method of custom component will not be executed. You can change this default behavior by setting `modalOptions.DestroyOnClose=true`.