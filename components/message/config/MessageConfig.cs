using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneOf;

namespace AntDesign
{
    public class MessageConfig
    {
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
