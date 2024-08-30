// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace AntDesign
{
    public class TransferSearchArgs
    {
        public TransferDirection Direction { get; private set; }

        public string Value { get; private set; }

        public TransferSearchArgs(TransferDirection direction, string value)
        {
            Direction = direction;
            Value = value;
        }
    }
}
