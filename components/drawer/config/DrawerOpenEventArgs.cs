// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable
namespace AntDesign;

public class DrawerOpenEventArgs
{
    /// <summary>
    /// 获取或设置一个值，该值指示是否应取消事件。
    /// 返回结果: true 如果应取消事件;否则为 false。
    /// Gets or sets a value indicating whether the event should be cancelled.
    /// Return result: true if the event should be cancelled; otherwise false.
    /// </summary>
    public bool Cancel { get; set; }
}
