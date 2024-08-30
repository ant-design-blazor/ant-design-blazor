// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace AntDesign
{
    public class TransferScrollArgs
    {
        public TransferDirection Direction { get; private set; }

        public EventArgs Args { get; private set; }

        public TransferScrollArgs(TransferDirection direction, EventArgs e)
        {
            Direction = direction;
            Args = e;
        }
    }
}
