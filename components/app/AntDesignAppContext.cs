namespace AntDesign;

/// <summary>
/// Services exposed to descendants of an <see cref="App"/> through a cascading parameter.
/// The context object is scoped to an App component subtree.
/// </summary>
public sealed class AntDesignAppContext
{
    internal AntDesignAppContext(IMessageService message, INotificationService notification, ModalService modal)
    {
        Message = message;
        Notification = notification;
        Modal = modal;
    }

    /// <summary>Gets the message service available in this App subtree.</summary>
    public IMessageService Message { get; }

    /// <summary>Gets the notification service available in this App subtree.</summary>
    public INotificationService Notification { get; }

    /// <summary>Gets the modal service available in this App subtree.</summary>
    public ModalService Modal { get; }
}
