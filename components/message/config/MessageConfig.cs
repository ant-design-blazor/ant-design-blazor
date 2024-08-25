// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class MessageConfig
    {
        internal int Order { get; set; }

        internal string AnimationClass { get; set; } = MessageAnimationType.Enter;

        internal CancellationTokenSource Cts { get; set; }

        public OneOf<string, RenderFragment> Content { get; set; }

        public double? Duration { get; set; } = null;

        public RenderFragment Icon { get; set; } = null;

        public event Action OnClose;

        internal void InvokeOnClose()
        {
            OnClose?.Invoke();
        }

        public string Key { get; set; } = null;

        public MessageType Type { get; set; }
    }
}
