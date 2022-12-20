// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace AntDesign.Tests.Notification
{
    /// <summary>
    /// This is an example unit test to show how you can mock INotificationService and verify it was called with the expected content.
    /// 這是一個示例單元測試，用於展示如何模擬 INotificationService 並驗證它是用預期的內容調用的。
    /// </summary>
    public class NotificationTest : AntDesignTestBase
    {
        private readonly Mock<INotificationService> _notificationServiceMock = new();

        public NotificationTest()
        {
            Services.AddScoped(provider => _notificationServiceMock.Object);
        }

        [Fact]
        public void ItShouldDispatchExpectedNotificationOnClick()
        {
            var expectedNotificationConfig = new NotificationConfig
            {
                Message = "Test Notification",
                Description = "Hello, I'm a test.",
                Key = "TestKey"
            };

            _notificationServiceMock
                .Setup(x => x.Info(It.Is<NotificationConfig>(x => string.Equals(x.Key, expectedNotificationConfig.Key) &&
                    string.Equals(x.Message.AsT0, expectedNotificationConfig.Message.AsT0) &&
                    string.Equals(x.Description.AsT0, expectedNotificationConfig.Description.AsT0))))
                .ReturnsAsync(new NotificationRef(new(), new()))
                .Verifiable();

            var systemUnderTest = RenderComponent<NotificationTestComponent>();
            systemUnderTest.Find("button#testNotificationBtn").Click();

            _notificationServiceMock.Verify();
        }
    }
}
