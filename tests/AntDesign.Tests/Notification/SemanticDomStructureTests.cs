using AntDesign;
using AntDesign.Docs.Components;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AntDesign.Tests.Notification;

public sealed class SemanticDomStructureTests : AntDesignTestBase
{
    [Fact]
    public void Message_item_renders_v6_notice_semantic_nodes()
    {
        var cut = RenderComponent<MessageItem>(parameters => parameters
            .Add(x => x.Config, new MessageConfig
            {
                Key = "semantic-message",
                Type = MessageType.Success,
                Content = "Hello, Ant Design!",
                Duration = 0,
            }));

        Assert.NotNull(cut.Find(".ant-message-notice"));
        Assert.NotNull(cut.Find(".ant-message-notice-wrapper"));
        Assert.NotNull(cut.Find(".ant-message-notice-icon"));
        Assert.Equal("Hello, Ant Design!", cut.Find(".ant-message-notice-title").TextContent.Trim());
    }

    [Fact]
    public void Notification_item_renders_v6_semantic_nodes()
    {
        var cut = RenderComponent<NotificationItem>(parameters => parameters
            .Add(x => x.Config, new NotificationConfig
            {
                Key = "semantic-notification",
                Message = "Hello World!",
                Description = "Hello World?",
                NotificationType = NotificationType.Success,
                Duration = 0,
                ShowProgress = true,
                Btn = builder => builder.AddContent(0, "My Button"),
            }));

        Assert.NotNull(cut.Find(".ant-notification-notice"));
        Assert.NotNull(cut.Find(".ant-notification-notice-wrapper"));
        Assert.NotNull(cut.Find(".ant-notification-notice-icon"));
        Assert.Equal("Hello World!", cut.Find(".ant-notification-notice-title").TextContent.Trim());
        Assert.Equal("Hello World?", cut.Find(".ant-notification-notice-description").TextContent.Trim());
        Assert.Equal("My Button", cut.Find(".ant-notification-notice-actions").TextContent.Trim());
        Assert.NotNull(cut.Find("button.ant-notification-notice-close"));
        Assert.NotNull(cut.Find(".ant-notification-close-icon"));
        Assert.NotNull(cut.Find("progress.ant-notification-notice-progress[value='100'][max='100']"));
    }

    [Fact]
    public void Semantic_preview_fixtures_render_the_documented_message_and_notification_slots()
    {
        var message = RenderComponent<MessageSemanticPreview>();
        Assert.NotNull(message.Find(".ant-message.ant-message-list.ant-message-top.ant-message-css-var .ant-message-list-content.ant-message-list-content-increase"));
        Assert.Equal(2, message.FindAll(".ant-message-notice").Count);

        var notification = RenderComponent<NotificationSemanticPreview>();
        Assert.NotNull(notification.Find(".ant-notification.ant-notification-list .ant-notification-list-content"));
        Assert.Equal(2, notification.FindAll(".ant-notification-notice").Count);
        Assert.Equal(2, notification.FindAll(".ant-notification-notice-wrapper").Count);
        Assert.Equal(2, notification.FindAll(".ant-notification-notice-icon").Count);
        Assert.Equal(2, notification.FindAll(".ant-notification-notice-section").Count);
        Assert.Equal(2, notification.FindAll(".ant-notification-notice-title").Count);
        Assert.Equal(2, notification.FindAll(".ant-notification-notice-description").Count);
        Assert.Equal(2, notification.FindAll(".ant-notification-notice-close").Count);
        Assert.Single(notification.FindAll(".ant-notification-notice-actions"));
        Assert.Single(notification.FindAll(".ant-notification-notice-progress"));
    }

    [Fact]
    public void Live_message_list_applies_the_v6_css_variable_scope()
    {
        var cut = RenderComponent<Message>();
        Services.GetRequiredService<MessageService>().Open(new MessageConfig
        {
            Key = "css-var-message",
            Type = MessageType.Success,
            Content = "CSS variable scope",
            Duration = 0,
        });

        Assert.NotNull(cut.Find(".ant-message.ant-message-list.ant-message-top.ant-message-css-var"));
    }
}
