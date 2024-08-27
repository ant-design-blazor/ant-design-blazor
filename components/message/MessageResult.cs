// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace AntDesign
{

    public class MessageResult
    {
        public MessageResult(Task task)
        {
            Task = task;
        }

        public Task Task { get; private set; }

        public MessageResult Then(Action action)
        {
            var t = Task.ContinueWith((result) =>
            {
                action?.Invoke();
            }, TaskScheduler.Current);
            return new MessageResult(t);
        }

    }
}
